using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClickDummyStudent
{
    public partial class FormLeiterNeuEingeben : Form
    {
        public FormLeiterNeuEingeben()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (sNummerTextField.Text != ""
                && nachnameTextField.Text != ""
                && vornameTextField.Text != ""
                && mailTextField.Text != "")
            {
                if (insertLeiter(sNummerTextField.Text, vornameTextField.Text, nachnameTextField.Text, mailTextField.Text, "Leiter"))
                {
                    //FormMitgliederNeuEingeben form2 = new FormMitgliederNeuEingeben();
                    //form2.Show();
                }
                else MessageBox.Show("Das Einfügen in die Datenbank hat leider nicht geklappt.");
            }
            else
            {
                MessageBox.Show("Bitte alle Felder ausfüllen!");
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private bool insertLeiter(string sNummer, string vorname, string nachname, string mail, string rolle)
        {
            Database db = new Database();
            List<string[]> output = db.ExecuteQuery("insert into Student values(\"" + sNummer + "\",\"" + vorname + "\",\"" + nachname + "\",\"" + mail + "\",\"" + rolle);
            return output != null ? true : false;
        }
    }
}
