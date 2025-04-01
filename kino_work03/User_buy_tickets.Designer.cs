namespace kino
{
    partial class User_buy_tickets
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox textBox1;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();

            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(300, 22);
            this.textBox1.TabIndex = 0;
            this.textBox1.ReadOnly = true;
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;

            // 
            // User_buy_tickets
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 700);
            this.Controls.Add(this.textBox1);
            this.Name = "User_buy_tickets";
            this.Text = "Покупка билетов";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
