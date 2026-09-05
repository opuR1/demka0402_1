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
        private List<Products> _allProducts = new List<Products>();
        public ProductList(Users user)
        {
            _user = user;
            InitializeComponent();
            _fullname = GetFullName();
            if (Application.Current.MainWindow is MainWindow visibleWindow)
            {
                visibleWindow.SetHeaderFullName(_fullname);
            }

            GetRole();
            LoadProducts();
            LoadFilters();

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
                _allProducts = db.Products.Include(p => p.Categories).Include(p => p.Producers).Include(p => p.Suppliers).Include(p => p.Units).ToList();
                lbProducts.ItemsSource = _allProducts;

                ApplyFilters();
            }
        }
        private void LoadFilters()
        {
            using (var db = kr_de1Entities.GetContext())
            {
                var producers = db.Producers.ToList();

                var allProducersItem = new Producers { ProducerId = 0, ProducerName = "Все производители"};
                producers.Insert(0, allProducersItem);

                cmbFilter.ItemsSource = producers;
                cmbFilter.DisplayMemberPath = "ProducerName";
                cmbFilter.SelectedValuePath = "ProducerId";

                cmbFilter.SelectedIndex = 0;
            }
        }
        private void ApplyFilters()
        {
            if (_allProducts == null || _allProducts.Count == 0) return;

            var filtered = _allProducts.AsEnumerable();

            string searchText = tbSearch.Text.Trim().ToLower();
            if (!string.IsNullOrEmpty(searchText))
            {
                filtered = filtered.Where(p => p.ProductName.ToLower().Contains(searchText) || p.Categories.CategoryName.ToLower().Contains(searchText) || p.Description.ToLower().Contains(searchText));
            }

            if (cmbFilter.SelectedValue is int selectedProducerId && selectedProducerId > 0)
            {
                filtered = filtered.Where(p => p.ProducerId == selectedProducerId);
            }

            if (cmbSort.SelectedItem is ComboBoxItem selectedSort)
            {
                switch (selectedSort.Content.ToString())
                {
                    case "Price asc":
                        filtered = filtered.OrderBy(p => p.Price);
                        break;
                    case "Price desc":
                        filtered = filtered.OrderByDescending(p => p.Price);
                        break;
                    case "Discount asc":
                        filtered = filtered.OrderBy(p => p.Discount);
                        break;
                    case "Discount desc":
                        filtered = filtered.OrderByDescending(p => p.Discount);
                        break;
                    case "Count asc":
                        filtered = filtered.OrderBy(p => p.Count);
                        break;
                    case "Count desc":
                        filtered = filtered.OrderByDescending(p => p.Count);
                        break;
                }
            }
            lbProducts.ItemsSource = filtered.ToList();
        }
        private void GetRole()
        {
            if(_user != null)
            {
                switch (_user.RoleId)
                {
                    case 1:
                        spFinder.Visibility = Visibility.Visible;
                        btnAdd.Visibility = Visibility.Visible;
                        btnEdit.Visibility = Visibility.Visible;
                        btnOrders.Visibility = Visibility.Visible;
                        break;
                    case 2:
                        spFinder.Visibility = Visibility.Visible;
                        btnOrders.Visibility = Visibility.Visible;
                        btnAdd.Visibility = Visibility.Collapsed;
                        btnEdit.Visibility = Visibility.Collapsed;
                        break;
                    case 3:
                        spFinder.Visibility = Visibility.Collapsed;
                        btnOrders.Visibility = Visibility.Collapsed;
                        btnAdd.Visibility = Visibility.Collapsed;
                        btnEdit.Visibility = Visibility.Collapsed;
                        break;
                    default:
                        spFinder.Visibility = Visibility.Collapsed;
                        btnOrders.Visibility = Visibility.Collapsed;
                        btnAdd.Visibility = Visibility.Collapsed;
                        btnEdit.Visibility = Visibility.Collapsed;
                        break;
                }
            }
            else
            {
                spFinder.Visibility = Visibility.Collapsed;
                btnAdd.Visibility = Visibility.Collapsed;
                btnEdit.Visibility = Visibility.Collapsed;
            }
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void cmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void cmbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Products selectedProduct = lbProducts.SelectedItem as Products;
            NavigationService.Navigate(new ProductEdit(selectedProduct));
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ProductEdit(null));
        }

        private void btnOrders_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new OrderList(_user));
        }
    }
}
