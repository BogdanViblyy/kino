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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace kino
{
    public partial class new_user : Form
    {
        public new_user()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button1_MouseClick(object sender, MouseEventArgs e)
        {
            this.Hide();
            Login login = new Login();
            login.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["kinoConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Получаем значения из TextBox1 и TextBox2
                    string userName = textBox1.Text;
                    string password = textBox2.Text;

                    // Проверяем, существует ли пользователь с таким именем
                    string checkQuery = "SELECT COUNT(*) FROM users WHERE userName = @userName";
                    using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@userName", userName);
                        int userCount = (int)checkCommand.ExecuteScalar();

                        if (userCount > 0)
                        {
                            // Если пользователь уже существует, выводим сообщение об ошибке
                            MessageBox.Show("Пользователь с таким именем уже существует!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return; // Прерываем выполнение метода
                        }
                    }

                    // Если пользователь не существует, добавляем его в базу данных
                    string insertQuery = "INSERT INTO users (userName, password) VALUES (@userName, @password)";
                    using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                    {
                        // Добавляем параметры для защиты от SQL-инъекций
                        insertCommand.Parameters.AddWithValue("@userName", userName);
                        insertCommand.Parameters.AddWithValue("@password", password);

                        // Выполняем запрос
                        int rowsAffected = insertCommand.ExecuteNonQuery();

                        // Проверяем, была ли успешная вставка
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Пользователь успешно зарегистрирован!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Не удалось зарегистрировать пользователя.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
