namespace MichalRymaszewskiGeolocation
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
            txtb_sumbitIP = new TextBox();
            btn_submitIP = new Button();
            ErrorMessage = new Label();
            lv_geo_result = new ListView();
            btn_saveDB = new Button();
            btn_removeDB = new Button();
            chk_autosave = new CheckBox();
            btn_refreshIP = new Button();
            SuspendLayout();
            // 
            // txtb_sumbitIP
            // 
            txtb_sumbitIP.Location = new Point(12, 12);
            txtb_sumbitIP.Name = "txtb_sumbitIP";
            txtb_sumbitIP.Size = new Size(532, 23);
            txtb_sumbitIP.TabIndex = 0;
            // 
            // btn_submitIP
            // 
            btn_submitIP.Location = new Point(550, 11);
            btn_submitIP.Name = "btn_submitIP";
            btn_submitIP.Size = new Size(103, 23);
            btn_submitIP.TabIndex = 1;
            btn_submitIP.Text = "Find IP/URL";
            btn_submitIP.UseVisualStyleBackColor = true;
            btn_submitIP.Click += btn_submitIP_Click;
            // 
            // ErrorMessage
            // 
            ErrorMessage.AutoSize = true;
            ErrorMessage.ForeColor = Color.Red;
            ErrorMessage.Location = new Point(286, 157);
            ErrorMessage.Name = "ErrorMessage";
            ErrorMessage.Size = new Size(78, 15);
            ErrorMessage.TabIndex = 2;
            ErrorMessage.Text = "ErrorMessage";
            ErrorMessage.TextAlign = ContentAlignment.TopCenter;
            // 
            // lv_geo_result
            // 
            lv_geo_result.Location = new Point(12, 53);
            lv_geo_result.Name = "lv_geo_result";
            lv_geo_result.Size = new Size(641, 69);
            lv_geo_result.TabIndex = 3;
            lv_geo_result.UseCompatibleStateImageBehavior = false;
            // 
            // btn_saveDB
            // 
            btn_saveDB.Location = new Point(12, 128);
            btn_saveDB.Name = "btn_saveDB";
            btn_saveDB.Size = new Size(103, 23);
            btn_saveDB.TabIndex = 4;
            btn_saveDB.Text = "Save IP/URL";
            btn_saveDB.UseVisualStyleBackColor = true;
            btn_saveDB.Click += btn_saveDB_Click;
            // 
            // btn_removeDB
            // 
            btn_removeDB.Location = new Point(286, 128);
            btn_removeDB.Name = "btn_removeDB";
            btn_removeDB.Size = new Size(103, 23);
            btn_removeDB.TabIndex = 5;
            btn_removeDB.Text = "Remove IP/URL";
            btn_removeDB.UseVisualStyleBackColor = true;
            btn_removeDB.Click += btn_removeDB_Click;
            // 
            // chk_autosave
            // 
            chk_autosave.AutoSize = true;
            chk_autosave.Location = new Point(12, 157);
            chk_autosave.Name = "chk_autosave";
            chk_autosave.Size = new Size(78, 19);
            chk_autosave.TabIndex = 6;
            chk_autosave.Text = "Auto save";
            chk_autosave.UseVisualStyleBackColor = true;
            // 
            // btn_refreshIP
            // 
            btn_refreshIP.Location = new Point(550, 128);
            btn_refreshIP.Name = "btn_refreshIP";
            btn_refreshIP.Size = new Size(103, 23);
            btn_refreshIP.TabIndex = 7;
            btn_refreshIP.Text = "Reload IP/URL";
            btn_refreshIP.UseVisualStyleBackColor = true;
            btn_refreshIP.Click += btn_refreshIP_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(667, 186);
            Controls.Add(btn_refreshIP);
            Controls.Add(chk_autosave);
            Controls.Add(btn_removeDB);
            Controls.Add(btn_saveDB);
            Controls.Add(lv_geo_result);
            Controls.Add(ErrorMessage);
            Controls.Add(btn_submitIP);
            Controls.Add(txtb_sumbitIP);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtb_sumbitIP;
        private Button btn_submitIP;
        private Label ErrorMessage;
        private ListView lv_geo_result;
        private Button btn_saveDB;
        private Button btn_removeDB;
        private CheckBox chk_autosave;
        private Button btn_refreshIP;
    }
}
