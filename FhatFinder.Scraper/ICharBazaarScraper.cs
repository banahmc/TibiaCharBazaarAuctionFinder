using FhatFinder.Shared.Dto;
using FhatFinder.Shared.Filters;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FhatFinder.Scraper
{
    public interface ICharBazaarScraper
    {
        Task<IEnumerable<CharBazaarAuctionDto>> GetAuctionsAsync(IAuctionFilter auctionFilter, CancellationToken cs);
    }
}
