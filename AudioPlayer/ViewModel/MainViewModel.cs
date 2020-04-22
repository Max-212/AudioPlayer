using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AudioPlayer.Model;
using AudioPlayer.View;
using Microsoft.VisualStudio.PlatformUI;
using System.Windows;
using Microsoft.Win32;

using VisioForge.Tools.TagLib.Mpeg;
using VisioForge.Tools.TagLib;
using System.Windows.Media;
using System.Threading;
using System.Windows.Threading;

namespace AudioPlayer.ViewModel
{
    public class MainViewModel : BaseModel
    {

        // Singleton
        
        private static MainViewModel instance;

        public static MainViewModel getInstance()
        {
            if (instance == null)
                instance = new MainViewModel();
            return instance;
        }

        DispatcherTimer timer;
        private MediaPlayer mediaPlayer = new MediaPlayer();
        private bool isRepeat = false;
        private bool isPause = false;

        private string currentTimeText;
        private string durationText;
        private double totalDuration;
        private Audio selectedAudio;
        private PlayList selectedPlayList = new PlayList();

        public ObservableCollection<PlayList> PlayLists { get; set; }
        public PlayList SelectedPlayList { get => selectedPlayList; set { selectedPlayList = value; OnPropertyChanged(); } }
        public Audio SelectedAudio { get => selectedAudio; set { selectedAudio = value; OnPropertyChanged(); Refresh(); } }

        public double Volume { get => mediaPlayer.Volume * 100; set { mediaPlayer.Volume = value / 100; OnPropertyChanged(); } } 
        public double TotalDuration { get => totalDuration; set { totalDuration = value; OnPropertyChanged(); } }
        public double SliderValue { get => mediaPlayer.Position.TotalSeconds; set { mediaPlayer.Position = TimeSpan.FromSeconds(value); ; OnPropertyChanged(); } }
        public string CurrentTimeText { get => currentTimeText; set { currentTimeText = value; OnPropertyChanged(); } }
        public string DurationText { get => durationText; set { durationText = value; OnPropertyChanged(); } }
        
        public bool IsPause { get => isPause; set { isPause = value; OnPropertyChanged(); } }
        public bool IsRepeat { get => isRepeat; set { isRepeat = value; OnPropertyChanged(); } }

        public List<string> Sorting { get; set; }
        public string SortedBy { get; set; }
      

        public ICommand AddPlayList
        {
            get
            {
                return new DelegateCommand( (obj) =>
                {
                    
                    string name = AddPlayListWindow.Show("Enter playlist name", obj as Window);
                    if(!name.Equals(string.Empty))
                    {
                        PlayLists.Add(new PlayList() { Name = name });
                        SelectedPlayList = PlayLists.Last();
                    }

                });
            }
        }
     
        public ICommand DeletePlayList
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if(!SelectedPlayList.Equals(PlayLists[0]))
                    {
                        PlayLists.Remove(SelectedPlayList);
                        SelectedPlayList = null;
                        SelectedAudio = null;
                    }

