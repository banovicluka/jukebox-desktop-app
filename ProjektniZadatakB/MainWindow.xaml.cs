using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ProjektniZadatakB
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class Singer
    {
        public String name;
        public String surname;

        public Singer(String name, String surname)
        {
            this.name = name;
            this.surname = surname;
        }

        public override String ToString()
        {
            return name + " " + surname;
        }


    }
    public partial class Song
    {
        public String name;
        public Singer singer;
        public bool favourite;

        public string artist
        {
            get { return singer.ToString(); }
        }

        public Song(String name, Singer singer)
        {
            this.name = name;
            this.singer = singer;
            favourite = false;
        }

        public static List<Song> GetSongs(String path)
        {
            List<Song> listOfSongs = new List<Song>();
            string[] music = Directory.GetFiles(path, "*.mp3", SearchOption.TopDirectoryOnly);
            foreach (string m in music)
            {
                FileInfo fi = new FileInfo(m);
                string[] strings = fi.Name.Split(" - ");
                string[] artist = strings[0].Split(" ");
                string artistName = artist[0];
                string artistSurname = "";
                for(int i = 1; i < artist.Length; i++)
                {
                    artistSurname += artist[i];
                    artistSurname += " ";
                }
                listOfSongs.Add(new Song(strings[1], new Singer(artistName, artistSurname)));
            }
            return listOfSongs;
        }

        public static List<Song> GetFavouriteSongs(List<Song> songs)
        {
            List<Song> favouriteSongs = new List<Song>();
            foreach(Song song in songs)
            {
                if(song.favourite)
                    favouriteSongs.Add(song);
            }
            return favouriteSongs;
        }


        public override String ToString()
        {
            return this.singer.name + " " + this.singer.surname + " - " + this.name;
        }
    }
    public partial class MainWindow : Window
    {
        public static List<Song> songs = new List<Song>();
        public static Queue<Song> chosenSongs = new Queue<Song>();
        public static List<Song> favouriteSongs = new List<Song>();
        public static String path = @"C:\Users\WIN10\source\repos\ProjektniZadatakB\ProjektniZadatakB\music";
        private bool startButtonClicked = false;
        private string[] music = Directory.GetFiles(path, "*.mp3", SearchOption.TopDirectoryOnly);
        private bool pause = false;
        private readonly DispatcherTimer timer;
        public MainWindow()
        {
            InitializeComponent();

            //Console.WriteLine(Directory.GetCurrentDirectory());
            //List<Song> songsList = new List<Song>();
            //String directory = System.AppDomain.CurrentDomain.BaseDirectory;
            /*songsList.Add(new Song("Jutro donosi kraj", new Singer("Vesna", "Pisarevic")));
            songsList.Add(new Song("Dobro jutro dzezeri", new Singer("Momcilo", "Bajagic Bajaga")));
            songsList.Add(new Song("Jutro donosi kraj", new Singer("Vesna", "Pisarevic")));
            songsList.Add(new Song("Dobro jutro dzezeri", new Singer("Momcilo", "Bajagic Bajaga"))); songsList.Add(new Song("Jutro donosi kraj", new Singer("Vesna", "Pisarevic")));
            songsList.Add(new Song("Dobro jutro dzezeri", new Singer("Momcilo", "Bajagic Bajaga"))); songsList.Add(new Song("Jutro donosi kraj", new Singer("Vesna", "Pisarevic")));
            songsList.Add(new Song("Dobro jutro dzezeri", new Singer("Momcilo", "Bajagic Bajaga"))); songsList.Add(new Song("Jutro donosi kraj", new Singer("Vesna", "Pisarevic")));
            songsList.Add(new Song("Dobro jutro dzezeri", new Singer("Momcilo", "Bajagic Bajaga"))); songsList.Add(new Song("Jutro donosi kraj", new Singer("Vesna", "Pisarevic")));
            songsList.Add(new Song("Dobro jutro dzezeri", new Singer("Momcilo", "Bajagic Bajaga"))); songsList.Add(new Song("Jutro donosi kraj", new Singer("Vesna", "Pisarevic")));
            songsList.Add(new Song("Dobro jutro dzezeri", new Singer("Momcilo", "Bajagic Bajaga"))); songsList.Add(new Song("Jutro donosi kraj", new Singer("Vesna", "Pisarevic")));
            songsList.Add(new Song("Dobro jutro dzezeri", new Singer("Momcilo", "Bajagic Bajaga"))); songsList.Add(new Song("Jutro donosi kraj", new Singer("Vesna", "Pisarevic")));
            songsList.Add(new Song("Dobro jutro dzezeri", new Singer("Momcilo", "Bajagic Bajaga")));
            */
            //StackPanel panel = new StackPanel();
            //ListViewItem lvi = new ListViewItem();
            //Image image = new Image();
            //image.Source.SetValue(UidProperty, "C:\\Users\\WIN10\\source\\repos\\ProjektniZadatakB\\ProjektniZadatakB\\buttons\\heart.png");
            //BitmapImage heartImage = new BitmapImage(new Uri("C:\\Users\\WIN10\\source\\repos\\ProjektniZadatakB\\ProjektniZadatakB\\buttons\\heart.png"));

            //Image image = stack.Children.OfType<Image>().FirstOrDefault();
            //stack.Orientation = Orientation.Horizontal;
            //stack.Children.Add(image);
            //stack.Children.Add(tb);
            //listOfSongs.ItemsSource = Song.GetSongs(path);
            MediaPlayer.Volume = volumeSlider.Value;
            songs = Song.GetSongs(path);
            favouriteSongs = Song.GetFavouriteSongs(songs);
            drawList(songs, listOfSongs);
            drawList(favouriteSongs, listOfFavoriteSongs);

            timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 1)
            };
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Start();
            
            
            
            
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (MediaPlayer.NaturalDuration.HasTimeSpan)
            {
                time.Content = MediaPlayer.Position.TotalMinutes.ToString("00") + ":" + MediaPlayer.Position.Seconds.ToString("00") + "/"
                    + MediaPlayer.NaturalDuration.TimeSpan.TotalMinutes.ToString("00") + ":" + MediaPlayer.NaturalDuration.TimeSpan.Seconds.ToString("00");
            }
            else
            {
                time.Content = "00:00/00:00";
            }
        }

        public void drawList(List<Song> songs,ListView listOfSongs)
        {
            Image image = new Image();


            //songs.ElementAt(0).favourite = true;
            //heartImage.Visibility = Visibility.Visible;
            foreach (Song i in songs)
            {
                StackPanel stack = new StackPanel();
                stack.Orientation = Orientation.Horizontal;
                if (i.favourite) 
                {
                    Image heartImage = new Image { Source = new BitmapImage(new Uri(@"C:\\Users\\WIN10\\source\\repos\\ProjektniZadatakB\\ProjektniZadatakB\\buttons\\heart2.png", UriKind.Absolute)), Height = 20, Width = 20 };
                    heartImage.Stretch = Stretch.Uniform;
                    stack.Children.Add(heartImage);
                }
                TextBlock tb = new TextBlock();
                //tb.Visibility = Visibility.Visible;
                tb.Text = i.ToString();
                tb.FontSize = 12;
                tb.FontWeight = FontWeights.Bold;
                //tb.Background = new SolidColorBrush(Colors.LightBlue);
                tb.Foreground = new SolidColorBrush(Colors.White);
                //stack.Background = new SolidColorBrush(Colors.LightBlue);
                stack.Children.Add(tb);
                ListViewItem lvi = new ListViewItem();
                //Color color = ColorTranslator.FromHtml("#73bf44");
                lvi.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri(@"C:\\Users\\WIN10\\source\\repos\\ProjektniZadatakB\\ProjektniZadatakB\\buttons\\list-item.png", UriKind.Absolute)) };
                //lvi.BorderBrush = new SolidColorBrush(Colors.Black);
                //lvi.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri(@"C:\\Users\\WIN10\\source\\repos\\ProjektniZadatakB\\ProjektniZadatakB\\buttons\\guitar.jpg", UriKind.Relative)) };
                lvi.Content = stack;
                lvi.Height = 50;
                listOfSongs.Items.Add(lvi);
            }
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            //omoguciti izbor iz omiljenih pjesama
            
            if(allSongsMenu.IsSelected && listOfSongs.SelectedItem != null)
            {
                /* ListViewItem lvi =(ListViewItem)listOfSongs.SelectedItems[0];
                 StackPanel stack = (StackPanel) lvi.Content;
                 stack.
                 listOfChosenSongs.Items.Add(lvi);*/
                int row = listOfSongs.SelectedIndex;
                chosenSongs.Enqueue(songs[row]);
                listOfChosenSongs.Items.Clear();
                //listOfChosenSongs.Items.Add(songs[row]);
                foreach(Song i in chosenSongs)
                {
                    StackPanel stack = new StackPanel();
                    stack.Orientation = Orientation.Horizontal;
                    
                    TextBlock tb = new TextBlock();
                    //tb.Visibility = Visibility.Visible;
                    tb.Text = i.ToString();
                    tb.FontSize = 12;
                    tb.FontWeight = FontWeights.Bold;
                    //tb.Background = new SolidColorBrush(Colors.LightBlue);
                    tb.Foreground = new SolidColorBrush(Colors.White);
                    //stack.Background = new SolidColorBrush(Colors.LightBlue);
                    stack.Children.Add(tb);
                    ListViewItem lvi = new ListViewItem();
                    //Color color = ColorTranslator.FromHtml("#73bf44");
                    lvi.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri(@"C:\\Users\\WIN10\\source\\repos\\ProjektniZadatakB\\ProjektniZadatakB\\buttons\\list-item.png", UriKind.Absolute)) };
                    //lvi.BorderBrush = new SolidColorBrush(Colors.Black);
                    //lvi.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri(@"C:\\Users\\WIN10\\source\\repos\\ProjektniZadatakB\\ProjektniZadatakB\\buttons\\guitar.jpg", UriKind.Relative)) };
                    lvi.Content = stack;
                    lvi.Height = 50;
                    listOfChosenSongs.Items.Add(lvi);

                }
                
            }
            else if (favouriteSongsMenu.IsSelected && listOfFavoriteSongs.SelectedItem != null)
            {
                int row = listOfFavoriteSongs.SelectedIndex;
                chosenSongs.Enqueue(favouriteSongs[row]);
                listOfChosenSongs.Items.Clear();
                //listOfChosenSongs.Items.Add(favouriteSongs[row]);
                foreach (Song i in chosenSongs)
                {
                    StackPanel stack = new StackPanel();
                    stack.Orientation = Orientation.Horizontal;
                    
                    TextBlock tb = new TextBlock();
                    //tb.Visibility = Visibility.Visible;
                    tb.Text = i.ToString();
                    tb.FontSize = 12;
                    tb.FontWeight = FontWeights.Bold;
                    //tb.Background = new SolidColorBrush(Colors.LightBlue);
                    tb.Foreground = new SolidColorBrush(Colors.White);
                    //stack.Background = new SolidColorBrush(Colors.LightBlue);
                    stack.Children.Add(tb);
                    ListViewItem lvi = new ListViewItem();
                    //Color color = ColorTranslator.FromHtml("#73bf44");
                    lvi.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri(@"C:\\Users\\WIN10\\source\\repos\\ProjektniZadatakB\\ProjektniZadatakB\\buttons\\list-item.png", UriKind.Absolute)) };
                    //lvi.BorderBrush = new SolidColorBrush(Colors.Black);
                    //lvi.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri(@"C:\\Users\\WIN10\\source\\repos\\ProjektniZadatakB\\ProjektniZadatakB\\buttons\\guitar.jpg", UriKind.Relative)) };
                    lvi.Content = stack;
                    lvi.Height = 50;
                    listOfChosenSongs.Items.Add(lvi);
                }
            }
        }

       /* private void listOfSongs_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listOfSongs.SelectedItem != null)
            {
                int row = listOfSongs.SelectedIndex;
                chosenSongs.Enqueue(songs[row]);
                listOfChosenSongs.Items.Add(songs[row]);

                //ItemCollection itms = listOfChosenSongs.Items;
               // itms.GetItemAt(0);
                //itms.
                
            }
        }*/

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (chosenSongs.Count > 0 && startButtonClicked==false) {
                //string[] music = Directory.GetFiles(path, "*.mp3", SearchOption.TopDirectoryOnly);
                foreach (string s in music)
                {
                    if (s.Contains(chosenSongs.First().name) && s.Contains(chosenSongs.First().singer.ToString()))
                    {
                        Song song = chosenSongs.Dequeue();
                        songName.Content = song.name;
                        artistName.Content = song.singer;
                        if (song.favourite)
                            likeImage.Source = new BitmapImage(new Uri(@"C:\Users\WIN10\source\repos\ProjektniZadatakB\ProjektniZadatakB\buttons\heart2.png"));
                        else
                            likeImage.Source = new BitmapImage(new Uri(@"C:\Users\WIN10\source\repos\ProjektniZadatakB\ProjektniZadatakB\buttons\unliked.png"));
                        //odradi logiku za prikaz punog i praznog srca prilikom pustanja pjesme
                        listOfChosenSongs.Items.RemoveAt(0);
                        MediaPlayer.Source = new Uri(s);
                        MediaPlayer.Play();
                        startButtonClicked = true;
                        //promjeni sliku na dugmetu
                        playImage.Source = new BitmapImage(new Uri(@"C:\\Users\\WIN10\\source\\repos\\ProjektniZadatakB\\ProjektniZadatakB\\buttons\\pause-button.png"));
                        playImage.Width = 70;
                        playImage.Height = 54;
                        playImage.Stretch = Stretch.Fill;
                        pause = false;
                        break;
                    }
                }
            }else if (startButtonClicked)
            {
                songName.Content = "";
                artistName.Content = "";
                chosenSongs.Clear();
                listOfChosenSongs.Items.Clear();
                MediaPlayer.Stop();
                MediaPlayer.Source = null;
                startButtonClicked = false;
                time.Content = "00:00/00:00";
                //promjeni sliku na dugmetu
                playImage.Source = new BitmapImage(new Uri(@"C:\\Users\\WIN10\\source\\repos\\ProjektniZadatakB\\ProjektniZadatakB\\buttons\\play.png"));
                playImage.Width = 70;
                playImage.Height = 54;
                playImage.Stretch = Stretch.Fill;
                pause = true;

            }

        }

        private void songEnded(object sender, RoutedEventArgs e)
        {
            if (chosenSongs.Count > 0)
            {
                foreach (string s in music)
                {
                    if (s.Contains(chosenSongs.First().name) && s.Contains(chosenSongs.First().singer.ToString()))
                    {
                        Song song = chosenSongs.Dequeue();
                        if (song.favourite)
                            likeImage.Source = new BitmapImage(new Uri(@"C:\Users\WIN10\source\repos\ProjektniZadatakB\ProjektniZadatakB\buttons\heart2.png"));
                        else
                            likeImage.Source = new BitmapImage(new Uri(@"C:\Users\WIN10\source\repos\ProjektniZadatakB\ProjektniZadatakB\buttons\unliked.png"));
                        songName.Content = song.name;
                        artistName.Content = song.singer; ;
                        listOfChosenSongs.Items.RemoveAt(0);
                        MediaPlayer.Source = new Uri(s);
                        MediaPlayer.Play();
                        if (pause == true)
                        {
                            playImage.Source = new BitmapImage(new Uri(@"C:\\Users\\WIN10\\source\\repos\\ProjektniZadatakB\\ProjektniZadatakB\\buttons\\pause-button.png"));
                            playImage.Width = 70;
                            playImage.Height = 54;
                            playImage.Stretch = Stretch.Fill;
                            pause = false;
                        }
                        break;
                    }
                }
            }else
            {
                Random random = new Random();
                int number = random.Next(songs.Count);
                Song song = songs.ElementAt(number);
                if(song.favourite)
                    likeImage.Source = new BitmapImage(new Uri(@"C:\Users\WIN10\source\repos\ProjektniZadatakB\ProjektniZadatakB\buttons\heart2.png"));
                else
                    likeImage.Source = new BitmapImage(new Uri(@"C:\Users\WIN10\source\repos\ProjektniZadatakB\ProjektniZadatakB\buttons\unliked.png"));
                songName.Content = song.name;
                artistName.Content = song.singer;
                string path = music.ElementAt(number);
                MediaPlayer.Source = new Uri(path);
                MediaPlayer.Play();
                if (pause == true)
                {
                    playImage.Source = new BitmapImage(new Uri(@"C:\\Users\\WIN10\\source\\repos\\ProjektniZadatakB\\ProjektniZadatakB\\buttons\\pause-button.png"));
                    playImage.Width = 70;
                    playImage.Height = 54;
                    playImage.Stretch = Stretch.Fill;
                    pause = false;
                }
            }
        }

        private void playButton_Click(object sender, RoutedEventArgs e)
        {
            if (startButtonClicked)
            {
                if (pause)
                {
                    MediaPlayer.Play();
                    pause = false;
                    playImage.Source = new BitmapImage(new Uri(@"C:\\Users\\WIN10\\source\\repos\\ProjektniZadatakB\\ProjektniZadatakB\\buttons\\pause-button.png"));
                }
                else
                {
                    MediaPlayer.Pause();
                    pause = true;
                    playImage.Source = new BitmapImage(new Uri(@"C:\\Users\\WIN10\\source\\repos\\ProjektniZadatakB\\ProjektniZadatakB\\buttons\\play.png"));
                }
            }
           
        }

        private void volumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MediaPlayer.Volume = volumeSlider.Value;
        }

        private void likeButton_Click(object sender, RoutedEventArgs e)
        {
            if (songName.Content != null && songName.Content.ToString() != "")
            {
                foreach (Song song in songs)
                {
                    if (song.name.Contains(songName.Content.ToString()))
                    {
                        if (song.favourite)
                        {
                            //promjena buttona
                            likeImage.Source = new BitmapImage(new Uri(@"C:\Users\WIN10\source\repos\ProjektniZadatakB\ProjektniZadatakB\buttons\unliked.png"));
                            song.favourite = false;
                        }
                        else
                        {
                            //promjena buttona
                            likeImage.Source = new BitmapImage(new Uri(@"C:\Users\WIN10\source\repos\ProjektniZadatakB\ProjektniZadatakB\buttons\heart2.png"));
                            song.favourite = true;
                        }
                        break;
                    }
                }
                listOfSongs.Items.Clear();
                listOfFavoriteSongs.Items.Clear();
                drawList(songs, listOfSongs);
                favouriteSongs = Song.GetFavouriteSongs(songs);
                drawList(favouriteSongs, listOfFavoriteSongs);

            }
        }

        private bool muted = false;

        private void muteButton_Click(object sender, RoutedEventArgs e)
        {
            volumeSlider.Value = 0;
            MediaPlayer.Volume = 0;
        }
    }
}
