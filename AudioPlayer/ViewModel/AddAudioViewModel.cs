using AudioPlayer.DAO;
using AudioPlayer.Model;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VisioForge.Tools.TagLib;
using VisioForge.Tools.TagLib.Mpeg;

namespace AudioPlayer.ViewModel
{
    class AddAudioViewModel : BaseModel
    {
        private MainViewModel MainVM;
        private Audio audioForAdd;
        private PlayList audios;
        private string search;


        public string Search { get => search; set { search = value; SearchAudios(); } }
        public PlayList Audios { get => audios; set { audios = value; OnPropertyChanged(); } }
        public ObservableCollection<Audio> SelectedAudios { get; set; } // Audios for Add
        public Audio AudioForAdd { get => audioForAdd; set { audioForAdd = value; OnPropertyChanged(); } }


        private void SearchAudios()
        {
            Audios = new PlayList("temp");
            if (Search.Equals(""))
            {
                Audios = MainVM.PlayLists[0];
            }
            else
            {
                foreach (var item in MainVM.PlayLists[0].Audios)
                {
                    if (item.Author.ToUpper().StartsWith(Search.ToUpper()))
                    {
                        Audios.Audios.Add(item);
                    }
                }
            }
        }

        public ICommand InSelectedAudios
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (!SelectedAudios.Contains(AudioForAdd))
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

        public ICommand AddAudiosToPlayList
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    foreach (var item in SelectedAudios)
                    {
                        
                        if (!MainVM.PlayLists[0].Audios.Contains(item))
                        {
                            MainVM.PlayLists[0].Audios.Add(item);
                            var id = MainVM.Database.InsertAudio(item);
                            MainVM.PlayLists[0].Audios.Last().Id = id;
                        }
                        if (!MainVM.SelectedPlayList.Audios.Contains(item) && MainVM.PlayLists.IndexOf(MainVM.SelectedPlayList) != 0)
                        {
                            MainVM.SelectedPlayList.Audios.Add(item);
                            MainVM.Database.InsertAudioToPlayList(item, MainVM.SelectedPlayList);
                        }
                    }
                    SelectedAudios.Clear();
                    MainVM.SearchAudios();
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
                    opd.Filter = "MP3 files (*.mp3)|*.mp3";
                    opd.Multiselect = true;

                    if (opd.ShowDialog() == true)
                    {
                        for (int i = 0; i < opd.FileNames.Length; i++)
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

                            var selectedAudio = SearchByPath(file);
                            if(selectedAudio == null)
                            {
                                SelectedAudios.Add(new Audio(artist, title, genre, file, year));
                            }
                            else
                            {
                                if(!SelectedAudios.Contains(selectedAudio))
                                {
                                    SelectedAudios.Add(selectedAudio);
                                }
                            }
                            
                        }
                    }
                });
            }
        }

        public Audio SearchByPath(string path)
        {
            foreach(Audio item in MainVM.PlayLists[0].Audios)
            {
                if (item.Path.Equals(path)) 
                    return item;
            }
            foreach(Audio item in SelectedAudios)
            {
                if (item.Path.Equals(path))
                    return item;
            }
            return null;
        }

        public AddAudioViewModel()
        {
            search = "";
            MainVM = MainViewModel.getInstance();
            SelectedAudios = new ObservableCollection<Audio>();
            Audios = MainVM.PlayLists[0];
        }
    }
}
