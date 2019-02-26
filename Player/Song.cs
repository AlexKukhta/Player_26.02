using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Player
{
    [Serializable]
    public class Song : PlayingItem
    {
        public LikeEnum LikeValue { get; set; }        
        public Artist Artist { get; set; }
        public Album Album { get; set; }

        public Song() : base()
        {
            LikeValue = LikeEnum.NoneLike;
            Artist = new Artist();
            Album = new Album();
        }

        public Song(TimeSpan duration, string title, string filePath) : base(duration, title, filePath)
        {
            LikeValue = LikeEnum.NoneLike;
            Artist = new Artist();
            Album = new Album();
        }

        public Song(TimeSpan duration, string title, Artist artist, Album album, string filePath) : this(duration, title, filePath)
        {
            Artist = artist;
            Album = album;
        }

        protected Song(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            LikeValue = (LikeEnum) info.GetValue("LikeValue", typeof(LikeEnum));
            Artist = (Artist) info.GetValue("Artist", typeof(Artist));
            Album = (Album) info.GetValue("Album", typeof(Album));
        }

        public void Like()
        {
            LikeValue = LikeEnum.Like;
        }

        public void Dislike()
        {
            LikeValue = LikeEnum.Dislike;
        }

        public void Deconstruct(out LikeEnum likeValue, out TimeSpan duration, out string title, out string artistName,
            out Genre genre, out byte[] thumbnale, out string albumName, out int albumYear)
        {
            likeValue = LikeValue;
            duration = Duration;
            title = Title;
            artistName = string.Empty;
            genre = Artist.Genre;
            thumbnale = new byte[0];
            albumName = string.Empty;
            albumYear = Album.Year;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("LikeValue", LikeValue);
            info.AddValue("Artist", Artist);
            info.AddValue("Album", Album);

            base.GetObjectData(info, context);
        }
    }
}
