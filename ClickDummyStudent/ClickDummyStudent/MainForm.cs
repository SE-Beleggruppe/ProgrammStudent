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
        public MainForm(Gruppe gruppe)
        {
            InitializeComponent();
            this.gruppe = gruppe;
            loadStudentenInGruppe(gruppe);
            updateThemen();
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

        private void loadStudentenInGruppe(Gruppe gruppe)
        {
            Database db = new Database();
            string query = "select * from Student where sNummer in (select sNummer from Zuordnung_GruppeStudent where Gruppenkennung=\"" + gruppe.gruppenKennung + "\")";
            List<string[]> output = db.ExecuteQuery(query);
            foreach (string[] info in output)
            {
                gruppe.addStudent(new Student(info[2],info[1],info[0],info[3],info[4]));
            }
            updateMitgliederData();
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
            cmb.DataSource = null;
            cmb.DataSource = rollen;
            int count = 0;
            foreach (Student info in gruppe.studenten)
            {
                mitgliederDataGridView.Rows[count].Cells[4].Value = info.rolle;
                count++;
            }
        }
    }
}
