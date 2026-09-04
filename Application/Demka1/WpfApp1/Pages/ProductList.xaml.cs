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

namespace WpfApp1.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProductList.xaml
    /// </summary>
    public partial class ProductList : Page
    {
        private string _fullname;
        public ProductList(Users user)
        {
            InitializeComponent();
            _fullname = GetFullName(user);
            if (Application.Current.MainWindow is MainWindow visibleWindow)
            {
                visibleWindow.SetHeaderFullName(_fullname);
            }
        }
        private string GetFullName(Users user)
        {
            string fullName;
            if (user != null)
            {
                fullName = $"{user.LastName} {user.FirstName} {user.MiddleName}";
            }
            else
            {
                fullName = string.Empty;
            }
            return fullName;
        }
    }
}
