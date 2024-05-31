using AtmiraPayNet.RPA.Models;
using AtmiraPayNet.RPA.Utils;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Windows;
using System.Windows.Controls;

namespace AtmiraPayNet.RPA.Windows
{
    public partial class Payments : Window
    {
        private readonly ChromeDriver _driver;
        private List<SimplePaymentModel> _simplePayments = [];

        public Payments(ChromeDriver driver)
        {
            InitializeComponent();

            _driver = driver;
            LoadPayments();
        }

        private void LoadPayments()
        {
            ScrapeTable();
            paymentsDataGrid.DataContext = this;
        }

        public void ScrapeTable()
        {
            try
            {
                _driver.Navigate().GoToUrl("https://localhost:7038/payments");

                WebDriverWait wait = new(_driver, TimeSpan.FromSeconds(10));

                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.Id("loading")));

                wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='app']/div/main/article/div/div/table/tbody/tr")));

                List<IWebElement> rowElements = [.. _driver.FindElements(By.XPath("//*[@id='app']/div/main/article/div/div/table/tbody/tr"))];

                rowElements.RemoveAt(rowElements.Count - 1);

                foreach (var row in rowElements)
                {
                    List<IWebElement> cellElements = [.. row.FindElements(By.TagName("td"))];

                    Status status = cellElements[4].Text == "Borrador" ? Status.Draft : Status.Generated;
                    IWebElement btn;
                    string textBtn;

                    if (status == Status.Draft)
                    {
                        btn = cellElements[5].FindElement(By.TagName("button"));
                        textBtn = "Editar";
                    }
                    else
                    {
                        btn = cellElements[6].FindElement(By.TagName("button"));
                        textBtn = "Descargar";
                    }

                    SimplePaymentModel payment = new()
                    {
                        SourceBank = cellElements[0].Text,
                        DestinationBank = cellElements[1].Text,
                        Amount = cellElements[2].Text,
                        Currency = cellElements[3].Text,
                        Status = status,
                        Btn = btn,
                        TextBtn = textBtn
                    };

                    _simplePayments.Add(payment);
                }

                paymentsDataGrid.ItemsSource = _simplePayments;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al extraer los datos de la tabla: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CreatePayment_Click(object sender, RoutedEventArgs e)
        {
            var paymentWindow = new Payment(_driver);
            paymentWindow.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.CommandParameter is SimplePaymentModel payment)
            {
                try
                {
                    payment.Btn.Click();

                    if (payment.Status == Status.Draft)
                    {
                        var paymentWindow = new Payment(_driver);
                        paymentWindow.Show();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ocurrió un error al hacer clic en el botón: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
