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
        DataGridViewComboBoxColumn cmb;
        public MainForm(string gruppenKennung, string belegkennung)
        {
            InitializeComponent();
            this.gruppe = getGruppeFromKennungS(gruppenKennung, belegkennung);
            this.gruppe.Belegkennung = belegkennung;
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
            mitgliederDataGridView.DataSource = null;
            mitgliederDataGridView.DataSource = gruppe.studenten;
            mitgliederDataGridView.Columns[4].Visible = false;
            cmb = new DataGridViewComboBoxColumn();
            cmb.HeaderText = "Rolle auswählen";
            cmb.Name = "cmb";
            updateRollen();
            mitgliederDataGridView.Columns.Add(cmb);
            string a = cmb.ValueMember;
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
            cmb.DataSource = null;
            cmb.DataSource = rollen;
            int count = 0;
            foreach (Student info in gruppe.studenten)
            {
#warning TO DO SET INFO.ROLLE AS VALUE OF COMBOBOX
                count++;
            }
        }

        private int getMinAnzahlMitglieder(string beKennung)
        {
            Database db = new Database();
            List<string[]> output = db.ExecuteQuery("select MinAnzMitglieder from Beleg where Belegkennung=\"" + beKennung + "\"");
            return Convert.ToInt32(output.First()[0]);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            Application.Exit();
        }
    }
}
