namespace SystemTrayAnimator.Forms
{
    partial class SettingsForm
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
        /// <param name="menuLanguage">Contains language strings.</param>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.btnOk = new System.Windows.Forms.Button();
            this.txtDirectoryName = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtFileExtensions = new System.Windows.Forms.TextBox();
            this.lblDirectoryName = new System.Windows.Forms.Label();
            this.lblFileExtensions = new System.Windows.Forms.Label();
            this.chckIncludeSubdirectories = new System.Windows.Forms.CheckBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(308, 182);
            this.btnOk.Margin = new System.Windows.Forms.Padding(4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(98, 32);
            this.btnOk.TabIndex = 6;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.ButtonOkClick);
            // 
            // txtDirectoryName
            // 
            this.txtDirectoryName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDirectoryName.Location = new System.Drawing.Point(12, 41);
            this.txtDirectoryName.Name = "txtDirectoryName";
            this.txtDirectoryName.Size = new System.Drawing.Size(457, 22);
            this.txtDirectoryName.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(414, 182);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(99, 32);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.ButtonCancelClick);
            // 
            // txtFileExtensions
            // 
            this.txtFileExtensions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFileExtensions.Location = new System.Drawing.Point(12, 101);
            this.txtFileExtensions.Name = "txtFileExtensions";
            this.txtFileExtensions.Size = new System.Drawing.Size(501, 22);
            this.txtFileExtensions.TabIndex = 4;
            // 
            // lblDirectoryName
            // 
            this.lblDirectoryName.AutoSize = true;
            this.lblDirectoryName.Location = new System.Drawing.Point(13, 18);
            this.lblDirectoryName.Name = "lblDirectoryName";
            this.lblDirectoryName.Size = new System.Drawing.Size(156, 17);
            this.lblDirectoryName.TabIndex = 0;
            this.lblDirectoryName.Text = "Directory with icon files:";
            // 
            // lblFileExtensions
            // 
            this.lblFileExtensions.AutoSize = true;
            this.lblFileExtensions.Location = new System.Drawing.Point(13, 81);
            this.lblFileExtensions.Name = "lblFileExtensions";
            this.lblFileExtensions.Size = new System.Drawing.Size(131, 17);
            this.lblFileExtensions.TabIndex = 3;
            this.lblFileExtensions.Text = "Icon file extensions:";
            // 
            // chckIncludeSubdirectories
            // 
            this.chckIncludeSubdirectories.AutoSize = true;
            this.chckIncludeSubdirectories.Location = new System.Drawing.Point(12, 146);
            this.chckIncludeSubdirectories.Name = "chckIncludeSubdirectories";
            this.chckIncludeSubdirectories.Size = new System.Drawing.Size(168, 21);
            this.chckIncludeSubdirectories.TabIndex = 5;
            this.chckIncludeSubdirectories.Text = "Include subdirectories";
            this.chckIncludeSubdirectories.UseVisualStyleBackColor = true;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Location = new System.Drawing.Point(475, 41);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(38, 23);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(525, 236);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.chckIncludeSubdirectories);
            this.Controls.Add(this.lblFileExtensions);
            this.Controls.Add(this.lblDirectoryName);
            this.Controls.Add(this.txtFileExtensions);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.txtDirectoryName);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormKeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.TextBox txtDirectoryName;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtFileExtensions;
        private System.Windows.Forms.Label lblDirectoryName;
        private System.Windows.Forms.Label lblFileExtensions;
        private System.Windows.Forms.CheckBox chckIncludeSubdirectories;
        private System.Windows.Forms.Button btnBrowse;
    }
}