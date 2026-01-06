using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Project.Models
{
    public class Movie
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } = "";

        [JsonPropertyName("year")]
        public int Year { get; set; }

        [JsonPropertyName("genre")]
        public List<string> Genre { get; set; } = new List<string>();

        [JsonPropertyName("director")]
        public string Director { get; set; } = "";

        [JsonPropertyName("imdbRating")]
        public double ImdbRating { get; set; }

        [JsonPropertyName("emoji")]
        public string Emoji { get; set; } = "";

        //get genres as a string
        public string GenreString => string.Join(", ", Genre);
    }
}
