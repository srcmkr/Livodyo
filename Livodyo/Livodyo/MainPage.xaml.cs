using System;
using System.Linq;
using System.Threading.Tasks;
using Livodyo.Models;
using Livodyo.State;
using Xamarin.Forms;

namespace Livodyo
{
    public partial class MainPage
    {
        private readonly AppState _appState;

        public MainPage(AppState appState)
        {
            // add app state & make available for object
            _appState = appState;

            // create layout
            InitializeComponent();
            BuildLayout();
        }

        // Creates Frame for visual binding covers to tag category
        public Frame GetTagSection(TagModel tag)
        {
            var mainStackLayout = new StackLayout();
            mainStackLayout.Children.Add(new Label
            {
                Text = $"Das Beste aus \"{tag.Tag}\"",
                TextColor = Color.FromHex("#ebd132")
            });

            var topScroller = new ScrollView
            {
                FlowDirection = FlowDirection.LeftToRight,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Always,
                Orientation = ScrollOrientation.Horizontal
            };

            var topScrollerLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
            };

            if (_appState.AudioBooks != null)
            {
                foreach (var top10Audiobook in _appState.AudioBooks.Where(c => c.Tags.Contains(tag.Id)).Take(20))
                {
                    topScrollerLayout.Children.Add(GetLargeAudioBook(top10Audiobook, 80,80));
                }
            }
            topScroller.Content = topScrollerLayout;

            mainStackLayout.Children.Add(topScroller);

            // wrap main layout within a frame and returns it
            var frame = new Frame
            {
                HasShadow = true,
                CornerRadius = 8,
                BackgroundColor = Color.FromHex("191514"),
                Content = mainStackLayout,
                HeightRequest = 150,
                WidthRequest = Width + 10,
                MinimumWidthRequest = Width + 10
            };

            return frame;
        }

        public Frame GetLargeAudioBook(AudioBookModel audioBook, int height, int width)
        {
            var mainStackLayout = new StackLayout();
            // insert preview image
            var image = new Image
            {
                Source = ImageSource.FromUri(new Uri(audioBook.GetThumbnailUrl())),
                HeightRequest = height,
                WidthRequest = width,
                MinimumWidthRequest = height,
                Aspect = Aspect.AspectFill,
            };

            // add "onclick" (=single tap) event
            var tapRecognizer = new TapGestureRecognizer();
            tapRecognizer.Tapped += async (s, e) => { await OpenDetails(audioBook.Id); };

            // adds image plus title
            mainStackLayout.Children.Add(image);
            mainStackLayout.Children.Add(new Label
            {
                TextColor = Color.FromHex("ebd132"),
                Text = audioBook.Title,
                HorizontalTextAlignment = TextAlignment.Center,
                LineBreakMode = LineBreakMode.TailTruncation,
                HeightRequest = 50
            });

            // wrap stack layout in frame and returns it
            var frame = new Frame
            {
                HasShadow = true,
                CornerRadius = 8,
                BackgroundColor = Color.FromHex("1b1b1d"),
                Content = mainStackLayout,
                HeightRequest = height + 10,
                WidthRequest = width + 10,
                MinimumWidthRequest = width + 10,
                GestureRecognizers = { tapRecognizer }
            };

            return frame;
        }

        // click handler for audio book details
        private async Task OpenDetails(Guid audioBookId)
        {
            var newPage = new AudioBookDetails(_appState, audioBookId);
            await Navigation.PushModalAsync(newPage);
        }

        // outsourced function for constructor to build view layout
        public async void BuildLayout()
        {
            // get latest data from api
            await _appState.SynchronizeAsync();

            // complete page
            var mainLayout = new StackLayout
            {
                BackgroundColor = Color.FromHex("191514"),
            };

            // goto download library
            var libButton = new Button
            {
                Text = "Meine Bibliothek öffnen",
                BackgroundColor = Color.FromHex("#191514"),
                TextColor = Color.FromHex("#d8e8e7"),
                FontSize = 16
            };

            libButton.Clicked += async (x, y) =>
            {
                await Navigation.PushModalAsync(new LibraryPage(_appState));
            };

            // upper logo bar
            var logoLayout = new StackLayout
            {
                HeightRequest = 200,
                Orientation = StackOrientation.Vertical,
                BackgroundColor = Color.FromHex("1b191c"),
                Children =
                {
                    new Image
                    {
                        HeightRequest = 130,
                        Source = ImageSource.FromFile("logo.png"),
                        Aspect = Aspect.AspectFit,
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                    },
                    libButton
                }
            };
            mainLayout.Children.Add(logoLayout);

            // horizontal scrolling for the top 5 audio books below the logo
            var topScroller = new ScrollView
            {
                FlowDirection = FlowDirection.LeftToRight,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Always,
                Orientation = ScrollOrientation.Horizontal
            };

            var topScrollerLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
            };

            if (_appState.AudioBooks != null)
            {
                foreach (var top5Audiobook in _appState.AudioBooks.Take(5))
                {
                    topScrollerLayout.Children.Add(GetLargeAudioBook(top5Audiobook, 150,100));
                }
            }

            topScroller.Content = topScrollerLayout;
            mainLayout.Children.Add(topScroller);

            // adding for (randomized) three tags a list of max 10 audiobooks
            if (_appState.AudioBooks != null)
            {
                foreach (var randomTag in _appState.Tags)
                {
                    mainLayout.Children.Add(GetTagSection(randomTag));
                }
            }

            // wrap everything to scrollview and sets to view
            Content = new ScrollView
            {
                Content = mainLayout
            };
        }
    }
}
