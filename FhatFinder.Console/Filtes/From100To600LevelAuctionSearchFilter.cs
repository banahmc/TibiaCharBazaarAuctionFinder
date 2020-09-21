using FhatFinder.Shared;
using FhatFinder.Shared.Filters;

namespace FhatFinder.Console.Filtes
{
    public class From100To600LevelAuctionSearchFilter : AuctionFilterBase
    {
        public From100To600LevelAuctionSearchFilter()
        {
            LevelRangeFrom = 150;
            LevelRangeTo = 600;
            BattlEyeState = BattlEye.Protected;
        }
    }
}
