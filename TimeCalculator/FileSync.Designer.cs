namespace TimeCalculator
{
    partial class FileSync
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileSync));
            textBox1 = new TextBox();
            progressBar1 = new ProgressBar();
            today_button = new Button();
            all_button = new Button();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.Enabled = false;
            textBox1.Location = new Point(8, 12);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(200, 50);
            textBox1.TabIndex = 0;
            textBox1.TextAlign = HorizontalAlignment.Center;
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(8, 68);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(200, 23);
            progressBar1.TabIndex = 1;
            // 
            // today_button
            // 
            today_button.Location = new Point(8, 97);
            today_button.Name = "today_button";
            today_button.Size = new Size(75, 23);
            today_button.TabIndex = 2;
            today_button.Text = "Today only";
            today_button.UseVisualStyleBackColor = true;
            today_button.Click += today_button_Click;
            // 
            // all_button
            // 
            all_button.Location = new Point(133, 97);
            all_button.Name = "all_button";
            all_button.Size = new Size(75, 23);
            all_button.TabIndex = 3;
            all_button.Text = "All";
            all_button.UseVisualStyleBackColor = true;
            all_button.Click += all_button_Click;
            // 
            // FileSync
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(216, 128);
            Controls.Add(all_button);
            Controls.Add(today_button);
            Controls.Add(progressBar1);
            Controls.Add(textBox1);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "FileSync";
            Text = "FileSync";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBox1;
        private ProgressBar progressBar1;
        private Button today_button;
        private Button all_button;
    }
}