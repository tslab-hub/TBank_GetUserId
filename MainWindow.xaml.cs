using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace TBank_GetUserId
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void GetUserIdButton_Click(object sender, RoutedEventArgs e)
        {
            UserIdTextBox.Text = "Загрузка...";
            UserIdTextBox.Text = await GetUserIdAsync(TokenTextBox.Text);
        }

        private void CopyUserIdButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(UserIdTextBox.Text))
            {
                Clipboard.SetText(UserIdTextBox.Text.Trim());
            }
        }

        public async Task<string> GetUserIdAsync(string token)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://invest-public-api.tbank.ru/rest/tinkoff.public.invest.api.contract.v1.UsersService/GetInfo"),
                Content = new StringContent("{}", Encoding.UTF8, "application/json")
            };

            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Authorization", $"Bearer {token}");

            using var client = new HttpClient();
            try
            {
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);
                if (doc.RootElement.TryGetProperty("userId", out var userIdElement))
                {
                    return userIdElement.GetString();
                }
                else
                {
                    return "UserId не найден в ответе";
                }
            }
            catch (HttpRequestException e)
            {
                return e.Message;
            }
            catch (JsonException e)
            {
                return $"Ошибка разбора JSON: {e.Message}";
            }
        }
    }
}
