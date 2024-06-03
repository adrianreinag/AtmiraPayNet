using AtmiraPayNet.RPA.Windows;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Windows;

namespace AtmiraPayNet.RPA
{
    public partial class MainWindow : Window
    {
        private static ChromeDriver? _driver;

        public MainWindow()
        {
            InitializeComponent();
            InitializeWebDriver();
        }

        private static void InitializeWebDriver()
        {
            var options = new ChromeOptions();
            var downloadDirectory = @"C:\Users\adrian.reinagalv\Downloads";

            options.AddUserProfilePreference("download.default_directory", downloadDirectory);
            options.AddUserProfilePreference("download.prompt_for_download", false);
            options.AddUserProfilePreference("download.directory_upgrade", true);
            options.AddUserProfilePreference("safebrowsing.enabled", true);

            _driver = new ChromeDriver(options);
        }

        private static void PerformWebAutomation()
        {
            _driver!.Navigate().GoToUrl("https://localhost:7038");
        }

        private void NavigateToAccess()
        {
            var accessWindow = new Access(_driver!);
            accessWindow.Show();
            Hide();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PerformWebAutomation();
            NavigateToAccess();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _driver!.Quit();
        }
    }
}