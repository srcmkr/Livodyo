/// <summary>
/// Pair programming session 1 (12.11.2020)
/// Authors: Deniz Ulu, Benjamin Bolzmann
/// BIS-268 Mobile Computing, WiSe 2020/21, Merz
/// </summary>
/// 
namespace Livodyo.WebAdmin.Models
{
    public class MovielistViewModel
    {
        public List<AudioBookModel> AudioBooks { get; set; }
        public List<TagModel> Tags { get; set; }
        public List<AuthorModel> Authors { get; set; }

        public AudioBookModel NewAudioBook { get; set; }
        public TagModel NewTag { get; set; }
        public AuthorModel NewAuthor { get; set; }
    }
}
