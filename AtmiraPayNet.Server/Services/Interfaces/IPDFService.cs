namespace AtmiraPayNet.Server.Services.Interfaces
{
    public interface IPDFService
    {
        string CreateBase64PDFFromTemplate(string template, Dictionary<string, string> keyValuePairs);
    }
}
