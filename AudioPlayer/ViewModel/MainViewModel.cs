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

namespace AudioPlayer.ViewModel
{
    public class MainViewModel : BaseModel
    {

        private static MainViewModel instance;

        public static MainViewModel getInstance()
        {
            if (instance == null)
                instance = new MainViewModel();
            return instance;
        }

        private Audio audioForAdd;
        private Audio selectedAudio;
        private PlayList selectedPlayList = new PlayList();
        

        public ObservableCollection<PlayList> PlayLists { get; set; }
        public ObservableCollection<Audio> SelectedAudios { get; set; }
        public PlayList SelectedPlayList { get => selectedPlayList; set { selectedPlayList = value; OnPropertyChanged(); } }
        public Audio SelectedAudio { get => selectedAudio; set { selectedAudio = value; OnPropertyChanged(); } }
        public Audio AudioForAdd { get => audioForAdd; set { audioForAdd = value; OnPropertyChanged(); } }

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

        public ICommand InSelectedAudios
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if(!SelectedAudios.Contains(AudioForAdd))
                    {
                        SelectedAudios.Add(AudioForAdd);
                    }
                    
                });
            }
        }

        public ICommand DeleteFromSelectedAudios
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    SelectedAudios.Remove(AudioForAdd);
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

        public ICommand AddAudiosToPlayList
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    foreach(var item in SelectedAudios)
                    {
                        if(!SelectedPlayList.Audios.Contains(item))
                        {
                            SelectedPlayList.Audios.Add(item);
                        }
                        if (!PlayLists[0].Audios.Contains(item))
                        {
                            PlayLists[0].Audios.Add(item);
                        }
                    }
                    SelectedAudios.Clear();
                });
            }
        }

        public ICommand SearchAudioFromComputer
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    var opd = new OpenFileDialog();
                    opd.Filter = "MP3 files (*.mp3)|*.mp3|All files (*.*)|*.*";
                    opd.Multiselect = true;

                    if (opd.ShowReadOnly() == true)
                    {
                        for(int i = 0; i < opd.FileNames.Length; i++)
                        {
                            var file = opd.FileNames[i];
                            AudioFile audio = new AudioFile(file, ReadStyle.Average);

                            string artist;
                            string title;
                            string year;
                            string genre;

                            if (audio.Tag.FirstArtist == null) artist = "Unknown";
                            else artist = audio.Tag.FirstArtist;
                        
                            if (audio.Tag.Title == null) title = "Unknown";             
                            else title = audio.Tag.Title;

                            if (audio.Tag.Year == 0) year = "Unknown";
                            else year = Convert.ToString(audio.Tag.Year);

                            if (audio.Tag.FirstGenre == null) genre = "Unknown";
                            else genre = audio.Tag.FirstGenre;
                            

                            SelectedAudios.Add(new Audio(artist, title, genre ,file, year));
                        }
                    }
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
                        if(PlayLists[i].Audios.Contains(SelectedAudio))
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

        public MainViewModel()
        { 
            PlayLists = new ObservableCollection<PlayList>();
            SelectedAudios = new ObservableCollection<Audio>();
            Sorting = new List<string>();
           
            Sorting.Add("Author");
            Sorting.Add("Title");
            Sorting.Add("Genre");
            Sorting.Add("Year");
            Sorting.Add("Mark");
            //SortedBy = Sorting[0];

            PlayLists.Add(new PlayList() { Name = "Default Play List" });
            SelectedPlayList = PlayLists[0];
            
        }
    }
}
