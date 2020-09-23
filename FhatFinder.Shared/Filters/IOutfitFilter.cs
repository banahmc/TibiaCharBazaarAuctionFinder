using FhatFinder.Shared.Dtos;
using System.Collections.Generic;

namespace FhatFinder.Shared.Filters
{
    public interface IOutfitFilter
    {
        List<OutfitAndAddonDto> OutfitsAndAddons { get; }
    }
}
