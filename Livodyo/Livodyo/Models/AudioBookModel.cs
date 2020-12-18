/// <summary>
/// Pair programming session 2 (23.11.2020)
/// Authors: Deniz Ulu, Benjamin Bolzmann
/// BIS-268 Mobile Computing, WiSe 2020/21, Merz
/// </summary>

using System;
using System.Collections.Generic;

namespace Livodyo.Models
{
    public class AudioBookModel
    {
        // Primary key
        public Guid Id { get; set; }

        // Audiobook data
        public string Title { get; set; }
        public DateTime PublicationDate { get; set; }
        public string Language { get; set; }
        public string AmazonLink { get; set; }
        public string YoutubeId { get; set; }

        // Relations
        public List<Guid> Tags { get; set; }
        public Guid AuthorId { get; set; }

        // Helper functions
        public string GetThumbnailUrl()
        {
            YoutubeId = YoutubeId.Replace("#", "");
            YoutubeId = YoutubeId.Replace("/", "");
            return $"https://img.youtube.com/vi/{YoutubeId}/0.jpg";
        }
    }
}
