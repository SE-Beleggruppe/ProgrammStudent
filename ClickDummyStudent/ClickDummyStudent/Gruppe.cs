using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickDummyStudent
{
    public class Gruppe
    {
        public string GruppenKennung { get; set; }
        public List<Student> Studenten { get; set; }
        public int ThemenNummer { get; set; }
        public string Password { get; set; }
        public string Belegkennung { get; set; }

        public Gruppe(string password, string belegkennung)
        {
            this.Password = password;
            this.Belegkennung = belegkennung;
            this.Studenten = new List<Student>();
        }

        public Gruppe(string kennung, int themennummer, string password)
        {
            this.GruppenKennung = kennung;
            this.ThemenNummer = themennummer;
            this.Password = password;
            this.Studenten = new List<Student>();
        }

        public void addStudent(Student student)
        {
            if (this.Studenten == null) this.Studenten = new List<Student>();
            if(student != null) this.Studenten.Add(student);
        }
    }
}