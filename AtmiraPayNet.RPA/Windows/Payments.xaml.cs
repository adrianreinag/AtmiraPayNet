using AtmiraPayNet.RPA.Models;
using System.Windows;
using AtmiraPayNet.RPA.Utils;
using System.Collections.Generic;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;

namespace AtmiraPayNet.RPA.Windows
{
    public partial class Payments : Window
    {
        private List<SimplePaymentModel> _simplePayments = [];
        private readonly ChromeDriver _driver;

        public Payments(ChromeDriver driver)
        {
            InitializeComponent();
            LoadPayments();
            _driver = driver;
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

                List<IWebElement> rowElements = [.. _driver.FindElements(By.XPath("//*[@id='app']/div/main/article/div/div/table/tbody/tr"))];

                foreach (var row in rowElements)
                {
                    List<IWebElement> cellElements = [.. row.FindElements(By.TagName("td"))];

                    SimplePaymentModel payment = new()
                    {
                        SourceBank = cellElements[0].Text,
                        DestinationBank = cellElements[1].Text,
                        Amount = cellElements[2].Text,
                        Currency = cellElements[3].Text,
                        Status = cellElements[4].Text == "Draft" ? Status.Draft : Status.Generated
                    };

                    if (payment.Status == Status.Draft)
                    {
                        payment.EditBtn = cellElements[5].FindElement(By.TagName("button"));
                    }
                    else
                    {
                        payment.ViewBtn = cellElements[5].FindElement(By.TagName("button"));
                        payment.DownloadBtn = cellElements[6].FindElement(By.TagName("button"));
                    }

                    _simplePayments.Add(payment);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al extraer los datos de la tabla: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
