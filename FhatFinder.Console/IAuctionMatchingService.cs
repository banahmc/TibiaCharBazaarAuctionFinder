using FhatFinder.Shared.Dto;
using FhatFinder.Shared.Filters;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FhatFinder.Console
{
    public interface IAuctionMatchingService
    {
        Task<List<CharBazaarAuctionDto>> GetAuctionsMatchingOutfit(IAuctionFilter auctionFilter, IOutfitFilter outfitFilter, CancellationToken cancellationToken);
    }
}
