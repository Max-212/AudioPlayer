using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AudioPlayer.Model
{
    public class AudioPlayer : BaseModel
    {
        private Audio audio;
        public MediaPlayer mediaPlayer = new MediaPlayer();

        private bool isRepeat = false;
        private bool isPause = false;
        private string currentTimeText;
        private string durationText;
        private double totalDuration;

        public Audio Audio { get => audio; set { audio = value; OnPropertyChanged(); } }
        public double Volume { get => mediaPlayer.Volume * 100; set { mediaPlayer.Volume = value / 100; OnPropertyChanged(); } }
        public double TotalDuration { get => totalDuration; set { totalDuration = value; OnPropertyChanged(); } }
        public string CurrentTimeText { get => currentTimeText; set { currentTimeText = value; OnPropertyChanged(); } }
        public string DurationText { get => durationText; set { durationText = value; OnPropertyChanged(); } }
        public bool IsPause { get => isPause; set { isPause = value; OnPropertyChanged(); } }
        public double SliderValue
        {
            get => mediaPlayer.Position.TotalSeconds;
            set { mediaPlayer.Position = TimeSpan.FromSeconds(value); ; OnPropertyChanged(); }
        }

        public AudioPlayer(Audio audio)
        {
            Audio = audio;
            mediaPlayer.Open(new Uri(Audio.Path));
        }

        ~AudioPlayer()
        {
            Stop();
        }

        public void Stop()
        {
            mediaPlayer.Stop();
        }

        public void Pause()
        {
            mediaPlayer.Pause();
            IsPause = true;
        }

        public void Play()
        {
            mediaPlayer.Play();
            IsPause = false;
        }

        public void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                SliderValue = mediaPlayer.Position.TotalSeconds;
                CurrentTimeText = String.Format("{0:mm\\:ss}", TimeSpan.FromSeconds(SliderValue));
                TotalDuration = mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                DurationText = String.Format("{0:mm\\:ss}", TimeSpan.FromSeconds(mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds));

            }
            catch (Exception exc)
            {

            }
        }
    }
}
