using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Livodyo.State;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace Livodyo
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public class AudioBookDetails : ContentPage
    {
        private Guid AudioBookId { get; }
        private AppState AppState { get; }

        public AudioBookDetails(AppState appState, Guid audiobookId)
        {
            // make appstate & audiobookId available for this object
            AudioBookId = audiobookId;
            AppState = appState;

            // generate view
            BuildLayout();
        }

        // async download of audiobook
        private async Task<bool> DownloadAudioBook(Button button)
        {
            // get audiobook from state
            var currentAudioBook = AppState.AudioBooks.Single(c => c.Id == AudioBookId);

            // usage of YoutubeClient (external Nuget package)
            var youtube = new YoutubeClient();
            
            button.Text = "Lade Metadaten...";
            var streamManifest = await youtube.Videos.Streams.GetManifestAsync(currentAudioBook.YoutubeId);

            button.Text = "Lade MP3...";
            var streamInfo = streamManifest.GetAudioOnly().WithHighestBitrate();
            
            // only continue, if stream is available (author or youtube could have deleted this video)
            if (streamInfo != null)
            {
                button.Text = "Lade Stream...";
                await youtube.Videos.Streams.GetAsync(streamInfo);
                var dlFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"{AudioBookId}.mp3");
                
                button.Text = "Herunterladen...";
                await youtube.Videos.Streams.DownloadAsync(streamInfo, dlFile, new Progress<double>(p => button.Text = $"Download: {p:P1}"));
                button.Text = "Abgeschlossen";
                return true;
            }

            return false;
        }

        public void BuildLayout()
        {
            // get full audiobook entity
            var currentAudioBook = AppState.AudioBooks.Single(c => c.Id == AudioBookId);

            // add main layout
            var mainLayout = new StackLayout { BackgroundColor = Color.FromHex("191514") };
            
            // adding youtube preview
            var webView = new WebView
            {
                HeightRequest = 250,
                WidthRequest = 250,
                MinimumWidthRequest = 150,
                Source = $"https://www.youtube.com/embed/{currentAudioBook.YoutubeId}"
            };
            mainLayout.Children.Add(webView);

            // add meta data from api
            var detailsStackLayout = new StackLayout
            {
                Padding = new Thickness(20),
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            detailsStackLayout.Children.Add(
                new Label
                {
                    Text = currentAudioBook.Title,
                    TextColor = Color.FromHex("#ebd132"),
                    FontSize = 24,
                    HorizontalTextAlignment = TextAlignment.Start
                }
            );

            // adding download button
            var button = new Button
            {
                Text = "Download",
                WidthRequest = 250,
                MinimumWidthRequest = 250,
                BackgroundColor = Color.Red,
                TextColor = Color.White,
                HorizontalOptions = LayoutOptions.Center,
                CornerRadius = 6,
                Padding = new Thickness(6),
                Margin = new Thickness(0,25,0,0)
            };

            // adding invisible my library button
            var libButton = new Button
            {
                Text = "Zur Bibliothek",
                WidthRequest = 250,
                MinimumWidthRequest = 250,
                BackgroundColor = Color.Red,
                TextColor = Color.White,
                HorizontalOptions = LayoutOptions.Center,
                CornerRadius = 6,
                Padding = new Thickness(6),
                Margin = new Thickness(0,25,0,0),
            };

            // add download button action event
            button.Clicked += async (x, y) =>
            {
                button.Text = "Initialisierung...";
                button.IsEnabled = false;
                var wasSuccessful = await DownloadAudioBook(button);
                if (wasSuccessful)
                {
                    // switch visibility for download & library button
                    libButton.IsVisible = true;
                    button.IsVisible = false;
                    // insert currentAudioBooks into AppState and save to local storage
                    AppState.DownloadedAudioBooks.Add(currentAudioBook);
                    AppState.Save();
                } else
                {
                    // An (unknown) error occured
                    button.Text = "Fehler";
                    libButton.IsVisible = false;
                    button.IsVisible = true;
                }
            };

            // add library button action event
            libButton.Clicked += async (x, y) =>
            {
                await Navigation.PushModalAsync(new LibraryPage(AppState));
            };

            // lets see if we already have this audiobook locally
            if (AppState.DownloadedAudioBooks.Any(c => c.Id == AudioBookId))
            {
                libButton.IsVisible = true;
                button.IsVisible = false;
            } else
            {
                libButton.IsVisible = false;
                button.IsVisible = true;
            }

            // add both buttons to stacklayout
            detailsStackLayout.Children.Add(libButton);
            detailsStackLayout.Children.Add(button);

            // add tags and display meta data to details page
            var tags = new List<string>();
            foreach (var tag in currentAudioBook.Tags)
            {
                var foundTag = AppState.Tags.SingleOrDefault(c => c.Id == tag);
                if (foundTag != null) tags.Add(foundTag.Tag);
            }

            detailsStackLayout.Children.Add(new Label { TextColor = Color.FromHex("d8e8e7"), Text = $"Autor: {AppState.Authors.SingleOrDefault(c => c.Id == currentAudioBook.AuthorId)?.Name}" });
            detailsStackLayout.Children.Add(new Label { TextColor = Color.FromHex("d8e8e7"), Text = $"Erschienen: {currentAudioBook.PublicationDate.ToShortDateString()}"});
            detailsStackLayout.Children.Add(new Label { TextColor = Color.FromHex("d8e8e7"), Text = $"Kategorien: { string.Join(", ", tags) }"});

            // add meta data table to main layout
            mainLayout.Children.Add(detailsStackLayout);

            // put everything to content view
            Content = mainLayout;
        }
    }
}