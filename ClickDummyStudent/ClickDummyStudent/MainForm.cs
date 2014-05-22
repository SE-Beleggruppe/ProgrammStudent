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
    public partial class MainForm : Form
    {
        Gruppe gruppe;
        List<string> rollen = new List<string>();
        int minAnzahl;
        int maxAnzahl;
        public MainForm(string gruppenKennung, string belegkennung)
        {
            InitializeComponent();

            minAnzahl = getMinAnzahlMitglieder(belegkennung);
            maxAnzahl = getMaxAnzahlMitglieder(belegkennung);

            this.gruppe = getGruppeFromKennungS(gruppenKennung, belegkennung);
            this.gruppe.Belegkennung = belegkennung;

            mitgliederDataGridView.AllowUserToAddRows = false;
            mitgliederDataGridView.UserDeletingRow += mitgliederDataGridView_UserDeletingRow;


            updateRollen();
            updateMitgliederData(null);
            updateThemen();
        }

        void mitgliederDataGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            DataGridViewRow rowToDelete = e.Row;
            string sNummerToDelete = (string)rowToDelete.Cells[2].Value;
            if (sNummerToDelete == "na") e.Cancel = true;

            Database db = new Database();
            db.ExecuteQuery("delete from Student where sNummer=\"" + sNummerToDelete + "\"");
            db.ExecuteQuery("delete from Zuordnung_GruppeStudent where sNummer=\"" + sNummerToDelete + "\"");

            gruppe = getGruppeFromKennungS(gruppe.gruppenKennung, gruppe.Belegkennung);
            updateMitgliederData(null);
        }



        private Gruppe getGruppeFromKennungS(string kennung, string belegkennung)
        {
            Database db = new Database();
            foreach (string[] info in db.ExecuteQuery("select * from Gruppe where Gruppenkennung=\"" + kennung + "\""))
            {
                Gruppe neu = new Gruppe(info[0], Convert.ToInt32(info[1]), info[2]);
                neu.Belegkennung = belegkennung;
                neu.studenten = null;
                foreach (string[] info2 in db.ExecuteQuery("select * from Student where sNummer in (select sNummer from Zuordnung_GruppeStudent where Gruppenkennung=\"" + kennung + "\")"))
                {
                    neu.addStudent(new Student(info2[2], info2[1], info2[0], info2[3], info2[4]));
                }
                int studentenCount = neu.studenten.Count;
                if(neu.studenten.Count < maxAnzahl)
                {
                    for (int i = 0; i < maxAnzahl - studentenCount; i++)
                        neu.addStudent(new Student("na", "na", "na", "na", "na"));
                }
                return neu;
            }
            return null;
        }

        private void updateMitgliederData(List<Student> errorStudenten)
        {
            mitgliederDataGridView.Rows.Clear();
            (mitgliederDataGridView.Columns[4] as DataGridViewComboBoxColumn).DataSource = rollen;
            (mitgliederDataGridView.Columns[4] as DataGridViewComboBoxColumn).MinimumWidth = 150;
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

                if (info.sNummer == "na" && number < minAnzahl) 
                    mitgliederDataGridView.Rows[number].DefaultCellStyle.BackColor = Color.Yellow;
            }
            if (errorStudenten != null)
            {
                foreach (Student info in errorStudenten)
                {
                    int number = mitgliederDataGridView.Rows.Add();
                    mitgliederDataGridView.Rows[number].Cells[0].Value = info.name;
                    mitgliederDataGridView.Rows[number].Cells[1].Value = info.vorname;
                    mitgliederDataGridView.Rows[number].Cells[2].Value = info.sNummer;
                    mitgliederDataGridView.Rows[number].Cells[3].Value = info.mail;
                    mitgliederDataGridView.Rows[number].Cells[4].Value = info.rolle;

                    mitgliederDataGridView.Rows[number].DefaultCellStyle.BackColor = Color.Red;
                }
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
            List<Student> error = new List<Student>();
            for (int i = 0; i < mitgliederDataGridView.Rows.Count; i++)
            {
                string name = (string)mitgliederDataGridView.Rows[i].Cells[0].Value;
                string vorname = (string)mitgliederDataGridView.Rows[i].Cells[1].Value;
                string sNummer = (string)mitgliederDataGridView.Rows[i].Cells[2].Value;
                string mail = (string) mitgliederDataGridView.Rows[i].Cells[3].Value;
                string rolle = (string)mitgliederDataGridView.Rows[i].Cells[4].FormattedValue.ToString();

                if (sNummer != "na" && sNummer != "" && sNummer != null)
                {
                    Student student = new Student(name, vorname, sNummer, mail, rolle);
                    if (mitgliederDataGridView.Rows[i].Cells[2].ReadOnly || checkSNummer(sNummer))
                    {
                        if (mitgliederDataGridView.Rows[i].Cells[2].ReadOnly || checkMail(mail))
                        {
                            if (mitgliederDataGridView.Rows[i].Cells[2].ReadOnly) updateStudent(student);
                            else insertStudent(student, gruppe);
                        }
                        else
                        {
                            // MAIL IST NICHT RICHTIG
                            error.Add(student);
                        }
                    }
                    else
                    {
                        // S-Nummer ist falsch
                        error.Add(student);
                    }
                    
                }
            }
            int themennummer = getThemenNummerFromThema((string)comboBoxThemen.SelectedItem);
            Database db = new Database();
            db.ExecuteQuery("update Gruppe set Themennummer=" + themennummer + " where Gruppenkennung=\"" + gruppe.gruppenKennung + "\"");


            this.gruppe = getGruppeFromKennungS(gruppe.gruppenKennung, gruppe.Belegkennung);
            this.gruppe.Belegkennung = gruppe.Belegkennung;
            updateRollen();
            updateMitgliederData(error);
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
                return true;
            }
            else return false;
        }
    }
}
