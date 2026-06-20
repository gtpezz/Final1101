using System.Text.Json;
using System.Windows;

namespace TradeTerminal.Desktop.Windows
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly ApiService _api = new();

        public LoginWindow()
        {
            InitializeComponent();
            Owner = Application.Current.MainWindow;
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLogin.Text) || string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                MessageBox.Show("Введите логин и пароль!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var result = await _api.AuthenticateAsync(txtLogin.Text, txtPassword.Password);
                if (result.ValueKind == JsonValueKind.Undefined)
                {
                    MessageBox.Show("Неверный логин или пароль!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Properties.Settings.Default.CurrentUserId = result.GetProperty("id").GetInt32();
                Properties.Settings.Default.CurrentUserFullName = result.GetProperty("fullName").GetString();
                Properties.Settings.Default.CurrentUserRole = result.GetProperty("roleName").GetString();
                Properties.Settings.Default.IsAuthenticated = true;
                Properties.Settings.Default.Save();

                DialogResult = true;
                Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка подключения к серверу!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
