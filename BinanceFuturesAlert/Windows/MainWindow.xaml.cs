using BinanceFuturesAlert.Models;
using BinanceFuturesAlert.Services;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BinanceFuturesAlert
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string TRADINGURL = @"https://www.binance.com/en/futures/";
        private static List<Candlestick> candlesticks = null;
        private Timer? timer;


        public MainWindow()
        {
            InitializeComponent();
            InitializeCryptos();
            ContentRendered += MainWindow_ContentRendered;

        }

        private void MainWindow_ContentRendered(object? sender, EventArgs e)
        {
            ConfigureTimer();
        }


        /// <summary>
        /// Adds crypto UI components to MainWindow
        /// </summary>
        private void InitializeCryptos()
        {
            if (candlesticks == null)
            {
                var service = new CandlestickService();
                candlesticks = Task.Run(() => service.GetCandlesticksAsync()).GetAwaiter().GetResult();
            }
            
            WrapPanel contentCryptos = (WrapPanel)FindName("contentCryptos");
            
            foreach (var candlestick in candlesticks)
            {
                Border border = new();
                border.CornerRadius = new CornerRadius(6);
                border.BorderBrush = new SolidColorBrush(Colors.Gray);
                border.BorderThickness = new Thickness(2);
                border.Background = new SolidColorBrush(Colors.LightGray);
                border.Margin = new Thickness(2);
                border.Name = "contentCrypto" + candlestick.Pair;
                RegisterName("contentCrypto" + candlestick.Pair, border);


                StackPanel panel = new();
                panel.Width = 100;
                panel.Height = 130;
                panel.Margin = new Thickness(5, 10, 5, 5);
                panel.VerticalAlignment = VerticalAlignment.Center;
                panel.HorizontalAlignment = HorizontalAlignment.Center;
                panel.Orientation = Orientation.Vertical;

                TextBlock contentCryptoName = new();
                contentCryptoName.Name = "CryptoName" + candlestick.Pair;
                contentCryptoName.TextAlignment = TextAlignment.Center;
                contentCryptoName.Margin = new Thickness(2);
                contentCryptoName.FontWeight = FontWeights.Bold;

                Style style = FindResource("LinkStyle") as Style;
                Hyperlink link = new Hyperlink();
                link.Inlines.Add(candlestick.Pair);
                link.NavigateUri = new Uri(TRADINGURL + candlestick.Pair);
                link.Style = style;
                link.RequestNavigate += new RequestNavigateEventHandler(Hyperlink_RequestNavigate);

                contentCryptoName.Inlines.Add(link);

                StackPanel contentPercentageChangeUp = new();
                contentPercentageChangeUp.Orientation = Orientation.Horizontal;
                contentPercentageChangeUp.Name = "contentPercentageChangeUp" + candlestick.Pair;
                contentPercentageChangeUp.HorizontalAlignment = HorizontalAlignment.Center;
                contentPercentageChangeUp.VerticalAlignment = VerticalAlignment.Center;
                RegisterName("contentPercentageChangeUp" + candlestick.Pair, contentPercentageChangeUp);

                BitmapImage bitImage = new BitmapImage(new Uri(@"/Images/up-arrow.png", UriKind.Relative));
                Image imageUpArrow = new();
                imageUpArrow.Height = 14;
                imageUpArrow.Name = "imageUpArrow" + candlestick.Pair;
                imageUpArrow.Source = bitImage;
                RegisterName("imageUpArrow" + candlestick.Pair, imageUpArrow);

                TextBlock contentPercentageUp = new();
                contentPercentageUp.Name = "CryptoPercentageUp" + candlestick.Pair;
                contentPercentageUp.Text = "0 %";
                contentPercentageUp.Margin = new Thickness(2, 4, 2, 4);
                contentPercentageUp.Padding = new Thickness(2, 0, 0, 0);
                contentPercentageUp.FontSize = 16;
                contentPercentageUp.TextAlignment = TextAlignment.Center;
                RegisterName("CryptoPercentageUp" + candlestick.Pair, contentPercentageUp);

                contentPercentageChangeUp.Children.Add(imageUpArrow);
                contentPercentageChangeUp.Children.Add(contentPercentageUp);

                TextBlock contentTimeUp = new();
                contentTimeUp.Name = "CryptoTimeUp" + candlestick.Pair;
                contentTimeUp.Text = "0 min";
                contentTimeUp.TextAlignment = TextAlignment.Center;
                contentTimeUp.Margin = new Thickness(2);
                RegisterName("CryptoTimeUp" + candlestick.Pair, contentTimeUp);

                StackPanel contentPercentageChangeDown = new();
                contentPercentageChangeDown.Orientation = Orientation.Horizontal;
                contentPercentageChangeDown.Name = "contentPercentageChangeDown" + candlestick.Pair;
                contentPercentageChangeDown.HorizontalAlignment = HorizontalAlignment.Center;
                contentPercentageChangeDown.VerticalAlignment = VerticalAlignment.Center;
                RegisterName("contentPercentageChangeDown" + candlestick.Pair, contentPercentageChangeDown);

                bitImage = new BitmapImage(new Uri(@"/Images/down-arrow.png", UriKind.Relative));
                Image imageDownArrow = new();
                imageDownArrow.Height = 14;
                imageDownArrow.Name = "imageDownArrow" + candlestick.Pair;
                imageDownArrow.Source = bitImage;
                RegisterName("imageDownArrow" + candlestick.Pair, imageDownArrow);

                TextBlock contentPercentageDown = new();
                contentPercentageDown.Name = "CryptoPercentageDown" + candlestick.Pair;
                contentPercentageDown.Text = "0 %";
                contentPercentageDown.Margin = new Thickness(2, 4, 2, 4);
                contentPercentageDown.FontSize = 16;
                contentPercentageDown.TextAlignment = TextAlignment.Center;
                RegisterName("CryptoPercentageDown" + candlestick.Pair, contentPercentageDown);

                contentPercentageChangeDown.Children.Add(imageDownArrow);
                contentPercentageChangeDown.Children.Add(contentPercentageDown);

                TextBlock contentTimeDown = new();
                contentTimeDown.Name = "CryptoTimeDown" + candlestick.Pair;
                contentTimeDown.Text = "0 min";
                contentTimeDown.TextAlignment = TextAlignment.Center;
                contentTimeDown.Margin = new Thickness(2);
                RegisterName("CryptoTimeDown" + candlestick.Pair, contentTimeDown);

                panel.Children.Add(contentCryptoName);
                panel.Children.Add(contentPercentageChangeUp);
                panel.Children.Add(contentTimeUp);
                panel.Children.Add(contentPercentageChangeDown);
                panel.Children.Add(contentTimeDown);
                border.Child = panel;

                contentCryptos.Children.Add(border);
            }
        }


        /// <summary>
        /// Configure a timer for background tasks
        /// </summary>
        private void ConfigureTimer()
        {
            timer = new Timer(
                TimerCallback,
                null,
                TimeSpan.Zero,           
                TimeSpan.FromMinutes(4)  
            );
        }


        /// <summary>
        /// Runs parallel tasks to update cryptos
        /// </summary>
        /// <param name="state"></param>
        private async void TimerCallback(object? state)
        {
            Dispatcher.Invoke(BeginUpdateCryptoUI);

            Task[] tasks = new Task[4];
            for (int i = 0; i < 4; i++)
            {
                int index = i;
                tasks[i] = Task.Run(() => UpdateCryptosAsync(index));
            }
            await Task.WhenAll(tasks);

            Dispatcher.Invoke(OrderCryptos);
            Dispatcher.Invoke(EndUpdateCryptoUI);
        }


        /// <summary>
        /// Update the candestlick data and UI
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private async Task UpdateCryptosAsync(int index)
        {
            var serviceCandlestick = new CandlestickService();
            var serviceCrypto = new CryptoService();

            for (int i = index; i < candlesticks.Count; i += 4)
            {
                Candlestick c;

                c = await Task.Run(() => serviceCandlestick.UpdateCandlestickAsync(candlesticks[i], "1m", 40));

                c.Crypto = serviceCrypto.GetCrypto(c.Pair, c.Candles);

                Dispatcher.Invoke(() =>
                {
                    TextBlock t = (TextBlock)FindName("CryptoPercentageUp" + c.Pair);
                    t.Text = (c.Crypto.PercentageChangeUp > 0) ? string.Format("{0:N2} %", c.Crypto.PercentageChangeUp) + " %" : "--";
                    t = (TextBlock)FindName("CryptoTimeUp" + c.Pair);
                    t.Text = (c.Crypto.MinutesChangeUp > 0) ? c.Crypto.MinutesChangeUp + " min" : "    --";

                    t = (TextBlock)FindName("CryptoPercentageDown" + c.Pair);
                    t.Text = (c.Crypto.PercentageChangeDown < 0) ? string.Format("{0:N2} %", c.Crypto.PercentageChangeDown) + " %" : " --";
                    t = (TextBlock)FindName("CryptoTimeDown" + c.Pair);
                    t.Text = (c.Crypto.MinutesChangeDown > 0) ? c.Crypto.MinutesChangeDown + " min" : "     --";
                });
            }
        }
                


        /// <summary>
        /// Open the URL in the browser
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
            e.Handled = true;
        }


        /// <summary>
        /// Sort the candlestick charts by their greatest percentage change in descending order
        /// </summary>
        public void OrderCryptos()
        {
            candlesticks.Sort((a, b) => b.Crypto.MaxPercentageChange.CompareTo(a.Crypto.MaxPercentageChange));
            Dispatcher.Invoke(() =>
            {
                WrapPanel contentFutures = (WrapPanel)this.FindName("contentCryptos");
                var fadeAnimation = new DoubleAnimation();
                fadeAnimation.From = 0;
                fadeAnimation.To = 1;
                for (int i = 0; i < candlesticks.Count; i++)
                {
                    Border border = (Border)this.FindName("contentCrypto" + candlesticks[i].Pair);
                    contentFutures.Children.Remove(border);
                    contentFutures.Children.Insert(i, border);
                    border.BeginAnimation(Border.OpacityProperty, fadeAnimation);
                }
            });
        }


        /// <summary>
        /// Update the UI when updating the cryptos
        /// </summary>
        private void BeginUpdateCryptoUI()
        {
            TextBlock textInformation = (TextBlock)this.FindName("textInformation");
            textInformation.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0ECB81"));
            textInformation.FontSize = 16;
            textInformation.Text = "Cargando...";
            Image image = (Image)this.FindName("loadingIcon");
            image.Visibility = Visibility.Visible;
        }


        /// <summary>
        /// Update the UI when updating the cryptos is finished
        /// </summary>
        private void EndUpdateCryptoUI()
        {
            TextBlock textInformation = (TextBlock)this.FindName("textInformation");
            textInformation.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EAECEF"));
            textInformation.Text = "Datos actualizados al " + DateTime.Now.ToString("dd'-'MM'-'yyyy HH':'mm");
            Image image = (Image)this.FindName("loadingIcon");
            image.Visibility = Visibility.Hidden;
        }
    }
}