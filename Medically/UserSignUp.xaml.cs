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

using MaterialDesignThemes.Wpf;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Medically
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class UserSignUp : Window
    {
        SqlConnection conn;
        SqlDataReader reader;
        string connectionString;
        public UserSignUp()
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

        private void cancelSignUpBTN_Click(object sender, RoutedEventArgs e)
        {
            UserLogin userLoginPage = new UserLogin();
            this.Close();
            
            userLoginPage.ShowDialog();
        }

        private void SignUpBtn_Click(object sender, RoutedEventArgs e)
        {
            conn = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\Users\15854\Documents\Resume_Projects\CS\Medical.ly\Medically\Medically\Medically_db.mdf; Integrated Security = True");

            string username = txtNewUserID.Text;
            string password = txtNewUserPass.Password.ToString();
            string firstName = txtFirstName.Text;
            string lastName = txtLastName.Text;
            string DOB = dateUserDOB.Text;
            string job = comboBoxUserPosition.Text;

            if (txtConNewUserPass.Password.ToString() != string.Empty || txtNewUserPass.Password.ToString() != string.Empty || txtNewUserID.Text != string.Empty || txtFirstName.Text != string.Empty || txtLastName.Text != string.Empty || dateUserDOB.Text != string.Empty || comboBoxUserPosition.Text != string.Empty) 
            {
                if (txtNewUserPass.Password.ToString() == txtConNewUserPass.Password.ToString())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("select * from Users where userID='" + txtNewUserID.Text + "'", conn);
                    reader = cmd.ExecuteReader();
                    
                    if (reader.Read())
                    {
                        reader.Close();
                        MessageBox.Show("User ID already exists please try another", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        reader.Close();

                        using (cmd = new SqlCommand("INSERT INTO Users(userID, userPass, userFirstName, userLastName, userDOB, userPosition) VALUES (@userID, @userPass, @userFirstName, @userLastName, @userDOB, @userPosition)", conn))
                        {
                            cmd.Parameters.AddWithValue("@userID", username);
                            cmd.Parameters.AddWithValue("@userPass", password);
                            cmd.Parameters.AddWithValue("@userFirstName", firstName);
                            cmd.Parameters.AddWithValue("@userLastName", lastName);
                            cmd.Parameters.AddWithValue("@userDOB", DOB);
                            cmd.Parameters.AddWithValue("@userPosition", job);

                            int result = cmd.ExecuteNonQuery();
                            conn.Close();
                            // Check Error
                            if (result <= 0)
                                MessageBox.Show("Error Creating Account", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            else
                            {

                                MessageBox.Show("Account has been created", "Success", MessageBoxButton.OK, MessageBoxImage.Exclamation);

                                UserLogin userLoginPage = new UserLogin();
                                this.Close();
                                userLoginPage.Show();
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please make sure both password inputs MATCH", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please make sure ALL fields have been filled out", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
