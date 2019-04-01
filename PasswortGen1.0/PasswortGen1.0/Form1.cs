using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace PasswortGen1._0
{
    public partial class Form1 : Form
    {
        public static String pfad1 = @"C:\PasswortGen\";
        public static String pfad2 = ".ini";
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool anza = true;
            int anz = 0;
            string anzs = tbanz.Text;
            if (tbanz.Text == "")
            {
                MessageBox.Show("Sie haben keine Zahl angegeben!", "Fehler", MessageBoxButtons.OK);
            }
            else
            foreach (char x in anzs)
            {
                if (!Char.IsDigit(x))
                {
                    anza = false;
                }
                if (anza == false)
                {
                    MessageBox.Show("Sie dürfen nur Zahlen eingeben!", "Fehler", MessageBoxButtons.OK);
                }
                else
                if (anzs.Length == 2)
                {
                    anza = true;
                    if (!Char.IsDigit(anzs[1]))
                    {
                        anza = false;
                    }
                    if (anza == false)
                    {
                            MessageBox.Show("Sie dürfen nur Zahlen eingeben!", "Fehler", MessageBoxButtons.OK);
                    }
                    anz = Convert.ToInt32(anzs);
                }
                else
                anz = Convert.ToInt32(anzs);

            }
            

            string ret = string.Empty;
            System.Text.StringBuilder SB = new System.Text.StringBuilder();
            string Content = "1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvw!öäüÖÄÜß";
            Random rnd = new Random();
            for (int i = 0; i < anz; i++)
            {
                SB.Append(Content[rnd.Next(Content.Length)]);
            }
            string passwort = SB.ToString();
            tbpasswort.Text = passwort;
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (tbpasswort.Text == String.Empty)
            {
                MessageBox.Show("Es wurde kein Passwort genneriert", "Fehler", MessageBoxButtons.OK);
            }
            else
            if (tbspeichern.Text == String.Empty)
            {
                MessageBox.Show("Sie haben keinen Dateinamen angegeben", "Fehler", MessageBoxButtons.OK);
            }
            else
            if (!Directory.Exists(pfad1))
            {
                System.IO.Directory.CreateDirectory(pfad1);
            }
            else
            if (File.Exists(pfad1 + tbspeichern.Text + pfad2))
            {
                File.Delete(pfad1 + tbspeichern.Text + pfad2);
            }
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(pfad1 + tbspeichern.Text + pfad2, true))
            {
                file.WriteLine(tbpasswort.Text);
            }
            if (tbspeichern.Text != String.Empty)
            {
                MessageBox.Show("Sie haben die Zahlen erfoldreich gespeichert", "Erfolg", MessageBoxButtons.OK);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string[] eingabe = null;
            try
            {
                // einlesen der Datei in ein String-Array
                eingabe = File.ReadAllLines(pfad1 + tbladen.Text + pfad2, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                // Datei nicht vorhanden ...
                MessageBox.Show("Fehler beim Einlesen:\r\n", ex.Message);
                return;
            }
            tbpasswort.Text = eingabe[0];
            MessageBox.Show("Sie haben das Passwort erfolgreich geladen", "Laden erfolgreich", MessageBoxButtons.OK);
        }
    }
}
