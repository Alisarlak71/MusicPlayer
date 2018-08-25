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
using System.Data.SqlClient;
//
using Microsoft.Win32;
//
using System.Windows.Threading;
//
using MahApps.Metro;
//
using MahApps.Metro.Controls;
//
using MahApps.Metro.Controls.Dialogs;
//
using TinyLittleMvvm;
//
using System.Threading.Tasks;



namespace music_player_test2.UserControl
{
    /// <summary>
    /// Interaction logic for Add_music.xaml
    /// </summary>
    /// 

    

    public partial class Add_music 
    {
        private MediaPlayer mediaPlayer = new MediaPlayer();

       public string path;
       public string path_music;
        Dictionary<string,string> type_dic = new Dictionary<string,string>();
        Dictionary<string, string> singer_dic = new Dictionary<string, string>();

        //
        string con = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\MusicPlayer";

        //read singer name and type of music from data:
       public void read_database()
        {
            //-------------------------------------------------------------------
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename="+con+"\\Music_Bank_main.mdf;Integrated Security=True;Connect Timeout=30";
            //-------------------------------------------------------------------
            SqlCommand command_singerName = new SqlCommand();
            command_singerName.CommandText = "SELECT * FROM Singer";
            command_singerName.Connection = connection;

            SqlCommand command_typMusic = new SqlCommand();
            command_typMusic.CommandText = "SELECT * FROM typ";
            command_typMusic.Connection = connection;

            connection.Open();
            //---------------------------------------------------
            SqlDataReader reader_singerName;
            reader_singerName = command_singerName.ExecuteReader();
            while (reader_singerName.Read())
            {
                string str = reader_singerName["name"].ToString();
                string id = reader_singerName["id"].ToString();
                singer_dic.Add(str, id);

                singer_name.Items.Add(str);
            }
            reader_singerName.Close();
            //---------------------------------------------------

            SqlDataReader reader_typMusic;

            reader_typMusic = command_typMusic.ExecuteReader();
            while (reader_typMusic.Read())
            {
                string str = reader_typMusic["name"].ToString();
                string id = reader_typMusic["id"].ToString();
                type_dic.Add(str, id);
                
                type_music.Items.Add(str);
            }
            reader_typMusic.Close(); 
        }

//--------------------------------------------------------------------------------------------------------
        public Add_music()
        {
            InitializeComponent();
            btn_playmusic.IsEnabled = false;
                      
                      
            //read singer name and type of music from data
            read_database();

        }

        private void btn_openmusic_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog music = new OpenFileDialog();
            music.Filter = "Audio Files(*.mp3)|*mp3";

            if (music.ShowDialog() == true)
            {
                mediaPlayer.Open(new Uri(music.FileName));
                btn_playmusic.IsEnabled = true;
                 path_music = music.FileName.ToString();
           
               if( btn_playmusic.Content as string == "توقف")
               {
                   btn_playmusic.Content = "پخش موزیک";
               }
             
            }
//---------------------------------------------------------------------------------------------------
        }

        private void btn_openimage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog img = new OpenFileDialog();
            img.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
            if (img.ShowDialog() == true)
            {
                ImageBrush imge = new ImageBrush(new BitmapImage(new Uri(img.FileName)));
                music_img.Source = imge.ImageSource;
                 path = img.FileName.ToString();

            }

        }
        

        private void btn_playmusic_Click(object sender, RoutedEventArgs e)
        {
            
           if(btn_playmusic.Content as string=="پخش موزیک")
           {
               mediaPlayer.Play();
               btn_playmusic.Content = "توقف";
           }

          else if (btn_playmusic.Content as string == "توقف")
           {
               mediaPlayer.Pause();
               btn_playmusic.Content = "پخش موزیک";
           }

    }

        private void btn_playmusic_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void btn_savemusic_Click(object sender, RoutedEventArgs e)
        {
            if (music_name.Text == "")
            {
                error_name.Visibility = Visibility.Visible;
            }

            else if (type_music.Text == "")
            {
                error_type.Visibility = Visibility.Visible;
            }

            else if (singer_name.Text == "")
            {
                error_singer.Visibility = Visibility.Visible;
            }
            else if(path_music==null)
            {
                error_link_music.Visibility = Visibility.Visible;
            }

        

            else
            {
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename="+con+"\\Music_Bank_main.mdf;Integrated Security=True;Connect Timeout=30";



                SqlCommand command_tb_music = new SqlCommand();

                string typ_id = "";
                typ_id = type_dic[type_music.Text];


                string singer_id = "";
                singer_id = singer_dic[singer_name.Text];


                command_tb_music.CommandText = "insert into Music (typ_id , singer_id , Music_name , Link_music , liked)  values('" + typ_id + "','" + singer_id + "',N'" + music_name.Text + "',N'" + path_music + "',0)";
                command_tb_music.Connection = connection;


                SqlCommand command_tb_singer = new SqlCommand();
                command_tb_singer.CommandText = "update Singer set img=N'" + path + "' where id='" + singer_id + "'";
                command_tb_singer.Connection = connection;


                connection.Open();
                command_tb_music.ExecuteNonQuery();
                command_tb_singer.ExecuteNonQuery();



                SqlCommand command_get_musicid = new SqlCommand();
                command_get_musicid.CommandText = "SELECT music_id FROM Music where Music_name=N'" + music_name.Text + "'   and   Link_music=N'" + path_music + "'";
                command_get_musicid.Connection = connection;

                SqlDataReader reader_music_id;
                reader_music_id = command_get_musicid.ExecuteReader();
                reader_music_id.Read();
                int id = Convert.ToInt32(reader_music_id["music_id"]);

                reader_music_id.Close();

                connection.Close();

                (App.Current.MainWindow as MainWindow).list_music.Add(new MusicList { music_name = music_name.Text, singer_name = singer_name.Text, link_music = path_music, link_image = path, type_music = type_music.Text, liked = 0, music_id = id, singer_id = Convert.ToInt32(singer_dic[singer_name.Text]), typ_id = Convert.ToInt32(type_dic[type_music.Text]) });
                (App.Current.MainWindow as MainWindow).ShowMessageAsync("آهنگ مورد نظر ذخیره شد", "");
                //music_name.Text = "";
                //type_music.Text = "";
                //singer_name.Text = "";

            }
          



        }

        private void singer_name_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            error_singer.Visibility = Visibility.Hidden;
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename="+con+"\\Music_Bank_main.mdf;Integrated Security=True;Connect Timeout=30";

            SqlCommand command_singer = new SqlCommand();
            command_singer.CommandText = "SELECT *FROM Singer";
            command_singer.Connection = connection;

            connection.Open();
            SqlDataReader reader_singer;
            reader_singer = command_singer.ExecuteReader();

              singer_name.Text=singer_name.SelectedItem.ToString();
              
              
     
               
                while (reader_singer.Read())
                {
                    if (singer_name.Text == reader_singer["name"].ToString())
                    {
                        if (reader_singer["img"].ToString() == "")
                        {
                            break;
                        }
                        ImageBrush br = new ImageBrush(new BitmapImage(new Uri(reader_singer["img"].ToString())));
                        music_img.Source = br.ImageSource;
                        path = reader_singer["img"].ToString();
                        break;
                    }
                  
                }
                
                reader_singer.Close();
            

        }

        private void music_name_SelectionChanged(object sender, RoutedEventArgs e)
        {
            error_name.Visibility = Visibility.Hidden;

        }

        private void type_music_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            error_type.Visibility = Visibility.Hidden;
        }

    }
}
