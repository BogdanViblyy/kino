using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace kino
{
    public partial class User_buy_tickets : Form
    {
        private string filmName;
        private Dictionary<string, bool> selectedSeats = new Dictionary<string, bool>();
        private List<string> selectedSeatsList = new List<string>();

        public User_buy_tickets(string filmName)
        {
            InitializeComponent();
            this.filmName = filmName;
            this.BackColor = Color.DarkRed; // Изменение цвета фона
            LoadSeats();  // Загрузка статусов мест из БД
            CreateSeatButtons(); // Создание кнопок мест
            textBox1.Text = filmName;
            textBox1.ReadOnly = true;
            textBox1.Enabled = false;
        }

        private void LoadSeats()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["kinoConnectionString"].ConnectionString;
            string query = "SELECT seatName, seatStatus FROM Seat WHERE filmName = @filmName";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@filmName", filmName);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string seatName = reader["seatName"].ToString();
                        bool seatStatus = Convert.ToBoolean(reader["seatStatus"]);
                        selectedSeats[seatName] = seatStatus;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при загрузке мест: " + ex.Message);
                }
            }
        }

        private void CreateSeatButtons()
        {
            int startX = 50, startY = 50, buttonSize = 50, gap = 10;
            char[] rows = { 'A', 'B', 'C' };
            int columns = 5;

            foreach (char row in rows)
            {
                for (int col = 1; col <= columns; col++)
                {
                    string seatName = $"{row}{col}";
                    Button seatButton = new Button
                    {
                        Name = seatName,
                        Text = seatName,
                        Size = new Size(buttonSize, buttonSize),
                        Location = new Point(startX + (col - 1) * (buttonSize + gap), startY + (row - 'A') * (buttonSize + gap)),
                        BackColor = selectedSeats.ContainsKey(seatName) && selectedSeats[seatName] ? Color.Red : Color.Black,
                        ForeColor = Color.White,
                        Enabled = !(selectedSeats.ContainsKey(seatName) && selectedSeats[seatName])
                    };

                    seatButton.Click += SeatButton_Click;
                    this.Controls.Add(seatButton);
                }
            }

            // Кнопка бронирования билетов
            Button bookButton = new Button
            {
                Name = "button_buy",
                Text = "Book Tickets",
                Size = new Size(200, 40),
                Location = new Point(startX, startY + 250), // Переместил ниже
                BackColor = Color.Green,
                ForeColor = Color.White
            };
            bookButton.Click += button_buy_Click;
            this.Controls.Add(bookButton);

            // Кнопка возврата
            Button backButton = new Button
            {
                Name = "button18",
                Text = "Back",
                Size = new Size(100, 40),
                Location = new Point(startX, startY + 300),
                BackColor = Color.Gray,
                ForeColor = Color.White
            };
            backButton.Click += button18_Click;
            this.Controls.Add(backButton);
        }

        private void SeatButton_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton == null) return;

            string seatName = clickedButton.Name;
            if (selectedSeatsList.Contains(seatName))
            {
                selectedSeatsList.Remove(seatName);
                clickedButton.BackColor = Color.Black; 
            }
            else
            {
                selectedSeatsList.Add(seatName);
                clickedButton.BackColor = Color.Gray; 
            }
        }

        private void button_buy_Click(object sender, EventArgs e)
        {
            if (selectedSeatsList.Count == 0)
            {
                MessageBox.Show("Выберите хотя бы одно место перед бронированием.");
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["kinoConnectionString"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    foreach (string seatName in selectedSeatsList)
                    {
                        string query = "UPDATE Seat SET seatStatus = 1 WHERE seatName = @seatName AND filmName = @filmName";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@seatName", seatName);
                        command.Parameters.AddWithValue("@filmName", filmName);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при бронировании мест: " + ex.Message);
                    return;
                }
            }

            MessageBox.Show("Билеты успешно забронированы!");
            selectedSeatsList.Clear();
            RefreshSeats();
        }

        private void RefreshSeats()
        {
            LoadSeats();
            foreach (Control control in this.Controls)
            {
                if (control is Button btn && btn.Name.Length == 2)
                {
                    string seatName = btn.Name;
                    btn.BackColor = selectedSeats.ContainsKey(seatName) && selectedSeats[seatName] ? Color.Red : Color.Black;
                    btn.Enabled = !(selectedSeats.ContainsKey(seatName) && selectedSeats[seatName]);
                }
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            this.Hide();
            User_choose user_Choose = new User_choose();
            user_Choose.Show();
        }
    }
}