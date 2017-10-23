using System;
using System.Media;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace AnalogClock
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
         
        Tuple<int, int> secs = Tuple.Create(0, 6);

        static string path = Environment.CurrentDirectory + @"\Sound\clockTick.wav";
        SoundPlayer player = new SoundPlayer(path);
         
        DoubleAnimation secAnim; 
         
        TimerCallback timCall;

        public MainWindow()
        {
            InitializeComponent();
            timCall = new TimerCallback(refreshAnimation); 
        }

        private void AnimBuilder()
        {
            secAnim = new DoubleAnimation
            {
                Name = "secAnim",
                From = secs.Item1,
                To = secs.Item2,
                Duration = TimeSpan.FromMilliseconds(875)

            };

            secAnim.Completed += SecAnim_Completed;
             
            secTrans.BeginAnimation(RotateTransform.AngleProperty, secAnim);

            player.Play();
        }

       
        private void SecAnim_Completed(object sender, EventArgs e)
        {

            secs = Tuple.Create(secs.Item1 +6, secs.Item2+6);

            object obj= new object();
            Timer timer = new Timer(timCall, obj, 150, 0);

        }
        

        void refreshAnimation(object obj)
        { 
            if(secs.Item1 >= 360)
                secs = Tuple.Create(0, 6);
                         
            Dispatcher.Invoke(() =>
            {
                secAnim.From = secs.Item1;
                secAnim.To = secs.Item2;
                secAnim.Duration = TimeSpan.FromMilliseconds(875);
                secTrans.BeginAnimation(RotateTransform.AngleProperty, secAnim);
                player.Play();

            }); 
        }

        private void Calc()
        {
            foreach (Rectangle tmp in Clock.Children)
            {
                if (tmp == SecondsArrow)
                    secs = Tuple.Create((6 * DateTime.Now.Second), (6 * DateTime.Now.Second)+ 6);
            }
        }

        private void Clock_Loaded(object sender, RoutedEventArgs e)
        {

            Calc();

            AnimBuilder();

        }
    }
}
