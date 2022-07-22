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

using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using MaterialDesignThemes.Wpf;


namespace Medically
{
    /// <summary>
    /// Interaction logic for UserLogin.xaml
    /// </summary>
    public partial class UserLogin : Window
    {
        SqlConnection conn;
        string connectionString; 
        SqlDataReader reader;
        SqlCommand cmd;
        public UserLogin()
        {
            InitializeComponent();

            connectionString = ConfigurationManager.ConnectionStrings["Medically.Properties.Settings.Medically_dbConnectionString"].ConnectionString;
        }

        public bool IsDarkTheme { get; set; }
        private readonly PaletteHelper paletteHelper = new PaletteHelper();

        private void toggleTheme(object sender, RoutedEventArgs e)
        {
            ITheme theme = paletteHelper.GetTheme();

            if (IsDarkTheme = theme.GetBaseTheme() == BaseTheme.Dark)
            {
                IsDarkTheme = false;
                theme.SetBaseTheme(Theme.Light);
            }
            else
            {
                IsDarkTheme = true;
                theme.SetBaseTheme(Theme.Dark);
            }
            paletteHelper.SetTheme(theme);
        }

        private void exitApp(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            errorMsgTextBox.Visibility = Visibility.Hidden;
            errorMsgTextBox.Text = "";
            conn = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\Users\15854\Documents\Resume_Projects\CS\Medical.ly\Medically\Medically\Medically_db.mdf; Integrated Security = True");
            conn.Open();
            {
                if (txtPassword.Password.ToString() != string.Empty || txtUsername.Text != string.Empty)
                {

                    cmd = new SqlCommand("select * from Users where userID='" + txtUsername.Text + "' and userPass='" + txtPassword.Password.ToString() + "' and userPosition='" + comboBoxJob.Text + "'", conn);
                    reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        string userFirst = (string)reader["userFirstName"];
                        string userLast = (string)reader["userLastName"];
                        reader.Close();
                        Homepage homepage = new Homepage();
                        //homepage.userID_label.Content = txtUsername.Text;
                        homepage.userID_label.Content = txtUsername.Text + " | " + userFirst + " " + userLast;
                        homepage.userPosition_label.Content = comboBoxJob.Text;
                        this.Close();
                        conn.Close();
                        homepage.Show();
                    }
                    else
                    {
                        reader.Close();
                        //MessageBox.Show("ERROR: No Account avilable with this username, password, and position ", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        errorMsgTextBox.Text = "ERROR: Incorrect userID, password, or position";
                        errorMsgTextBox.Visibility = Visibility.Visible;
                    }

                }
                else
                {
                    //MessageBox.Show("Please enter value in all field.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    errorMsgTextBox.Text = "ERROR: Please enter a value in all fields.";
                    errorMsgTextBox.Visibility = Visibility.Visible;
                }
            }
        }

        private void createAccBTN_Click(object sender, RoutedEventArgs e)
        {
            UserSignUp newUser = new UserSignUp();
            this.Close();
            newUser.Show();
        }
    }
}
