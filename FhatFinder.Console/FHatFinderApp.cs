using FhatFinder.Console.Filtes;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FhatFinder.Console
{
    public class FHatFinderApp : IFHatFinderApp
    {
        private readonly ILogger<FHatFinderApp> _logger;
        private readonly IAuctionMatchingService _auctionMatchingService;

        public FHatFinderApp(
            ILogger<FHatFinderApp> logger,
            IAuctionMatchingService auctionMatchingService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _auctionMatchingService = auctionMatchingService ?? throw new ArgumentNullException(nameof(auctionMatchingService));
        }

        public async Task Run(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Retrieving auctions...");

            var auctionFilter = new GetAllAuctionFilter();
            var outfitFilter = new MageOutfitWithFeruHatOutfitFilter();

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var auctions = await _auctionMatchingService.GetAuctionsMatchingOutfit(auctionFilter, outfitFilter, cancellationToken);
            stopwatch.Stop();

            _logger.LogInformation($"Found {auctions.Count()} matching auction(s) in {stopwatch.Elapsed.TotalSeconds} seconds...");
            int i = 0;
            foreach (var auction in auctions)
            {
                ++i;
                _logger.LogInformation($"{i}: {auction.CharacterName} -- {auction.Outfit}_{auction.Addons} -- {auction.AuctionUrl}");
            }
        }
    }
}
