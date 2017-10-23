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

namespace AnalogClock
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        
        Tuple<int,int> hour = Tuple.Create(0,0);
        Tuple<int, int> min = Tuple.Create(0, 0);
        Tuple<int, int> sec = Tuple.Create(1, 1);

        DoubleAnimation secAnim;
        DoubleAnimation minAnim;
        DoubleAnimation hourAnim;


        TimerCallback timCall;

        public MainWindow()
        {
            InitializeComponent();
            timCall = new TimerCallback(refreshAnimation);
        }

        private void AnimBuilder()
        {
            #region creating animations
            secAnim = new DoubleAnimation
            {
                Name = "secAnim",
                From = secs.Item1,
                To = 360,
                Duration = TimeSpan.FromSeconds(60 - secs.Item2)

            };

            secAnim.Completed += SecAnim_Completed;

            minAnim = new DoubleAnimation
            {
                Name = "minAnim",
                From = mins.Item1,
                To = 360,
                Duration = TimeSpan.FromMinutes(60 - mins.Item2)

            };

            minAnim.Completed += MinAnim_Completed;

            hourAnim = new DoubleAnimation
            {
                Name = "hourAnim",
                From = hours.Item1,
                To = 360,
                Duration = TimeSpan.FromHours(12 - hours.Item2)

            };

            hourAnim.Completed += HourAnim_Completed;
             
            #endregion

            #region animation begin
            secTrans.BeginAnimation(RotateTransform.AngleProperty, secAnim);
            minTrans.BeginAnimation(RotateTransform.AngleProperty, minAnim);
            hourTrans.BeginAnimation(RotateTransform.AngleProperty, hourAnim);

            #endregion


        }

        private void HourAnim_Completed(object sender, EventArgs e)
        {
            object hourName = hourAnim.Name;

            Timer hourTimer = new Timer(timCall, hourName, 1, 0);
 

        }

        private void MinAnim_Completed(object sender, EventArgs e)
        {

            object minName = minAnim.Name;
            Timer minTimer = new Timer(timCall, minName, 1, 0);

        }

        private void SecAnim_Completed(object sender, EventArgs e)
        {

            object secName = secAnim.Name;

            Timer timer = new Timer(timCall, secName, 1, 0);
       
        }

        void refreshAnimation(object obj)
        {
            string name = (string)obj + "Anim";
        
            switch ((string)obj) { 
                case "minAnim":
                    {

                        mins = Tuple.Create(1, 1);

                        Dispatcher.Invoke(() =>
                        {
                            minAnim.From = mins.Item1;
                            minAnim.Duration = TimeSpan.FromMinutes(60);
                            minAnim.RepeatBehavior = RepeatBehavior.Forever;
                            minTrans.BeginAnimation(RotateTransform.AngleProperty, minAnim);
                        });

                        break;
                    }
                case "secAnim":
                    {
                       
                        secs = Tuple.Create(1, 1);
                         
                        Dispatcher.Invoke(() =>
                        {
                            secAnim.From = secs.Item1;
                            secAnim.Duration = TimeSpan.FromSeconds(60);
                            secAnim.RepeatBehavior = RepeatBehavior.Forever;
                            secTrans.BeginAnimation(RotateTransform.AngleProperty, secAnim);
                        });
                        
                        break;
                    }
                case "hourAnim":
                    {
                        hours = Tuple.Create(1, 1);

                        Dispatcher.Invoke(() =>
                        {
                            hourAnim.From = hours.Item1;
                            hourAnim.Duration = TimeSpan.FromHours(60);
                            hourAnim.RepeatBehavior = RepeatBehavior.Forever;
                            hourTrans.BeginAnimation(RotateTransform.AngleProperty, hourAnim);
                        });

                        break;
                    }
                default: break;
            }

        }

        private void Calc()
        {
            foreach (Rectangle tmp in Clock.Children)
            {
                if (tmp == HourArrow)
                {
                    switch (DateTime.Now.Hour)
                    {
                        case 0: hours = Tuple.Create(0,DateTime.Now.Hour); break;
                        case 1: hours = Tuple.Create(30, DateTime.Now.Hour); break;
                        case 2: hours = Tuple.Create(60, DateTime.Now.Hour); break;
                        case 3: hours = Tuple.Create(90, DateTime.Now.Hour); break;
                        case 4: hours = Tuple.Create(120, DateTime.Now.Hour); break;
                        case 5: hours = Tuple.Create(150, DateTime.Now.Hour); break;
                        case 6: hours = Tuple.Create(180, DateTime.Now.Hour); break;
                        case 7: hours = Tuple.Create(210, DateTime.Now.Hour); break;
                        case 8: hours = Tuple.Create(240, DateTime.Now.Hour); break;
                        case 9: hours = Tuple.Create(270, DateTime.Now.Hour); break;
                        case 10: hours = Tuple.Create(300, DateTime.Now.Hour); break;
                        case 11: hours = Tuple.Create(330, DateTime.Now.Hour); break;
                        case 12: hours = Tuple.Create(360, DateTime.Now.Hour); break;
                        default: hours = Tuple.Create(0, DateTime.Now.Hour); break;
                    }
                }
                else if (tmp == MinuteArrow)
                {
                    int buff = DateTime.Now.Minute;
                    int counter = 0;

                    while (counter != buff)
                    {
                        Tuple<int, int> tm = Tuple.Create(((int)mins.Item1) + 6, DateTime.Now.Minute);
                        mins = tm;
                        counter++;
                    }
                }
                else if (tmp == SecondsArrow)
                    secs = Tuple.Create((secs.Item1 * 6* DateTime.Now.Second),DateTime.Now.Second);
            }
        }

        private void Clock_Loaded(object sender, RoutedEventArgs e)
        {

            Calc();

            AnimBuilder();

        }
    }
}
