using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RSAEncryption
{
    public partial class Form1 : Form
    {
        UnicodeEncoding ByteConverter = new UnicodeEncoding();
        RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

        byte[] plaintext;
        byte[] encryptedtext;
        public Form1()
        {
            InitializeComponent();
            textBox1.MaxLength = 58;
        }

        private void encryptButton_Click(object sender, EventArgs e)
        {
            try
            {
                string Path = null;
              
                plaintext = Convert.FromBase64String(textBox1.Text);
                           
                encryptedtext = Encryption(plaintext, RSA.ExportParameters(false), false);
                using (var sfd = new SaveFileDialog())
                {
                    sfd.Filter = "txt files (*.txt)|*.txt";
                    sfd.RestoreDirectory = true;
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        Path = sfd.FileName;
                    }
                }
                using (StreamWriter writer = new StreamWriter(Path))
                {
                    writer.Write(Convert.ToBase64String(encryptedtext));
                }
                resultTextBox.Text = Convert.ToBase64String(encryptedtext);
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("Encryption failed.");
            }
        }

        private void decryptButton_Click(object sender, EventArgs e)
        {
            try
            {
                plaintext = Convert.FromBase64String(textBox2.Text);
                byte[] decryptedtext = Decryption(plaintext, RSA.ExportParameters(true), false);
                resultTextBox.Text = Convert.ToBase64String(decryptedtext);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Console.WriteLine("Encryption failed.");
            }
        }


        /*---- Encryption and Decryption methods ----*/
        static public byte[] Encryption(byte[] Data, RSAParameters RSAKey, bool DoOAEPPadding)
        {
            try
            {
                byte[] encryptedData;
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.ImportParameters(RSAKey);
                    string test = RSA.ToString();
                    encryptedData = RSA.Encrypt(Data, DoOAEPPadding);
                }   
                return encryptedData;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        static public byte[] Decryption(byte[] Data, RSAParameters RSAKey, bool DoOAEPPadding)
        {
            try
            {
                byte[] decryptedData;
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.ImportParameters(RSAKey);
                    decryptedData = RSA.Decrypt(Data, DoOAEPPadding);
                }
                return decryptedData;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            var content = string.Empty;
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                string path;
                ofd.Filter = "txt files (*.txt)|*.txt";
                ofd.RestoreDirectory = true;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    path = ofd.FileName;
                    var data = ofd.OpenFile();
                    using (StreamReader sr = new StreamReader(data))
                    {
                        content = sr.ReadToEnd();
                    }
                }
            }
            try
            {
                byte[] decryptedtext = Decryption(Convert.FromBase64String(content),
                RSA.ExportParameters(true), false);
                resultTextBox.Text = Convert.ToBase64String(decryptedtext);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Console.WriteLine("Encryption failed.");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
