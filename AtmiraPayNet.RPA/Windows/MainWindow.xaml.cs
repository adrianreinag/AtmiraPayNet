using OpenQA.Selenium.Chrome;
using System.Windows;


namespace AtmiraPayNet.RPA.Windows
{
    public partial class MainWindow : Window
    {
        private static ChromeDriver _driver;

        public MainWindow()
        {
            InitializeComponent();
            InitializeWebDriver();
        }

        private static void InitializeWebDriver()
        {
            _driver = new ChromeDriver();
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
