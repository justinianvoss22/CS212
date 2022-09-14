using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FernNamespace
{
    /*
     * Justin Voss
     * this class draws a fractal fern when the constructor is called.
     * Program 3 - Fractal Fern  -- October 2020.
     * 
     * Bugs: WPF and shape objects are the wrong tool for the task 
     */
    
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Fern f = new Fern(sizeSlider.Value, reduxSlider.Value, biasSlider.Value, canvas);
        }


        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Fern f = new Fern(sizeSlider.Value, reduxSlider.Value, biasSlider.Value, canvas);
        }
    }
    class Fern
    {
        private static int BERRYMIN = 10;
        private static double TENDRILMIN = 0.5;
        private static double DELTATHETA = 0.01;
        private static double SEGLENGTH = 7.0;
        private static Random random = new Random();
       
        // starts randomly left or right
        private static int left_or_right = random.Next(1,2);
        private static int random_thickness = random.Next(0, 6);

        int random_color_number = random.Next(0, 6);

        /* 
         * Fern constructor erases screen and draws a fern
         * 
         * Size: number of 3-pixel segments of tendrils
         * Redux: how much smaller children clusters are compared to parents
         * Turnbias: how likely to turn right vs. left (0=always left, 0.5 = 50/50, 1.0 = always right)
         * canvas: the canvas that the fern will be drawn on
         */
        public Fern(double size, double redux, double turnbias, Canvas canvas)
        {
            
            
            
           // cluster((int)(canvas.Width / 2), (int)(canvas.Height / 2), size, redux, turnbias, canvas);

            // adds a background
            //ImageBrush myImageBrush = new ImageBrush();
            //myImageBrush.ImageSource = new BitmapImage(new Uri("/Photos/charlie_brown_no_tree.jpg", UriKind.Relative));
            //canvas.Background = myImageBrush;


            //also, added Snoopy png as my last object! When fixing images to fit in canvas, snoopy sometimes doesn't show up.
            
            // delete old canvas contents
            canvas.Children.Clear();


            // draw a new fern at the base of the Charlie Brown tree with given parameters
            
            // added a random thickness element
            random_thickness = random.Next(1, 4);
            tendril((int)((canvas.Width / 2)-8), (int)(canvas.Height-57), size/2, redux, turnbias, Math.PI, canvas);
        }

        /*
         * cluster draws a cluster at the given location and then draws a bunch of tendrils out in 
         * regularly-spaced directions out of the cluster.
         */
        private void cluster(int x, int y, double size, double redux, double turnbias, Canvas canvas, int left_or_right, double direction)
        {

            // compute the angle of the outgoing tendril
            // uses radians, and every degree in radians is pi/180
            //double theta = Math.PI / 180 * 30;

            // a slightly random variation in the calculation of the angle
            double theta = (Math.PI * 10 * random.NextDouble() / 180) - Math.PI * 65 / 180;
            
            tendril(x, y, size, redux, turnbias, direction, canvas);

            if (left_or_right == 1) // when its on the left direction
            { 
                // subtracts theta to make the tendril go to the left
                tendril(x, y, size/2, redux, turnbias, direction - theta, canvas);


            }
            if (left_or_right == -1) // when its on the right direction
            {
                // adds theta to make the tendril go to the right
                tendril(x, y, size/2, redux, turnbias, direction + theta, canvas);
                

            }

            if (size > BERRYMIN)
                berry(x, y, 10, canvas);
           
        }

        /*
         * tendril draws a tendril (a randomly-wavy line) in the given direction, for the given length, 
         * and draws a cluster at the other end if the line is big enough.
         */
        private void tendril(int x1, int y1, double size, double redux, double turnbias, double direction, Canvas canvas)
        {
            int x2 = x1, y2 = y1;
            for (int i = 0; i < size; i+= 2 )
            {
                // makes the turnbias slightly random
                direction += (random.NextDouble() < turnbias) ? -1 * DELTATHETA : DELTATHETA;
                // direction += directions * deltatheta;
                double new_length = SEGLENGTH;

                x1 = x2; y1 = y2;
                x2 = x1 + (int)(SEGLENGTH * Math.Sin(direction));
                y2 = y1 + (int)(SEGLENGTH * Math.Cos(direction));
                byte red = (byte)(100 + size / 2);
                byte green = (byte)(220 - size / 3);
               

                line(x1, y1, x2, y2, red, green, 0, 1 + random_thickness, canvas);
            }
            left_or_right *= -1;

            if (size > TENDRILMIN)

                cluster(x2, y2, size/(redux), redux, turnbias, canvas, left_or_right, direction);

        }



        /*
         * draw a colored circle Christmas ornament centered at (x,y), radius radius, onto canvas
         */
        private void berry(int x, int y, double radius, Canvas canvas)
        {
            Ellipse myEllipse = new Ellipse();
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();
            int color1Random = random.Next(0, 255);
            int color2Random = random.Next(0, 255);
            int color3Random = random.Next(0, 255);
            int newRandom = random.Next(1, 3);
            
            // adds a random element to the Christmas ornament: either red or green

            if (newRandom == 1)
            {
                //red
                mySolidColorBrush.Color = Color.FromArgb(255, 255, 0, 0);
            }
            if (newRandom == 2)
            {
                // green
                mySolidColorBrush.Color =  Color.FromArgb(255, 30, 121, 44);
            }

            myEllipse.Fill = mySolidColorBrush;
            myEllipse.StrokeThickness = 0;
            myEllipse.Stroke = Brushes.Black;
            myEllipse.HorizontalAlignment = HorizontalAlignment.Center;
            myEllipse.VerticalAlignment = VerticalAlignment.Center;
            myEllipse.Width = 2 * (radius-1);
            myEllipse.Height = 2 * (radius-1);
            myEllipse.SetCenter(x, y);
            canvas.Children.Add(myEllipse);
        }

        /*
         * draw a line segment (x1,y1) to (x2,y2) with given color, thickness on canvas
         */
        private void line(int x1, int y1, int x2, int y2, byte r, byte g, byte b, double thickness, Canvas canvas)
        {
            Line myLine = new Line();
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();

            // add some randomness for the color of the tree. Christmas themed randomness.
            if (random_color_number == 0) // red
                mySolidColorBrush.Color = Color.FromArgb(255, 255, 0, 0);
            if (random_color_number == 1) // green
                mySolidColorBrush.Color = Color.FromArgb(255, r, g, b);
            if (random_color_number == 2) // brown for the rest. We want the brown to be the most likely
                mySolidColorBrush.Color = Color.FromArgb(255, 83, 53, 10);
            if (random_color_number == 3)
                mySolidColorBrush.Color = Color.FromArgb(255, 83, 53, 10);
            if (random_color_number == 4)
                mySolidColorBrush.Color = Color.FromArgb(255, 83, 53, 10);
            if (random_color_number == 5)
                mySolidColorBrush.Color = Color.FromArgb(255, 83, 53, 10);
            if (random_color_number == 6)
                mySolidColorBrush.Color = Color.FromArgb(255, 83, 53, 10);
            myLine.X1 = x1;
            myLine.Y1 = y1;
            myLine.X2 = x2;
            myLine.Y2 = y2;
            myLine.Stroke = mySolidColorBrush;
            myLine.VerticalAlignment = VerticalAlignment.Center;
            myLine.HorizontalAlignment = HorizontalAlignment.Left;
            myLine.StrokeThickness = thickness ;
            canvas.Children.Add(myLine);
        }
    }
}

/*
 * this class is needed to enable us to set the center for an ellipse (not built in?!)
 */
public static class EllipseX
{
    public static void SetCenter(this Ellipse ellipse, double X, double Y)
    {
        Canvas.SetTop(ellipse, Y - ellipse.Height / 2);
        Canvas.SetLeft(ellipse, X - ellipse.Width / 2);
    }
}
