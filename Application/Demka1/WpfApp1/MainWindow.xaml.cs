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
using WpfApp1.Models;
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
        private Users _user = null;
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
            
            if (e.Content is ProductList)
            {
                tbHeader.Text = "Список товаров";
                btnBack.Visibility = Visibility.Collapsed;
            }
            if (e.Content is ProductEdit)
            {
                tbHeader.Text = "Редактирование/Добавление товара";
                btnBack.Visibility = Visibility.Visible;
            }
            if (e.Content is OrderList)
            {
                tbHeader.Text = "Список заказов";
                btnBack.Visibility = Visibility.Visible;
            }
            if (e.Content is OrderEdit)
            {
                tbHeader.Text = "Редактирование/Добавление заказа";
                btnBack.Visibility = Visibility.Visible;
            }
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            tbName.Text = "";
            MainFrame.Navigate(new Auth());
            authService.ClearUser();
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.Content is ProductList)
            {
                MainFrame.Navigate(new Auth());
            }
            else
            {
                MainFrame.GoBack();
            }
        }
    }
}
