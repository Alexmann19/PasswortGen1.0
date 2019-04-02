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
using System.Security.Cryptography;

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

        public static string pass = "";

        private static byte[] EncryptString(byte[] clearText, byte[] Key, byte[] IV)
        {
            MemoryStream ms = new MemoryStream();
            Rijndael alg = Rijndael.Create();
            alg.Key = Key;
            alg.IV = IV;
            CryptoStream cs = new CryptoStream(ms, alg.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(clearText, 0, clearText.Length);
            cs.Close();
            byte[] encryptedData = ms.ToArray();
            return encryptedData;
        }

        public static string EncryptString1(string passw, string master)
        {
            byte[] clearBytes = System.Text.Encoding.Unicode.GetBytes(passw);
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(master, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            byte[] encryptedData = EncryptString(clearBytes, pdb.GetBytes(32), pdb.GetBytes(16));
            return Convert.ToBase64String(encryptedData);
        }

        private static byte[] DecryptString(byte[] cipherData, byte[] Key, byte[] IV)
        {
            MemoryStream ms = new MemoryStream();
            Rijndael alg = Rijndael.Create();
            alg.Key = Key;
            alg.IV = IV;
            CryptoStream cs = new CryptoStream(ms, alg.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(cipherData, 0, cipherData.Length);
            cs.Close();
            byte[] decryptedData = ms.ToArray();
            return decryptedData;
        }

        public static string DecryptString1(string passw, string master)
        {
            byte[] cipherBytes = Convert.FromBase64String(passw);
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(master, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            byte[] decryptedData = DecryptString(cipherBytes, pdb.GetBytes(32), pdb.GetBytes(16));
            return System.Text.Encoding.Unicode.GetString(decryptedData);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool anza = true;
            int anz = 0;
            string anzs = tbanz.Text;
            if (tbanz.Text == "")
            {
                MessageBox.Show("Sie haben keine Zahl angegeben!", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    MessageBox.Show("Sie dürfen nur Zahlen eingeben!", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                            MessageBox.Show("Sie dürfen nur Zahlen eingeben!", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    anz = Convert.ToInt32(anzs);
                }
                else
                anz = Convert.ToInt32(anzs);

            }
            

            string ret = string.Empty;
            System.Text.StringBuilder SB = new System.Text.StringBuilder();
            string Content = "1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvw!+-§$%&?";
            Random rnd = new Random();
            for (int i = 0; i < anz; i++)
            {
                SB.Append(Content[rnd.Next(Content.Length)]);
            }
            string passwort = SB.ToString();
            tbpasswort.Text = passwort;
            pass = EncryptString1(tbpasswort.Text, tbmaster.Text);
            tbhash.Text = pass;

        }

        private void button2_Click(object sender, EventArgs e)
        {
        
            if (tbpasswort.Text == String.Empty)
            {
                MessageBox.Show("Es wurde kein Passwort genneriert", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            if (tbspeichern.Text == String.Empty)
            {
                MessageBox.Show("Sie haben keinen Dateinamen angegeben", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                file.WriteLine(pass);
            }
            if (tbspeichern.Text != String.Empty)
            {
                MessageBox.Show("Sie haben die Zahlen erfoldreich gespeichert", "Erfolg", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (tbmasterload.Text == "")
            {
                MessageBox.Show("Kein Masterpasswort gesetzt", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
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
                string passload = DecryptString1(eingabe[0], tbmasterload.Text);
                tbpasswort.Text = passload;
                MessageBox.Show("Sie haben das Passwort erfolgreich geladen", "Laden erfolgreich", MessageBoxButtons.OK);
            }
        }
    }
}
