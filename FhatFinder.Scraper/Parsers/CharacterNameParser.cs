using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FhatFinder.Shared.Dtos;

namespace FhatFinder.Scraper.Parsers
{
    public class CharacterNameParser : IParser<IElement, CharacterNameDto>
    {
        public CharacterNameDto Parse(IElement element)
        {
            var auctionCharacterNameElement = element?.QuerySelector(".AuctionHeader > .AuctionCharacterName > a");
            if (auctionCharacterNameElement != null
                && auctionCharacterNameElement is IHtmlAnchorElement anchorElement)
            {
                var txt = anchorElement.Text;
                var href = anchorElement.Href;
                if (!string.IsNullOrEmpty(txt) &&
                    !string.IsNullOrEmpty(href))
                {
                    return new CharacterNameDto { CharacterName = txt, AuctionUrl = href };
                }
            }

            return null;
        }
    }
}
