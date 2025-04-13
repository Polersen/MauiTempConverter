using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MauiTempConverter
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnEmptyPressed(object sender, EventArgs e)
        {
            var button = sender as Button;

            button.BackgroundColor = Color.FromArgb("#BABABA");
            await button.ScaleTo(0.85, 100, Easing.CubicIn);//Scaling all buttons in and out when pressed and released to create sense of depth
        }

        private async void OnEmptyReleased(object sender, EventArgs e)
        {
            SekEntry.Text = null;
            UsdEntry.Text = null;

            var button = sender as Button;

            button.BackgroundColor = Color.FromArgb("#EBEBEB");
            await button.ScaleTo(1, 100, Easing.CubicOut);
        }

        private async void OnConvertPressed(object sender, EventArgs e)
        {
            var button = sender as Button;

            button.BackgroundColor = Color.FromArgb("#BABABA");
            await button.ScaleTo(0.85, 100, Easing.CubicIn);
        }

        private async void OnConvertReleased(object sender, EventArgs e)
        {
            //Avoid processing empty values
            if (string.IsNullOrEmpty(SekEntry.Text) && string.IsNullOrEmpty(UsdEntry.Text))
            {
                SekEntry.Text = "0";
                UsdEntry.Text = "0";
            }

            //But if at least one field has a value
            else if (!string.IsNullOrEmpty(SekEntry.Text) || !string.IsNullOrEmpty(UsdEntry.Text))
            {
                //Default 0 when user is pressing convert more than once
                if (Decimal.TryParse(SekEntry.Text, out decimal parsedSek) && Decimal.TryParse(UsdEntry.Text, out decimal parsedUsd))
                {
                    SekEntry.Text = "0";
                    UsdEntry.Text = "0";
                }

                //Handling case for successful SEK parse
                else if (Decimal.TryParse(SekEntry.Text, out parsedSek))
                {
                    UsdEntry.Text = (await ConvertToUSD(parsedSek)).ToString();
                }

                //Handling case for successful USD parse
                else if (Decimal.TryParse(UsdEntry.Text, out parsedUsd))
                {
                    SekEntry.Text = (await ConvertToSEK(parsedUsd)).ToString();
                }

                //Default 0 when parsing fails to catch error
                else
                {
                    SekEntry.Text = "0";
                    UsdEntry.Text = "0";
                }
            }

            var button = sender as Button;

            button.BackgroundColor = Color.FromArgb("#EBEBEB");
            await button.ScaleTo(1, 100, Easing.CubicOut);
        }

        private async void OnStopPressed(object sender, EventArgs e)
        {
            var button = sender as Button;

            button.BackgroundColor = Color.FromArgb("#BABABA");
            await button.ScaleTo(0.85, 100, Easing.CubicIn);
        }

        private async void OnStopReleased(object sender, EventArgs e)
        {
            var button = sender as Button;

            button.BackgroundColor = Color.FromArgb("#EBEBEB");
            await button.ScaleTo(1, 100, Easing.CubicOut);

            Application.Current?.Quit();
        }

        private async Task<decimal> ConvertToSEK(decimal usd)
        {
            decimal rate = await GetSekRateAsync();
            return Math.Round(usd * rate, 5);
        }

        private async Task<decimal> ConvertToUSD(decimal sek)
        {
            decimal rate = await GetSekRateAsync();
            return Math.Round(sek / rate, 5);
        }

        private async Task<decimal> GetSekRateAsync()
        {
            var service = new ExchangeRateService();

            try
            {
                return await service.GetExchangeRateAsync();//Better readability in its own method rather than calling it twice above
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }
    }
}
