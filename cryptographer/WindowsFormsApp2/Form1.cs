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
using System.Text.RegularExpressions;  
using System.Threading;
using System.Security.Cryptography;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string EnCode = "kf9hbcn6qkbycj0";
        string FileDir = "";

        private void button1_Click(object sender, EventArgs e)
        {
            string f = String.Empty; bool t = true;
            while (t)
            {
                t = false;
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    f = openFileDialog1.FileName;
                    if (IsEmpty(f))
                    {
                        MessageBox.Show("File is empty. Please choose another file.", "Error");
                        t = true;
                    }
                    else
                    {
                        if (IsEncrypted(f))
                        {
                            MessageBox.Show("File is already encrypted. Please choose another file.", "Error");
                            t = true;
                        }
                        else
                        {
                            FileDir = f;
                            textBox2.Clear();
                            textBox5.Clear();
                            textBox10.Hide();
                            textBox11.Hide();
                            flowLayoutPanel1.Show();
                            flowLayoutPanel3.Hide();
                        }
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string f = String.Empty; bool t = true;
            while (t)
            {
                t = false;
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    f = openFileDialog1.FileName;
                    if (IsEmpty(f))
                    {
                        MessageBox.Show("File is empty. Please choose another file.", "Error");
                        t = true;
                    }
                    else
                    {
                        if (!IsEncrypted(f))
                        {
                            MessageBox.Show("File is not encrypted. Please choose another file.", "Error");
                            t = true;
                        }
                        else
                        {
                            FileDir = f;
                            textBox8.Clear();
                            textBox9.Hide();
                            flowLayoutPanel2.Show();
                            flowLayoutPanel3.Hide();
                        }
                    }
                }
            }
        }

        bool IsEncrypted(string dir)
        {
            bool t;
            if (File.ReadLines(dir).First() == EnCode) { t = true; } else { t = false; }
            return t;
        }
        bool IsEmpty(string dir)
        {
            bool t;
            if (File.ReadAllText(dir) == String.Empty) { t = true; } else { t = false; }
            return t;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Regex expression = new Regex("^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])([a-zA-Z0-9]+)$");
            if ((!expression.IsMatch(textBox2.Text)) || (textBox2.Text.Length < 8))
            {
                textBox10.Hide();
                textBox11.Hide();
                Thread.Sleep(100);
                textBox10.Show();
                return;
            }
            else
            {
                if (!(textBox2.Text == textBox5.Text))
                {
                    textBox11.Hide();
                    textBox10.Hide();
                    Thread.Sleep(100);
                    textBox11.Show();
                    return;
                }
            }
            flowLayoutPanel1.Hide();
            Encrypt(textBox2.Text);
            MessageBox.Show("File has been successfully encrypted.");
            flowLayoutPanel3.Show();
            flowLayoutPanel4.Hide();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            flowLayoutPanel3.Show();
            flowLayoutPanel1.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (!ValidatePass(textBox8.Text, out string salt))
            {
                textBox9.Hide();
                Thread.Sleep(100);
                textBox9.Show();
                return;
            }
            flowLayoutPanel2.Hide();
            Decrypt(textBox8.Text, salt);
            MessageBox.Show("File has been successfully decrypted.");
            flowLayoutPanel3.Show();
            flowLayoutPanel4.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            flowLayoutPanel3.Show();
            flowLayoutPanel2.Hide();
        }

        bool ValidatePass(string pass, out string salt)
        {
            salt = File.ReadAllLines(FileDir).Skip(1).Take(1).First();
            string hash = File.ReadAllLines(FileDir).Skip(2).Take(1).First();
            byte[] saltbytes = Convert.FromBase64String(salt);
            Rfc2898DeriveBytes password = new Rfc2898DeriveBytes(pass, saltbytes, 10000);
            return Convert.ToBase64String(password.GetBytes(32)) == hash;
        }

        void Encrypt(string pass)
        {           
            progressBar1.Step = 1;
            progressBar1.Value = 0;
            flowLayoutPanel4.Show();
            byte[] saltbytes = new byte[64];
             RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
             provider.GetNonZeroBytes(saltbytes);
             string salt = Convert.ToBase64String(saltbytes);
             Rfc2898DeriveBytes password = new Rfc2898DeriveBytes(pass, saltbytes, 10000);             
            using (RijndaelManaged alg = new RijndaelManaged())
            {
                alg.Key = password.GetBytes(32);
                alg.IV = password.GetBytes(16);
                alg.Mode = CipherMode.CBC;
                alg.Padding = PaddingMode.Zeros;
                ICryptoTransform encryptor = alg.CreateEncryptor(alg.Key, alg.IV);
                string hash = Convert.ToBase64String(alg.Key);              
                MemoryStream ms = new MemoryStream();
                FileStream fs = new FileStream(FileDir, FileMode.Open);
                progressBar1.Maximum = (int)fs.Length;
                CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
                int data;
                while ((data = fs.ReadByte()) != -1)
                {
                    cs.WriteByte((byte)data);
                    progressBar1.PerformStep();
                }
                cs.Close();
                fs.Close();
                ms.Close();
                File.WriteAllText(FileDir, EnCode + "\r\n" + salt + "\r\n" + hash + "\r\n");
                File.AppendAllText(FileDir, Convert.ToBase64String(ms.ToArray()));
            }
        }
        void Decrypt(string pass, string salt)
        {
            progressBar1.Step = 1;
            progressBar1.Value = 0;
            flowLayoutPanel4.Show();
            byte[] saltbytes = Convert.FromBase64String(salt);
            Rfc2898DeriveBytes password = new Rfc2898DeriveBytes(pass, saltbytes, 10000);
            var t = Regex.Split(File.ReadAllText(FileDir), "\r\n").Skip(3);
            string f = String.Join("\r\n", t.ToArray());
            using (RijndaelManaged alg = new RijndaelManaged())
            {
                alg.Key = password.GetBytes(32);
                alg.IV = password.GetBytes(16);
                alg.Mode = CipherMode.CBC;
                alg.Padding = PaddingMode.Zeros;
                ICryptoTransform decryptor = alg.CreateDecryptor(alg.Key, alg.IV);
                MemoryStream ms = new MemoryStream(Convert.FromBase64String(f));
                progressBar1.Maximum = (int)ms.Length;
                FileStream fs = new FileStream(FileDir, FileMode.Create);
                CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
                int data;
                while ((data = cs.ReadByte()) != -1)
                {
                    fs.WriteByte((byte)data);
                    progressBar1.PerformStep();
                }
                cs.Close();
                fs.Close();
                ms.Close();
            }
        }
    }
}
