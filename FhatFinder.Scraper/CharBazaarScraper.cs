using AngleSharp;
using AngleSharp.Dom;
using FhatFinder.Scraper.Parsers;
using FhatFinder.Shared;
using FhatFinder.Shared.Dto;
using FhatFinder.Shared.Dtos;
using FhatFinder.Shared.Filters;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FhatFinder.Scraper
{
    public class CharBazaarScraper : ICharBazaarScraper
    {
        private readonly ILogger<CharBazaarScraper> _logger;
        private readonly IBrowsingContext _browsingContext;
        private readonly IParser<IDocument, PageCountDto> _pageCountParser;
        private readonly IParser<IDocument, IEnumerable<CharBazaarAuctionDto>> _auctionInfoParser;

        public CharBazaarScraper(
            ILogger<CharBazaarScraper> logger,
            IBrowsingContext browsingContext,
            IParser<IDocument, PageCountDto> pageCountParser,
            IParser<IDocument, IEnumerable<CharBazaarAuctionDto>> auctionInfoParser
        )
        {
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
            _browsingContext = browsingContext ?? throw new System.ArgumentNullException(nameof(browsingContext));
            _pageCountParser = pageCountParser ?? throw new System.ArgumentNullException(nameof(pageCountParser));
            _auctionInfoParser = auctionInfoParser ?? throw new System.ArgumentNullException(nameof(auctionInfoParser));
        }

        public async Task<IEnumerable<CharBazaarAuctionDto>> GetAuctionsAsync(IAuctionFilter auctionFilter, CancellationToken cs)
        {
            var auctions = new List<CharBazaarAuctionDto>();

            var url = GetFullUrl(auctionFilter);

            var totalPageCount = 0;
            using (var firstPageContent = await _browsingContext.OpenAsync(url, cs))
            {
                totalPageCount = _pageCountParser.Parse(firstPageContent)?.TotalPageCount ?? 0;
                _logger.LogInformation($"CharBazaar - Total page count: {totalPageCount}");

                var auctionsOnPage = GetAuctionInfoOnPage(firstPageContent, auctionFilter.CurrentPage);
                if (auctionsOnPage.Any())
                {
                    auctions.AddRange(auctionsOnPage);
                }
            }

            if (totalPageCount > 1)
            {
                var tasks = new List<Task<IEnumerable<CharBazaarAuctionDto>>>();

                for (var i = 2; i <= totalPageCount; i++)
                {
                    auctionFilter.CurrentPage = i;
                    var nextPageUrl = GetFullUrl(auctionFilter);
                    tasks.Add(GetAuctionsOnPage(nextPageUrl, auctionFilter.CurrentPage, cs));
                }

                var auctionsOnOtherPages = await Task.WhenAll<IEnumerable<CharBazaarAuctionDto>>(tasks);
                foreach (var auctionsOnOtherPage in auctionsOnOtherPages)
                {
                    if (auctionsOnOtherPages.Any())
                    {
                        auctions.AddRange(auctionsOnOtherPage);
                    }
                }
            }

            _logger.LogInformation($"CharBazaar - Found {auctions.Count()} auction(s) in total on {totalPageCount} page(s)");

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

        private async Task<IEnumerable<CharBazaarAuctionDto>> GetAuctionsOnPage(string url, int page, CancellationToken cs)
        {
            using (var pageContent = await _browsingContext.OpenAsync(url, cs))
            {
                _logger.LogInformation($"CharBazaar - Retrieving auctions from url: {url}");
                return GetAuctionInfoOnPage(pageContent, page);
            }
        }

        private IEnumerable<CharBazaarAuctionDto> GetAuctionInfoOnPage(IDocument pageContent, int page)
        {
            var auctionsOnPage = _auctionInfoParser.Parse(pageContent);

            _logger.LogInformation($"CharBazaar - Page number: {page}, auctions on page: {auctionsOnPage.Count()}");

            return auctionsOnPage;
        }
    }
}
