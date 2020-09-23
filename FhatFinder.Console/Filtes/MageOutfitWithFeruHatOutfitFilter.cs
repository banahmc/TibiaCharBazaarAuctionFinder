using FhatFinder.Shared;
using FhatFinder.Shared.Dtos;
using FhatFinder.Shared.Filters;
using System.Collections.Generic;

namespace FhatFinder.Console.Filtes
{
    public class MageOutfitWithFeruHatOutfitFilter : IOutfitFilter
    {
        public List<OutfitAndAddonDto> OutfitsAndAddons => new List<OutfitAndAddonDto>
        {
            new OutfitAndAddonDto { Outfit = Outfit.MaleMage, Addons = Addon.FirstAndSecond },
            new OutfitAndAddonDto { Outfit = Outfit.FemaleSummoner, Addons = Addon.FirstAndSecond },
        };
    }
}
