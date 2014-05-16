namespace ClickDummyStudent
{
    partial class MainForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.mitgliederDataGridView = new System.Windows.Forms.DataGridView();
            this.editTableButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.comboBoxThemen = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.mitgliederDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // mitgliederDataGridView
            // 
            this.mitgliederDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.mitgliederDataGridView.Location = new System.Drawing.Point(12, 12);
            this.mitgliederDataGridView.Name = "mitgliederDataGridView";
            this.mitgliederDataGridView.Size = new System.Drawing.Size(620, 215);
            this.mitgliederDataGridView.TabIndex = 0;
            // 
            // editTableButton
            // 
            this.editTableButton.Location = new System.Drawing.Point(375, 233);
            this.editTableButton.Name = "editTableButton";
            this.editTableButton.Size = new System.Drawing.Size(104, 23);
            this.editTableButton.TabIndex = 1;
            this.editTableButton.Text = "Tabelle bearbeiten";
            this.editTableButton.UseVisualStyleBackColor = true;
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(485, 233);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(71, 23);
            this.saveButton.TabIndex = 2;
            this.saveButton.Text = "Speichern";
            this.saveButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(562, 233);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(71, 23);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "Abbrechen";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // comboBoxThemen
            // 
            this.comboBoxThemen.FormattingEnabled = true;
            this.comboBoxThemen.Location = new System.Drawing.Point(62, 235);
            this.comboBoxThemen.Name = "comboBoxThemen";
            this.comboBoxThemen.Size = new System.Drawing.Size(242, 21);
            this.comboBoxThemen.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 238);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Thema:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 267);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxThemen);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.editTableButton);
            this.Controls.Add(this.mitgliederDataGridView);
            this.Name = "MainForm";
            this.Text = "MainForm";
            ((System.ComponentModel.ISupportInitialize)(this.mitgliederDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView mitgliederDataGridView;
        private System.Windows.Forms.Button editTableButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.ComboBox comboBoxThemen;
        private System.Windows.Forms.Label label1;
    }
}

