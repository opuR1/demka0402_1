using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using WpfApp1.Service;

namespace WpfApp1.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProductEdit.xaml
    /// </summary>
    public partial class ProductEdit : Page
    {
        private Products _product;
        private bool IsNewProduct;
        private kr_de1Entities db = kr_de1Entities.GetContext();
        private string _sourceFilePath = null;
        private Dictionary<int, string> _producers = new Dictionary<int, string>
        {
            {1, "М500"}, {2, "Изостронг"}, {3, "Knauf"}, {4, "MixMaster"},
            {5, "ЛСР"}, {6, "ВОЛМА"}, {7, "Vinylon"}, {8, "Павловский завод"},
            {9, "Weber"}, {10, "Hesler"}, {11, "Armero"}, {12, "Wenzo Roma"},
            {13, "KILIMGRIN"}, {14, "Исток"}, {15, "RUIZ"}, {16, "Husqvarna"}, {17, "Delta"}
        };
        private Dictionary<int, string> _units = new Dictionary<int, string>
        {
            {1, "шт."}, {2, ""}
        };
        private Dictionary<int, string> _categories = new Dictionary<int, string>
        {
            {1, "Общестроительные материалы"},
            {2, "Стеновые и фасадные материалы"},
            {3, "Сухие строительные смеси и гидроизоляция"},
            {4, "Ручной инструмент"},
            {5, "Защита лица, глаз, головы"}
        };

        public ProductEdit(Products product)
        {
            InitializeComponent();
            _product = product;
            LoadComboBoxes();
            IsNewProduct = product == null;

            if (IsNewProduct)
            {
                _product = new Products();
            }
            else
            {
                LoadProduct();
            }
        }

        private void LoadProduct()
        {
            tbName.Text = _product.ProductName;
            tbPrice.Text = _product.Price.ToString();
            tbCount.Text = _product.Count.ToString();
            tbDiscount.Text = _product.Discount.ToString();
            tbDescription.Text = _product.Description.ToString();
            cmbUnits.SelectedValue = _product.UnitId;
            cmbCategory.SelectedValue = _product.CategoryId;
            cmbProducer.SelectedValue = _product.ProducerId;
            cmbSupplier.SelectedValue = _product.SupplierId;

            if (!string.IsNullOrEmpty(_product.ImagePath))
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(_product.ImagePath, UriKind.RelativeOrAbsolute);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                imgPicture.Source = bitmap;
            }
        }

        private void LoadComboBoxes()
        {
            cmbCategory.SelectedValuePath = "Key";
            cmbCategory.DisplayMemberPath = "Value";
            cmbCategory.ItemsSource = _categories;

            cmbProducer.SelectedValuePath = "Key";
            cmbProducer.DisplayMemberPath = "Value";
            cmbProducer.ItemsSource = _producers;

            cmbSupplier.SelectedValuePath = "Key";
            cmbSupplier.DisplayMemberPath = "Value";
            cmbSupplier.ItemsSource = _producers;

            cmbUnits.SelectedValuePath = "Key";
            cmbUnits.DisplayMemberPath = "Value";
            cmbUnits.ItemsSource = _units;
        }

        private bool ValidateFields(out decimal price, out int count, out int discount)
        {
            string error = "";
            price = 0; count = 0; discount = 0;

            if (string.IsNullOrWhiteSpace(tbName.Text)) error += "Введите название товара.\n";
            if (cmbUnits.SelectedValue == null) error += "Выберите единицу измерения.\n";
            if (cmbCategory.SelectedValue == null) error += "Выберите категорию.\n";
            if (cmbProducer.SelectedValue == null) error += "Выберите производителя.\n";
            if (cmbSupplier.SelectedValue == null) error += "Выберите поставщика.\n";
            if (!decimal.TryParse(tbPrice.Text, out price) || price <= 0) error += "Введите корректную цену > 0.\n";
            if (!int.TryParse(tbCount.Text, out count) || count < 0) error += "Количество должно быть целым числом >= 0.\n";
            if (!int.TryParse(tbDiscount.Text, out discount) || discount < 0 || discount > 100) error += "Скидка должна быть числом от 0 до 100.\n";

            if (!string.IsNullOrWhiteSpace(error))
            {
                tblError.Text = error;
                return false;
            }
            tblError.Text = "";
            return true;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ValidateFields(out decimal validPrice, out int validCount, out int validDiscount))
                {
                    _product.ProductName = tbName.Text;
                    _product.Description = tbDescription.Text;
                    _product.Price = validPrice;
                    _product.Count = validCount;
                    _product.Discount = validDiscount;
                    _product.UnitId = (int)cmbUnits.SelectedValue;
                    _product.CategoryId = (int)cmbCategory.SelectedValue;
                    _product.ProducerId = (int)cmbProducer.SelectedValue;
                    _product.SupplierId = (int)cmbSupplier.SelectedValue;

                    if (!string.IsNullOrEmpty(_sourceFilePath))
                    {
                        string fileName = System.IO.Path.GetFileName(_sourceFilePath);
                        string targetFolder = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");

                        System.IO.Directory.CreateDirectory(targetFolder);
                        string targetPath = System.IO.Path.Combine(targetFolder, fileName);

                        System.IO.File.Copy(_sourceFilePath, targetPath, true);

                        _product.Photo = fileName;
                        _sourceFilePath = null;
                    }

                    if (IsNewProduct)
                    {
                        _product.ItemNumber = RandomGenerationService.GenerateRandomString(6);
                        db.Products.Add(_product);
                        MessageBox.Show("Товар успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Данные товара успешно обновлены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    db.SaveChanges();
                    NavigationService.GoBack();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при сохранении данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (IsNewProduct)
            {
                NavigationService.GoBack();
                return;
            }

            var result = MessageBox.Show($"Вы уверены, что хотите удалить товар {_product.ProductName}?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var productDB = db.Products.FirstOrDefault(p => p.ProductId == _product.ProductId);

                    if (productDB != null)
                    {
                        db.Products.Remove(productDB);
                        db.SaveChanges();
                        MessageBox.Show("Товар успешно удален!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Товар уже удален или не найден в базе данных.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }

                    NavigationService.GoBack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении товара: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnEditImg_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                _sourceFilePath = openFileDialog.FileName;

                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(_sourceFilePath);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();

                imgPicture.Source = bitmap;
            }
        }
    }
}
