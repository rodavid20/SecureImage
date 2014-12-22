using System;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace SecureImage
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            // Show the dialog and get result.
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                txtFile.Text = openFileDialog1.FileName;
            }
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtFile.Text))
            {
                //Get the Input File Extension.
                string fileExt = Path.GetExtension(txtFile.Text);
                //Get the Input File path without extension. 
                string fileNameWoExt = txtFile.Text.Remove(txtFile.Text.Length - fileExt.Length, fileExt.Length);
                
                //Build the File Path for the encrypted (output) file.
                string output = fileNameWoExt + "_enc" + fileExt;
                Encrypt(txtFile.Text, output);
                MessageBox.Show("Image encrypted", "Success");
                txtFile.Text = String.Empty;
            }
            else
            {
                MessageBox.Show("Please select a jpg image", "Error");
            }
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtFile.Text))
            {
                //Get the Input File Extension.
                string fileExt = Path.GetExtension(txtFile.Text);
                //Get the Input File path without extension. 
                string fileNameWoExt = txtFile.Text.Remove(txtFile.Text.Length - fileExt.Length, fileExt.Length);

                //Build the File Path for the encrypted (output) file.
                string output = fileNameWoExt + "_dec" + fileExt;
                Decrypt(txtFile.Text, output);
                MessageBox.Show("Image decrypted", "Success");
                txtFile.Text = String.Empty;
            }
            else
            {
                MessageBox.Show("Please select a encrypted jpg image", "Error");
            }
        }      

        private void Encrypt(string inputFilePath, string outputfilePath)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (FileStream fsOutput = new FileStream(outputfilePath, FileMode.Create))
                {
                    using (CryptoStream cs = new CryptoStream(fsOutput, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        using (FileStream fsInput = new FileStream(inputFilePath, FileMode.Open))
                        {
                            int data;
                            while ((data = fsInput.ReadByte()) != -1)
                            {
                                cs.WriteByte((byte)data);
                            }
                        }
                    }
                }
            }
        }

        private void Decrypt(string inputFilePath, string outputfilePath)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (FileStream fsInput = new FileStream(inputFilePath, FileMode.Open))
                {
                    using (CryptoStream cs = new CryptoStream(fsInput, encryptor.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        using (FileStream fsOutput = new FileStream(outputfilePath, FileMode.Create))
                        {
                            int data;
                            while ((data = cs.ReadByte()) != -1)
                            {
                                fsOutput.WriteByte((byte)data);
                            }
                        }
                    }
                }
            }
        }
    }
}
