using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FhatFinder.Shared;
using FhatFinder.Shared.Dtos;
using System;
using System.Linq;

namespace FhatFinder.Scraper.Parsers
{
    public class AuctionOutfitParser : IParser<IElement, OutfitAndAddonDto>
    {
        public OutfitAndAddonDto Parse(IElement element)
        {
            var auctionOutfitImage = element?.QuerySelectorAll(".AuctionBody > .AuctionOutfit > .AuctionOutfitImage").FirstOrDefault();
            if (auctionOutfitImage != null
                && auctionOutfitImage is IHtmlImageElement imageElement)
            {
                var src = imageElement.Source;
                if (!string.IsNullOrEmpty(src))
                {
                    var outfitImage = src.Substring(src.LastIndexOf("/") + 1);
                    var outfitImageParts = outfitImage.Substring(0, outfitImage.IndexOf(".gif")).Split("_");
                    if (outfitImageParts.Length == 2 &&
                        Enum.TryParse<Outfit>(outfitImageParts[0], out Outfit outfit) &&
                        Enum.TryParse<Addon>(outfitImageParts[1], out Addon addons) &&
                        Enum.IsDefined(typeof(Outfit), outfit) &&
                        Enum.IsDefined(typeof(Addon), addons))
                    {
                        return new OutfitAndAddonDto { Outfit = outfit, Addons = addons };
                    }
                }
            }

            return null;
        }
    }
}
