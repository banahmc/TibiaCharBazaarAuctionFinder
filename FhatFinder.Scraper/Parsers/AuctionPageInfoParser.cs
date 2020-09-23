using AngleSharp.Dom;
using FhatFinder.Shared.Dto;
using FhatFinder.Shared.Dtos;
using System.Collections.Generic;

namespace FhatFinder.Scraper.Parsers
{
    public class AuctionPageInfoParser : IParser<IDocument, List<CharBazaarAuctionDto>>
    {
        private readonly IParser<IElement, CharacterNameDto> _characterNameParser;
        private readonly IParser<IElement, OutfitAndAddonDto> _outfitPraser;

        public AuctionPageInfoParser(
            IParser<IElement, CharacterNameDto> characterNameParser,
            IParser<IElement, OutfitAndAddonDto> outfitPraser)
        {
            _characterNameParser = characterNameParser;
            _outfitPraser = outfitPraser;
        }

        public List<CharBazaarAuctionDto> Parse(IDocument document)
        {
            var auctions = new List<CharBazaarAuctionDto>();
            var auctionElements = document?.DocumentElement.QuerySelectorAll(".Auction");
            if (auctionElements != null)
            {
                foreach (var auctionElement in auctionElements)
                {
                    var characterName = _characterNameParser.Parse(auctionElement);
                    if (characterName != null)
                    {
                        var outfit = _outfitPraser.Parse(auctionElement);
                        auctions.Add(new CharBazaarAuctionDto
                        {
                            CharacterName = characterName.CharacterName,
                            AuctionUrl = characterName.AuctionUrl,
                            Outfit = outfit?.Outfit ?? Shared.Outfit.Unknown,
                            Addons = outfit?.Addons ?? Shared.Addon.None
                        });
                    }
                }
            }

            return auctions;
        }
    }
}
