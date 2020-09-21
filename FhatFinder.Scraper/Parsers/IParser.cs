namespace FhatFinder.Scraper.Parsers
{
    public interface IParser<in TParseInput, out TParseResult>
    {
        TParseResult Parse(TParseInput input);
    }
}
