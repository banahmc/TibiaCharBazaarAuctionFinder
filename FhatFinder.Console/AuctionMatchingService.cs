﻿using FhatFinder.Scraper;
using FhatFinder.Shared.Dto;
using FhatFinder.Shared.Filters;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FhatFinder.Console
{
    public class AuctionMatchingService : IAuctionMatchingService
    {
        private readonly ICharBazaarScraper _charBazaarScraper;

        public AuctionMatchingService(ICharBazaarScraper charBazaarScraper)
        {
            _charBazaarScraper = charBazaarScraper ?? throw new System.ArgumentNullException(nameof(charBazaarScraper));
        }

        public async Task<IEnumerable<CharBazaarAuctionDto>> GetAuctionsMatchingOutfit(IAuctionFilter auctionFilter, IOutfitFilter outfitFilter, CancellationToken cs)
        {
            var auctions = await _charBazaarScraper.GetAuctionsAsync(auctionFilter, cs);
            var matchingAuctions = new List<CharBazaarAuctionDto>();

            foreach (var auction in auctions)
            {
                if (auction.Outfit == outfitFilter.Outfit &&
                    auction.Addons == outfitFilter.Addons)
                {
                    matchingAuctions.Add(auction);
                }
            }

            return matchingAuctions;
        }
    }
}
