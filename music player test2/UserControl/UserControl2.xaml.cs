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
using MaterialDesignThemes.Wpf;
//
using MahApps.Metro.Controls;
//
using MahApps.Metro.IconPacks;
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
    /// Interaction logic for UserControl2.xaml
    /// </summary>
    public partial class UserControl2 
    {
        //
        string con = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\MusicPlayer";



        public UserControl2()
        {
            InitializeComponent();

            int index1 = 0;
            while (index1 < (App.Current.MainWindow as MainWindow).list_music.Count)
            {
                if ((App.Current.MainWindow as MainWindow).save_mu_id == (App.Current.MainWindow as MainWindow).list_music[index1].singer_id)
                {
                    //set picture music:
                    BitmapImage BitImg = new BitmapImage(new Uri((App.Current.MainWindow as MainWindow).list_music[index1].link_image));
                    img.Source = BitImg;

                    //
                    ImageBrush back = new ImageBrush(BitImg);
                    background.Background = back;

                    break;
                }
                index1++;
            }

            //--------------------------------------------------------------------------------------
            int index2 = 0;
            while (index2 < (App.Current.MainWindow as MainWindow).list_music.Count)
            {
                if ((App.Current.MainWindow as MainWindow).list_music[index2].singer_id == (App.Current.MainWindow as MainWindow).save_mu_id)
                {
                    //Grid grid = new Grid();
                    customm_grid grid = new customm_grid();
                    grid.Height = 50;
                    grid.Width = 1200;
                  //  grid.Background = Brushes.Gray;
                    grid.grid_id = index2;




                    //create button delete:
                    btn delete = new btn();
                    //delete.Content = "Delete";
                    delete.Cursor = Cursors.Hand;
                    delete.Width = 55;
                    delete.Height = 50;
                    delete.HorizontalAlignment = HorizontalAlignment.Right;
                    delete.Style = (Style)FindResource("MetroCircleButtonStyle");
                    PackIconMaterialLight icon_delete = new PackIconMaterialLight();
                    icon_delete.Kind = PackIconMaterialLightKind.Delete;
                    delete.Content = icon_delete;
                    delete.id = (App.Current.MainWindow as MainWindow).list_music[index2].music_id;

                    delete.Click += btn_delete_click;



                    //create button play:
                    btn play = new btn();
                    play.Content = "play";
                    play.Width = 50;
                    play.Height = 50;
                    play.Foreground = Brushes.Black;
                    play.HorizontalAlignment = HorizontalAlignment.Center;
                    play.Style = (Style)FindResource("MetroCircleButtonStyle");
                    PackIconMaterialLight icon_play = new PackIconMaterialLight();
                    icon_play.Kind = PackIconMaterialLightKind.Play;
                    play.Content = icon_play;
                    play.id = (App.Current.MainWindow as MainWindow).list_music[index2].music_id;

                    play.Click += btn_play_click;

                    //create singer textblock
                    TextBlock singer = new TextBlock();
                    singer.FontSize = 20;
                    singer.Text = (App.Current.MainWindow as MainWindow).list_music[index2].singer_name;
                    singer.Foreground = Brushes.Yellow;
                    singer.Style = (Style)FindResource("customFont");
                    singer.HorizontalAlignment = HorizontalAlignment.Left;
                    singer.VerticalAlignment = VerticalAlignment.Center;
                    singer.Margin = new Thickness(5, 5, 5, 5);


                    //create music name textblock:
                    TextBlock music_name = new TextBlock();
                    music_name.FontSize = 20;
                    music_name.Text = (App.Current.MainWindow as MainWindow).list_music[index2].music_name;
                    music_name.Foreground = Brushes.Yellow;
                    music_name.HorizontalAlignment = HorizontalAlignment.Left;
                    music_name.VerticalAlignment = VerticalAlignment.Center;
                    music_name.Style = (Style)FindResource("customFont");
                    music_name.Margin = new Thickness(140, 5, 5, 5);


                    grid.Children.Add(play);
                    grid.Children.Add(singer);
                    grid.Children.Add(music_name);
                    grid.Children.Add(delete);


                    list_view.Items.Add(grid);
                    
                }


            

                  index2++;

            }
        }


        public class customm_card : Card
        {
            public int id;
        }
        public class btn : Button
        {
            public int id;
        }

        public class customm_grid : Grid
        {
            public int grid_id;
        }





        //----------------------------------------------------------------------------------------------------------

        // button in the  list view :
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

        //---------------------------------------------------------------------------------------------------------

        public void btn_delete_click(Object sender, EventArgs e)
        {
            btn delete = sender as btn;

            int index = 0;

            while (index < (App.Current.MainWindow as MainWindow).list_music.Count)
            {
                if (delete.id == (App.Current.MainWindow as MainWindow).list_music[index].music_id)
                {
                    SqlConnection connection = new SqlConnection();
                    connection.ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename="+con+"\\Music_Bank_main.mdf;Integrated Security=True;Connect Timeout=30";

                    SqlCommand command_delete = new SqlCommand();
                    command_delete.CommandText = "DELETE FROM Music WHERE music_id='" + delete.id + "'";
                    command_delete.Connection = connection;

                    connection.Open();
                    command_delete.ExecuteNonQuery();
                    connection.Close();

                    (App.Current.MainWindow as MainWindow).list_music.RemoveAt(index);

                    (App.Current.MainWindow as MainWindow).ShowMessageAsync("آهنگ مورد نظر حذف شد", "");
                    (App.Current.MainWindow as MainWindow).Singer_music_frame.Content = new Singers_music();

                    break;


                }
                index++;
            }








        }
    }
}
