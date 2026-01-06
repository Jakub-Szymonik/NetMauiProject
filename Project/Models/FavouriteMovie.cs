using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models
{
    // stores favourite movies and when it was added
    public class FavouriteMovie
    {
        public string Title { get; set; } = "";
        public int Year { get; set; }
        public List<string> Genre { get; set; } = new List<string>();
        public string Emoji { get; set; } = "";
        public DateTime AddedDate { get; set; }

        public string GenreString => string.Join(", ", Genre);
    }
}
