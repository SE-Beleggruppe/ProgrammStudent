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
    public partial class MainForm : Form
    {
        Gruppe gruppe;
        List<string> rollen = new List<string>();
        int maxAnzahl;
        public MainForm(string gruppenKennung, string belegkennung)
        {
            InitializeComponent();
            this.gruppe = getGruppeFromKennungS(gruppenKennung, belegkennung);
            this.gruppe.Belegkennung = belegkennung;
            maxAnzahl = getMaxAnzahlMitglieder(belegkennung);
            updateRollen();
            updateMitgliederData();
            updateThemen();
        }


        private Gruppe getGruppeFromKennungS(string kennung, string belegkennung)
        {
            Database db = new Database();
            foreach (string[] info in db.ExecuteQuery("select * from Gruppe where Gruppenkennung=\"" + kennung + "\""))
            {
                Gruppe neu = new Gruppe(info[0], Convert.ToInt32(info[1]), info[2]);
                neu.Belegkennung = belegkennung;
                foreach (string[] info2 in db.ExecuteQuery("select * from Student where sNummer in (select sNummer from Zuordnung_GruppeStudent where Gruppenkennung=\"" + kennung + "\")"))
                {
                    neu.addStudent(new Student(info2[2], info2[1], info2[0], info2[3], info2[4]));
                }
                int minAnz = getMinAnzahlMitglieder(neu.Belegkennung);
                int studentenCount = neu.studenten.Count;
                if(neu.studenten.Count < minAnz)
                {
                    for (int i = 0; i < minAnz - studentenCount; i++)
                        neu.addStudent(new Student("na", "na", "na", "na", "na"));
                }
                return neu;
            }
            return null;
        }

        private void updateMitgliederData()
        {
            mitgliederDataGridView.Rows.Clear();
            (mitgliederDataGridView.Columns[4] as DataGridViewComboBoxColumn).DataSource = rollen;
            (mitgliederDataGridView.Columns[3] as DataGridViewTextBoxColumn).MinimumWidth = 250;
            foreach (Student info in gruppe.studenten)
            {
                int number = mitgliederDataGridView.Rows.Add();
                mitgliederDataGridView.Rows[number].Cells[0].Value = info.name;
                mitgliederDataGridView.Rows[number].Cells[1].Value = info.vorname;
                mitgliederDataGridView.Rows[number].Cells[2].Value = info.sNummer;
                if (info.sNummer != "na") mitgliederDataGridView.Rows[number].Cells[2].ReadOnly = true;
                mitgliederDataGridView.Rows[number].Cells[3].Value = info.mail;
                mitgliederDataGridView.Rows[number].Cells[4].Value = info.rolle;
            }
        }

        private void updateThemen()
        {
            Database db = new Database();
            List<string> erg = new List<string>();
            List<string[]> output = db.ExecuteQuery("select Aufgabe from Thema where Themennummer in (select Themennummer from Zuordnung_BelegThema where Belegkennung=\"" + gruppe.Belegkennung + "\")");
            foreach (string[] info in output)
            {
                erg.Add(info[0]);
            }
            comboBoxThemen.DataSource = erg;

            comboBoxThemen.SelectedItem = db.ExecuteQuery("select Aufgabe from Thema where Themennummer in (select Themennummer from Gruppe where Gruppenkennung=\"" + gruppe.gruppenKennung + "\")").First()[0];
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

        private int getMinAnzahlMitglieder(string beKennung)
        {
            Database db = new Database();
            List<string[]> output = db.ExecuteQuery("select MinAnzMitglieder from Beleg where Belegkennung=\"" + beKennung + "\"");
            return Convert.ToInt32(output.First()[0]);
        }

        private int getMaxAnzahlMitglieder(string beKennung)
        {
            Database db = new Database();
            List<string[]> output = db.ExecuteQuery("select MaxAnzMitglieder from Beleg where Belegkennung=\"" + beKennung + "\"");
            return Convert.ToInt32(output.First()[0]);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            Application.Exit();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < mitgliederDataGridView.Rows.Count; i++)
            {
                string name = (string)mitgliederDataGridView.Rows[i].Cells[0].Value;
                string vorname = (string)mitgliederDataGridView.Rows[i].Cells[1].Value;
                string sNummer = (string)mitgliederDataGridView.Rows[i].Cells[2].Value;
                string mail = (string) mitgliederDataGridView.Rows[i].Cells[3].Value;
                string rolle = (string)mitgliederDataGridView.Rows[i].Cells[4].FormattedValue.ToString();

                if (sNummer != "na" && sNummer != "" && sNummer != null)
                {
                    if (mitgliederDataGridView.Rows[i].Cells[2].ReadOnly) updateStudent(new Student(name, vorname, sNummer, mail, rolle));
                    else insertStudent(new Student(name, vorname, sNummer, mail, rolle), gruppe);
                }
            }
            int themennummer = getThemenNummerFromThema((string)comboBoxThemen.SelectedItem);
            Database db = new Database();
            db.ExecuteQuery("update Gruppe set Themennummer=" + themennummer + " where Gruppenkennung=\"" + gruppe.gruppenKennung + "\"");


            this.gruppe = getGruppeFromKennungS(gruppe.gruppenKennung, gruppe.Belegkennung);
            this.gruppe.Belegkennung = gruppe.Belegkennung;
            updateRollen();
            updateMitgliederData();
            updateThemen();

            MessageBox.Show("Änderungen erfolgreich gespeichert!");
        }

        private void updateStudent(Student student)
        {
            Database db = new Database();
            string query = "update Student set Nachname=\"" + student.name + "\", Vorname=\"" + student.vorname + "\", Mail=\"" + student.mail + "\", Rolle=\"" + student.rolle + "\" where sNummer=\"" + student.sNummer + "\"";
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

        private int getThemenNummerFromThema(string thema)
        {
            Database db = new Database();
            return Convert.ToInt32(db.ExecuteQuery("select Themennummer from Thema where Aufgabe=\"" + thema + "\"").First()[0]);
        }
    }
}
