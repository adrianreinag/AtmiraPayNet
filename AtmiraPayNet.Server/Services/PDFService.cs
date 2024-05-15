using AtmiraPayNet.Server.Services.Interfaces;
using AtmiraPayNet.Shared.DTO;
using iText.Html2pdf;
using Microsoft.AspNetCore.Mvc;

namespace AtmiraPayNet.Server.Services
{
    public class PDFService : IPDFService
    {
        public string CreateBase64PDFFromTemplate(string template, Dictionary<string, string> keyValuePairs)
        {
            string html = File.ReadAllText(template);

            foreach (var keyValuePair in keyValuePairs)
            {
                var key = $"{{{{{keyValuePair.Key}}}}}";
                html = html.Replace(key, keyValuePair.Value.ToString());
            }

            using var memoryStream = new MemoryStream();

            var converterProperties = new ConverterProperties();

            HtmlConverter.ConvertToPdf(html, memoryStream, converterProperties);

            return Convert.ToBase64String(memoryStream.ToArray());
        }
    }
}
