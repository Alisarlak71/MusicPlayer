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


namespace music_player_test2.UserControl
{
    /// <summary>
    /// Interaction logic for Favorite_music.xaml
    /// </summary>
    public partial class Favorite_music 
    {
        //
        string con = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\MusicPlayer";


        public Favorite_music()
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
                    (App.Current.MainWindow as MainWindow).save_file.Add(play.id.ToString());

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
            while (index < (App.Current.MainWindow as MainWindow).list_music.Count)
            {
                if (card.id == (App.Current.MainWindow as MainWindow).list_music[index].music_id)
                {
                    (App.Current.MainWindow as MainWindow).save_mu_id = card.id;
                    (App.Current.MainWindow as MainWindow).favorite_music_frame.Content = new UserControl1();


                    break;
                }

                index++;
            }

        }
        //------------------------------------------------------------------------------


        private void Addtolist()
        {
            listview.Items.Clear();
            int index = 0;
            while (index < (App.Current.MainWindow as MainWindow).list_music.Count)
            {
                if ((App.Current.MainWindow as MainWindow).list_music[index].liked == 1)
                {
                    //create card                                                                   
                    CustomButton card1 = new CustomButton();

                    card1.Width = 300;
                    card1.Height = 300;
                    card1.Cursor = Cursors.Hand;
                    card1.FlowDirection = FlowDirection.LeftToRight;

                    card1.id = (App.Current.MainWindow as MainWindow).list_music[index].music_id;
                    //

                    //  BitmapImage bitmap = new BitmapImage();
                    ImageBrush bimg;
                    bimg = new ImageBrush(new BitmapImage(new Uri((App.Current.MainWindow as MainWindow).list_music[index].link_image)));
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



                    //create button play:
                    //Button play = new Button();
                    btn play = new btn();
                    play.Content = "play";
                    play.Name = "test";
                    play.Width = 50;
                    play.Height = 50;
                    play.Foreground = Brushes.White;
                    play.HorizontalAlignment = HorizontalAlignment.Left;
                    play.Style = (Style)FindResource("MetroCircleButtonStyle");
                    PackIconMaterialLight icon_play = new PackIconMaterialLight();
                    icon_play.Kind = PackIconMaterialLightKind.Play;
                    play.Content = icon_play;
                    play.Visibility = Visibility.Hidden;
                    play.id = (App.Current.MainWindow as MainWindow).list_music[index].music_id;



                    play.Click += btn_play_click;




                    //create button liked:
                    btn like = new btn();
                    like.Width = 50;
                    like.Height = 50;
                    like.Foreground = Brushes.White;
                    like.HorizontalAlignment = HorizontalAlignment.Right;
                    like.Style = (Style)FindResource("MetroCircleButtonStyle");
                    PackIconMaterialLight icon_like = new PackIconMaterialLight();
                    icon_like.Kind = PackIconMaterialLightKind.Heart;
                    like.Content = icon_like;
                    like.Visibility = Visibility.Hidden;
                    like.id = (App.Current.MainWindow as MainWindow).list_music[index].music_id;


                    like.Click += btn_like_click;


                    temp.Children.Add(play);
                    temp.Children.Add(like);




                    //create singer textblock:
                    TextBlock singer = new TextBlock();
                    singer.FontSize = 15;
                    singer.Text = (App.Current.MainWindow as MainWindow).list_music[index].singer_name;
                    singer.Foreground = Brushes.White;
                    singer.HorizontalAlignment = HorizontalAlignment.Right;
                    singer.VerticalAlignment = VerticalAlignment.Bottom;
                    singer.Margin = new Thickness(5, 5, 5, 5);
                    singer.FontWeight = FontWeights.UltraBold;
                    singer.FontSize = 20;
                    singer.Foreground = Brushes.Yellow;
                    singer.Style = (Style)FindResource("customFont");
                    temp.Children.Add(singer);


                    //create music name textblock:


                    TextBlock music_name = new TextBlock();
                    music_name.FontSize = 15;
                    music_name.Text = (App.Current.MainWindow as MainWindow).list_music[index].music_name;
                    music_name.Foreground = Brushes.White;
                    music_name.HorizontalAlignment = HorizontalAlignment.Left;
                    music_name.VerticalAlignment = VerticalAlignment.Bottom;
                    music_name.Margin = new Thickness(5, 5, 5, 5);
                    music_name.FontWeight = FontWeights.UltraBold;
                    music_name.FontSize = 20;
                    music_name.Foreground = Brushes.Yellow;
                    music_name.Style = (Style)FindResource("customFont");
                    temp.Children.Add(music_name);

                    /////
                    card1.Content = temp;

                    listview.Items.Add(new Tile()
                    {
                        Name = card1,
                    });
                }
                    index++;
                
             


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
            




        private void UserFavorite_Loaded(object sender, RoutedEventArgs e)
        {
            Addtolist();
        }

   


    


    }
}
