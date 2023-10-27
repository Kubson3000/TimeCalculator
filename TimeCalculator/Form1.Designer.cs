namespace TimeCalculator
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            start_button = new Button();
            stop_button = new Button();
            textBox1 = new TextBox();
            progressBar1 = new ProgressBar();
            panel1 = new Panel();
            hide_secret_button = new Button();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // start_button
            // 
            start_button.Enabled = false;
            start_button.Location = new Point(12, 84);
            start_button.Name = "start_button";
            start_button.Size = new Size(100, 100);
            start_button.TabIndex = 0;
            start_button.Text = "Start";
            start_button.UseVisualStyleBackColor = true;
            start_button.Click += start_button_Click;
            // 
            // stop_button
            // 
            stop_button.Enabled = false;
            stop_button.Location = new Point(122, 84);
            stop_button.Name = "stop_button";
            stop_button.Size = new Size(100, 100);
            stop_button.TabIndex = 1;
            stop_button.Text = "Stop";
            stop_button.UseVisualStyleBackColor = true;
            stop_button.Click += stop_button_Click;
            // 
            // textBox1
            // 
            textBox1.Enabled = false;
            textBox1.Location = new Point(12, 12);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(210, 37);
            textBox1.TabIndex = 2;
            textBox1.TextAlign = HorizontalAlignment.Center;
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(12, 55);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(210, 23);
            progressBar1.TabIndex = 3;
            // 
            // panel1
            // 
            panel1.Controls.Add(hide_secret_button);
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(236, 193);
            panel1.TabIndex = 4;
            panel1.Visible = false;
            // 
            // hide_secret_button
            // 
            hide_secret_button.Location = new Point(147, 157);
            hide_secret_button.Name = "hide_secret_button";
            hide_secret_button.Size = new Size(75, 23);
            hide_secret_button.TabIndex = 0;
            hide_secret_button.Text = "Hide";
            hide_secret_button.UseVisualStyleBackColor = true;
            hide_secret_button.Click += hide_secret_button_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(234, 192);
            Controls.Add(panel1);
            Controls.Add(progressBar1);
            Controls.Add(textBox1);
            Controls.Add(stop_button);
            Controls.Add(start_button);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            MaximizeBox = false;
            Name = "Form1";
            Text = "Time Evidence";
            KeyDown += Form1_KeyDown;
            panel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button start_button;
        private Button stop_button;
        private TextBox textBox1;
        private ProgressBar progressBar1;
        private Panel panel1;
        private Button hide_secret_button;
    }
}