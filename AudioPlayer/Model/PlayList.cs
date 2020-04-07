using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioPlayer.Model
{
    public class PlayList : BaseModel
    {
        private string name;
        private ObservableCollection<Audio> audios = new ObservableCollection<Audio>();

        public string Name { get => name; set { name = value; OnPropertyChanged(); } }
        public ObservableCollection<Audio> Audios { get => audios; set { audios = value; OnPropertyChanged(); } }
        public readonly int Count;
        
        public PlayList()
        {
            
            Count = audios.Count();
        }
    }
}