                    SelectedPlayList = PlayLists[0];
                });
            }
        }

        public ICommand DeleteAudio
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    Audio deletedAudio = SelectedAudio; 
                    if (PlayLists.IndexOf(SelectedPlayList) == 0)
                    {
                       for(int i = 0; i < PlayLists.Count; i++)
                       {
                            PlayLists[i].Audios.Remove(deletedAudio);
                       }
                    }
                    PlayLists[PlayLists.IndexOf(SelectedPlayList)].Audios.Remove(deletedAudio);
                    SelectedAudio = null;
                    
                });
            }
        }

        public ICommand ClearPlayList
        {
            get
            {
                return new DelegateCommand(() =>
                {

                    if(PlayLists.IndexOf(SelectedPlayList) == 0)
                    {
                        for(int i = 0; i < PlayLists.Count; i++)
                        {
                            PlayLists[i].Audios.Clear();
                        }
                    }
                    else
                    {
                        SelectedPlayList.Audios.Clear();
                    }
                    SelectedAudio = null;
                });
            }
        }
        
        public ICommand EditAudio
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    SelectedAudio.NameView = SelectedAudio.Author + " - " + SelectedAudio.Name;

                    for (int i = 0; i < PlayLists.Count; i++)
                    {
                        if(PlayLists[i].Audios.Contains(SelectedAudio) && i!= PlayLists.IndexOf(SelectedPlayList))
                        {
                            PlayLists[i].Audios[PlayLists[i].Audios.IndexOf(SelectedAudio)] = SelectedAudio;
                        }
                        
                    }

                });
            }
        }

        public ICommand Sort
        {
            get
            {
                return new DelegateCommand(() =>
                {

                    if (SortedBy.Equals(Sorting[0]))
                    {
                        SelectedPlayList.Audios = new ObservableCollection<Audio>(SelectedPlayList.Audios.OrderBy(a => a.Author));
                    }
                    else if (SortedBy.Equals(Sorting[1]))
                    {
                        SelectedPlayList.Audios = new ObservableCollection<Audio>(SelectedPlayList.Audios.OrderBy(a => a.Name));
                    }
                    else if (SortedBy.Equals(Sorting[2]))
                    {
                        SelectedPlayList.Audios = new ObservableCollection<Audio>(SelectedPlayList.Audios.OrderBy(a => a.Genre));
                    }
                    else if (SortedBy.Equals(Sorting[3]))
                    {
                        SelectedPlayList.Audios = new ObservableCollection<Audio>(SelectedPlayList.Audios.OrderBy(a => a.Year));
                    }
                    else
                    {
                        SelectedPlayList.Audios = new ObservableCollection<Audio>(SelectedPlayList.Audios.OrderByDescending(a => a.Mark));
                    }



                });
            }
        }



        public void Refresh()
        {
            IsPause = false;
            if (timer.IsEnabled) timer.Stop();
            mediaPlayer.Stop();
            if (SelectedAudio!= null)
            {
                mediaPlayer.Open(new Uri(SelectedAudio.Path));
                mediaPlayer.Play();
                mediaPlayer.MediaEnded += MediaEnded;
                timer.Start();
            }
        }

        public ICommand Play_Pause
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if(isPause)
                    {
                        mediaPlayer.Play();
                    }
                    else
                    {
                        mediaPlayer.Pause();
                    }
                    IsPause = !IsPause;


                });
            }
        }

        public ICommand SetRepeat
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    IsRepeat = !IsRepeat;
                });
                
            }
        }

        public ICommand Previous
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if(SelectedPlayList.Audios.IndexOf(SelectedAudio) == 0)
                    {
                        SelectedAudio = SelectedPlayList.Audios.Last();
                    }
                    else
                    {
                        SelectedAudio = SelectedPlayList.Audios[SelectedPlayList.Audios.IndexOf(SelectedAudio) - 1];
                    }
                    
                });

            }
        }

        public ICommand Next
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (SelectedPlayList.Audios.IndexOf(SelectedAudio) == SelectedPlayList.Audios.Count - 1)
                    {
                        SelectedAudio = SelectedPlayList.Audios.First();
                    }
                    else
                    {
                        SelectedAudio = SelectedPlayList.Audios[SelectedPlayList.Audios.IndexOf(SelectedAudio) + 1];
                    }
                    
                });
            }
        }


        private void MediaEnded(object sender, EventArgs e)
        {
            if(!isRepeat)
            {
                if(SelectedPlayList.Audios.IndexOf(SelectedAudio) == SelectedPlayList.Audios.Count - 1)
                {
                    SelectedAudio = SelectedPlayList.Audios.First();
                }
                else
                {
                    SelectedAudio = SelectedPlayList.Audios[SelectedPlayList.Audios.IndexOf(SelectedAudio) + 1];
                }
                
            }
            else
            {
                Refresh();
            }
        }

       

        public MainViewModel()
        { 
            PlayLists = new ObservableCollection<PlayList>();
            
            Sorting = new List<string>();
           
            Sorting.Add("Author");
            Sorting.Add("Title");
            Sorting.Add("Genre");
            Sorting.Add("Year");
            Sorting.Add("Mark");
            SortedBy = Sorting[0];

            PlayLists.Add(new PlayList() { Name = "Default Play List" });
            SelectedPlayList = PlayLists[0];

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += Timer_Tick;
           


        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                SliderValue = mediaPlayer.Position.TotalSeconds;
                CurrentTimeText = String.Format("{0:mm\\:ss}", TimeSpan.FromSeconds(SliderValue));
                TotalDuration = mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                DurationText = String.Format("{0:mm\\:ss}", TimeSpan.FromSeconds(mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds));

            }
            catch(Exception exc)
            {

            }



        }
    }
}
