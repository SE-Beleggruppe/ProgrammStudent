using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace ClickDummyStudent
{
    public partial class FormLeiterNeuEingeben : Form
    {
        public string Belegkennung;
        public FormLeiterNeuEingeben( string belegKennung)
        {
            InitializeComponent();
            this.Belegkennung = belegKennung;
            sNummerTextField.Text = "s12345";
            nachnameTextField.Text = "Test";
            vornameTextField.Text = "Test";
            mailTextField.Text = "mail@test.de";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (sNummerTextField.Text != ""
                && nachnameTextField.Text != ""
                && vornameTextField.Text != ""
                && mailTextField.Text != "")
            {
                if (!checkSNummer(sNummerTextField.Text))
                {
                    MessageBox.Show("S-Nummer ist fehlerhaft oder schon in der Datenbank vorhanden.");
                    return;
                }
                if (!checkMail(mailTextField.Text))
                {
                    MessageBox.Show("Dies ist keine gültige E-Mail-Adresse.");
                    return;
                }
                
                Student leiter = new Student(nachnameTextField.Text, vornameTextField.Text, sNummerTextField.Text, mailTextField.Text, "Leitung");
                FormMitgliederNeuEingeben form2 = new FormMitgliederNeuEingeben(leiter, this.Belegkennung);
                form2.Show();
                this.Close();
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

        

        private bool checkSNummer(string sNummer)
        {
            Database db = new Database();
            if (sNummer == "") return false;
            if (sNummer.Length != 6) return false;
            if (!sNummer.StartsWith("s")) return false;
            string nummer = sNummer.Substring(1);
            int n;
            bool isNummer = int.TryParse(nummer, out n);
            if (!isNummer) return false;

            List<string[]> output = db.ExecuteQuery("select * from Student");
            foreach (string[] info in output)
            {
                if (info[0] == sNummer) return false;
            }

            return true;
        }

        private bool checkMail(string mail)
        {
            Regex regExp = new Regex("\\b[!#$%&'*+./0-9=?_`a-z{|}~^-]+@[.0-9a-z-]+\\.[a-z]{2,6}\\b");
            Match match = regExp.Match(mail);
            if (match.Success)
            {
                Database db = new Database();
                List<string[]> output = db.ExecuteQuery("select * from Student");
                foreach (string[] info in output)
                {
                    if(info[3] == mail) return false;
                }
                return true;
            }
            else return false;
        }
    }
}
