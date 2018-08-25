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
//
using MahApps.Metro.IconPacks;
//
using MaterialDesignThemes.Wpf;
//
using MahApps.Metro.Controls;
//
using System.Data.SqlClient;
//
using MahApps.Metro.Controls.Dialogs;
//
using TinyLittleMvvm;
//
using System.Threading.Tasks;


namespace music_player_test2.UserControl
{
    /// <summary>
    /// Interaction logic for Singers_music.xaml
    /// </summary>
    public partial class Singers_music 
    {

        //
        string con = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\MusicPlayer";


        public Singers_music()
        {
            InitializeComponent();

        }

        public void btn_play_click(Object sender, EventArgs e)
        {
            btn play = sender as btn;

            int index = 0;
            while (index < (App.Current.MainWindow as MainWindow).list_music.Count)
            {
                if (play.id == (App.Current.MainWindow as MainWindow).list_music[index].music_id)
                {
                    (App.Current.MainWindow as MainWindow).music_list.mediaPlayer.Open(new Uri((App.Current.MainWindow as MainWindow).list_music[index].link_music));

                    //
                    (App.Current.MainWindow as MainWindow).get_mu_id = play.id;
                    break;
                }
                index++;
            }

            (App.Current.MainWindow as MainWindow).media_play();

        }

        //------------------------------------------------------------------------

        public void btn_like_click(Object sender, EventArgs e)
        {
            btn like = sender as btn;

            int index = 0;
            while (index < (App.Current.MainWindow as MainWindow).list_music.Count)
            {
                if (like.id == (App.Current.MainWindow as MainWindow).list_music[index].music_id)
                {
                    // liked music:
                    if ((App.Current.MainWindow as MainWindow).list_music[index].liked == 0)
                    {
                        SqlConnection connection = new SqlConnection();
                        connection.ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename="+con+"\\Music_Bank_main.mdf;Integrated Security=True;Connect Timeout=30";

                        SqlCommand command_like = new SqlCommand();
                        command_like.CommandText = "update Music set liked=1 where music_id= '" + (App.Current.MainWindow as MainWindow).list_music[index].music_id + "'";
                        command_like.Connection = connection;

                        connection.Open();
                        command_like.ExecuteNonQuery();
                        connection.Close();

                        (App.Current.MainWindow as MainWindow).list_music[index].liked = 1;
                        break;
                    }
                    // disliked music:
                    else if ((App.Current.MainWindow as MainWindow).list_music[index].liked == 1)
                    {
                        SqlConnection connection = new SqlConnection();
                        connection.ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename="+con+"\\Music_Bank_main.mdf;Integrated Security=True;Connect Timeout=30";

                        SqlCommand command_like = new SqlCommand();
                        command_like.CommandText = "update Music set liked=0 where music_id= '" + (App.Current.MainWindow as MainWindow).list_music[index].music_id + "'";
                        command_like.Connection = connection;

                        connection.Open();
                        command_like.ExecuteNonQuery();
                        connection.Close();
                        (App.Current.MainWindow as MainWindow).list_music[index].liked = 0;

                        break;
                    }

                }
                index++;
            }

        }

        //-----------------------------------------------------------------------------



        public void card1_enter(Object sender, EventArgs e)
        {
            CustomButton c1 = sender as CustomButton;
            foreach (object o in LogicalTreeHelper.GetChildren(c1))
            {
                foreach (object o2 in LogicalTreeHelper.GetChildren(o as Grid))
                {
                    if (o2 is Button)
                    {

                        (o2 as Button).Visibility = System.Windows.Visibility.Visible;
                    }
                }
            }
        }

        //--------------------------------------------------------------------------

        public void card1_leave(Object sender, EventArgs e)
        {

            CustomButton c1 = sender as CustomButton;
            foreach (object o in LogicalTreeHelper.GetChildren(c1))
            {
                foreach (object o2 in LogicalTreeHelper.GetChildren(o as Grid))
                {
                    if (o2 is Button)
                    {

                        (o2 as Button).Visibility = System.Windows.Visibility.Hidden;
                    }
                }
            }

        }
        //--------------------------------------------------------------------------------------------

        public void btn_card1_click(Object sender, EventArgs e)
        {
            CustomButton card = sender as CustomButton;
            int index = 0;
            int counter = 0;
            while (index < (App.Current.MainWindow as MainWindow).list_music.Count)
            {
                if (card.id == (App.Current.MainWindow as MainWindow).list_music[index].singer_id)
                {
                    (App.Current.MainWindow as MainWindow).save_mu_id = card.id;
                    (App.Current.MainWindow as MainWindow).Singer_music_frame.Content = new UserControl2();
                    counter++;

                    break;
                }
                



                index++;
            }
            if(counter==0)   
                {
                    (App.Current.MainWindow as MainWindow).ShowMessageAsync("آهنگی وجود ندارد","");
                }
        }
        //------------------------------------------------------------------------------




        private void Addtolist()
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename="+con+"\\Music_Bank_main.mdf;Integrated Security=True;Connect Timeout=30";

            SqlCommand command_singer = new SqlCommand();
            command_singer.CommandText = "SELECT *FROM Singer";
            command_singer.Connection = connection;

            connection.Open();
            SqlDataReader reader_singer;
            reader_singer = command_singer.ExecuteReader();


            listview.Items.Clear();
            while (reader_singer.Read())
            {
                if (reader_singer["img"].ToString() == "")
                {
                    break;
                }

                else
                {
                    //create card                                                                   
                    CustomButton card1 = new CustomButton();

                    card1.Width = 300;
                    card1.Height = 300;
                    card1.Cursor = Cursors.Hand;
                    card1.FlowDirection = FlowDirection.LeftToRight;

                    card1.id = Convert.ToInt32(reader_singer["id"]);
                    //

                    //  BitmapImage bitmap = new BitmapImage();
                    ImageBrush bimg;
                    bimg = new ImageBrush(new BitmapImage(new Uri(reader_singer["img"].ToString())));
                    card1.Background = bimg;

                    card1.MouseEnter += card1_enter;
                    card1.MouseLeave += card1_leave;
                    card1.MouseLeftButtonDown += btn_card1_click;

                    /// transparent background and wrapper tag
                    /// create Grid:
                    Grid temp = new Grid();
                    temp.VerticalAlignment = VerticalAlignment.Bottom;
                    temp.FlowDirection = FlowDirection.LeftToRight;
                    temp.Height = 100;
                    temp.Background = (Brush)FindResource("MytransparentBackground");




                    //create singer textblock:
                    TextBlock singer = new TextBlock();
                    singer.FontSize = 15;
                    singer.Text = reader_singer["name"].ToString();
                    singer.Foreground = Brushes.White;
                    singer.HorizontalAlignment = HorizontalAlignment.Center;
                    singer.VerticalAlignment = VerticalAlignment.Top;
                    singer.Margin = new Thickness(5, 5, 5, 5);
                    singer.FontWeight = FontWeights.UltraBold;
                    singer.FontSize = 30;
                    singer.Foreground = Brushes.Yellow;
                    singer.Style = (Style)FindResource("customFont");

                    temp.Children.Add(singer);



                    /////
                    card1.Content = temp;

                    listview.Items.Add(new Tile()
                    {
                        Name = card1,
                    });

                }
            }
        }
        public class Tile
        {
            public CustomButton Name { get; set; }
        }
        public class CustomButton : Card
        {
            public int id;
        }

        public class btn : Button
        {
            public int id;
        }



        private void Singer_music_Loaded_1(object sender, RoutedEventArgs e)
        {
            Addtolist();

        }


    }
}
