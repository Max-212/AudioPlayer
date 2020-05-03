using AudioPlayer.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioPlayer.DAO
{
    public interface IDAO
    {

        void UpdateSettingStyle(string style);
        string SelectStyle();

        int InsertAudio(Audio audio);
        int InsertPlayList(PlayList playList);
        void InsertAudioToPlayList(Audio audio, PlayList playList);

        void DeleteFromPlayList(Audio audio, PlayList playList);
        void DeletePlaylist(PlayList playList);
        void DeleteAudio(Audio audio);

        void UpdateAudio(Audio audio);

        ObservableCollection<PlayList> SelectAll(); 
    }
}
