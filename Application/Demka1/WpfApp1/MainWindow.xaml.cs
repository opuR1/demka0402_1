using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1.Pages;
using WpfApp1.Service;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AuthorizationService authService = new AuthorizationService();
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new Auth());
        }
        public void SetHeaderFullName(string FullName)
        {
            if(string.IsNullOrEmpty(FullName))
            {
                tbName.Text = "Гость";
            }
            else
            {
                tbName.Text = FullName;
            }
            
        }
        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (e.Content is Auth)
            {
                HeaderPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                HeaderPanel.Visibility = Visibility.Visible;
            }
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            tbName.Text = "";
            MainFrame.Navigate(new Auth());
            authService.ClearUser();
        }
    }
}
