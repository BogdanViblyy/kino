using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Configuration;

namespace kino
{
    public partial class Admin : Form
    {
        public Admin()
        {
            InitializeComponent();
        }

        private void Admin_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Проверяем, что нажатие было на строку (не на заголовок столбца)
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Используем индексы столбцов
                textBox1.Text = row.Cells[1].Value?.ToString(); // Первый столбец
                textBox2.Text = row.Cells[2].Value?.ToString(); // Второй столбец
                textBox3.Text = row.Cells[3].Value?.ToString(); // Третий столбец
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Получаем значения из текстовых полей
            string filmName = textBox1.Text;
            string filmYear = textBox2.Text;
            string filmImg = textBox3.Text;

            // Проверяем, что все поля заполнены
            if (string.IsNullOrEmpty(filmName) || string.IsNullOrEmpty(filmYear) || string.IsNullOrEmpty(filmImg))
            {
                MessageBox.Show("Заполните все поля перед добавлением фильма.");
                return;
            }

            // Строка подключения к базе данных
            string connectionString = ConfigurationManager.ConnectionStrings["kinoConnectionString"].ConnectionString;

            // SQL-запрос для добавления записи
            string query = "INSERT INTO film (filmName, filmYear, filmImg) VALUES (@filmName, @filmYear, @filmImg)";

            // Используем подключение к базе данных
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    // Открываем подключение
                    connection.Open();

                    // Создаем команду с SQL-запросом и подключением
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Добавляем параметры в запрос
                        command.Parameters.AddWithValue("@filmName", filmName);
                        command.Parameters.AddWithValue("@filmYear", filmYear);
                        command.Parameters.AddWithValue("@filmImg", filmImg);

                        // Выполняем запрос
                        int rowsAffected = command.ExecuteNonQuery();

                        // Проверяем, была ли добавлена запись
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Фильм успешно добавлен в базу данных.");
                        }
                        else
                        {
                            MessageBox.Show("Не удалось добавить фильм.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Обрабатываем ошибки
                    MessageBox.Show("Ошибка при добавлении фильма: " + ex.Message);
                }
            }
        
    }

        private void button3_Click(object sender, EventArgs e)
        {
            // Строка подключения к базе данных
            string connectionString = ConfigurationManager.ConnectionStrings["kinoConnectionString"].ConnectionString;

            // SQL-запрос для получения данных из таблицы film
            string query = "SELECT filmName, filmYear, filmImg FROM film";

            // Используем подключение к базе данных
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    // Открываем подключение
                    connection.Open();

                    // Создаем адаптер данных
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);

                    // Создаем DataTable для хранения данных
                    DataTable dataTable = new DataTable();

                    // Заполняем DataTable данными из базы
                    dataAdapter.Fill(dataTable);

                    // Привязываем DataTable к dataGridView1
                    dataGridView1.DataSource = dataTable;

                    // Сообщение об успешном обновлении
                    MessageBox.Show("Данные успешно обновлены.");
                }
                catch (Exception ex)
                {
                    // Обрабатываем ошибки
                    MessageBox.Show("Ошибка при обновлении данных: " + ex.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Получаем значение из текстового поля
            string filmName = textBox1.Text;

            // Проверяем, что поле не пустое
            if (string.IsNullOrEmpty(filmName))
            {
                MessageBox.Show("Введите название фильма для удаления.");
                return;
            }

            // Строка подключения к базе данных
            string connectionString = ConfigurationManager.ConnectionStrings["kinoConnectionString"].ConnectionString;

            // SQL-запрос для удаления записи
            string query = "DELETE FROM film WHERE filmName = @filmName";

            // Используем подключение к базе данных
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    // Открываем подключение
                    connection.Open();

                    // Создаем команду с SQL-запросом и подключением
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Добавляем параметр в запрос
                        command.Parameters.AddWithValue("@filmName", filmName);

                        // Выполняем запрос
                        int rowsAffected = command.ExecuteNonQuery();

                        // Проверяем, была ли удалена запись
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Фильм успешно удален из базы данных.");
                        }
                        else
                        {
                            MessageBox.Show("Фильм с таким названием не найден.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Обрабатываем ошибки
                    MessageBox.Show("Ошибка при удалении фильма: " + ex.Message);
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Используем индексы столбцов
                textBox1.Text = row.Cells[1].Value?.ToString(); // Первый столбец
                textBox2.Text = row.Cells[2].Value?.ToString(); // Второй столбец
                textBox3.Text = row.Cells[3].Value?.ToString(); // Третий столбец
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox1.Image = LoadImageFromUrl(textBox3.Text);
            }
        }
        private Image LoadImageFromUrl(string imageUrl)
        {
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    byte[] imageBytes = webClient.DownloadData(imageUrl);
                    using (MemoryStream ms = new MemoryStream(imageBytes))
                    {
                        return Image.FromStream(ms);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке изображения: " + ex.Message);
                return null;
            }
        }
    }
}
