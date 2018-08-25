using System;
using System.Collections.Generic;
using System.Linq;
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
using MahApps.Metro.Controls;
//
using System.Windows.Threading;
//
using System.IO;
//
using MahApps.Metro;
//
using MahApps.Metro.Controls;
//
using MahApps.Metro.Controls.Dialogs;
//
using TinyLittleMvvm;
using System.Threading.Tasks;
//
using System.Data.SqlClient;



namespace music_player_test2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        //
        string filePath = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\MusicPlayer\\numbers.txt";
        string con = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\MusicPlayer";



        public int flag_btn_play;
        public int flag_btn_stop;
        public string str_search;

        public List<MusicList> list_music = new List<MusicList>();
        public MusicList music_list = new MusicList();

        public List<string> save_file = new List<string>();
   
        public  int save_mu_id;

        //get mu id for set img in main player:
        public int  get_mu_id;

       public DispatcherTimer timer = new DispatcherTimer();
       public DispatcherTimer timer1 = new DispatcherTimer();
       public delegate void timerTick();
       timerTick tick;

 //---------------------------------------------------------------------------------------------------

        public void media_play()
        {

            if (get_mu_id == 0)
            {
                btn_img_play.Cursor = Cursors.Hand;
                ImageBrush br = new ImageBrush(new BitmapImage(new Uri("image/play-button (5).png", UriKind.Relative)));
                btn_img_play.Source = br.ImageSource;
                flag_btn_play = 1;
                (App.Current.MainWindow as MainWindow).ShowMessageAsync("اهنگی برای پخش موجود نیس", "");

           
            }

            else
            {
                //
                ImageBrush br = new ImageBrush(new BitmapImage(new Uri("image/pause.png", UriKind.Relative)));
                btn_img_play.Source = br.ImageSource;
                flag_btn_play = -1;


                //
                System.Threading.Thread.Sleep(400);
                if ((App.Current.MainWindow as MainWindow).music_list.mediaPlayer.NaturalDuration.HasTimeSpan == true)
                {
                    double duration = (App.Current.MainWindow as MainWindow).music_list.mediaPlayer.NaturalDuration.TimeSpan.TotalMilliseconds;
                    slider.Maximum = duration;
                }
                timer1.Start();
                timer.Start();



                //play music:
                slider.IsEnabled = true;

                (App.Current.MainWindow as MainWindow).music_list.mediaPlayer.Play();

                music_list.mediaPlayer.MediaEnded += mediaPlayer_MediaEnded;




                //set image in main player:
                int index = 0;
                while (index < (App.Current.MainWindow as MainWindow).list_music.Count)
                {
                    if (get_mu_id == (App.Current.MainWindow as MainWindow).list_music[index].music_id)
                    {
                        ImageBrush img = new ImageBrush(new BitmapImage(new Uri((App.Current.MainWindow as MainWindow).list_music[index].link_image)));
                        img_main_player.Source = img.ImageSource;
                        break;
                    }
                    index++;
                }
            }
        

        }

//----------------------------------------------------------------------------------------

        public void media_pause()
        {
            (App.Current.MainWindow as MainWindow).music_list.mediaPlayer.Pause();
        }

//------------------------------------------------------------------------------------

public async void error_click_mahapp ()
        {
            await this.ShowMessageAsync("Result", "Your choice was: {result}");
        }


        public MainWindow()
        {
            InitializeComponent();


            flag_btn_play = 0;
            flag_btn_stop = 0;
            timer1 = new DispatcherTimer();
            timer1.Interval = TimeSpan.FromSeconds(1);
            timer1.Tick += new EventHandler(timer1_Tick);
            tick = new timerTick(changeStatus);
            slider.AddHandler(Slider.PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(slider_MouseLeftButtonDown), true); 

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            contentFrame.NavigationService.Navigate(new music_player_test2.UserControl.All_music());
        }

//-----------------------------------------------------------------------------------
        void timer1_Tick(object sender, EventArgs e)
        {
           
            Dispatcher.Invoke(tick); }

//---------------------------------------------------------------------------------
        void changeStatus()
        {
       
               slider.Value = (App.Current.MainWindow as MainWindow).music_list.mediaPlayer.Position.TotalMilliseconds;

        }

