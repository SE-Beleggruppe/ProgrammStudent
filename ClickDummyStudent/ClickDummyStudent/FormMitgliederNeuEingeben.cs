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
        Gruppe gruppe;
        public string Belegkennung;
        List<string> rollen;
        public FormMitgliederNeuEingeben(Student leiter, string belegKennung)
        {
            InitializeComponent();
            this.Belegkennung = belegKennung;
            gruppe = new Gruppe("p", Belegkennung);
            string newGruppenkennung = getGruppenKennung();

            if (newGruppenkennung != "-1") gruppe.gruppenKennung = newGruppenkennung;
            else
            {
                MessageBox.Show("Keine freien Gruppen für diesen Beleg verfügbar, bitte bei dem Dozenten melden");
                Application.Exit();
            }

            gruppe.themenNummer = getThemenNummer();
            gruppe.addStudent(leiter);
            for (int i = 0; i < getMinAnzahlMitglieder() - 1; i++)
            {
                gruppe.addStudent(new Student("na","na","na","na","na"));
            }
            updateRollen();
            RefreshDatagrid(this.gruppe);
            alleMitgliederDataGridView.Rows[0].ReadOnly = true;
        }

        private void commitButton_Click(object sender, EventArgs e)
        {
            if (newPasswortTextBox.Text == "")
            {
                MessageBox.Show("Bitte geben Sie ein Passwort ein, mit dem SIe später auf die Gruppe zugreifen können.");
                return;
            }
            gruppe.password = newPasswortTextBox.Text;
            saveGruppeInDatabase();
            MessageBox.Show("Anmeldung abgeschlossen! \nIhre Gruppenkennung lautet: " + gruppe.gruppenKennung + "\n(Wichtig für das spätere Anmelden!)");
            MainForm form = new MainForm(gruppe.gruppenKennung, gruppe.Belegkennung);
            form.Show();
        }

        private void updateRollen()
        {
            rollen = new List<string>();
            Database db = new Database();
            List<string[]> output = db.ExecuteQuery("Select Rolle from Rolle where Rolle in (select Rolle from Zuordnung_BelegRolle where Belegkennung=\"" + gruppe.Belegkennung + "\")");
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
            foreach (Student info in gruppe.studenten)
            {
                int number = alleMitgliederDataGridView.Rows.Add();
                alleMitgliederDataGridView.Rows[number].Cells[0].Value = info.name;
                alleMitgliederDataGridView.Rows[number].Cells[1].Value = info.vorname;
                alleMitgliederDataGridView.Rows[number].Cells[2].Value = info.sNummer;
                if (info.sNummer != "na") alleMitgliederDataGridView.Rows[number].Cells[2].ReadOnly = true;
                alleMitgliederDataGridView.Rows[number].Cells[3].Value = info.mail;
                alleMitgliederDataGridView.Rows[number].Cells[4].Value = info.rolle;
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private string getGruppenKennung()
        {
            Database db = new Database();
            List<string[]> output1 = db.ExecuteQuery("select Casekennung from Zuordnung_BelegCases where Belegkennung=\"" + gruppe.Belegkennung + "\" and Casekennung not in (select Gruppenkennung from Zuordnung_GruppeBeleg)");
            foreach (string[] info in output1)
            {
                return info[0];
            }
            return "-1";
        }

        private bool isCaseFree(string caseS)
        {
            Database db = new Database();
            List<string[]> output = db.ExecuteQuery("select Gruppenkennung from Gruppe");
            foreach (string[] info in output)
            {
                if(info[0] == caseS) return false;
            }
            return true;
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

        private int getMinAnzahlMitglieder()
        {
            Database db = new Database();
            List<string[]> output = db.ExecuteQuery("select MinAnzMitglieder from Beleg where Belegkennung=\"" + gruppe.Belegkennung + "\"");
            int anz;
            int.TryParse(output.First()[0], out anz);
            return anz;   
        }

        private void saveGruppeInDatabase()
        {
            for (int i = 0; i < alleMitgliederDataGridView.Rows.Count; i++)
            {
                string name = (string)alleMitgliederDataGridView.Rows[i].Cells[0].Value;
                string vorname = (string)alleMitgliederDataGridView.Rows[i].Cells[1].Value;
                string sNummer = (string)alleMitgliederDataGridView.Rows[i].Cells[2].Value;
                string mail = (string)alleMitgliederDataGridView.Rows[i].Cells[3].Value;
                string rolle = (string)alleMitgliederDataGridView.Rows[i].Cells[4].FormattedValue.ToString();

                if (sNummer != "na" && sNummer != "" && sNummer != null)
                {
                    if (checkSNummer(sNummer))
                    {
                        insertStudent(new Student(name, vorname, sNummer, mail, rolle), gruppe);
                    }
                    else
                    {
                        MessageBox.Show("Student " + name + " " + vorname + " hat weder 'na' noch eine gültige S-Nummer eingetragen und wurde nicht hinzugefügt. (" + sNummer + ")");
                    }
                    
                }
            }
            Database db = new Database();
            string query = "insert into Gruppe values(\"" + gruppe.gruppenKennung + "\"," + gruppe.themenNummer + ",\"" + gruppe.password + "\")";
            db.ExecuteQuery(query);
            query = "insert into Zuordnung_GruppeBeleg values(\"" + gruppe.gruppenKennung + "\",\"" + gruppe.Belegkennung + "\")";
            db.ExecuteQuery(query);
        }

        private void insertStudent(Student student, Gruppe gruppe)
        {
            Database db = new Database();
            string query = "insert into Student values(\"" + student.sNummer + "\",\"" + student.vorname + "\",\"" + student.name + "\",\"" + student.mail + "\",\"" + student.rolle + "\")";
            db.ExecuteQuery(query);
            query = "insert into Zuordnung_GruppeStudent values(\"" + gruppe.gruppenKennung + "\",\"" + student.sNummer + "\")";
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
