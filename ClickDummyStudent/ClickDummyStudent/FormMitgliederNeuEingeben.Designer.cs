namespace ClickDummyStudent
{
    partial class FormMitgliederNeuEingeben
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
            this.label1 = new System.Windows.Forms.Label();
            this.alleMitgliederDataGridView = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.newPasswortTextBox = new System.Windows.Forms.TextBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.CommitButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.alleMitgliederDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 17);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(859, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Sie können nun weitere Gruppenmitglieder eintragen, oder dies im Nachhinein erled" +
    "igen.";
            // 
            // alleMitgliederDataGridView
            // 
            this.alleMitgliederDataGridView.AllowUserToAddRows = false;
            this.alleMitgliederDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.alleMitgliederDataGridView.Location = new System.Drawing.Point(30, 69);
            this.alleMitgliederDataGridView.Margin = new System.Windows.Forms.Padding(6);
            this.alleMitgliederDataGridView.Name = "alleMitgliederDataGridView";
            this.alleMitgliederDataGridView.Size = new System.Drawing.Size(1282, 288);
            this.alleMitgliederDataGridView.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 377);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(555, 25);
            this.label2.TabIndex = 2;
            this.label2.Text = "Legen Sie abschließend noch ein Gruppenkennwort fest:";
            // 
            // newPasswortTextBox
            // 
            this.newPasswortTextBox.Location = new System.Drawing.Point(594, 371);
            this.newPasswortTextBox.Margin = new System.Windows.Forms.Padding(6);
            this.newPasswortTextBox.Name = "newPasswortTextBox";
            this.newPasswortTextBox.Size = new System.Drawing.Size(270, 31);
            this.newPasswortTextBox.TabIndex = 3;
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(1162, 365);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(6);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(150, 44);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "Abbrechen";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // CommitButton
            // 
            this.CommitButton.Location = new System.Drawing.Point(880, 367);
            this.CommitButton.Margin = new System.Windows.Forms.Padding(6);
            this.CommitButton.Name = "CommitButton";
            this.CommitButton.Size = new System.Drawing.Size(270, 44);
            this.CommitButton.TabIndex = 5;
            this.CommitButton.Text = "Anmeldung abschließen";
            this.CommitButton.UseVisualStyleBackColor = true;
            this.CommitButton.Click += new System.EventHandler(this.commitButton_Click);
            // 
            // FormMitgliederNeuEingeben
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1347, 502);
            this.Controls.Add(this.CommitButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.newPasswortTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.alleMitgliederDataGridView);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "FormMitgliederNeuEingeben";
            this.Text = "FormErstanmeldung2";
            ((System.ComponentModel.ISupportInitialize)(this.alleMitgliederDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView alleMitgliederDataGridView;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox newPasswortTextBox;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button CommitButton;
    }
}