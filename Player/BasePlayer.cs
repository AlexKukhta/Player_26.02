using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Player
{
    public abstract class BasePlayer<T> : IDisposable where T : PlayingItem
    {
        private int _volume;
        private bool _disposed;
        const int MIN_VOLUME = 0;
        const int MAX_VOLUME = 100;

        public List<T> Playlist { get; }
        public T CurrentItem { get; protected set; }
        public bool IsLocked { get; private set; }

        public event EventHandler PlayerStarted;
        public event EventHandler PlayerStopped;
        public event EventHandler SongStarted;
        public event EventHandler SongsListChanged;
        public event EventHandler VolumeChanged;
        public event EventHandler PlayerLocked;
        public event EventHandler PlayerUnlocked;

        protected BasePlayer()
        {
            Playlist = new List<T>();
        }

        public int Volume
        {
            get { return _volume; }
            set
            {
                if (value < MIN_VOLUME)
                {
                    _volume = MIN_VOLUME;
                }
                else if (value > MAX_VOLUME)
                {
                    _volume = MAX_VOLUME;
                }
                else
                {
                    value = _volume;
                }

                VolumeChanged?.Invoke(this, null);
            }
        }

        public void VolumeUp()
        {
            _volume++;
        }

        public void VolumeDown()
        {
            _volume--;
        }

        public void VolumeChange(int step)
        {
            _volume += step;
        }

        public void Add(T item)
        {
            Playlist.Add(item);

            SongsListChanged?.Invoke(this, null);
        }

        public void Remove(T item)
        {
            Playlist.Remove(item);

            SongsListChanged?.Invoke(this, null);
        }

        public void ClearPlayList()
        {
            Playlist.Clear();

            SongsListChanged?.Invoke(this, null);
        }

        public virtual Task Play()
        {
            PlayerStarted?.Invoke(this, null);

            return new Task(() => { });
        }

        protected virtual void Play(T item)
        {
            SongStarted?.Invoke(this, null);
        }

        public virtual void Stop()
        {
            PlayerStopped?.Invoke(this, null);
        }

        public abstract void LoadFolder(string directoryPath);

        public abstract void SaveAsPlaylist(string filePath);

        public abstract void LoadPlaylist(string filePath);

        public virtual void Lock()
        {
            IsLocked = true;

            PlayerLocked?.Invoke(this, null);
        }

        public virtual void UnLock()
        {
            IsLocked = false;

            PlayerUnlocked?.Invoke(this, null);
        }

        public virtual void Dispose()
        {
            DisposingAlgorith();
            GC.SuppressFinalize(this);
        }

        protected virtual void DisposingAlgorith()
        {
            if (!_disposed)
            {
                Playlist.Clear();
                _disposed = true;
            }
        }

        ~BasePlayer()
        {
            DisposingAlgorith();
        }
    }
}
