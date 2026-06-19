using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        private readonly ApiService _api = new();
        private readonly OrderState _order = OrderState.Instance;
        private JsonElement _orderData;
        private List<OrderItem> _items = new();

        public class OrderItem
        {
            public string? ProductName { get; set; }
            public int Quantity { get; set; }
            public decimal Price { get; set; }
            public decimal Total { get; set; }
        }

        public OrderWindow(JsonElement order)
        {
            InitializeComponent();
            Owner = Application.Current.MainWindow;
            _orderData = order;
            LoadOrder();
        }

        private void LoadOrder()
        {
            try
            {
                // Номер заказа
                var orderNumber = _orderData.GetProperty("orderNumber").GetInt32();
                lblOrderNumber.Text = orderNumber.ToString();

                // Клиент
                var userFullName = _orderData.TryGetProperty("userFullName", out var userProp)
                    ? userProp.GetString()
                    : null;
                lblClient.Text = string.IsNullOrEmpty(userFullName) ? "Гость" : userFullName;

                // Код получения
                var pickupCode = _orderData.GetProperty("pickupCode").GetString();
                lblPickupCode.Text = pickupCode;

                // Товары
                _items.Clear();
                decimal total = 0;

                if (_orderData.TryGetProperty("items", out var itemsProp))
                {
                    foreach (var item in itemsProp.EnumerateArray())
                    {
                        var productName = item.TryGetProperty("productName", out var nameProp)
                            ? nameProp.GetString()
                            : "Неизвестный товар";
                        var quantity = item.GetProperty("quantity").GetInt32();
                        var price = item.GetProperty("priceAtOrder").GetDecimal();
                        var itemTotal = item.GetProperty("total").GetDecimal();

                        _items.Add(new OrderItem
                        {
                            ProductName = productName,
                            Quantity = quantity,
                            Price = price,
                            Total = itemTotal
                        });
                        total += itemTotal;
                    }
                }

                lvItems.ItemsSource = _items;
                lblTotal.Text = total.ToString("C");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки заказа: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Сохранить талон в txt (Задание 10)
        /// </summary>
        private void BtnSaveTalon_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var saveFileDialog = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "Text files (*.txt)|*.txt",
                    DefaultExt = "txt",
                    FileName = $"Талон_заказа_{lblOrderNumber.Text}_{DateTime.Now:yyyyMMdd_HHmmss}"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    var sb = new StringBuilder();
                    sb.AppendLine("=".PadRight(50, '='));
                    sb.AppendLine($"          ТАЛОН ЗАКАЗА №{lblOrderNumber.Text}");
                    sb.AppendLine("=".PadRight(50, '='));
                    sb.AppendLine();
                    sb.AppendLine($"Дата заказа: {DateTime.Now:dd.MM.yyyy HH:mm}");
                    sb.AppendLine($"Клиент: {lblClient.Text}");
                    sb.AppendLine($"Код получения: {lblPickupCode.Text}");
                    sb.AppendLine();
                    sb.AppendLine("-".PadRight(50, '-'));
                    sb.AppendLine("Товары:");

                    foreach (OrderItem item in lvItems.Items)
                    {
                        sb.AppendLine($"  {item.ProductName}");
                        sb.AppendLine($"    {item.Quantity} шт. x {item.Price:C} = {item.Total:C}");
                    }

                    sb.AppendLine("-".PadRight(50, '-'));
                    sb.AppendLine($"ИТОГО: {lblTotal.Text}");
                    sb.AppendLine("=".PadRight(50, '='));
                    sb.AppendLine();
                    sb.AppendLine("Спасибо за покупку!");

                    File.WriteAllText(saveFileDialog.FileName, sb.ToString(), Encoding.UTF8);

                    MessageBox.Show("Талон успешно сохранен!", "Успешно",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Оформить заказ - сохранить в БД (Задание 9)
        /// </summary>
        private async void BtnCheckout_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = MessageBox.Show(
                    "Вы уверены, что хотите оформить заказ?",
                    "Подтверждение",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    // Заказ уже создан со статусом "Новый"
                    // Можно обновить статус если нужно
                    // await _api.UpdateOrderStatusAsync(_order.OrderId, 1);

                    MessageBox.Show($"Заказ №{lblOrderNumber.Text} успешно оформлен!\nКод получения: {lblPickupCode.Text}",
                        "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);

                    _order.ClearOrder();
                    DialogResult = true;
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка оформления заказа: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Очистить корзину
        /// </summary>
        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Вы уверены, что хотите очистить корзину?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                _order.ClearOrder();
                DialogResult = true;
                Close();
            }
        }
    }
}
