using System;
using System.Linq;
using Livodyo.Models;
using Livodyo.State;
using Xamarin.Forms;

namespace Livodyo
{
    public partial class MainPage : ContentPage
    {
        private readonly AppState _appState;

        public MainPage(AppState appState)
        {
            _appState = appState;
            InitializeComponent();

            BuildLayout();
        }

        public Frame GetLargeAudioBook(AudioBookModel audioBook)
        {
            var mainStackLayout = new StackLayout();
            var image = new Image
            {
                Source = ImageSource.FromUri(new Uri(audioBook.GetThumbnailUrl())),
                HeightRequest = 100,
                WidthRequest = 100,
            };
            mainStackLayout.Children.Add(image);
            mainStackLayout.Children.Add(new Label
            {
                TextColor = Color.FromHex("ebd132"),
                Text = audioBook.Title,
                HorizontalTextAlignment = TextAlignment.Center,
                LineBreakMode = LineBreakMode.TailTruncation,
                HeightRequest = 50
            });

            var frame = new Frame
            {
                HasShadow = true,
                CornerRadius = 8,
                BackgroundColor = Color.FromHex("1b1b1d"),
                Content = mainStackLayout,
                HeightRequest = 110,
                WidthRequest = 110
            };
            
            return frame;
        }

        public async void BuildLayout()
        {
            await _appState.SynchronizeAsync();

            // complete page
            var mainLayout = new StackLayout
            {
                BackgroundColor = Color.FromHex("191514"),
            };

            // upper logo bar
            var logoLayout = new StackLayout
            {
                HeightRequest = 150,
                Orientation = StackOrientation.Vertical,
                BackgroundColor = Color.FromHex("1b191c"),
                Children =
                {
                    new Image
                    {
                        HeightRequest = 130,
                        Source = ImageSource.FromFile("livodyo-logo.png"),
                        Aspect = Aspect.AspectFit,
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                    }
                }
            };
            mainLayout.Children.Add(logoLayout);

            //var topScroller = new ScrollView();
            var topScrollerLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
            };

            foreach (var top3Audiobook in _appState.AudioBooks.Take(3))
            {
                topScrollerLayout.Children.Add(GetLargeAudioBook(top3Audiobook));
            }
            mainLayout.Children.Add(topScrollerLayout);

            Content = new ScrollView
            {
                Content = mainLayout
            };
        }
    }
}
