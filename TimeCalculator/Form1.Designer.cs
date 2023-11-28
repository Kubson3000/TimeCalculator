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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            start_button = new Button();
            stop_button = new Button();
            textBox1 = new TextBox();
            progressBar1 = new ProgressBar();
            panel1 = new Panel();
            version_label = new Label();
            memes_checkbox = new CheckBox();
            open_ping_button = new Button();
            sync_button = new Button();
            show_dir_button = new Button();
            reset_login_button = new Button();
            login_button = new Button();
            password_input = new TextBox();
            username_input = new TextBox();
            label2 = new Label();
            label1 = new Label();
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
            stop_button.Text = "Update\r\nLogout Time\r\n";
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
            panel1.Controls.Add(version_label);
            panel1.Controls.Add(memes_checkbox);
            panel1.Controls.Add(open_ping_button);
            panel1.Controls.Add(sync_button);
            panel1.Controls.Add(show_dir_button);
            panel1.Controls.Add(reset_login_button);
            panel1.Controls.Add(login_button);
            panel1.Controls.Add(password_input);
            panel1.Controls.Add(username_input);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(hide_secret_button);
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(236, 193);
            panel1.TabIndex = 4;
            panel1.Visible = false;
            // 
            // version_label
            // 
            version_label.AutoSize = true;
            version_label.Location = new Point(175, 117);
            version_label.Name = "version_label";
            version_label.Size = new Size(37, 15);
            version_label.TabIndex = 11;
            version_label.Text = "v1.1.9";
            // 
            // memes_checkbox
            // 
            memes_checkbox.AutoSize = true;
            memes_checkbox.Location = new Point(3, 113);
            memes_checkbox.Name = "memes_checkbox";
            memes_checkbox.Size = new Size(71, 19);
            memes_checkbox.TabIndex = 10;
            memes_checkbox.Text = "MEMES?";
            memes_checkbox.UseVisualStyleBackColor = true;
            memes_checkbox.CheckedChanged += memes_checkbox_CheckedChanged;
            // 
            // open_ping_button
            // 
            open_ping_button.Location = new Point(153, 138);
            open_ping_button.Name = "open_ping_button";
            open_ping_button.Size = new Size(80, 23);
            open_ping_button.TabIndex = 9;
            open_ping_button.Text = "Open ping";
            open_ping_button.UseVisualStyleBackColor = true;
            open_ping_button.Click += open_ping_button_Click;
            // 
            // sync_button
            // 
            sync_button.Location = new Point(153, 167);
            sync_button.Name = "sync_button";
            sync_button.Size = new Size(80, 23);
            sync_button.TabIndex = 8;
            sync_button.Text = "Open sync";
            sync_button.UseVisualStyleBackColor = true;
            sync_button.Click += sync_button_Click;
            // 
            // show_dir_button
            // 
            show_dir_button.Location = new Point(3, 138);
            show_dir_button.Name = "show_dir_button";
            show_dir_button.Size = new Size(80, 23);
            show_dir_button.TabIndex = 7;
            show_dir_button.Text = "Show Dir";
            show_dir_button.UseVisualStyleBackColor = true;
            show_dir_button.Click += show_dir_button_Click;
            // 
            // reset_login_button
            // 
            reset_login_button.Location = new Point(3, 167);
            reset_login_button.Name = "reset_login_button";
            reset_login_button.Size = new Size(80, 23);
            reset_login_button.TabIndex = 6;
            reset_login_button.Text = "Reset data";
            reset_login_button.UseVisualStyleBackColor = true;
            reset_login_button.Click += reset_login_button_Click;
            // 
            // login_button
            // 
            login_button.Location = new Point(175, 41);
            login_button.Name = "login_button";
            login_button.Size = new Size(58, 23);
            login_button.TabIndex = 5;
            login_button.Text = "Login";
            login_button.UseVisualStyleBackColor = true;
            login_button.Click += login_button_Click;
            // 
            // password_input
            // 
            password_input.Location = new Point(69, 41);
            password_input.Name = "password_input";
            password_input.PasswordChar = 'ඞ';
            password_input.Size = new Size(100, 23);
            password_input.TabIndex = 4;
            // 
            // username_input
            // 
            username_input.Location = new Point(69, 12);
            username_input.Name = "username_input";
            username_input.Size = new Size(100, 23);
            username_input.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(3, 41);
            label2.Name = "label2";
            label2.Size = new Size(57, 15);
            label2.TabIndex = 2;
            label2.Text = "Password";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(3, 15);
            label1.Name = "label1";
            label1.Size = new Size(60, 15);
            label1.TabIndex = 1;
            label1.Text = "Username";
            // 
            // hide_secret_button
            // 
            hide_secret_button.Location = new Point(175, 12);
            hide_secret_button.Name = "hide_secret_button";
            hide_secret_button.Size = new Size(58, 23);
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
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "Form1";
            Text = "Time Evidence";
            KeyDown += Form1_KeyDown;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
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
        private Label label2;
        private Label label1;
        private Button login_button;
        private TextBox password_input;
        private TextBox username_input;
        private Button reset_login_button;
        private Button show_dir_button;
        private Button sync_button;
        private Button open_ping_button;
        private CheckBox memes_checkbox;
        private Label version_label;
    }
}