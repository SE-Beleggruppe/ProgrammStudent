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
        public FormMitgliederNeuEingeben(Student leiter, string belegKennung)
        {
            InitializeComponent();
            this.Belegkennung = belegKennung;
            gruppe = new Gruppe("p", Belegkennung);
            string newGruppenkennung = getGruppenKennung();
            if (newGruppenkennung != "-1") gruppe.gruppenKennung = newGruppenkennung;
            else MessageBox.Show("Keine freien Gruppen für diesen Beleg verfügbar, bitte bei dem Dozenten melden");
            gruppe.themenNummer = getThemenNummer();
            gruppe.addStudent(leiter);
            for (int i = 0; i < getMinAnzahlMitglieder() - 1; i++)
            {
                gruppe.addStudent(new Student("na","na","na","na","na"));
            }

            RefreshDatagrid(this.gruppe);
            alleMitgliederDataGridView.Rows[0].ReadOnly = true;
        }

        private void commitButton_Click(object sender, EventArgs e)
        {
            gruppe.password = newPasswortTextBox.Text;
            saveGruppeInDatabase();
            MessageBox.Show("Anmeldung abgeschlossen! \nIhre Gruppenkennung lautet: " + gruppe.gruppenKennung);
        }

        private void RefreshDatagrid(Gruppe gruppe)
        {
            alleMitgliederDataGridView.DataSource = null;
            alleMitgliederDataGridView.DataSource = gruppe.studenten;
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
#warning TO DO DATABASE
            return 2;
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
            Database db = new Database();
            string query = "insert into Gruppe values(\"" + gruppe.gruppenKennung + "\"," + gruppe.themenNummer + ",\"" + gruppe.password + "\")";
            db.ExecuteQuery(query);
            query = "insert into Zuordnung_GruppeBeleg values(\"" + gruppe.gruppenKennung + "\",\"" + gruppe.Belegkennung + "\")";
            db.ExecuteQuery(query);

            foreach (Student info in gruppe.studenten)
            {
                if(info.sNummer != "na") insertStudent(info);
            }
        }

        private void insertStudent(Student student)
        {
            Database db = new Database();
            string query = "insert into Student values(\"" + student.sNummer + "\",\"" + student.vorname + "\",\"" + student.name + "\",\"" + student.mail + "\",\"" + student.rolle + "\")";
            db.ExecuteQuery(query);
        }
    }
}
