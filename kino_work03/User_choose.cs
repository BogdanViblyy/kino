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
            // Увеличение размеров формы
            this.Size = new Size(900, 700); // Ширина 900, Высота 700
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.DarkRed;
            this.ForeColor = Color.White;
            this.Font = new Font("Arial", 12, FontStyle.Bold);

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
                    txt.Width = 200;

                    // Disable input for textBox1 (movie name) and textBox2 (movie year)
                    if (txt == textBox1 || txt == textBox2)
                    {
                        txt.ReadOnly = true;
                    }
                }
            }

            // Увеличенный PictureBox, размещаем его выше
            pictureBox1.BackColor = Color.Black;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.Size = new Size((int)(this.ClientSize.Width * 0.7), (int)(this.ClientSize.Height * 0.5));
            pictureBox1.Location = new Point((this.ClientSize.Width - pictureBox1.Width) / 2, 20);

            // Перемещение текстовых полей и кнопок ниже картинки
            textBox1.Location = new Point((this.ClientSize.Width - textBox1.Width) / 2, pictureBox1.Bottom + 20);
            textBox2.Location = new Point((this.ClientSize.Width - textBox2.Width) / 2, textBox1.Bottom + 10);
            button1.Location = new Point((this.ClientSize.Width - button1.Width) / 2, textBox2.Bottom + 20);
            button2.Location = new Point((this.ClientSize.Width - button2.Width) / 2, button1.Bottom + 20);
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

            Image img = LoadImageFromUrl(pildid[tt]);
            if (img != null)
            {
                pictureBox1.Image = img;
                AdjustPictureBoxSize(img);
            }

            tt = (tt + 1) % pildid.Count;
        }

        private void AdjustPictureBoxSize(Image img)
        {
            int maxWidth = (int)(this.ClientSize.Width * 0.7); // 70% ширины формы
            int maxHeight = (int)(this.ClientSize.Height * 0.5); // 50% высоты формы

            int newWidth = img.Width;
            int newHeight = img.Height;

            if (newWidth > maxWidth || newHeight > maxHeight)
            {
                double ratioX = (double)maxWidth / newWidth;
                double ratioY = (double)maxHeight / newHeight;
                double ratio = Math.Min(ratioX, ratioY);

                newWidth = (int)(newWidth * ratio);
                newHeight = (int)(newHeight * ratio);
            }

            // Увеличение до 90% доступного места
            newWidth = Math.Min((int)(newWidth * 1.3), maxWidth);
            newHeight = Math.Min((int)(newHeight * 1.3), maxHeight);

            pictureBox1.Size = new Size(newWidth, newHeight);
            pictureBox1.Location = new Point((this.ClientSize.Width - newWidth) / 2, 20);
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
