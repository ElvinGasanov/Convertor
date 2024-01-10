using System.Net;
using System.Xml.Linq;

namespace WebConvertor;

public class Parser
{
    public float ParseXML(string selectedDate, string charCode, out float nom)
    {
        string url = $"https://www.cbr.ru/scripts/XML_daily.asp?date_req={selectedDate}";

        WebClient client = new();
        string xmlData = client.DownloadString(url);

        XDocument doc = XDocument.Parse(xmlData);

        IEnumerable<XElement> t1 = doc.Descendants("Valute").ToList();

        var c = t1.FirstOrDefault(x => x.Element("CharCode").Value == charCode);


        if (c == null || c.Element == null)
        {
            nom = 1;
            return 1;
        }

        var v = c.Element("Value").Value;


        string selectedCurrency = (
            from currency in t1
            where currency.Element("CharCode").Value == charCode
            select currency.Element("Value").Value
        ).FirstOrDefault();


        string Nominal = (
            from currency in t1
            where currency.Element("CharCode").Value == charCode
            select currency.Element("Nominal").Value
        ).FirstOrDefault();

        nom = float.Parse(Nominal);

        if (selectedCurrency != null)
        {
            return float.Parse(selectedCurrency);
        }
        else
        {
            return 0;
        }
    }
}
