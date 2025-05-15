using System;
using System.Data.SqlClient;
using System.Windows;

namespace FitnessApp
{
    public partial class DashboardWindow1 : Window
    {
        private string connectionString = "Data Source=localhost;Initial Catalog=FitnessDB;Integrated Security=True";
        private int userId;

        public DashboardWindow1(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            LoadDashboardData();
        }

        private void LoadDashboardData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT TOP 1 ActivityLevel, CaloriesBurned, Weight, Pulse, Pressure FROM UserStats WHERE UserId = @UserId ORDER BY Date DESC", conn);
                cmd.Parameters.AddWithValue("@UserId", userId);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    ActivityProgressBar.Value = Convert.ToDouble(reader["ActivityLevel"]);
                    CaloriesTextBlock.Text = reader["CaloriesBurned"] + " kcal";
                    WeightTextBlock.Text = reader["Weight"] + " kg";
                    HealthTextBlock.Text = $"{reader["Pulse"]} bpm, {reader["Pressure"]} mm Hg. st.";
                }
                else
                {
                    MessageBox.Show("Keine Daten. Aktivität hinzufügen.");
                }
            }
        }

        private void Goals_Click(object sender, RoutedEventArgs e)
        {
            GoalsWindow goalsWindow = new GoalsWindow(userId);
            goalsWindow.Show();
        }

        private void AddActivity_Click(object sender, RoutedEventArgs e)
        {
            AddActivityWindow addActivityWindow = new AddActivityWindow(userId);
            addActivityWindow.Show();
        }
    }
}


