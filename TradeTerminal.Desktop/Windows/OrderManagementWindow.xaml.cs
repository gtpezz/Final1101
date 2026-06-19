using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TradeTerminal.Desktop.Windows
{
    /// <summary>
    /// Логика взаимодействия для OrderManagementWindow.xaml
    /// </summary>
    public partial class OrderManagementWindow : Window
    {
        private readonly ApiService _api = new();
        private List<JsonElement> _allOrders = new();
        private List<JsonElement> _statuses = new();
        private JsonElement? _selectedOrder;

        public class OrderDisplay
        {
            public int OrderNumber { get; set; }
            public DateTime OrderDate { get; set; }
            public string? UserFullName { get; set; }
            public string? StatusName { get; set; }
            public decimal TotalAmount { get; set; }
            public string? PickupCode { get; set; }
            public int OrderId { get; set; }
            public int StatusId { get; set; }
            public DateTime? DeliveryDate { get; set; }
        }

        public OrderManagementWindow()
        {
            InitializeComponent();
            Owner = Application.Current.MainWindow;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadStatusesAsync();
            await LoadOrdersAsync();
        }

        private async Task LoadStatusesAsync()
        {
            try
            {
                var result = await _api.GetOrderStatusesAsync();
                _statuses = result.EnumerateArray().ToList();

                cmbStatus.Items.Clear();
                foreach (var s in _statuses)
                {
                    cmbStatus.Items.Add(s.GetProperty("name").GetString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки статусов: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task LoadOrdersAsync()
        {
            try
            {
                var result = await _api.GetOrdersAsync();
                _allOrders = result.EnumerateArray().ToList();

                var displayList = new ObservableCollection<OrderDisplay>();
                foreach (var o in _allOrders)
                {
                    displayList.Add(new OrderDisplay
                    {
                        OrderId = o.GetProperty("id").GetInt32(),
                        OrderNumber = o.GetProperty("orderNumber").GetInt32(),
                        OrderDate = o.GetProperty("orderDate").GetDateTime(),
                        UserFullName = o.TryGetProperty("userFullName", out var user) ? user.GetString() : null,
                        StatusName = o.GetProperty("statusName").GetString(),
                        TotalAmount = o.GetProperty("totalAmount").GetDecimal(),
                        PickupCode = o.GetProperty("pickupCode").GetString(),
                        StatusId = o.GetProperty("statusId").GetInt32(),
                        DeliveryDate = o.TryGetProperty("deliveryDate", out var date) && date.ValueKind != JsonValueKind.Null
                            ? date.GetDateTime()
                            : (DateTime?)null
                    });
                }

                lvOrders.ItemsSource = displayList;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки заказов: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LvOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvOrders.SelectedItem is OrderDisplay selected)
            {
                _selectedOrder = _allOrders.FirstOrDefault(o => o.GetProperty("id").GetInt32() == selected.OrderId);

                cmbStatus.SelectedIndex = -1;
                for (int i = 0; i < _statuses.Count; i++)
                {
                    if (_statuses[i].GetProperty("id").GetInt32() == selected.StatusId)
                    {
                        cmbStatus.SelectedIndex = i;
                        break;
                    }
                }

                dpDeliveryDate.SelectedDate = selected.DeliveryDate;
            }
        }

        private async void CmbStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_selectedOrder == null || cmbStatus.SelectedIndex < 0) return;

            var orderId = _selectedOrder.Value.GetProperty("id").GetInt32();
            var statusId = _statuses[cmbStatus.SelectedIndex].GetProperty("id").GetInt32();

            try
            {
                await _api.UpdateOrderStatusAsync(orderId, statusId);
                await LoadOrdersAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления статуса: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void DpDeliveryDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_selectedOrder == null || dpDeliveryDate.SelectedDate == null) return;

            var orderId = _selectedOrder.Value.GetProperty("id").GetInt32();
            var deliveryDate = dpDeliveryDate.SelectedDate.Value;

            try
            {
                await _api.UpdateDeliveryDateAsync(orderId, deliveryDate);
                await LoadOrdersAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления даты доставки: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void BtnFindOrder_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtOrderNumber.Text))
            {
                await LoadOrdersAsync();
                return;
            }

            if (!int.TryParse(txtOrderNumber.Text, out var number))
            {
                MessageBox.Show("Введите корректный номер заказа", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var result = await _api.GetOrderByNumberAsync(number);
                if (result.ValueKind == JsonValueKind.Undefined)
                {
                    MessageBox.Show($"Заказ №{number} не найден", "Информация",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    await LoadOrdersAsync();
                    return;
                }

                var order = result;
                var displayList = new ObservableCollection<OrderDisplay>
            {
                new OrderDisplay
                {
                    OrderId = order.GetProperty("id").GetInt32(),
                    OrderNumber = order.GetProperty("orderNumber").GetInt32(),
                    OrderDate = order.GetProperty("orderDate").GetDateTime(),
                    UserFullName = order.TryGetProperty("userFullName", out var user) ? user.GetString() : null,
                    StatusName = order.GetProperty("statusName").GetString(),
                    TotalAmount = order.GetProperty("totalAmount").GetDecimal(),
                    PickupCode = order.GetProperty("pickupCode").GetString(),
                    StatusId = order.GetProperty("statusId").GetInt32(),
                    DeliveryDate = order.TryGetProperty("deliveryDate", out var date) && date.ValueKind != JsonValueKind.Null
                        ? date.GetDateTime()
                        : (DateTime?)null
                }
            };

                lvOrders.ItemsSource = displayList;
                _allOrders = new List<JsonElement> { order };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка поиска: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            txtOrderNumber.Text = "";
            await LoadOrdersAsync();
        }

        private async void BtnDeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedOrder == null)
            {
                MessageBox.Show("Выберите заказ для удаления", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = MessageBox.Show(
                $"Удалить заказ №{_selectedOrder.Value.GetProperty("orderNumber").GetInt32()}?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var orderId = _selectedOrder.Value.GetProperty("id").GetInt32();
                    await _api.DeleteOrderAsync(orderId);
                    _selectedOrder = null;
                    await LoadOrdersAsync();

                    MessageBox.Show("Заказ удален", "Успешно",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
