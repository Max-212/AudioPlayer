using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AudioPlayer.Model
{
    public class Audio : BaseModel
    {
        private string nameView;
        private string name;
        private string genre;
        private int mark;
        private string author;
        private string year;
        private string path;

        
        public string NameView { get => nameView; set { nameView = value; OnPropertyChanged(); } }
        public string Name { get => name; set { name = value; OnPropertyChanged(); } }
        public string Genre { get => genre; set { genre = value; OnPropertyChanged(); } }
        public int Mark { get => mark; set { mark = value; OnPropertyChanged(); } }
        public string Author { get => author; set { author = value; OnPropertyChanged(); } }
        public string Year { get => year; set { year = value; OnPropertyChanged(); } }
        public string Path { get => path; set { path = value; OnPropertyChanged(); } }

        

        public Audio(string author, string name, string genre, string path, string year)
        {
            Author = author;
            Name = name;
            NameView = Author + " - " + Name;
            Genre = genre;
            Path = path;
            Year = year;
            Mark = 0;
        }

    }
}
