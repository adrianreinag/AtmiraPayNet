using AtmiraPayNet.RPA.Utils;
using OpenQA.Selenium;

namespace AtmiraPayNet.RPA.Models
{
    internal class SimplePaymentModel
    {
        public required string SourceBank{ get; set; }
        public required string DestinationBank{ get; set; }
        public required string Amount { get; set; }
        public required string Currency { get; set;}
        public required Status Status { get; set; }
        public IWebElement? EditBtn { get; set; }
        public IWebElement? ViewBtn { get; set; }
        public IWebElement? DownloadBtn { get; set; }
    }
}
