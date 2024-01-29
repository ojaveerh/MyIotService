using Database.Data.Models;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace DeviceImitator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer dispatcherTimer;
        string _token;

        public MainWindow()
        {
            InitializeComponent();
            dispatcherTimer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 5)
            };
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Start();

            GetToken();
            RegisterDevice();
        }

        private void GetToken()
        {
            var client = new HttpClient();
            var body = new { username = "test", password = "test1" };
            var response = client.PostAsJsonAsync("https://localhost:7247/api/Login", body).Result;
            
            _token = response.Content.ReadAsStringAsync().Result;
        }
        private void RegisterDevice()
        {
            var client = new HttpClient();
            var body = new { deviceNr = 123, name = "Factory device", description = "Suitable for factories", userName = "test", dataId = 1000, dataName = "Inside temperature", minRange = -150, maxRange=150, value=23 };
            var response = client.PostAsJsonAsync("https://localhost:7047/RegisterDeviceAsync", body).Result; 
        }

        // Register device - pöördub DeviceControlleri poole registreerimiseks Post - devicenr, Name, DaeviceData
        //kontrollib, kas on juba registreeritud kasutaja külge

        private async void DispatcherTimer_Tick(object? sender, EventArgs e)
        {
            dispatcherTimer.Stop();
            await GetDesiredTemperatureFromBackend();
            dispatcherTimer.Start();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int.TryParse(temperatureTextBox.Text, out int temperature);

            if (temperature > 150 || temperature < -150)
            {
                MessageBox.Show("Value must be between 150 and -150");
                return;
            }

            SetDesiredTemperature(temperature).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Get device inside temperature 
        /// </summary>
        private async Task GetDesiredTemperatureFromBackend()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            using HttpResponseMessage response = await client.GetAsync("https://localhost:7047/Device/123/GetDeviceInsideTemperature");

            var temperature = Convert.ToInt32(await response.Content.ReadAsStringAsync());

            thermometer.Value = temperature;

            if (temperature <= 0)
                thermometer.Foreground = Brushes.Blue;
            else
                thermometer.Foreground = Brushes.Red;
        }

        /// <summary>
        /// Set device inside temperature 
        /// </summary>
        /// <param name="newTemperature">Prefered new temperature</param>
        private async Task SetDesiredTemperature(int newTemperature)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            client.PostAsJsonAsync($"https://localhost:7047/Device/123/SetDeviceInsideTemperature?temperature={newTemperature}", new { });
        }

        private void thermometer_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (thermometer.Value <= 0)
                thermometer.Foreground = Brushes.Blue;
            else
                thermometer.Foreground = Brushes.Red;

            textBoxCurrentTemp.Text = $"Current inside temperature: {thermometer.Value}";
        }
    }
}