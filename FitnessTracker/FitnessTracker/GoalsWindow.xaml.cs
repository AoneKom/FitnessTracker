using System;
using System.Data.SqlClient;
using System.Windows;

namespace FitnessApp
{
    public partial class GoalsWindow : Window
    {
        private string connectionString = "Data Source=localhost;Initial Catalog=FitnessDB;Integrated Security=True";
        private int userId;

        public GoalsWindow(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            LoadGoals();
        }

        private void LoadGoals()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Цели
                SqlCommand cmd = new SqlCommand("SELECT CaloriesTarget, StepsTarget FROM Goals WHERE UserId=@UserId", conn);
                cmd.Parameters.AddWithValue("@UserId", userId);
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    CaloriesGoalBox.Text = reader["CaloriesTarget"].ToString();
                    StepsGoalBox.Text = reader["StepsTarget"].ToString();
                }
                reader.Close();

                // Прогресс
                cmd = new SqlCommand("SELECT SUM(CaloriesBurned) AS TotalCalories, SUM(Steps) AS TotalSteps FROM Activities WHERE UserId=@UserId AND CAST(Date AS DATE) = CAST(GETDATE() AS DATE)", conn);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@UserId", userId);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    double burned = reader["TotalCalories"] != DBNull.Value ? Convert.ToDouble(reader["TotalCalories"]) : 0;
                    int steps = reader["TotalSteps"] != DBNull.Value ? Convert.ToInt32(reader["TotalSteps"]) : 0;

                    double calGoal = string.IsNullOrEmpty(CaloriesGoalBox.Text) ? 1 : Convert.ToDouble(CaloriesGoalBox.Text);
                    double stepGoal = string.IsNullOrEmpty(StepsGoalBox.Text) ? 1 : Convert.ToDouble(StepsGoalBox.Text);

                    double calPercent = Math.Min(100, (burned / calGoal) * 100);
                    double stepPercent = Math.Min(100, (steps / stepGoal) * 100);

                    CaloriesProgressBar.Value = calPercent;
                    StepsProgressBar.Value = stepPercent;

                    CaloriesProgressText.Text = $"Kalorienverbrauch: {burned} / {calGoal}";
                    StepsProgressText.Text = $"Abgeschlossene Schritte: {steps} / {stepGoal}";
                }
            }
        }

        private void SaveGoals_Click(object sender, RoutedEventArgs e)
        {
            if (!double.TryParse(CaloriesGoalBox.Text, out double calGoal) ||
                !int.TryParse(StepsGoalBox.Text, out int stepGoal))
            {
                MessageBox.Show("Bitte geben Sie korrekte Zahlenwerte ein.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(@"
IF EXISTS (SELECT 1 FROM Goals WHERE UserId=@UserId)
    UPDATE Goals SET CaloriesTarget=@Calories, StepsTarget=@Steps WHERE UserId=@UserId
ELSE
    INSERT INTO Goals (UserId, CaloriesTarget, StepsTarget) VALUES (@UserId, @Calories, @Steps)", conn);

                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@Calories", calGoal);
                cmd.Parameters.AddWithValue("@Steps", stepGoal);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Tore gerettet!");
                LoadGoals();
            }
        }
    }
}


