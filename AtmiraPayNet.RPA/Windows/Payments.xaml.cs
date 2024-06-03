using AtmiraPayNet.RPA.Models;
using AtmiraPayNet.RPA.Utils;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace AtmiraPayNet.RPA.Windows
{
    public partial class Payments : Window
    {
        private readonly ChromeDriver _driver;
        private List<SimplePaymentModel> _simplePayments = [];
        private FileSystemWatcher? _watcher;
        private readonly string _downloadDirectory = @"C:\Users\adrian.reinagalv\Downloads";


        public Payments(ChromeDriver driver)
        {
            InitializeComponent();
            _driver = driver;
            SetupFileWatcher();
            LoadPayments();
        }

        private void LoadPayments()
        {
            ScrapeTable();
            paymentsDataGrid.DataContext = this;
        }

        private void SetupFileWatcher()
        {
            _watcher = new FileSystemWatcher
            {
                Path = _downloadDirectory,
                Filter = "*.pdf",
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.Size
            };
            _watcher.Created += OnPdfDownloaded;
            _watcher.EnableRaisingEvents = true;
        }

        private async void OnPdfDownloaded(object sender, FileSystemEventArgs e)
        {
            await Task.Delay(5000); // Esperar para asegurar que la descarga ha terminado

            if (IsFileReady(e.FullPath))
            {
                Dispatcher.Invoke(() =>
                {
                    if (MessageBox.Show("El PDF se ha descargado correctamente. ¿Quieres abrirlo?", "Descarga completada", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        Process.Start(new ProcessStartInfo(e.FullPath) { UseShellExecute = true });
                    }
                });
            }
        }

        private static bool IsFileReady(string fileName)
        {
            try
            {
                using (FileStream inputStream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    return inputStream.Length > 0;
                }
            }
            catch (IOException)
            {
                return false;
            }
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
            _driver.Navigate().GoToUrl("https://localhost:7038/payment");

            var paymentWindow = new Payment(_driver);
            paymentWindow.Show();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
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
                    else
                    {
                        var downloadTask = Task.Run(() =>
                        {
                            _watcher!.EnableRaisingEvents = true;
                            Task.Delay(5000).Wait();
                        });

                        await downloadTask;

                        var downloadedFiles = Directory.GetFiles(_downloadDirectory, "*.pdf");
                        if (downloadedFiles.Length > 0)
                        {
                            var latestFile = new FileInfo(downloadedFiles.OrderByDescending(f => new FileInfo(f).CreationTime).First());
                            if (IsFileReady(latestFile.FullName))
                            {
                                Dispatcher.Invoke(() =>
                                {
                                    if (MessageBox.Show("El PDF se ha descargado correctamente. ¿Quieres abrirlo?", "Descarga completada", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                                    {
                                        Process.Start(new ProcessStartInfo(latestFile.FullName) { UseShellExecute = true });
                                    }
                                });
                            }
                        }
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
