using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models
{
    // holds all the users data
    public class UserData
    {
        public string UserName { get; set; } = "";
        public List<FavouriteMovie> Favourites { get; set; } = new List<FavouriteMovie>();
        public List<ViewedMovie> ViewHistory { get; set; } = new List<ViewedMovie>();
    }
}
