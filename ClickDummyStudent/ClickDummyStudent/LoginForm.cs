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
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            Regex regExBelegKennung = new Regex("^beleg");
            Regex regExGruppenKennung = new Regex("^case");
               if (checkGruppeLogin(loginTextField.Text,passwordTextField.Text))
               {
                    MainForm mainForm = new MainForm();
                    mainForm.Show();
                    Hide();
                }
                else
                {
                  if (checkBelegLogin(loginTextField.Text, passwordTextField.Text))
                  {
                      FormLeiterNeuEingeben leiterEingeben = new FormLeiterNeuEingeben(loginTextField.Text);
                     leiterEingeben.Show();
                     Hide();
                  }
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
            List<string[]> output = db.ExecuteQuery("select * from Gruppe");
            foreach (string[] info in output)
            {
                if (info[0] == login)
                {
                    if (info[2] == password)
                    {
                        return true;
                    }
                    else return false;
                }
            }
            return false;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
