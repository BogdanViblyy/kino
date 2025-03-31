using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace kino
{
    public partial class User_choose : Form
    {
        public User_choose()
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
                }
                else if (c is TextBox txt)
                {
                    txt.BackColor = Color.Black;
                    txt.ForeColor = Color.White;
                    txt.BorderStyle = BorderStyle.FixedSingle;
                }
            }
            pictureBox1.BackColor = Color.Black;
        }

        int tt = 0;
        List<string> pildid = new List<string>();
        List<string> names = new List<string>();
        List<string> years = new List<string>();
        List<int> filmIds = new List<int>();

        private void button1_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["kinoConnectionString"].ConnectionString;
            string query = "SELECT filmImg, filmName, filmYear, filmId FROM film";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                pildid.Add(reader["filmImg"].ToString());
                                names.Add(reader["filmName"].ToString());
                                years.Add(reader["filmYear"].ToString());
                                filmIds.Add(reader.GetInt32(3));
                            }
                        }
                    }

                    if (pildid.Count > 0)
                    {
                        UpdateUI();
                    }
                    else
                    {
                        MessageBox.Show("Нет изображений для отображения.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Произошла ошибка: " + ex.Message);
                }
            }
        }

        private void UpdateUI()
        {
            textBox1.Text = names[tt];
            textBox2.Text = years[tt];
            pictureBox1.Image = LoadImageFromUrl(pildid[tt]);
            tt = (tt + 1) % pildid.Count;
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

        private void button2_MouseClick(object sender, MouseEventArgs e)
        {
            this.Hide();
            string selectedFilmName = textBox1.Text;
            User_buy_tickets userBuyTickets = new User_buy_tickets(selectedFilmName);
            userBuyTickets.Show();
        }
    }
}