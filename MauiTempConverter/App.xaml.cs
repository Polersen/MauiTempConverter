namespace MauiTempConverter
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = new Window(new MainPage());
#if WINDOWS
            window.Width = 400;
            window.Height = 400;
#endif
            return window;
        }
    }
}