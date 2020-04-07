using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace AudioPlayer.Model
{
    public class Audio : BaseModel
    {
        private string nameView;
        private string name;
        private string genre;
        private TimeSpan duration;
        private int mark;
        private string author;
        private int year;


        public string NameView { get => nameView; set { nameView = value; OnPropertyChanged(); } }
        public string Name { get => name; set { name = value; OnPropertyChanged(); } }
        public string Genre { get => genre; set { genre = value; OnPropertyChanged(); } }
        public TimeSpan Duration { get => duration; set { duration = value; OnPropertyChanged(); } }
        public int Mark { get => mark; set { mark = value; OnPropertyChanged(); } }
        public string Author { get => author; set { author = value; OnPropertyChanged(); } }
        public int Year { get => year; set { year = value; OnPropertyChanged(); } }

        public Audio(string author, string name)
        {
            Author = author;
            Name = name;
            NameView = Author + " - " + Name;
        }

    }
}
