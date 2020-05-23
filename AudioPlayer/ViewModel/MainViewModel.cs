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
using AudioPlayer.DAO;

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

        private MainViewModel()
        {
            
            Database = new DatabaseCommands();
            PlayLists = Database.SelectAll();

            Sorting = new List<string>();
            Sorting.Add("Author");
            Sorting.Add("Title");
            Sorting.Add("Genre");
            Sorting.Add("Year");
            Sorting.Add("Mark");
            SortedBy = Sorting[0];

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            search = "";
            SelectedPlayList = PlayLists[0];
        }

        DispatcherTimer timer;     
        private bool isRepeat = false;
        private AudioPlayer.Model.AudioPlayer audioPlayer;
        private string search;
        private Audio selectedAudio;
        private PlayList selectedPlayList;
        private PlayList currentAudios;
        private Audio currentTrack;

   
        public AudioPlayer.Model.AudioPlayer AudioPlayer { get => audioPlayer; set { audioPlayer = value; OnPropertyChanged(); } }
        public ObservableCollection<PlayList> PlayLists { get; set; }
        public Audio SelectedAudio { get => selectedAudio; set { selectedAudio = value; OnPropertyChanged(); SetCurrentTrack(); } }
        public PlayList CurrentAudios { get => currentAudios; set { currentAudios = value; OnPropertyChanged(); } }
        public Audio CurrentTrack { get => currentTrack; set { currentTrack = value; Refresh(); OnPropertyChanged(); } }
        public PlayList SelectedPlayList 
        { 
            get => selectedPlayList;
            set 
            { 
                CurrentTrack = null;
                selectedPlayList = value;
                CurrentAudios = value;
                SearchAudios();
                OnPropertyChanged();
            }
        }
        public string Search { get => search; set { search = value; SearchAudios(); } }
        public bool IsRepeat { get => isRepeat; set { isRepeat = value; OnPropertyChanged(); } }
        public List<string> Sorting { get; set; }
        public string SortedBy { get; set; }
        

        public IDAO Database { get; }

        public ICommand AddPlayList
        {
            get
            {
                return new DelegateCommand( (obj) =>
                { 
                    string name = AddPlayListWindow.Show("Enter playlist name", obj as Window);
                    if(!name.Equals(string.Empty))
                    {
                        PlayLists.Add(new PlayList(name));
                        SelectedPlayList = PlayLists.Last();
                        var id = Database.InsertPlayList(SelectedPlayList);
                        SelectedPlayList.Id = id;
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
                    if (!SelectedPlayList.Equals(PlayLists[0]))
                    {
                        Database.DeletePlaylist(SelectedPlayList);
                        PlayLists.Remove(SelectedPlayList);
                        SelectedPlayList = null;
                        SelectedAudio = null;
                        CurrentTrack = null;
                        
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
                    Audio deletedAudio = CurrentTrack;
                    CurrentAudios.Audios.Remove(deletedAudio);
                    if (PlayLists.IndexOf(SelectedPlayList) == 0)
                    {
                       for(int i = 0; i < PlayLists.Count; i++)
                       {
                            PlayLists[i].Audios.Remove(deletedAudio);
                            Database.DeleteAudio(deletedAudio);
                       }
                    }
                    else
                    {
                        PlayLists[PlayLists.IndexOf(SelectedPlayList)].Audios.Remove(deletedAudio);
                        Database.DeleteFromPlayList(deletedAudio, SelectedPlayList);
                    }
                    SelectedAudio = null;
                    CurrentTrack = null;
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
                        foreach (var item in PlayLists[0].Audios)
                        {
                            Database.DeleteAudio(item);
                        }
                        for (int i = 0; i < PlayLists.Count; i++)
                        {
                            PlayLists[i].Audios.Clear();
                        }
                        
                    }
                    else
                    {
                        foreach (var item in SelectedPlayList.Audios)
                        {
                            Database.DeleteFromPlayList(item, SelectedPlayList);
                        }
                        SelectedPlayList.Audios.Clear();
                    }
                    CurrentAudios.Audios.Clear();
                    SelectedAudio = null;
                    CurrentTrack = null;
                });
            }
        }
        
        public ICommand EditAudio
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    CurrentTrack.NameView = CurrentTrack.Author + " - " + CurrentTrack.Name;
                    Database.UpdateAudio(CurrentTrack);
                });
            }
        }

        public ICommand Sort
        {
            get
            {
                return new DelegateCommand(() =>
                {

                    if (CurrentAudios == null) return;

                    if (SortedBy.Equals(Sorting[0]))
                    {
                        CurrentAudios.Audios = new ObservableCollection<Audio>(CurrentAudios.Audios.OrderBy(a => a.Author));
                    }
                    else if (SortedBy.Equals(Sorting[1]))
                    {
                        CurrentAudios.Audios = new ObservableCollection<Audio>(CurrentAudios.Audios.OrderBy(a => a.Name));
                    }
                    else if (SortedBy.Equals(Sorting[2]))
                    {
                        CurrentAudios.Audios = new ObservableCollection<Audio>(CurrentAudios.Audios.OrderBy(a => a.Genre));
                    }
                    else if (SortedBy.Equals(Sorting[3]))
                    {
                        CurrentAudios.Audios = new ObservableCollection<Audio>(CurrentAudios.Audios.OrderBy(a => a.Year));
                    }
                    else
                    {
                        CurrentAudios.Audios = new ObservableCollection<Audio>(CurrentAudios.Audios.OrderByDescending(a => a.Mark));
                    }

                });
            }
        }

        public void SearchAudios()
        {
            CurrentAudios = new PlayList("temp");
            if(Search.Equals(""))
            {
                CurrentAudios = SelectedPlayList;
            }
            else if (SelectedPlayList != null)
            {
                foreach (var item in SelectedPlayList.Audios)
                {
                    if (item.Author.ToUpper().StartsWith(Search.ToUpper())) 
                    {
                        CurrentAudios.Audios.Add(item);
                    }
                }
            }
            Sort.Execute(null);            
        }

        private void Refresh()
        {           
            if (timer.IsEnabled) timer.Stop();
            if (CurrentTrack != null)
            {
                if(AudioPlayer != null)
                {
                    AudioPlayer.Stop();
                }
                AudioPlayer = new AudioPlayer.Model.AudioPlayer(CurrentTrack);//
                timer.Tick += AudioPlayer.Timer_Tick;
                if (System.IO.File.Exists(CurrentTrack.Path))
                {
                    AudioPlayer.Play();
                    AudioPlayer.mediaPlayer.MediaEnded += MediaEnded;
                    timer.Start();
                }
                else
                { 
                    Database.DeleteAudio(SelectedAudio);
                    SelectedPlayList.Audios.Remove(SelectedAudio);
                    PlayLists[0].Audios.Remove(SelectedAudio);
                    SelectedAudio = null;
                }
                
            }
        }

        private void SetCurrentTrack()
        {
            if(SelectedAudio != null)
            {
                CurrentTrack = SelectedAudio;
            }
        }

        public ICommand Play_Pause
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if(AudioPlayer.IsPause)
                    {
                        AudioPlayer.Play();
                    }
                    else
                    {
                        AudioPlayer.Pause();
                    }
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
                    if(CurrentAudios.Audios.Count == 0)
                    {
                        CurrentTrack = null;
                        return;
                    }

                    if(CurrentAudios.Audios.IndexOf(SelectedAudio) == 0)
                    {
                        SelectedAudio = CurrentAudios.Audios.Last();
                    }
                    else
                    {
                        SelectedAudio = CurrentAudios.Audios[CurrentAudios.Audios.IndexOf(SelectedAudio) - 1];
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
                    if (CurrentAudios.Audios.Count == 0)
                    {
                        CurrentTrack = null;
                        return;
                    }

                    if (CurrentAudios.Audios.IndexOf(SelectedAudio) == CurrentAudios.Audios.Count - 1)
                    {
                        SelectedAudio = CurrentAudios.Audios.First();
                    }
                    else
                    {
                        SelectedAudio = CurrentAudios.Audios[CurrentAudios.Audios.IndexOf(SelectedAudio) + 1];
                    }
                    
                });
            }
        }

        public ICommand Random
        {
            get
            {
                return new DelegateCommand(() =>
                {


                    if (CurrentAudios.Audios.Count <= 1) return;

                    int randomIndex;
                    do
                    {
                        randomIndex = new Random().Next(0, CurrentAudios.Audios.Count);
                    }
                    while (randomIndex == CurrentAudios.Audios.IndexOf(SelectedAudio));

                    SelectedAudio = CurrentAudios.Audios[randomIndex];
                });
            }
        }

        private void MediaEnded(object sender, EventArgs e)
        {
            if(!IsRepeat)
            { 
                Next.Execute(null);
            }
            else
            {
                Refresh();
            }
        }

    }
}
