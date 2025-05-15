using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace FitnessApp
{
    public partial class AddActivityWindow : Window
    {
        private string connectionString = "Data Source=localhost;Initial Catalog=FitnessDB;Integrated Security=True";
        private int userId;

        public AddActivityWindow(int userId)
        {
            InitializeComponent();
            this.userId = userId;
        }

        private void SaveActivity_Click(object sender, RoutedEventArgs e)
        {
            string activityType = (ActivityTypeBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "";
            if (!double.TryParse(DurationBox.Text, out double duration) ||
                !double.TryParse(CaloriesBox.Text, out double calories) ||
                !int.TryParse(StepsBox.Text, out int steps))
            {
                MessageBox.Show("Bitte überprüfen Sie, ob alle Felder korrekt sind.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(@"INSERT INTO Activities (UserId, ActivityType, Duration, CaloriesBurned, Steps, Date)
                                                  VALUES (@UserId, @Type, @Duration, @Calories, @Steps, GETDATE())", conn);

                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@Type", activityType);
                cmd.Parameters.AddWithValue("@Duration", duration);
                cmd.Parameters.AddWithValue("@Calories", calories);
                cmd.Parameters.AddWithValue("@Steps", steps);

                cmd.ExecuteNonQuery();

                MessageBox.Show("Aktivität hinzugefügt!");
                this.Close();
            }
        }
    }
}
