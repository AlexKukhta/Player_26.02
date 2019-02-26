using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Player
{
    public static class SongsExtension
    {
        public static List<Song> SortByGenre(this List<Song> songs, Genre genre)
        {
            var newList = new List<Song>();

            foreach(var item in songs)
            {
                if (item.Artist.Genre == genre)
                {
                    newList.Add(item);
                }
            }

            foreach (var item in songs)
            {
                if (item.Artist.Genre != genre)
                {
                    newList.Add(item);
                }
            }

            return newList;
        }
    }
}
