using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FhatFinder.Shared.Dtos;
using System.Linq;

namespace FhatFinder.Scraper.Parsers
{
    public class PageCountParser : IParser<IDocument, PageCountDto>
    {
        public PageCountDto Parse(IDocument document)
        {
            int count = 0;

            // If we have "First" and "Last" page navigation options then we check last page number
            // Otherwise we count "Page Links"

            var elements = document.DocumentElement.QuerySelectorAll(".FirstOrLastElement > a");
            if (elements.Length >= 2)
            {
                if (elements[1] is IHtmlAnchorElement anchorElement)
                {
                    var href = anchorElement.Href;
                    if (!string.IsNullOrEmpty(href))
                    {
                        var lastPageNbrStr = href.Substring(href.IndexOf("currentpage=") + 12);
                        if (int.TryParse(lastPageNbrStr, out var lastPageNbr))
                        {
                            count = lastPageNbr;
                        }
                    }
                }
            } 
            else
            {
                elements = document.DocumentElement.QuerySelectorAll(".PageLink");
                count = elements.Count() / 2;
            }

            return new PageCountDto { TotalPageCount = count };
        }
    }
}
