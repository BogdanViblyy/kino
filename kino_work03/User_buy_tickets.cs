using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kino
{
    public partial class User_buy_tickets : Form
    {
        private string filmName; // Поле для хранения названия фильма
        private Dictionary<string, bool> selectedSeats = new Dictionary<string, bool>(); // Словарь для хранения статуса мест
        private List<string> selectedSeatsList = new List<string>(); // Список для хранения выбранных мест

        // Конструктор, принимающий filmName
        public User_buy_tickets(string filmName)
        {
            InitializeComponent();
            this.filmName = filmName; // Инициализируем поле
            LoadSeats(); // Загружаем данные о местах при создании формы
            AssignButtonClickHandlers(); // Назначаем обработчики для кнопок
        }

        // Метод для загрузки данных о местах из базы данных
        private void LoadSeats()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["kinoConnectionString"].ConnectionString;
            string query = "SELECT seatName, seatStatus FROM Seat WHERE filmName = @filmName";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@filmName", filmName);
                textBox1.Text = filmName;

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string seatName = reader["seatName"].ToString();
                        bool seatStatus = Convert.ToBoolean(reader["seatStatus"]);

                        // Добавляем место в словарь
                        selectedSeats[seatName] = seatStatus;

                        // Окрашиваем кнопку в зависимости от статуса места
                        ColorButton(seatName, seatStatus);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при загрузке данных: " + ex.Message);
                }
            }
        }

        // Метод для окрашивания кнопки в зависимости от статуса места
        private void ColorButton(string seatName, bool seatStatus)
        {
            // Окрашиваем кнопку в зависимости от статуса места
            Button button = this.Controls[seatName] as Button;
            if (button != null)
            {
                if (seatStatus != true)
                {
                    button.BackColor = Color.Green; // Место свободно
                }
                else
                {
                    button.BackColor = Color.Red; // Место занято
                }
            }
        }

        // Метод для назначения обработчиков событий для всех кнопок
        private void AssignButtonClickHandlers()
        {
            foreach (Control control in this.Controls)
            {
                if (control is Button) // Проверяем, является ли элемент кнопкой
                {
                    if (control == button_buy) // Пропускаем кнопку button_buy
                    {
                        continue; // Переходим к следующей итерации цикла
                    }

                    Button button = (Button)control;
                    button.Click += Button_Click; // Назначаем обработчик события Click
                }
            }
        }

        // Обработчик события Click для кнопок
        private void Button_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button; // Получаем кнопку, которая была нажата
            if (clickedButton != null)
            {
                string seatName = clickedButton.Name; // Получаем имя кнопки

                // Проверяем, была ли кнопка уже выбрана
                bool isSelected = selectedSeatsList.Contains(seatName);

                if (isSelected)
                {
                    // Если кнопка уже была выбрана, возвращаем её исходный цвет
                    bool seatStatus = selectedSeats[seatName];
                    ColorButton(seatName, seatStatus);

                    // Удаляем имя кнопки из списка выбранных мест
                    selectedSeatsList.Remove(seatName);
                }
                else
                {
                    // Если кнопка не была выбрана, меняем её цвет на серый
                    clickedButton.BackColor = Color.Gray;

                    // Добавляем имя кнопки в список выбранных мест
                    selectedSeatsList.Add(seatName);
                }

                // Выводим информацию для отладки
                Console.WriteLine($"Кнопка {seatName} нажата. Выбранные места: " + string.Join(", ", selectedSeatsList));
            }
        }

        // Метод для обновления статуса мест в базе данных
        private void UpdateSeatsStatus()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["kinoConnectionString"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    foreach (string seatName in selectedSeatsList)
                    {
                        string query = "UPDATE Seat SET seatStatus = @seatStatus WHERE seatName = @seatName AND filmName = @filmName";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@seatStatus", true); // Место занято
                        command.Parameters.AddWithValue("@seatName", seatName);
                        command.Parameters.AddWithValue("@filmName", filmName);

                        command.ExecuteNonQuery();

                        // Обновляем статус места в словаре
                        selectedSeats[seatName] = true;

                        // Обновляем цвет кнопки
                        ColorButton(seatName, true);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при обновлении статуса мест: " + ex.Message);
                }
            }
        }

        // Метод для отображения сообщения о покупке
        private void ShowPurchaseMessage()
        {
            // Создаем строку с названиями выбранных мест (без "button")
            string seats = string.Join(", ", selectedSeatsList.Select(s => s.Replace("button", "")));

            // Создаем новое окно с сообщением
            Form purchaseMessageForm = new Form
            {
                Text = "Покупка билетов",
                Size = new Size(400, 200),
                StartPosition = FormStartPosition.CenterScreen
            };

            Label messageLabel = new Label
            {
                Text = $"Вы купили билеты на места: {seats}",
                AutoSize = true,
                Location = new Point(50, 50)
            };

            purchaseMessageForm.Controls.Add(messageLabel);
            purchaseMessageForm.ShowDialog();
        }

        // Обработчик для кнопки "Назад"
        private void button18_Click(object sender, EventArgs e)
        {
            this.Hide();
            User_choose user_Choose = new User_choose();
            user_Choose.Show();
        }

        // Обработчик для кнопки "Купить"
        private void button_buy_Click_1(object sender, EventArgs e)
        {
            if (selectedSeatsList.Count == 0)
            {
                MessageBox.Show("Выберите хотя бы одно место перед покупкой.");
                return;
            }

            // Обновляем статус мест в базе данных
            UpdateSeatsStatus();

            // Отображаем сообщение о покупке
            ShowPurchaseMessage();

            // Очищаем список выбранных мест
            selectedSeatsList.Clear();

            // Перезагружаем данные о местах
            LoadSeats();
        }

        // Обработчики для других кнопок (заглушки)
        private void button2_Click(object sender, EventArgs e) { }
        private void button7_Click(object sender, EventArgs e) { }
        private void button9_Click(object sender, EventArgs e) { }
        private void button22_Click(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
    }
}