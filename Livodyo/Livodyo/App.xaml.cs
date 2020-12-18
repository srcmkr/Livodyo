/// <summary>
/// Pair programming session 2 (23.11.2020)
/// Authors: Deniz Ulu, Benjamin Bolzmann
/// BIS-268 Mobile Computing, WiSe 2020/21, Merz
/// </summary>

using Livodyo.State;
using Xamarin.Forms;

namespace Livodyo
{
    public partial class App : Application
    {
        public App()
        {
            var appState = new AppState();
            InitializeComponent();

            MainPage = new MainPage(appState);
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
