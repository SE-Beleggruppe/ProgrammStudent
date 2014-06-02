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
    public partial class FormMitgliederNeuEingeben : Form
    {
        Gruppe _gruppe;
        public string Belegkennung;
        List<string> rollen;
        public FormMitgliederNeuEingeben(Student leiter, string belegKennung)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            this.Text = "Neue Gruppe in Beleg " + belegKennung;
            this.Belegkennung = belegKennung;
            _gruppe = new Gruppe("p", Belegkennung);
            string newGruppenkennung = getGruppenKennung();

            if (newGruppenkennung != "-1") _gruppe.GruppenKennung = newGruppenkennung;
            else
            {
                MessageBox.Show("Keine freien Gruppen für diesen Beleg verfügbar, bitte bei dem Dozenten melden", "Fehler");
                Application.Exit();
            }

            _gruppe.ThemenNummer = getThemenNummer();
            _gruppe.addStudent(leiter);
            for (int i = 0; i < getMaxAnzahlMitglieder(belegKennung) - 1; i++)
            {
                _gruppe.addStudent(new Student("na","na","na","na","na"));
            }
            updateRollen();
            RefreshDatagrid(this._gruppe);
            alleMitgliederDataGridView.Rows[0].ReadOnly = true;
        }

        private void commitButton_Click(object sender, EventArgs e)
        {
            if (newPasswortTextBox.Text == "")
            {
                MessageBox.Show("Bitte geben Sie ein Passwort ein, mit dem SIe später auf die Gruppe zugreifen können.", "Fehler");
                return;
            }
            _gruppe.Password = newPasswortTextBox.Text;
            saveGruppeInDatabase();
        }

        private void updateRollen()
        {
            rollen = new List<string>();
            Database db = new Database();
            List<string[]> output = db.ExecuteQuery("Select Rolle from Rolle where Rolle in (select Rolle from Zuordnung_BelegRolle where Belegkennung=\"" + _gruppe.Belegkennung + "\")");
            foreach (string[] info in output)
            {
                rollen.Add(info[0]);
            }
            rollen.Add("na");
        }

        private void RefreshDatagrid(Gruppe gruppe)
        {
            alleMitgliederDataGridView.Rows.Clear();
            (alleMitgliederDataGridView.Columns[4] as DataGridViewComboBoxColumn).DataSource = rollen;
            (alleMitgliederDataGridView.Columns[3] as DataGridViewTextBoxColumn).MinimumWidth = 250;
            foreach (Student info in gruppe.Studenten)
            {
                int number = alleMitgliederDataGridView.Rows.Add();
                alleMitgliederDataGridView.Rows[number].Cells[0].Value = info.Name;
                alleMitgliederDataGridView.Rows[number].Cells[1].Value = info.Vorname;
                alleMitgliederDataGridView.Rows[number].Cells[2].Value = info.SNummer;
                if (info.SNummer != "na") alleMitgliederDataGridView.Rows[number].Cells[2].ReadOnly = true;
                alleMitgliederDataGridView.Rows[number].Cells[3].Value = info.Mail;
                alleMitgliederDataGridView.Rows[number].Cells[4].Value = info.Rolle;
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private string getGruppenKennung()
        {
            Database db = new Database();
            List<string[]> output1 = db.ExecuteQuery("select Casekennung from Zuordnung_BelegCases where Belegkennung=\"" + _gruppe.Belegkennung + "\" and Casekennung not in (select Gruppenkennung from Zuordnung_GruppeBeleg)");
            foreach (string[] info in output1)
            {
                return info[0];
            }
            return "-1";
        }

        private int getThemenNummer()
        {
            Database db = new Database();
            string query = "select Themennummer from Zuordnung_BelegThema where Belegkennung=\"" + Belegkennung + "\"";
            List<string[]> output = db.ExecuteQuery(query);
            int nr = 0;
            if(output.Count > 0) int.TryParse(output.First()[0], out nr);
            return nr;
        }

        private int getMaxAnzahlMitglieder(string beKennung)
        {
            Database db = new Database();
            List<string[]> output = db.ExecuteQuery("select MaxAnzMitglieder from Beleg where Belegkennung=\"" + beKennung + "\"");
            return Convert.ToInt32(output.First()[0]);
        }

        private void saveGruppeInDatabase()
        {
            string fehlermeldung = "";
            for (int i = 0; i < alleMitgliederDataGridView.Rows.Count; i++)
            {
                string name = (string)alleMitgliederDataGridView.Rows[i].Cells[0].Value;
                string vorname = (string)alleMitgliederDataGridView.Rows[i].Cells[1].Value;
                string sNummer = (string)alleMitgliederDataGridView.Rows[i].Cells[2].Value;
                string mail = (string)alleMitgliederDataGridView.Rows[i].Cells[3].Value;
                string rolle = (string)alleMitgliederDataGridView.Rows[i].Cells[4].FormattedValue.ToString();

                if (sNummer != "na" && !string.IsNullOrEmpty(sNummer))
                {
                    if (!checkSNummer(sNummer))
                    {
                       fehlermeldung += "Student " + name + " " + vorname + " hat weder 'na' noch eine gültige S-Nummer eingetragen und konnte nicht hinzugefügt werden. (" + sNummer + ")\n";
                    }
                }
            }
            if (fehlermeldung != "")
            {
                MessageBox.Show(fehlermeldung, "Fehler");
                return;
            }
            for (int i = 0; i < alleMitgliederDataGridView.Rows.Count; i++)
            {
                string name = (string)alleMitgliederDataGridView.Rows[i].Cells[0].Value;
                string vorname = (string)alleMitgliederDataGridView.Rows[i].Cells[1].Value;
                string sNummer = (string)alleMitgliederDataGridView.Rows[i].Cells[2].Value;
                string mail = (string)alleMitgliederDataGridView.Rows[i].Cells[3].Value;
                string rolle = (string)alleMitgliederDataGridView.Rows[i].Cells[4].FormattedValue.ToString();

                if (sNummer != "na" && !string.IsNullOrEmpty(sNummer))
                {
                    insertStudent(new Student(name,vorname,sNummer,mail,rolle), _gruppe );
                }
            }
            Database db = new Database();
            string query = "insert into Gruppe values(\"" + _gruppe.GruppenKennung + "\"," + _gruppe.ThemenNummer + ",internal_encrypt(\"" + _gruppe.Password + "\"))";
            db.ExecuteQuery(query);
            query = "insert into Zuordnung_GruppeBeleg values(\"" + _gruppe.GruppenKennung + "\",\"" + _gruppe.Belegkennung + "\")";
            db.ExecuteQuery(query);


            MessageBox.Show("Anmeldung abgeschlossen! \nIhre Gruppenkennung lautet: " + _gruppe.GruppenKennung + "\n(Wichtig für das spätere Anmelden!)", "ACHTUNG!!");
            MainForm form = new MainForm(_gruppe.GruppenKennung, _gruppe.Belegkennung);
            form.Show();
            this.Hide();
        }

        private void insertStudent(Student student, Gruppe gruppe)
        {
            Database db = new Database();
            string query = "insert into Student values(\"" + student.SNummer + "\",\"" + student.Vorname + "\",\"" + student.Name + "\",\"" + student.Mail + "\",\"" + student.Rolle + "\")";
            db.ExecuteQuery(query);
            query = "insert into Zuordnung_GruppeStudent values(\"" + gruppe.GruppenKennung + "\",\"" + student.SNummer + "\")";
            db.ExecuteQuery(query);
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

            List<string[]> output = db.ExecuteQuery("select sNummer from Student");
            foreach (string[] info in output)
            {
                if (info[0] == sNummer) return false;
            }

            return true;
        }
    }
}
