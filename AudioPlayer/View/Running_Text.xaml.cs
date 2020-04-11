using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AudioPlayer.View
{
    /// <summary>
    /// Логика взаимодействия для Running_Text.xaml
    /// </summary>
    public partial class Running_Text : UserControl
    {
        public Running_Text()
        {
            InitializeComponent();
            
           
            
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Selected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(Running_Text),
                        new FrameworkPropertyMetadata(
                        string.Empty,
                        FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                        new PropertyChangedCallback(TextChanged),
                        new CoerceValueCallback(CoerceText)));

        private static void TextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Running_Text s = d as Running_Text;
            s.Text = (string)e.NewValue;
            s.text = s.Text;
            if (s.text.Length > ((int)s.Width / 7))
                s.MainText.Text = s.text.Substring(0, ((int)s.Width / 7) - 3) + "...";
            else s.MainText.Text = s.text;


        }

       
        private static object CoerceText(DependencyObject d, object value)
        {
            return value;
        }

        public Brush TextForeground
        {
            get { return (Brush)GetValue(TextForegroundProperty); }
            set { SetValue(TextForegroundProperty, value); }
        }

        public static readonly DependencyProperty TextForegroundProperty =
           DependencyProperty.Register("TextForeground", typeof(Brush), typeof(Running_Text),
                       new FrameworkPropertyMetadata(
                       new SolidColorBrush(),
                       FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                       new PropertyChangedCallback(ForegroundChanged),
                       new CoerceValueCallback(CoerceForeground)));

        private static void ForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Running_Text s = d as Running_Text;
            s.MainText.Foreground = s.TextForeground;
            

        }

        private static object CoerceForeground(DependencyObject d, object value)
        {
            return value;
        }


        private Thread thread;
        private string text;

        public void RunText()
        { 
            int VisibleTextLenght = ((int)ActualWidth / 7);

            while (true)
            {
                for (int i = 0; i < text.Length; i++)
                {

                    if ((VisibleTextLenght + i) > text.Length)
                    {
                        Thread.Sleep(400);
                        break;
                    }
                    Dispatcher.Invoke(() => MainText.Text = text.Substring(i, VisibleTextLenght));
                    Thread.Sleep(150);
                }
                Dispatcher.Invoke(() => MainText.Text = text.Substring(0, VisibleTextLenght));
                Thread.Sleep(300);
            }
        }


        private void TextBoxMouseEnter(object sender, MouseEventArgs e)
        {
            if (text.Length > (int)(ActualWidth / 7))
            {
                thread = new Thread(new ThreadStart(this.RunText));
                thread.Start();
            }
        }

        private void TexBoxMouseLeave(object sender, MouseEventArgs e)
        {
            if (text.Length > ((int)ActualWidth / 7))
            {
                thread.Abort();
                MainText.Text = text.Substring(0, ((int)ActualWidth / 7) - 3) + "...";
            }
        }

        private void TexBlockLoaded(object sender, RoutedEventArgs e)
        {
            text = Text;
            if (text.Length > ((int)ActualWidth / 7))
                MainText.Text = text.Substring(0, ((int)ActualWidth / 7) - 3) + "...";
            MainText.Foreground = TextForeground;
            
        }
    }
}
