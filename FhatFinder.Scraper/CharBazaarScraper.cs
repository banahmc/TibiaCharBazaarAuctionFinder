using AngleSharp;
using AngleSharp.Dom;
using FhatFinder.Scraper.Parsers;
using FhatFinder.Shared;
using FhatFinder.Shared.Dto;
using FhatFinder.Shared.Dtos;
using FhatFinder.Shared.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FhatFinder.Scraper
{
    public class CharBazaarScraper : ICharBazaarScraper
    {
        private const int RequestLimit = 25;

        private readonly ILogger<CharBazaarScraper> _logger;
        private readonly IBrowsingContext _browsingContext;
        private readonly IParser<IDocument, PageCountDto> _pageCountParser;
        private readonly IParser<IDocument, List<CharBazaarAuctionDto>> _auctionInfoParser;

        private readonly HashSet<string> _success = new HashSet<string>();
        private readonly HashSet<string> _fail = new HashSet<string>();

        public CharBazaarScraper(
            ILogger<CharBazaarScraper> logger,
            IBrowsingContext browsingContext,
            IParser<IDocument, PageCountDto> pageCountParser,
            IParser<IDocument, List<CharBazaarAuctionDto>> auctionInfoParser
        )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _browsingContext = browsingContext ?? throw new ArgumentNullException(nameof(browsingContext));
            _pageCountParser = pageCountParser ?? throw new ArgumentNullException(nameof(pageCountParser));
            _auctionInfoParser = auctionInfoParser ?? throw new ArgumentNullException(nameof(auctionInfoParser));
        }

        public async Task<List<CharBazaarAuctionDto>> GetAuctionsAsync(IAuctionFilter auctionFilter, CancellationToken cs)
        {
            (int numberOfPages, List<CharBazaarAuctionDto> auctions) = await GetNumberOfPagesAndFirstPageAuctions(auctionFilter, cs);

            if (numberOfPages > 1)
            {
                var auctionPagesUrls = GetAuctionUrlsForFilter(auctionFilter, numberOfPages, 2);
                var auctionsOnPages = await GetAuctions(auctionPagesUrls, cs);
                if (auctionsOnPages.Any())
                {
                    auctions.AddRange(auctionsOnPages);
                }
            }

            _logger.LogInformation($"CharBazaar - Found {auctions.Count()} auction(s) on {numberOfPages} page(s)");
            _logger.LogInformation($"Requests stats - " +
                $"requests sent: {_success.Count + _fail.Count}, " +
                $"success: {_success.Count}, " +
                $"fail: {_fail.Count}, " +
                $"rate: {Math.Round((double)(_success.Count * 100) / numberOfPages, 2)}%");

            return auctions;
        }

        // TODO: See if this shouldn't be placed elsewhere outside of this class. Maybe inside Filter?
        private string GetFullUrl(IAuctionFilter auctionFilter)
        {
            var url = Constants.CharBazaarUrl;

            if (auctionFilter != null)
            {
                var filterQuery = AuctionFilterBuilder.Build(auctionFilter);
                if (!string.IsNullOrEmpty(filterQuery))
                {
                    url += "&" + filterQuery;
                }
            }

            return url;
        }

        private async Task<List<CharBazaarAuctionDto>> GetAuctionsOnPage(string url, CancellationToken cs)
        {
            using (var pageContent = await _browsingContext.OpenAsync(url, cs))
            {
                _logger.LogInformation($"CharBazaar - Retrieving auctions from url: {url}");
                return GetAuctionInfoOnPage(pageContent, url);
            }
        }

        private List<CharBazaarAuctionDto> GetAuctionInfoOnPage(IDocument pageContent, string url)
        {
            var auctionsOnPage = _auctionInfoParser.Parse(pageContent);

            if (auctionsOnPage.Count() > 0)
            {
                _success.Add(url);
            }
            else
            {
                _fail.Add(url);
            }

            return auctionsOnPage;
        }

        private async Task<(int, List<CharBazaarAuctionDto>)> GetNumberOfPagesAndFirstPageAuctions(IAuctionFilter auctionFilter, CancellationToken cs)
        {
            var auctions = new List<CharBazaarAuctionDto>();
            var numberOfPages = 0;

            var url = GetFullUrl(auctionFilter);
            using (var firstPageContent = await _browsingContext.OpenAsync(url, cs))
            {
                numberOfPages = _pageCountParser.Parse(firstPageContent)?.TotalPageCount ?? 0;
                _logger.LogInformation($"CharBazaar - Total page count: {numberOfPages}");

                var auctionsOnPage = GetAuctionInfoOnPage(firstPageContent, url);
                if (auctionsOnPage.Any())
                {
                    auctions.AddRange(auctionsOnPage);
                }
            }

            return (numberOfPages, auctions);
        }

        private async Task<List<CharBazaarAuctionDto>> GetAuctions(List<string> urls, CancellationToken cs)
        {
            var auctions = new List<CharBazaarAuctionDto>();

            var tasks = new List<Task<List<CharBazaarAuctionDto>>>();

            var throttler = new SemaphoreSlim(RequestLimit);

            for (var i = 0; i < urls.Count; i++)
            {
                var url = urls[i];

                await throttler.WaitAsync();

                tasks.Add(Task.Run(async () =>
                {
                    try
                    {
                        var result = await GetAuctionsOnPage(url, cs);

                        await Task.Delay(5000);

                        return result;
                    }
                    finally
                    {
                        throttler.Release();
                    }
                }));
            }

            var auctionsOnPages = await Task.WhenAll<List<CharBazaarAuctionDto>>(tasks);
            foreach (var auctionsOnOtherPage in auctionsOnPages)
            {
                if (auctionsOnOtherPage.Any())
                {
                    auctions.AddRange(auctionsOnOtherPage);
                }
            }

            return auctions;
        }

        private List<string> GetAuctionUrlsForFilter(IAuctionFilter auctionFilter, int numberOfPages, int startPageNumber)
        {
            var _requestUrls = new List<string>();
            for (var i = startPageNumber; i <= numberOfPages; i++)
            {
                auctionFilter.CurrentPage = i;
                _requestUrls.Add(GetFullUrl(auctionFilter));
            }
            return _requestUrls;
        }
    }
}
