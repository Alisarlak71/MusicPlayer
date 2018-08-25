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
using MahApps.Metro;
//
using MahApps.Metro.Controls;

namespace music_player_test2.UserControl
{
    /// <summary>
    /// Interaction logic for Themes.xaml
    /// </summary>
    public partial class Themes 
    {
        // change themes--------------------------------------------------------------------------------------
        public static readonly DependencyProperty ColorsProperty
         = DependencyProperty.Register("Colors",
                                       typeof(List<KeyValuePair<string, Color>>),
                                       typeof(MainWindow),
                                       new PropertyMetadata(default(List<KeyValuePair<string, Color>>)));

        public List<KeyValuePair<string, Color>> Colors
        {
            get { return (List<KeyValuePair<string, Color>>)GetValue(ColorsProperty); }
            set { SetValue(ColorsProperty, value); }
        }



        //--------------------------------------------------------------------------------------------------------



        public Themes()
        {
            InitializeComponent();


            this.DataContext = this;

            this.Colors = typeof(Colors)
                .GetProperties()
                .Where(prop => typeof(Color).IsAssignableFrom(prop.PropertyType))
                .Select(prop => new KeyValuePair<String, Color>(prop.Name, (Color)prop.GetValue(null)))
                .ToList();

            var theme = ThemeManager.DetectAppStyle(Application.Current);
            ThemeManager.ChangeAppStyle(Application.Current, theme.Item2, theme.Item1);


        }

    

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var theme = ThemeManager.DetectAppStyle(Application.Current);
            ThemeManager.ChangeAppStyle(Application.Current, theme.Item2, ThemeManager.GetAppTheme("Base" + ((Button)sender).Content));
        }

        private void AccentSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedAccent = AccentSelector.SelectedItem as Accent;
            if (selectedAccent != null)
            {
                var theme = ThemeManager.DetectAppStyle(Application.Current);
                ThemeManager.ChangeAppStyle(Application.Current, selectedAccent, theme.Item1);
              //  Application.Current.MainWindow.Activate();
            }

        }

        private void ColorsSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //var selectedColor = this.ColorsSelector.SelectedItem as KeyValuePair<string, Color>?;
            //if (selectedColor.HasValue)
            //{
            //    var theme = ThemeManager.DetectAppStyle(Application.Current);
            //    ThemeManagerHelper.CreateAppStyleBy(selectedColor.Value.Value, true);
            //    Application.Current.MainWindow.Activate();
            //}
        }
    }
}
