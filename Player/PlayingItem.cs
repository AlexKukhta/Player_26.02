using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Player
{
    public abstract class PlayingItem : ISerializable
    {
        public TimeSpan Duration { get; set; }
        public string Title { get;  set; }
        public string FilePath { get; set; }

        public PlayingItem()
        {
            Duration = new TimeSpan();
            Title = string.Empty;
            FilePath = string.Empty;
        }

        public PlayingItem(TimeSpan duration, string title, string filePath)
        {
            Duration = duration;
            Title = title;
            FilePath = filePath;
        }

        protected PlayingItem(SerializationInfo info, StreamingContext context)
        {
            Duration = (TimeSpan)info.GetValue("Duration", typeof(TimeSpan));
            Title = (string)info.GetValue("Title", typeof(string));
            FilePath = (string)info.GetValue("FilePath", typeof(string));
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Duration", Duration);
            info.AddValue("Title", Title);
            info.AddValue("FilePath", FilePath);
        }
    }
}
