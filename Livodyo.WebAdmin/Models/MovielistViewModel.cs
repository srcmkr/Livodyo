using System.Collections.Generic;
using Livodyo.Models;

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