//----------------------------------------------------------------------------------
     
        void timer_Tick(object sender, EventArgs e)
        {

     
            if ((App.Current.MainWindow as MainWindow).music_list.mediaPlayer.NaturalDuration.HasTimeSpan == true)
            {


                if ((App.Current.MainWindow as MainWindow).music_list.mediaPlayer.Source != null)
                    time_label.Content = String.Format("{0} / {1}", (App.Current.MainWindow as MainWindow).music_list.mediaPlayer.Position.ToString(@"mm\:ss"), (App.Current.MainWindow as MainWindow).music_list.mediaPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
                else
                    time_label.Content = "No file selected...";
            }
        }

//-------------------------------------------------------------------------------

        private void slider_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            if (get_mu_id != 0)
            {
                if (slider.Value > 0)
                {
                    Point p = e.GetPosition((IInputElement)sender);
                    double val = p.X * 100 / ((Slider)sender).ActualWidth;
                    long newPoz = (long)((double)(App.Current.MainWindow as MainWindow).music_list.mediaPlayer.NaturalDuration.TimeSpan.Ticks * val / 100);

                    (App.Current.MainWindow as MainWindow).music_list.mediaPlayer.Position = TimeSpan.FromTicks(newPoz) <= (App.Current.MainWindow as MainWindow).music_list.mediaPlayer.NaturalDuration.TimeSpan ? TimeSpan.FromTicks(newPoz) : (App.Current.MainWindow as MainWindow).music_list.mediaPlayer.Position;
                }
            }
         
        }

   


//-------------------------------------------------------------------------------
        public void mediaPlayer_MediaEnded(object sender,  EventArgs  e)
        {
            music_list.mediaPlayer.Position = TimeSpan.Zero;

            End_music_func(null,null);
        }

//--------------------------------------------------------------------------------
        private void End_music_func(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            timer1.Stop();

            music_list.mediaPlayer.Stop();
            ImageBrush br = new ImageBrush(new BitmapImage(new Uri("image/play.png", UriKind.Relative)));
            btn_img_play.Source = br.ImageSource;
            flag_btn_play = 1;

            slider.Value = 0;
            time_label.Content = "00:00";  
        }

