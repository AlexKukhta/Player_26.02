using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Player
{
    class Program
    {
        static void Main(string[] args)
        {
            PlayerActionsAsync();

            Console.ReadLine();
        }

        private static async void PlayerActionsAsync()
        {
            var filePath = "c:\\WavSongs\\songs.xml";
            var directory = "c:\\WavSongs";
            var skin = new ClassicSkin();
            var player = new Player<Song>(skin);

            player.PlayerLocked += OnPlayerLocked;
            player.PlayerUnlocked += OnPlayerUnlocked;
            player.PlayerStarted += OnPlayerStarted;
            player.PlayerStopped += OnPlayerStopped;
            player.SongStarted += OnSongStarted;
            player.SongsListChanged += OnSongsListChanged;
            player.VolumeChanged += OnVolumeChanged;

            if (File.Exists(filePath))
            {
                player.LoadPlaylist(filePath);
            }
            else
            {
                player.LoadFolder(directory);
                GenerateLikes(player.Playlist);
            }

            await player.Play();
            player.VolumeUp();

            await player.Play();
            player.Lock();

            await player.Play();
            player.UnLock();

            player.Stop();

            player.SaveAsPlaylist("c:\\WavSongs\\songs.xml");

            player.Dispose();
        }

        private static void OnSongStarted(object sender, EventArgs e)
        {
            if (sender is Player<Song> player)
            {
                VisualizeChanges(player);
            }
        }

        private static void OnVolumeChanged(object sender, EventArgs e)
        {
            if(sender is Player<Song> player)
            {
                VisualizeChanges(player);
            }
        }

        private static void OnSongsListChanged(object sender, EventArgs e)
        {
            if (sender is Player<Song> player)
            {
                VisualizeChanges(player);
            }
        }

        private static void OnPlayerStopped(object sender, EventArgs e)
        {
            if (sender is Player<Song> player)
            {
                VisualizeChanges(player);
            }
        }

        private static void OnPlayerStarted(object sender, EventArgs e)
        {
            if (sender is Player<Song> player)
            {
                VisualizeChanges(player);
            }
        }

        private static void OnPlayerUnlocked(object sender, EventArgs e)
        {
            if (sender is Player<Song> player)
            {
                VisualizeChanges(player);
            }
        }

        private static void OnPlayerLocked(object sender, EventArgs e)
        {
            if (sender is Player<Song> player)
            {
                VisualizeChanges(player);
            }
        }

        private static void VisualizeChanges(Player<Song> player)
        {
            player.NewScreen();
            Console.WriteLine($"Play is locked = {player.IsLocked}. Volume is {player.Volume}");
            ListSongs(player);
        }

        public static void ListSongs<T>(Player<T> player) where T : Song
        {
            var songs = player.Playlist;

            for (var i = 0; i < songs.Count; i++)
            {
                dynamic songData = GetSongData(songs[i]);
                
                TraceInfo(player, songData.title, songData.minutes, songData.seconds, songData.albumYear, songData.likeValue, 
                    songData.genre, player.CurrentItem == songs[i]);
            }
        }

        public static object GetSongData(Song song)
        {
            song.Deconstruct(out LikeEnum likeValue, out TimeSpan duration, out string title, out string artistName,
                out Genre genre, out byte[] thumbnale, out string albumName, out int albumYear);

            return new
            {
                title,
                minutes = duration.Minutes,
                seconds = duration.Seconds,
                albumYear,
                likeValue,
                genre
            };
        }

        public static List<Song> FilterByGenre(List<Song> songs, Genre genre)
        {
            return songs.OrderBy(s => s.Artist.Genre).ToList();
        }

        public static void TraceInfo<T>(Player<T> player, string title, int minutes, int seconds, int albumYear, 
            LikeEnum likeValue, Genre genre, bool isCurrentSong) where T : Song
        {
            var output = $"Title {title.ThreeDots(25)}" + Environment.NewLine +
                         $"Minutes {minutes}" + Environment.NewLine +
                         $"Seconds {seconds}" + Environment.NewLine +
                         $"AlbumYear {albumYear}" + Environment.NewLine +
                         $"Genre {genre}" + Environment.NewLine;

            if(isCurrentSong)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                player.Render(output);
            }
            else
            {
                switch (likeValue)
                {
                    case LikeEnum.Like:
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(output);
                        break;
                    case LikeEnum.Dislike:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(output);
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Gray;
                        player.Render(output);
                        break;
                }
            }            
        }
        
        public static void GenerateLikes(List<Song> songs)
        {
            for (var i = 0; i < songs.Count(); i++)
            {
                var modulo = i % 3;

                if (modulo == 0)
                {
                    songs[i].Like();
                }
                else if (modulo == 1)
                {
                    songs[i].Dislike();
                }
            }
        }
    }
}
