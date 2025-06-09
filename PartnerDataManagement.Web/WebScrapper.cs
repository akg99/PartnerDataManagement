using HtmlAgilityPack;

namespace PartnerDataManagement.Web
{
    public class WebScraper
    {
        public static List<string> ScrapeBusinessNames(string url)
        {
            var web = new HtmlWeb();
            var doc = web.Load(url);
            var names = new List<string>();

            foreach (var node in doc.DocumentNode.SelectNodes("//h2[@class='business-name']"))
            {
                names.Add(node.InnerText.Trim());
            }

            return names;
        }
    }
}
