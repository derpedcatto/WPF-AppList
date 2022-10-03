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
using System.Windows.Shapes;

namespace WPF_AppList._4_Resources_Styles
{
    /// <summary>
    /// Interaction logic for StylesWindow.xaml
    /// </summary>
    public partial class StylesWindow : Window
    {
        public StylesWindow()
        {
            InitializeComponent();
        }


        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            var button = new Button() { Content = "New Button" };
            Field.Children.Add(button);
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            Style? style = this.FindResource("BgCentered") as Style;
            var label = new Label() 
            {
                Content = "New Label",
                Style = style
            };
            Field.Children.Add(label);
        }

        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            Style? style = this.FindResource("CenteredContent") as Style;
            var button = new Button()
            {
                Padding = new Thickness(20),
                Content = "Button 3",
                Style = style
            };
            Field.Children.Add(button);
        }

        private void Button4_Click(object sender, RoutedEventArgs e)
        {
            FontFamily? fontFamily = this.FindResource("Comic") as FontFamily;
            var button = new Button()
            {
                Content = "Button 4",
                FontFamily = fontFamily
            };
            Field.Children.Add(button);
        }
    }
}
