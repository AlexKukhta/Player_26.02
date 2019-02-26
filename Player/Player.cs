using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Player
{
    public class Player<T> : BasePlayer<T> where T : Song
    {
        private const string extension = ".wav";
        private bool _disposed;
        private SoundPlayer soundPlayer;
        private ISkin _skin;

        public Player(ISkin skin) : base()
        {
            soundPlayer = new SoundPlayer();
            _skin = skin;
        }

        public void Render(string text)
        {
            _skin.Render(text);
        }

        public void NewScreen()
        {
            _skin.Clear();
        }

        public override void LoadFolder(string directoryPath)
        {
            var files = Directory.GetFiles(directoryPath);

            foreach(var filePath in files)
            {
                var fileInfo = new FileInfo(filePath);

                if(string.Equals(extension, fileInfo.Extension))
                {
                    var name = fileInfo.Name;
                    var duration = new TimeSpan(fileInfo.Length);

                    var song = new Song(duration, name, filePath);

                    Add(song as T);
                }
            }
        }

        public override Task Play()
        {
            base.Play();

            return Task.Run(() =>
            {
                foreach (var item in Playlist)
                {
                    CurrentItem = item;
                    Play(item);
                }
            });            
        }

        protected override void Play(T item)
        {
            base.Play(item);

            soundPlayer.SoundLocation = item.FilePath;
            soundPlayer.PlaySync();
        }

        public override void Stop()
        {
            soundPlayer.Stop();

            base.Stop();
        }

        public override void SaveAsPlaylist(string filePath)
        {
            var formatter = new XmlSerializer(typeof(List<Song>));
            var fs = new FileStream(filePath, FileMode.Create);

            formatter.Serialize(fs, Playlist);
            fs.Close();
        }

        public override void LoadPlaylist(string filePath)
        {
            var formatter = new XmlSerializer(typeof(List<T>));
            var fs = new FileStream(filePath, FileMode.Open);
            var deserializedItems = (List<T>) formatter.Deserialize(fs);

            foreach(var item in deserializedItems)
            {
                Add(item);
            }

            fs.Close();
        }

        public override void Dispose()
        {
            DisposingAlgorith();
            GC.SuppressFinalize(this);
        }

        protected override void DisposingAlgorith()
        {
            if (!_disposed)
            {
                _skin = null;
                soundPlayer = null;
                _disposed = true;
            }

            base.DisposingAlgorith();
        }

        ~Player()
        {
            DisposingAlgorith();
        }
    }
}
