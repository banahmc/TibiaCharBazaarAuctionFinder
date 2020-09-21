using FhatFinder.Shared;
using FhatFinder.Shared.Filters;

namespace FhatFinder.Console.Filtes
{
    public class MageOutfitWithFeruHatOutfitFilter : IOutfitFilter
    {
        public Outfit Outfit => Outfit.Mage;
        public Addon Addons => Addon.FirstAndSecond;
    }
}
