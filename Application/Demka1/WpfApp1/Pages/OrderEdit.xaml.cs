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
    /// Логика взаимодействия для OrderEdit.xaml
    /// </summary>
    public partial class OrderEdit : Page
    {
        private Orders _order;
        private bool IsNew;
        private kr_de1Entities db = kr_de1Entities.GetContext();
        private Dictionary<int, string> _pickupPoints = new Dictionary<int, string>
        {
            {1, "г.Лесной, ул. Вишневая, 32"},
            {2, "г.Лесной, ул. Подгорная, 8"},
            {3, "г.Лесной, ул. Шоссейная, 24"},
            {4, "г.Лесной, ул. Зеленая, 32"},
            {5, "г.Лесной, ул. Маяковского, 47"},
            {6, "г.Лесной, ул. Светлая, 46"},
            {7, "г.Лесной, ул. Цветочная, 8"},
            {8, "г.Лесной, ул. Коммунистическая, 1"},
            {9, "г.Лесной, ул. Спортивная, 46"},
            {10, "г.Лесной, ул. Гоголя, 41"},
            {11, "г.Лесной, ул. Северная, 13"},
            {12, "г.Лесной, ул. Молодежная, 50"},
            {13, "г.Лесной, ул. Новая, 19"},
            {14, "г.Лесной, ул. Октябрьская, 19"},
            {15, "г.Лесной, ул. Садовая, 4"},
            {16, "г.Лесной, ул. Фрунзе, 43"},
            {17, "г.Лесной, ул. Школьная, 50"},
            {18, "г.Лесной, ул. Коммунистическая, 20"},
            {19, "г.Лесной,  ул. 8 Марта"},
            {20, "г.Лесной, ул. Комсомольская, 26"},
            {21, "г.Лесной, ул. Чехова, 3"},
            {22, "г.Лесной, ул. Дзержинского, 28"},
            {23, "г.Лесной, ул. Набережная, 30"},
            {24, "г.Лесной, ул. Чехова, 24"},
            {25, "г.Лесной,  ул. Степная, 30"},
            {26, "г.Лесной, ул. Коммунистическая, 43"},
            {27, "г.Лесной, ул. Солнечная, 25"},
            {28, "г.Лесной, ул. Шоссейная, 40"},
            {29, "г.Лесной, ул. Партизанская, 49"},
            {30, "г.Лесной, ул. Победы, 46"},
            {31, "г.Лесной, ул. Полевая, 35"},
            {32, "г.Лесной, ул. Маяковского, 44"},
            {33, "г.Лесной, ул. Клубная, 44"},
            {34, "г.Лесной, ул. Некрасова, 12"},
            {35, "г.Лесной, ул. Комсомольская, 17"},
            {36, "г.Лесной, ул. Мичурина, 26"}

        };

        private Dictionary<int, string> _allUsers = new Dictionary<int, string>
        {
            {1, "Ворсин Петр Евгеньевич"},
            {2, "Старикова Елена Павловна"},
            {3, "Одинцов Серафим Артёмович"},
            {4, "Степанов Михаил Артёмович"},
            {5, "Ворсин Петр Евгеньевич"},
            {6, "Старикова Елена Павловна"},
            {7, "Михайлюк Анна Вячеславовна"},
            {8, "Ситдикова Елена Анатольевна"},
            {9, "Никифорова Весения Николаевна"},
            {10, "Сазонов Руслан Германович"}
        };

        private Dictionary<int, string> _OStatuses = new Dictionary<int, string>
        {
            {1, "Завершен"},
            {2, "Новый"}
        };
        public OrderEdit(Orders order)
        {
            InitializeComponent();
            _order = order;
            LoadCMB();
            IsNew = _order == null;
            if (IsNew)
            {
                _order = new Orders();
            }
            else
            {
                LoadOrder();
            }
        }
        private void LoadOrder()
        {
            tbOrderDate.Text = _order.OrderDate.ToString();
            tbDeliveryDate.Text = _order.DeliveryDate.ToString();
            tbCode.Text = _order.Code.ToString();
            cmbOrderStatus.SelectedValue = _order.OrderStatusId.ToString();
            cmbPickupPoint.SelectedValue = _order.PickupPointId.ToString();
            cmbUser.SelectedValue = _order.UserId.ToString();
        }


        private void LoadCMB()
        {
            cmbOrderStatus.SelectedValuePath = "Key";
            cmbOrderStatus.DisplayMemberPath = "Value";
            cmbOrderStatus.ItemsSource = _OStatuses;

            cmbPickupPoint.SelectedValuePath = "Key";
            cmbPickupPoint.DisplayMemberPath = "Value";
            cmbPickupPoint.ItemsSource = _pickupPoints;

            cmbUser.SelectedValuePath = "Key";
            cmbUser.DisplayMemberPath = "Value";
            cmbUser.ItemsSource = _allUsers;
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DateTime.TryParse(tbOrderDate.Text, out DateTime parsedDate))
                {
                    _order.OrderDate = parsedDate;
                }
                else
                {
                    throw new Exception("Введите корректную дату заказа(дд.мм.гггг)!");
                }

                if (DateTime.TryParse(tbDeliveryDate.Text, out DateTime parsedDDate))
                {
                    _order.DeliveryDate = parsedDDate;
                }
                else
                {
                    throw new Exception("Введите корректную дату доставки(дд.мм.гггг)!");
                }

                _order.PickupPointId = Convert.ToInt32(cmbPickupPoint.SelectedValue);
                _order.UserId = Convert.ToInt32(cmbUser.SelectedValue);
                _order.OrderStatusId = Convert.ToInt32(cmbOrderStatus.SelectedValue);

                _order.Code = tbCode.Text;

                if (IsNew)
                {
                    db.Orders.Add(_order);
                    MessageBox.Show("Заказ успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    db.Entry(_order).State = EntityState.Modified;
                    MessageBox.Show("Данные заказа успешно обновлены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                db.SaveChanges();
                NavigationService.GoBack();
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при сохранении данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (IsNew)
            {
                NavigationService.GoBack();
                return;
            }

            var result = MessageBox.Show($"Вы уверены, что хотите удалить заказ {_order.OrderId}?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var orderDB = db.Orders.FirstOrDefault(p => p.OrderId == _order.OrderId);

                    if (orderDB != null)
                    {
                        db.Orders.Remove(orderDB);
                        db.SaveChanges();
                        MessageBox.Show("Заказ успешно удален!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Заказ уже удален или не найден в базе данных.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }

                    NavigationService.GoBack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении заказа: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
