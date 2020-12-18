/// <summary>
/// Pair programming session 3 (16.12.2020)
/// Authors: Deniz Ulu, Benjamin Bolzmann
/// BIS-268 Mobile Computing, WiSe 2020/21, Merz
/// </summary>

using Livodyo.Models;
using Livodyo.State;
using MediaManager;
using System;
using System.IO;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Livodyo
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LibraryPage : ContentPage
    {
        private AppState AppState { get; }

        public LibraryPage(AppState appState)
        {
            AppState = appState;
            InitializeComponent();

            // generate view
            BuildLayout();
        }

        public void BuildLayout()
        {
            var stackLayout = new StackLayout
            {
                BackgroundColor = Color.FromHex("1b1b1d"),
                Padding = new Thickness(5)
            };

            // add livodyo logo
            stackLayout.Children.Add(
                new Image
                {
                    HeightRequest = 130,
                    Source = ImageSource.FromFile("logo.png"),
                    Aspect = Aspect.AspectFit,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.Fill,
                }
            );

            // add upper label
            stackLayout.Children.Add(new Label
            {
                Text = "Ihre heruntergeladenen Hörbücher",
                FontSize = 18,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = Color.FromHex("#ebd132"),
                Padding = 5,
            });

            var scrollView = new ScrollView
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            var scrollStackLayout = new StackLayout();

            // list all downloaded audiobooks from litedb
            foreach (var audioBooks in AppState.DownloadedAudioBooks)
            {
                scrollStackLayout.Children.Add(GetAudioBookEntryView(audioBooks));
            }

            scrollView.Content = scrollStackLayout;

            stackLayout.Children.Add(scrollView);

            Content = stackLayout;
        }

        public Frame GetAudioBookEntryView(AudioBookModel audioBook)
        {

            var innerStackLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            // the play button
            var playButton = new Button
            {
                Margin = new Thickness(0, 0, 10, 0),
                Text = "▶",
                BackgroundColor = Color.Red,
                TextColor = Color.White,
                IsVisible = true
            };

            playButton.Clicked += async (x, y) =>
            {
                var dlFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"{audioBook.Id}.mp3");
                if (playButton.Text == "▶" || playButton.Text == "⏯")
                {
                    await CrossMediaManager.Current.Play(dlFile);
                    playButton.Text = "⏸";
                }
                else
                {
                    await CrossMediaManager.Current.Pause();
                    playButton.Text = "⏯";
                }

            };

            innerStackLayout.Children.Add(playButton);

            // the audiobook title
            innerStackLayout.Children.Add(new Label
            {
                Text = audioBook.Title,
                TextColor = Color.FromHex("#ebd132"),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalTextAlignment = TextAlignment.Center
            });

            var author = AppState.Authors?.SingleOrDefault(c => c.Id == audioBook.AuthorId);
            var authorName = (author == null) ? "" : author.Name;
            // the audiobook author
            innerStackLayout.Children.Add(new Label
            {
                Text = authorName,
                TextColor = Color.FromHex("#ebd132"),
                HorizontalTextAlignment = TextAlignment.End,
                VerticalTextAlignment = TextAlignment.Center
            });

            // return beautiful frame with innerStackLayout for audiobook
            return new Frame
            {
                Content = innerStackLayout,
                HasShadow = true,
                BackgroundColor = Color.FromHex("1b191c"),
                CornerRadius = 5,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
        }
    }
}