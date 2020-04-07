using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AudioPlayer.View
{
    /// <summary>
    /// Логика взаимодействия для Rating.xaml
    /// </summary>
    public partial class Rating : UserControl
    {
        public Rating()
        {
            InitializeComponent();
        }


        public int Selected
        {
            get { return (int)GetValue(SelectedProperty); }
            set { SetValue(SelectedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Selected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedProperty =
            DependencyProperty.Register("Selected", typeof(int), typeof(Rating),
                        new FrameworkPropertyMetadata(
                        0,
                        FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                        new PropertyChangedCallback(SelectedChanged),
                        new CoerceValueCallback(CoerceSelected)));

        private static void SelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Rating s = d as Rating;
            s.Selected = (int)e.NewValue;
            s.LoadDP();

        }

        private static object CoerceSelected(DependencyObject d, object value)
        {
            return value;
        }


        SolidColorBrush orange = new SolidColorBrush(Color.FromRgb(255, 102, 0));
        SolidColorBrush clear = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        

        private void s1_MouseEnter(object sender, MouseEventArgs e)
        {
            s1.Fill = orange;
            s2.Fill = clear;
            s3.Fill = clear;
            s4.Fill = clear;
            s5.Fill = clear;
        }

        private void s2_MouseEnter(object sender, MouseEventArgs e)
        {
            s1.Fill = orange;
            s2.Fill = orange;
            s3.Fill = clear;
            s4.Fill = clear;
            s5.Fill = clear;
        }

        private void s3_MouseEnter(object sender, MouseEventArgs e)
        {
            s1.Fill = orange;
            s2.Fill = orange;
            s3.Fill = orange;
            s4.Fill = clear;
            s5.Fill = clear;
        }

        private void s4_MouseEnter(object sender, MouseEventArgs e)
        {
            s1.Fill = orange;
            s2.Fill = orange;
            s3.Fill = orange;
            s4.Fill = orange;
            s5.Fill = clear;

        }

        private void s5_MouseEnter(object sender, MouseEventArgs e)
        {
            s1.Fill = orange;
            s2.Fill = orange;
            s3.Fill = orange;
            s4.Fill = orange;
            s5.Fill = orange;

        }





        private void s1_MouseLeave(object sender, MouseEventArgs e)
        {
            s1.Fill = clear;
        }

        private void s2_MouseLeave(object sender, MouseEventArgs e)
        {
            s1.Fill = clear;
            s2.Fill = clear;
        }

        private void s3_MouseLeave(object sender, MouseEventArgs e)
        {
            s1.Fill = clear;
            s2.Fill = clear;
            s3.Fill = clear;


        }

        private void s4_MouseLeave(object sender, MouseEventArgs e)
        {
            s1.Fill = clear;
            s2.Fill = clear;
            s3.Fill = clear;
            s4.Fill = clear;
        }

        private void s5_MouseLeave(object sender, MouseEventArgs e)
        {
            s1.Fill = clear;
            s2.Fill = clear;
            s3.Fill = clear;
            s4.Fill = clear;
            s5.Fill = clear;
        }








        private void s1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Selected = 1;
            s1.Fill = orange;
            s2.Fill = clear;
            s3.Fill = clear;
            s4.Fill = clear;
            s5.Fill = clear;
        }

        private void s2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Selected = 2;
            s1.Fill = orange;
            s2.Fill = orange;
            s3.Fill = clear;
            s4.Fill = clear;
            s5.Fill = clear;
        }

        private void s3_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Selected = 3;
            s1.Fill = orange;
            s2.Fill = orange;
            s3.Fill = orange;
            s4.Fill = clear;
            s5.Fill = clear;
        }

        private void s4_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Selected = 4;
            s1.Fill = orange;
            s2.Fill = orange;
            s3.Fill = orange;
            s4.Fill = orange;
            s5.Fill = clear;
        }

        private void s5_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Selected = 5;
            s1.Fill = orange;
            s2.Fill = orange;
            s3.Fill = orange;
            s4.Fill = orange;
            s5.Fill = orange;
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {

            if (Selected == 1)
            {
                s1.Fill = orange;
                s2.Fill = clear;
                s3.Fill = clear;
                s4.Fill = clear;
                s5.Fill = clear;

            }

            if (Selected == 2)
            {
                s1.Fill = orange;
                s2.Fill = orange;
                s3.Fill = clear;
                s4.Fill = clear;
                s5.Fill = clear;

            }

            if (Selected == 3)
            {
                s1.Fill = orange;
                s2.Fill = orange;
                s3.Fill = orange;
                s4.Fill = clear;
                s5.Fill = clear;

            }

            if (Selected == 4)
            {
                s1.Fill = orange;
                s2.Fill = orange;
                s3.Fill = orange;
                s4.Fill = orange;
                s5.Fill = clear;

            }

            if (Selected == 5)
            {
                s1.Fill = orange;
                s2.Fill = orange;
                s3.Fill = orange;
                s4.Fill = orange;
                s5.Fill = orange;

            }

        }

        private void Load(object sender, RoutedEventArgs e)
        {
            if (Selected == 1)
            {
                s1.Fill = orange;
                s2.Fill = clear;
                s3.Fill = clear;
                s4.Fill = clear;
                s5.Fill = clear;

            }

            if (Selected == 2)
            {
                s1.Fill = orange;
                s2.Fill = orange;
                s3.Fill = clear;
                s4.Fill = clear;
                s5.Fill = clear;

            }

            if (Selected == 3)
            {
                s1.Fill = orange;
                s2.Fill = orange;
                s3.Fill = orange;
                s4.Fill = clear;
                s5.Fill = clear;

            }

            if (Selected == 4)
            {
                s1.Fill = orange;
                s2.Fill = orange;
                s3.Fill = orange;
                s4.Fill = orange;
                s5.Fill = clear;

            }

            if (Selected == 5)
            {
                s1.Fill = orange;
                s2.Fill = orange;
                s3.Fill = orange;
                s4.Fill = orange;
                s5.Fill = orange;

            }
        }

        public void LoadDP()
        {
            if (Selected == 1)
            {
                s1.Fill = orange;
                s2.Fill = clear;
                s3.Fill = clear;
                s4.Fill = clear;
                s5.Fill = clear;

            }

            if (Selected == 2)
            {
                s1.Fill = orange;
                s2.Fill = orange;
                s3.Fill = clear;
                s4.Fill = clear;
                s5.Fill = clear;

            }

            if (Selected == 3)
            {
                s1.Fill = orange;
                s2.Fill = orange;
                s3.Fill = orange;
                s4.Fill = clear;
                s5.Fill = clear;

            }

            if (Selected == 4)
            {
                s1.Fill = orange;
                s2.Fill = orange;
                s3.Fill = orange;
                s4.Fill = orange;
                s5.Fill = clear;

            }

            if (Selected == 5)
            {
                s1.Fill = orange;
                s2.Fill = orange;
                s3.Fill = orange;
                s4.Fill = orange;
                s5.Fill = orange;

            }
        }
    }
}
