using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPF_AppList._5_4_Homework
{
    /// <summary>
    /// Interaction logic for FormRegistrationWindow.xaml
    /// </summary>
    public partial class FormRegistrationWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public FormRegistrationWindow()
        {
            InitializeComponent();
        }

        #endregion


        #region Methods

        private void ConfirmRegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            bool cancelRegistration = false;
            TextBox[] textboxArray = { UsernameTextBox, EmailTextBox, PasswordTextBox };

            foreach (TextBox item in textboxArray)
            {
                if (item.Text == string.Empty)
                {
                    item.BorderBrush = Brushes.PaleVioletRed;
                    cancelRegistration = true;
                    ConfirmRegistrationButton.Background = Brushes.PaleVioletRed;
                }
                else
                {
                    item.BorderBrush = Brushes.Green;
                }
            }

            if (!cancelRegistration)
            {
                ConfirmRegistrationButton.Background = Brushes.Green;
                Task.Run(() =>
                {
                    MessageBox.Show("Registration completed!");
                });
            }
            // 
        }

        #endregion
    }
}