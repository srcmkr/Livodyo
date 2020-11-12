using System.Net.Http;
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
