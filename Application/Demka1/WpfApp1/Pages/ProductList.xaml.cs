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
using System.Data.Entity;

namespace WpfApp1.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProductList.xaml
    /// </summary>
    public partial class ProductList : Page
    {
        private string _fullname;
        private Users _user;
        public ProductList(Users user)
        {
            _user = user;
            InitializeComponent();
            _fullname = GetFullName();
            if (Application.Current.MainWindow is MainWindow visibleWindow)
            {
                visibleWindow.SetHeaderFullName(_fullname);
            }
            //GetRole();
            LoadProducts();

        }
        private string GetFullName()
        {
            string fullName;
            if (_user != null)
            {
                fullName = $"{_user.LastName} {_user.FirstName} {_user.MiddleName}";
            }
            else
            {
                fullName = string.Empty;
            }
            return fullName;
        }
        private void LoadProducts()
        {
            using (var db = kr_de1Entities.GetContext())
            {
                var productsList = db.Products.Include(p => p.Categories).Include(p => p.Producers).Include(p => p.Suppliers).Include(p => p.Units).ToList();
                lbProducts.ItemsSource = productsList;
            }
        }
        private void GetRole()
        {
            switch(_user.RoleId)
            {
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                default: 
                    break;
            }
        }
    }
}