//-------------------------------------------------------------------------------

        private void HamburgerMenuControl_OnItemClick(object sender, ItemClickEventArgs e)
        {
            try
            { //frame.Content = null;
                frame.Content = new music_player_test2.UserControl.All_music();

                //add_music_frame.Content = null;
                add_music_frame.Content = new music_player_test2.UserControl.Add_music();

                //favorite_music_frame.Content = null;

                favorite_music_frame.Content = new music_player_test2.UserControl.Favorite_music();

                //Singer_music_frame.Content = null;
                Singer_music_frame.Content = new music_player_test2.UserControl.Singers_music();

                //recent_music_frame.Content = null;
                recent_music_frame.Content = new music_player_test2.UserControl.Recent_paly_music();

                Theme_music_frame.Content = new music_player_test2.UserControl.Themes();

                //test for message box error mahapp:
                //  error_click_mahapp();

                flyout.IsOpen = true;
                contentFrame.Visibility = Visibility.Hidden;
                this.HamburgerMenuControl.Content = e.ClickedItem;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
               
        }

//-------------------------------------------------------------------------------

        private void btnPlay_MouseEnter(object sender, MouseEventArgs e)
        {
            if (flag_btn_play != -1)
            {
                btn_img_play.Cursor = Cursors.Hand;
                ImageBrush br = new ImageBrush(new BitmapImage(new Uri("image/play-button (5).png", UriKind.Relative)));
                btn_img_play.Source = br.ImageSource;
                flag_btn_play = 1;
            }
        }
//------------------------------------------------------------------------------------


        private void btnPlay_MouseLeave(object sender, MouseEventArgs e)
        {
      
            if (flag_btn_play != -1)
            {
                ImageBrush br = new ImageBrush(new BitmapImage(new Uri("image/play.png", UriKind.Relative)));
                btn_img_play.Source = br.ImageSource;
            }
        }
 //------------------------------------------------------------------------------------

        public void btnPlay_Clicks(object sender, RoutedEventArgs e)
        {
            flag_btn_play = -1 * (flag_btn_play);
            if (flag_btn_play == -1)
            {
                ImageBrush br = new ImageBrush(new BitmapImage(new Uri("image/pause.png", UriKind.Relative)));
                btn_img_play.Source = br.ImageSource;
                media_play();
            }
            else
            {
                btn_img_play.Cursor = Cursors.Hand;
                ImageBrush br = new ImageBrush(new BitmapImage(new Uri("image/play-button (5).png", UriKind.Relative)));
                btn_img_play.Source = br.ImageSource;
                flag_btn_play = 1;
                media_pause();
            }
        }

//------------------------------------------------------------------------------------
      
        private void btnStop_MouseEnter(object sender, MouseEventArgs e)
        {
            if (flag_btn_stop != -1)
            {
                btn_img_stop.Cursor = Cursors.Hand;
                ImageBrush br = new ImageBrush(new BitmapImage(new Uri("image/circle-stop-512.png", UriKind.Relative)));
                btn_img_stop.Source = br.ImageSource;
                flag_btn_stop = 1;

            }

        }
 //------------------------------------------------------------------------------------
    
        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            flag_btn_stop = -1 * (flag_btn_stop);
            if (flag_btn_stop == -1)
            {
                ImageBrush br = new ImageBrush(new BitmapImage(new Uri("image/256-256-7f74b69bcd480651e5a1fad3c73e845d.png", UriKind.Relative)));
                btn_img_stop.Source = br.ImageSource;
                (App.Current.MainWindow as MainWindow).music_list.mediaPlayer.Stop();

            }
            flag_btn_stop = -1 * (flag_btn_stop);

            if(flag_btn_play==-1)
            {
                ImageBrush br = new ImageBrush(new BitmapImage(new Uri("image/play.png", UriKind.Relative)));
                btn_img_play.Source = br.ImageSource;
                flag_btn_play = -1 * (flag_btn_play);
            }

            slider.Value = 0;

        }
 //------------------------------------------------------------------------------------
    
        private void btnStop_MouseLeave(object sender, MouseEventArgs e)
        {
            if (flag_btn_stop!= -1)
            {
               
                ImageBrush br = new ImageBrush(new BitmapImage(new Uri("image/256-256-7f74b69bcd480651e5a1fad3c73e845d.png", UriKind.Relative)));
                btn_img_stop.Source = br.ImageSource;
            }
        }
 //------------------------------------------------------------------------------------

        private void ChangeMediaVolume(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            (App.Current.MainWindow as MainWindow).music_list.mediaPlayer.Volume = (double)volumSlider.Value;
        }
 //------------------------------------------------------------------------------------
        private void btn_search_Click(object sender, RoutedEventArgs e)
        {

            //char test = ' ';
            //string[] word = text_box_search.Text.Split(test);
            //string temp = null;
            //int i = 0;


            //foreach (string s in word)
            //{
            //    if (s != "")
            //    {
            //        temp = temp + s + ' ';
            //    }
            //}

            //str_search = temp;
            str_search = text_box_search.Text;

            frame.Content = new UserControl.Search_music();
            add_music_frame.Content = new UserControl.Search_music();
            favorite_music_frame.Content = new UserControl.Search_music();
            Singer_music_frame.Content = new UserControl.Search_music();
            contentFrame.Content = new UserControl.Search_music();
            recent_music_frame.Content = new UserControl.Search_music();
           // Pop_music_frame.Content = new UserControl.Search_music();
            //madahi_frame.Content = new UserControl.Search_music();
            //sonati_music_frame.Content = new UserControl.Search_music();
            //other_music_frame.Content = new UserControl.Search_music();
            text_box_search.Text="";

          
           
            
        }

//-------------------------------------------------------------------------------------
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            int i=0;
            //Create file and add link_music in the file :
           
          
                while(i<save_file.Count)
                {
                    if (search_file(save_file[i]) == 0)
                    {
                        StreamWriter writer = File.AppendText(filePath);
                        writer.WriteLine(save_file[i]);
                        writer.Close();
                    }
                        i++;
                }

            

            // Shutdown the application.
            Application.Current.Shutdown();
       
        }

        private int search_file(string st)
        {
            StreamReader reader = new StreamReader(filePath);

            string line;

            while ((line = reader.ReadLine()) != null)
            {

                if (line == st)
                {
                    return 1;
                }


            }
            reader.Close();
            return 0;




        }

        private void TreeViewItem_Selected(object sender, RoutedEventArgs e)
        {
             // new music_player_test2.UserControl.pop_music();


        }

        private void Grid_MouseDown_pop(object sender, MouseButtonEventArgs e)
        {
            types_music.Content = new music_player_test2.UserControl.pop_music();
        }

        private void Grid_MouseDown_madahi(object sender, MouseButtonEventArgs e)
        {
            types_music.Content = new music_player_test2.UserControl.Madahi();

        }

        private void Grid_MouseDown_sonati(object sender, MouseButtonEventArgs e)
        {

            types_music.Content = new music_player_test2.UserControl.Sonati_music();
        }


   




    }
}
