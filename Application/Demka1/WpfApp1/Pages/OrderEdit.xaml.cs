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
        private List<PickupPoints> _pickupPoints;

        private List<Users> _allUsers;

        private List<OrderStatuses> _orderStatuses;
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
            _orderStatuses = db.OrderStatuses.ToList();
            cmbOrderStatus.SelectedValuePath = "OStatusId";
            cmbOrderStatus.DisplayMemberPath = "OStatusName";
            cmbOrderStatus.ItemsSource = _orderStatuses;

            _pickupPoints = db.PickupPoints.ToList();
            cmbPickupPoint.SelectedValuePath = "PointId";
            cmbPickupPoint.DisplayMemberPath = "FullAdress";
            cmbPickupPoint.ItemsSource = _pickupPoints;

            _allUsers = db.Users.ToList();
            cmbUser.SelectedValuePath = "UserId";
            cmbUser.DisplayMemberPath = "FullName";
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
