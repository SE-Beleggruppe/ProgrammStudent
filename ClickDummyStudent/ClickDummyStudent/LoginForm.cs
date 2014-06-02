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
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            this.Text = "Bitte loggen Sie sich ein";
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            Regex regExBelegKennung = new Regex("^beleg");
            Regex regExGruppenKennung = new Regex("^case");
               if (checkGruppeLogin(loginTextField.Text,passwordTextField.Text))
               {
                    MainForm mainForm = new MainForm(loginTextField.Text, getBelegKennungFromGruppenKennung(loginTextField.Text));
                    mainForm.Show();
                    Hide();
                }
                else
                {
                    if (checkBelegLogin(loginTextField.Text, passwordTextField.Text))
                    {
                        if (freieGruppen(loginTextField.Text))
                        {
                            if (!isInBelegZeitraum(loginTextField.Text))
                            {
                                MessageBox.Show("Der Anmeldezeitraum für diesen Beleg ist leider abgelaufen. Bitte melden Sie sich beim Dozenten.","Fehler");
                                return;
                            }
                            FormLeiterNeuEingeben leiterEingeben = new FormLeiterNeuEingeben(loginTextField.Text);
                            leiterEingeben.Show();
                            Hide();
                        }
                        else
                        {
                            MessageBox.Show("Für diesen Beleg gibt es keine freien Gruppen mehr. Bitte melden Sie sich beim Dozenten.", "Fehler");
                            Application.Exit();
                        }
                    }
                    else MessageBox.Show("Kombination Login/Passwort ist falsch.", "Fehler");
                }   
        }

        private bool checkBelegLogin(string login, string password)
        {
            Database db = new Database();
            List<string[]> output = db.ExecuteQuery("select * from Beleg");
            foreach (string[] info in output)
            {
                if (info[0] == login)
                {
                    if (info[6] == password)
                    {
                        return true;
                    }
                    else return false;
                }
            }
            return false;
        }

        private bool checkGruppeLogin(string login, string password)
        {
            Database db = new Database();
            List<string[]> output = db.ExecuteQuery("select * from Gruppe where Gruppenkennung=\"" + login + "\" and Passwort=internal_encrypt(\"" + password + "\")");
            if (output.Count != 0) return true;
            return false;
        }

        private bool freieGruppen(string BelegKennung)
        {
            Database db = new Database();
            List<string[]> output1 = db.ExecuteQuery("select Casekennung from Zuordnung_BelegCases where Belegkennung=\"" + BelegKennung + "\" and Casekennung not in (select Gruppenkennung from Zuordnung_GruppeBeleg)");
            foreach (string[] info in output1)
            {
                return true;
            }
            return false;
        }

        private string getBelegKennungFromGruppenKennung(string kennung)
        {
            Database db = new Database();
            string query = "select Belegkennung from Zuordnung_GruppeBeleg where Gruppenkennung=\"" + kennung + "\"";
            List<string[]> output = db.ExecuteQuery(query);
            foreach (string[] info in output)
                return info[0];
            return null;
        }

        private bool isInBelegZeitraum(string belegkennung)
        {
            Database db = new Database();
            DateTime anfang = Convert.ToDateTime(db.ExecuteQuery("select StartDatum from Beleg where Belegkennung=\"" + belegkennung + "\"").First()[0]);
            DateTime ende = Convert.ToDateTime(db.ExecuteQuery("select EndDatum from Beleg where Belegkennung=\"" + belegkennung + "\"").First()[0]);
            if (DateTime.Today < anfang || DateTime.Today > ende) return false;
            return true;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
