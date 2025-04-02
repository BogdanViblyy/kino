using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows.Forms;
using System.Drawing;

namespace kino
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            CustomizeUI();
        }

        private void CustomizeUI()
        {
            this.BackColor = Color.DarkRed;
            this.ForeColor = Color.White;
            this.Font = new Font("Arial", 10, FontStyle.Bold);

            foreach (Control c in this.Controls)
            {
                if (c is Label lbl)
                {
                    lbl.ForeColor = Color.White;
                }
                else if (c is Button btn)
                {
                    btn.BackColor = Color.Black;
                    btn.ForeColor = Color.White;
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderSize = 1;
                    btn.FlatAppearance.BorderColor = Color.White;


                    if (btn.Name == "buttonLogin")
                    {
                        btn.Click += buttonLogin_Click;
                    }
                    else if (btn.Name == "buttonRegister")
                    {
                        btn.Click += buttonRegister_Click;
                    }
                }
                else if (c is TextBox txt)
                {
                    txt.BackColor = Color.Black;
                    txt.ForeColor = Color.White;
                    txt.BorderStyle = BorderStyle.FixedSingle;
                }
            }
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            string login = Login_txt.Text.Trim();
            string password = Password_txt.Text.Trim();

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите логин и пароль", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (login != "Sign Up")
            {
                LoginUser(login, password);
            }
            else
            {
                SignUpUser(login, password);
            }
        }


        private void buttonRegister_Click(object sender, EventArgs e)
        {
            string login = Login_txt.Text.Trim();
            string password = Password_txt.Text.Trim();

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите логин и пароль", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SignUpUser(login, password);
        }


        private void LoginUser(string login, string password)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["kinoConnectionString"]?.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT COUNT(1) FROM users WHERE userName = @Login AND password = @Password";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Login", login);
                        command.Parameters.AddWithValue("@Password", password);

                        int userCount = Convert.ToInt32(command.ExecuteScalar());

                        if (userCount == 1)
                        {
                            if (login == "Admin" && password == "123")
                            {
                                MessageBox.Show("Вы вошли как Admin!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.Hide();
                                Admin Admin = new Admin();
                                Admin.Show();
                                return;
                            }
                            else
                            {
                                MessageBox.Show("Вы успешно вошли в систему!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.Hide();
                                User_choose user_Choose = new User_choose();
                                user_Choose.Show();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Неверный логин или пароль", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void SignUpUser(string login, string password)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["kinoConnectionString"]?.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string checkQuery = "SELECT COUNT(1) FROM users WHERE userName = @Login";
                    using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@Login", login);
                        int userCount = Convert.ToInt32(checkCommand.ExecuteScalar());

                        if (userCount > 0)
                        {
                            MessageBox.Show("Этот логин уже занят!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    string insertQuery = "INSERT INTO users (userName, password) VALUES (@Login, @Password)";
                    using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                    {
                        insertCommand.Parameters.AddWithValue("@Login", login);
                        insertCommand.Parameters.AddWithValue("@Password", password);

                        int rowsAffected = insertCommand.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Аккаунт успешно создан!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Hide();
                            User_choose user_Choose = new User_choose();
                            user_Choose.Show();
                        }
                        else
                        {
                            MessageBox.Show("Ошибка при создании аккаунта", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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