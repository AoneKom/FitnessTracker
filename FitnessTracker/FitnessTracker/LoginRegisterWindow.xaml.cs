using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace FitnessApp
{
    public partial class LoginRegisterWindow : Window
    {
        private string connectionString = "Data Source=localhost;Initial Catalog=FitnessDB;Integrated Security=True";


        public LoginRegisterWindow()
        {
            InitializeComponent();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text.Trim();
            string username = UsernameTextBox.Text.Trim();
            string password = PasswordBox.Password;

            if (email == "" || username == "" || password == "")
            {
                MessageTextBlock.Text = "Bitte füllen Sie alle Felder aus.";
                return;
            }

            string hashedPassword = HashPassword(password);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Users (Email, Username, PasswordHash) VALUES (@Email, @Username, @PasswordHash)", conn);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@PasswordHash", hashedPassword);

                try
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Registrierung erfolgreich!");
                }
                catch (SqlException ex)
                {
                    MessageTextBlock.Text = "Fehler bei der Registrierung: " + ex.Message;
                }
            }
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text.Trim();
            string password = PasswordBox.Password;
            string hashedPassword = HashPassword(password);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Users WHERE Username=@Username AND PasswordHash=@PasswordHash", conn);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@PasswordHash", hashedPassword);

                int count = (int)cmd.ExecuteScalar();
                if (count == 1)
                {
                    MessageBox.Show("Erfolgreich angemeldet!");
            
                }
                else
                {
                    MessageTextBlock.Text = "Falscher Benutzername oder falsches Passwort.";
                }
            }
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }
    }
}
