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

namespace AudioPlayer.ViewModel
{
    public class MainViewModel : BaseModel
    {
        
        private Audio selectedAudio;
        private PlayList selectedPlayList = new PlayList();

        
        public ObservableCollection<PlayList> PlayLists { get; set; }
        public PlayList SelectedPlayList { get => selectedPlayList; set { selectedPlayList = value; OnPropertyChanged(); } }
        public Audio SelectedAudio { get => selectedAudio; set { selectedAudio = value; OnPropertyChanged(); } }

        public ICommand AddPlayList
        {
            get
            {
                return new DelegateCommand( () =>
                { 
                    PlayLists.Add(new PlayList() { Name = AddPlayListWindow.Show("Enter playlist name") });
                    SelectedPlayList = PlayLists.Last();

                });
            }
        }

        public ICommand DeletePlayList
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if(!SelectedPlayList.Name.Equals("Default Play List"))
                    {
                        PlayLists.Remove(SelectedPlayList);
                        SelectedPlayList = null;
                        SelectedAudio = null;
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
                    PlayLists[PlayLists.IndexOf(SelectedPlayList)].Audios.Remove(SelectedAudio);
                    SelectedAudio = null;
                });
            }
        }

        public MainViewModel()
        { 
            PlayLists = new ObservableCollection<PlayList>();
            
            

            PlayLists.Add(new PlayList() { Name = "Default Play List" });
            PlayLists.Add(new PlayList() { Name = "List1"});
            PlayLists.Add(new PlayList() { Name = "List2" });
            PlayLists.Add(new PlayList() { Name = "List3" });
            PlayLists.Add(new PlayList() { Name = "List4" });
            PlayLists[0].Audios.Add(new Audio("Nirvana", "Smells like teen spirit") { Genre = "Grunge", Mark = 4});
            PlayLists[0].Audios.Add(new Audio("Nirvana", "Rape me") { Genre = "Heavy Metal", Mark = 3 });
            PlayLists[0].Audios.Add(new Audio("Nirvana", "Smells like teen spirit") { Genre = "Grunge", Mark = 4 });
            PlayLists[0].Audios.Add(new Audio("Nirvana", "Rape me") { Genre = "Heavy Metal", Mark = 3 });
            PlayLists[0].Audios.Add(new Audio("Nirvana", "Smells like teen spirit") { Genre = "Grunge", Mark = 4 });
            PlayLists[0].Audios.Add(new Audio("Nirvana", "Rape me") { Genre = "Heavy Metal", Mark = 3 });
            PlayLists[0].Audios.Add(new Audio("Nirvana", "Smells like teen spirit") { Genre = "Grunge", Mark = 4 });
            PlayLists[0].Audios.Add(new Audio("Nirvana", "Rape me") { Genre = "Heavy Metal", Mark = 3 });

            SelectedAudio = PlayLists[0].Audios[0];
            PlayLists[1].Audios.Add(PlayLists[0].Audios[2]);
            SelectedPlayList = PlayLists[0];
            
        }
    }
}
