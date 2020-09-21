using AngleSharp.Dom;

namespace FhatFinder.Parser
{
    public interface IHtmlParser
    {
        string Parse(IDocument document);
    }
}
