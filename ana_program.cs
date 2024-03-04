using System;
using System.IO;
using System.Text;
using System.Linq;
using Renci.SshNet;
using System.Drawing;
using Newtonsoft.Json;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Socket_GUI
{
    public partial class ana_program : Form
    {
        private Timer timer;
        public static string veri;
        private int pictureBoxIndex;
        private SftpClient sftpClient;
        private readonly Image yeniResim;
        private PictureBox[] pictureBoxes;
        private bool exitButtonClicked = false;
        private static readonly Random random = new Random();
        private const string ListBoxDataFile = "kayitli_oturumlar.txt";
        private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";


        // PROGRAM BAŞLATILDIĞINDA YAPILACAKLAR ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                                                                                                                                                                                              //
        public ana_program()                                                                                                                                                                                  //
        {                                                                                                                                                                                                     //
            InitializeComponent();                                                                                                                                                                            //
            LoadListBoxData();                                                                                                                                                                                //
            textbox10_ilkdeger();                                                                                                                                                                             //
            WriteToLogFile("Program Başlatıldı...");                                                                                                                                                          //
            timer = new Timer();                                                                                                                                                                              //
            timer.Interval = 50;                                                                                                                                                                              //
            timer.Tick += Timer_Tick;                                                                                                                                                                         //
            yeniResim = Resource1.turuncunokta;                                                                                                                                                               //
            tabControl1.TabPages[1].Enabled = false;                                                                                                                                                          //
            pictureBoxes = new PictureBox[] { pictureBox3, pictureBox4, pictureBox5, pictureBox6, pictureBox7, pictureBox8 };                                                                                 //
            label28.Text     = null;                                                                                                                                                                          //
            label42.Text     = null;                                                                                                                                                                          //
            button1.Enabled  = false;                                                                                                                                                                         //
            button2.Enabled  = false;                                                                                                                                                                         //
            button3.Enabled  = false;                                                                                                                                                                         //
            button4.Enabled  = false;                                                                                                                                                                         //
            button5.Enabled  = false;                                                                                                                                                                         //
            button6.Enabled  = false;                                                                                                                                                                         //
            button7.Enabled  = false;                                                                                                                                                                         //
            button8.Enabled  = false;                                                                                                                                                                         //
            button9.Enabled  = false;                                                                                                                                                                         //
            button14.Enabled = false;                                                                                                                                                                         //
            button15.Enabled = false;                                                                                                                                                                         //
            comboBox20.KeyDown += comboBox_KeyDown;                                                                                                                                                           //
            comboBox19.KeyDown += comboBox_KeyDown;                                                                                                                                                           //
            comboBox18.KeyDown += comboBox_KeyDown;                                                                                                                                                           //
            comboBox17.KeyDown += comboBox_KeyDown;                                                                                                                                                           //
            comboBox16.KeyDown += comboBox_KeyDown;                                                                                                                                                           //
            comboBox15.KeyDown += comboBox_KeyDown;                                                                                                                                                           //
            comboBox14.KeyDown += comboBox_KeyDown;                                                                                                                                                           //
            comboBox13.KeyDown += comboBox_KeyDown;                                                                                                                                                           //
            comboBox12.KeyDown += comboBox_KeyDown;                                                                                                                                                           //
            comboBox11.KeyDown += comboBox_KeyDown;                                                                                                                                                           //
            comboBox10.KeyDown += comboBox_KeyDown;                                                                                                                                                           //
            comboBox9.KeyDown  += comboBox_KeyDown;                                                                                                                                                           //
            comboBox8.KeyDown  += comboBox_KeyDown;                                                                                                                                                           //
            comboBox7.KeyDown  += comboBox_KeyDown;                                                                                                                                                           //
            comboBox6.KeyDown  += comboBox_KeyDown;                                                                                                                                                           //
            comboBox5.KeyDown  += comboBox_KeyDown;                                                                                                                                                           //
            comboBox4.KeyDown  += comboBox_KeyDown;                                                                                                                                                           //
            comboBox3.KeyDown  += comboBox_KeyDown;                                                                                                                                                           //
            comboBox2.KeyDown  += comboBox_KeyDown;                                                                                                                                                           //
            comboBox1.KeyDown  += comboBox_KeyDown;                                                                                                                                                           //
        }                                                                                                                                                                                                     //


        // BUTONLAR ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                                                                                                                                                                                              //
        // "BAĞLAN" BUTONU                                                                                                                                                                                    //     
        private async void button1_Click_1(object sender, EventArgs e)                                                                                                                                        //
        {                                                                                                                                                                                                     //
            if (IsValidIPv4(textBox2.Text))                                                                                                                                                                   //
            {                                                                                                                                                                                                 //
                label28.Text = null;                                                                                                                                                                          //
                treeView1.Nodes.Clear();                                                                                                                                                                      //
                timer.Start();                                                                                                                                                                                //
                pictureBoxIndex = 0;                                                                                                                                                                          //
                button1.Enabled = false;                                                                                                                                                                      //
                button8.Enabled = true;                                                                                                                                                                       //
                await Delay(2000);                                                                                                                                                                            //
                await Task.Run(() => ConnectToSftp());                                                                                                                                                        //
            }                                                                                                                                                                                                 //
            else                                                                                                                                                                                              //
            {                                                                                                                                                                                                 //
                MessageBox.Show("Geçersiz IP Adresi !                              ", "Geçersiz Format", MessageBoxButtons.OK, MessageBoxIcon.Warning);                                                       //
                WriteToLogFile("Geçersiz IP Adresi!");                                                                                                                                                        //
            }                                                                                                                                                                                                 //
        }                                                                                                                                                                                                     //
                                                                                                                                                                                                              //
        private void UpdateButton1State()                                                                                                                                                                     //
        {                                                                                                                                                                                                     //
            button1.Enabled = !string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrEmpty(textBox2.Text) && !string.IsNullOrEmpty(textBox8.Text);                                                           //
        }                                                                                                                                                                                                     //
                                                                                                                                                                                                              //
        // "DIŞARI AKTAR" BUTONU                                                                                                                                                                              //     
        private void button2_Click(object sender, EventArgs e)                                                                                                                                                //
        {                                                                                                                                                                                                     //
            DialogResult result = MessageBox.Show("Dosya Aktarımını Başlatmak İstiyor musunuz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);                                                   //
            if (result == DialogResult.Yes)                                                                                                                                                                   //
            {                                                                                                                                                                                                 //
                WriteToLogFile("Dosya Aktarımı Başlatılıyor...");                                                                                                                                             //
                button15.Enabled = true;                                                                                                                                                                      //
                button2.Enabled = false;                                                                                                                                                                      //
                string aktarilacak_dosya_yolu = dosyaYoluAl("veri_tabani.json", listBox4.SelectedItem.ToString());                                                                                            //
                DosyaKopyala(aktarilacak_dosya_yolu, textBox7.Text);                                                                                                                                          //
            }                                                                                                                                                                                                 //
        }                                                                                                                                                                                                     //
                                                                                                                                                                                                              //
        // "VERİLERİ AL" BUTONU                                                                                                                                                                               //     
        private void button3_Click(object sender, EventArgs e)                                                                                                                                                //
        {                                                                                                                                                                                                     //
            tabControl1.TabPages[1].Enabled = true;                                                                                                                                                           //
            tabControl1.SelectedIndex = 1;                                                                                                                                                                    //
        }                                                                                                                                                                                                     //
                                                                                                                                                                                                              //
        // "DOSYA SEÇ" BUTONU                                                                                                                                                                                 //     
        private void button4_Click(object sender, EventArgs e)                                                                                                                                                //
        {                                                                                                                                                                                                     //
            string selectedFilePath = treeView1.SelectedNode?.FullPath.Replace("\\", "/");                                                                                                                    //
            if (!string.IsNullOrEmpty(selectedFilePath))                                                                                                                                                      //
            {                                                                                                                                                                                                 //
                string extension = Path.GetExtension(selectedFilePath);                                                                                                                                       //
                if (!string.IsNullOrEmpty(extension))                                                                                                                                                         //
                {                                                                                                                                                                                             //
                    if (!listBox3.Items.Contains(selectedFilePath))                                                                                                                                           //
                        listBox3.Items.Add(selectedFilePath);                                                                                                                                                 //
                }                                                                                                                                                                                             //
                else                                                                                                                                                                                          //
                    MessageBox.Show("Seçilen öğe bir dosya değil!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);                                                                                   //
            }                                                                                                                                                                                                 //
        }                                                                                                                                                                                                     //
                                                                                                                                                                                                              //
        // "DOSYA SİL" BUTONU                                                                                                                                                                                 //     
        private void button5_Click(object sender, EventArgs e)                                                                                                                                                //
        {                                                                                                                                                                                                     //
            var selectedItems = listBox3.SelectedItems.Cast<object>().ToList();                                                                                                                               //
            foreach (var selectedItem in selectedItems)                                                                                                                                                       //
            {                                                                                                                                                                                                 //
                listBox3.Items.Remove(selectedItem);                                                                                                                                                          //
            }                                                                                                                                                                                                 //
        }                                                                                                                                                                                                     //
                                                                                                                                                                                                              //
        // "DİZİN SEÇ" BUTONU                                                                                                                                                                                 //     
        private void button6_Click(object sender, EventArgs e)                                                                                                                                                //
        {                                                                                                                                                                                                     //
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();                                                                                                                             //
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)                                                                                                                                         //
                textBox7.Text = folderBrowserDialog1.SelectedPath;                                                                                                                                            //
        }                                                                                                                                                                                                     //
                                                                                                                                                                                                              //
        // "KAYDET" BUTONU                                                                                                                                                                                    //     
        private void button7_Click(object sender, EventArgs e)                                                                                                                                                //
        {                                                                                                                                                                                                     //
            string birlesmisDeger = "";                                                                                                                                                                       //
            if (!string.IsNullOrEmpty(textBox2.Text) && !string.IsNullOrEmpty(textBox3.Text))                                                                                                                 //
            {                                                                                                                                                                                                 //
                if (IsValidIPv4(textBox2.Text))                                                                                                                                                               //
                {                                                                                                                                                                                             //
                    birlesmisDeger = textBox2.Text + " / " + textBox3.Text + " / " + textBox1.Text; ;                                                                                                         //
                    if (!listBox2.Items.Contains(birlesmisDeger))                                                                                                                                             //
                        listBox2.Items.Add(birlesmisDeger);                                                                                                                                                   //
                }                                                                                                                                                                                             //
                else                                                                                                                                                                                          //
                    MessageBox.Show("Geçersiz IP Adresi !                              ", "Geçersiz Format", MessageBoxButtons.OK, MessageBoxIcon.Warning);                                                   //
                WriteToLogFile("Geçersiz IP Adresi!");                                                                                                                                                        //
            }                                                                                                                                                                                                 //
        }                                                                                                                                                                                                     //
                                                                                                                                                                                                              //
        private void UpdateButton7State()                                                                                                                                                                     //
        {                                                                                                                                                                                                     //
            button7.Enabled = !string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrEmpty(textBox2.Text) && !string.IsNullOrEmpty(textBox3.Text) && !string.IsNullOrEmpty(textBox8.Text);                   //
        }                                                                                                                                                                                                     //
                                                                                                                                                                                                              //
        // "İPTAL" BUTONU                                                                                                                                                                                     //       
        private void button8_Click_1(object sender, EventArgs e)                                                                                                                                              //
        {                                                                                                                                                                                                     //
            label28.Text = null;                                                                                                                                                                              //
            label42.Text = null;                                                                                                                                                                              //
            PictureBox[] pictureBoxes = { pictureBox3, pictureBox4, pictureBox5, pictureBox6, pictureBox7, pictureBox8 };                                                                                     //
            for (int i = 0; i < pictureBoxes.Length; i++)                                                                                                                                                     //
                pictureBoxes[i].Image = Resource1.beyaznokta;                                                                                                                                                 //
            pictureBoxes[5].Image = null;                                                                                                                                                                     //
            timer.Stop();                                                                                                                                                                                     //
            button8.Enabled = false;                                                                                                                                                                          //
            button1.Enabled = true;                                                                                                                                                                           //
            if (sftpClient != null)                                                                                                                                                                           //
            {                                                                                                                                                                                                 //
                if (sftpClient.IsConnected)                                                                                                                                                                   //
                {                                                                                                                                                                                             //
                    try                                                                                                                                                                                       //
                    {                                                                                                                                                                                         //
                        sftpClient.Disconnect();                                                                                                                                                              //
                    }                                                                                                                                                                                         //
                    catch (Exception ex)                                                                                                                                                                      //
                    {                                                                                                                                                                                         //
                        MessageBox.Show("Bağlantıyı Kapatma Sırasında Hata Oluştu: " + ex.Message, "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);                                                      //
                        WriteToLogFile("Bağlantıyı Kapatma Sırasında Hata Oluştu: " + ex.Message);                                                                                                            //
                    }                                                                                                                                                                                         //
                }                                                                                                                                                                                             //
                sftpClient.Dispose();                                                                                                                                                                         //
                sftpClient = null;                                                                                                                                                                            //
                treeView1.Nodes.Clear();                                                                                                                                                                      //
                if (listBox3.SelectedIndex != -1)                                                                                                                                                             //
                {                                                                                                                                                                                             //
                    listBox3.Items.RemoveAt(listBox3.SelectedIndex);                                                                                                                                          //
                }                                                                                                                                                                                             //
                listBox3.Items.Clear();                                                                                                                                                                       //
                if (treeView1.SelectedNode != null)                                                                                                                                                           //
                {                                                                                                                                                                                             //
                    treeView1.Nodes.Remove(treeView1.SelectedNode);                                                                                                                                           //
                }                                                                                                                                                                                             //
                treeView1.Nodes.Clear();                                                                                                                                                                      //
                button4.Enabled = false;                                                                                                                                                                      //
            }                                                                                                                                                                                                 //
        }                                                                                                                                                                                                     //
                                                                                                                                                                                                              //
        // "DOSYA ADI DEĞİŞ" BUTONU                                                                                                                                                                           //
        private void button9_Click(object sender, EventArgs e)                                                                                                                                                //
        {                                                                                                                                                                                                     //
                                                                                                                                                                                                              //
        }                                                                                                                                                                                                     //
                                                                                                                                                                                                              //
        // "ÇIKIŞ" BUTONU                                                                                                                                                                                     //       
        private void button10_Click(object sender, EventArgs e)                                                                                                                                               //
        {                                                                                                                                                                                                     //
            DialogResult result = MessageBox.Show("Çıkış Yapmak İstediğinizden Emin Misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);                                                     //
            if (result == DialogResult.Yes)                                                                                                                                                                   //
            {                                                                                                                                                                                                 //
                exitButtonClicked = true;                                                                                                                                                                     //
                SaveListBoxData();                                                                                                                                                                            //
                this.Close();                                                                                                                                                                                 //
            }                                                                                                                                                                                                 //
        }                                                                                                                                                                                                     //
                                                                                                                                                                                                              //
        // "LİSTELE" BUTONU                                                                                                                                                                                   //       
        private void button11_Click(object sender, EventArgs e)                                                                                                                                               //
        {                                                                                                                                                                                                     //
            button2.Enabled = false;                                                                                                                                                                          //
            bool hata_flag = false;                                                                                                                                                                           //
            bool result3 = CheckComboBoxes(comboBox10, comboBox9, comboBox8, comboBox7, comboBox6, comboBox5, comboBox4, comboBox3, comboBox2, comboBox1);                                                    //
            if (string.IsNullOrEmpty(textBox4.Text))                                                                                                                                                          //
            {                                                                                                                                                                                                 //
                textBox4.Focus();                                                                                                                                                                             //
                hata_flag = true;                                                                                                                                                                             //
            }                                                                                                                                                                                                 //
            else                                                                                                                                                                                              //
                hata_flag = false;                                                                                                                                                                            //
                                                                                                                                                                                                              //
            if (result3 && !hata_flag)                                                                                                                                                                        //
            {                                                                                                                                                                                                 //
                listBox4.Items.Clear();                                                                                                                                                                       //
                string jsonFilePath = "veri_tabani.json";                                                                                                                                                     //
                string jsonData = System.IO.File.ReadAllText(jsonFilePath);                                                                                                                                   //
                List<Dictionary<string, string>> data = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(jsonData);                                                                            //
                string[] requiredKeys = {                                                                                                                                                                     //
                                            "ucus_yapilan_il", "ucus_yapilan_havaalani", "ucus_yapilan_hava_araci",                                                                                           //
                                            "hava_araci_kuyruk_numarasi", "ucus_kontrolu", "hava_durumu",                                                                                                     //
                                            "pist_durumu", "ani_manevra_yapildi_mi", "veri_kaydediliyor_mu",                                                                                                  //
                                            "teknik_sorun_oldu_mu", "kaza_kirim_oldu_mu"                                                                                                                      //
                                            };                                                                                                                                                                //
                string ucus_yapilan_il = comboBox2.SelectedItem?.ToString();                                                                                                                                  //
                string ucus_yapilan_havaalani = comboBox1.SelectedItem?.ToString();                                                                                                                           //
                string ucus_yapilan_hava_araci = comboBox3.SelectedItem?.ToString();                                                                                                                          //
                string hava_araci_kuyruk_numarasi = textBox4.Text;                                                                                                                                            //
                string ucus_kontrolu = comboBox4.SelectedItem?.ToString();                                                                                                                                    //
                string hava_durumu = comboBox5.SelectedItem?.ToString();                                                                                                                                      //
                string pist_durumu = comboBox6.SelectedItem?.ToString();                                                                                                                                      //
                string ani_manevra_yapildi_mi = comboBox7.SelectedItem?.ToString();                                                                                                                           //
                string veri_kaydediliyor_mu = comboBox8.SelectedItem?.ToString();                                                                                                                             //
                string teknik_sorun_oldu_mu = comboBox9.SelectedItem?.ToString();                                                                                                                             //
                string kaza_kirim_oldu_mu = comboBox10.SelectedItem?.ToString();                                                                                                                              //
                List<string> matchingFiles = new List<string>();                                                                                                                                              //
                foreach (var item in data)                                                                                                                                                                    //
                {                                                                                                                                                                                             //
                    bool matchesAllKeys = true;                                                                                                                                                               //
                    foreach (string key in requiredKeys)                                                                                                                                                      //
                    {                                                                                                                                                                                         //
                        if (!item.ContainsKey(key))                                                                                                                                                           //
                        {                                                                                                                                                                                     //
                            matchesAllKeys = false;                                                                                                                                                           //
                            break;                                                                                                                                                                            //
                        }                                                                                                                                                                                     //
                    }                                                                                                                                                                                         //
                    if (matchesAllKeys &&                                                                                                                                                                     //
                        item["ucus_yapilan_il"] == ucus_yapilan_il &&                                                                                                                                         //
                        item["ucus_yapilan_havaalani"] == ucus_yapilan_havaalani &&                                                                                                                           //
                        item["ucus_yapilan_hava_araci"] == ucus_yapilan_hava_araci &&                                                                                                                         //
                        item["hava_araci_kuyruk_numarasi"] == hava_araci_kuyruk_numarasi &&                                                                                                                   //
                        item["ucus_kontrolu"] == ucus_kontrolu &&                                                                                                                                             //
                        item["hava_durumu"] == hava_durumu &&                                                                                                                                                 //
                        item["pist_durumu"] == pist_durumu &&                                                                                                                                                 //
                        item["ani_manevra_yapildi_mi"] == ani_manevra_yapildi_mi &&                                                                                                                           //
                        item["veri_kaydediliyor_mu"] == veri_kaydediliyor_mu &&                                                                                                                               //
                        item["teknik_sorun_oldu_mu"] == teknik_sorun_oldu_mu &&                                                                                                                               //
                        item["kaza_kirim_oldu_mu"] == kaza_kirim_oldu_mu)                                                                                                                                     //
                    {                                                                                                                                                                                         //
                        matchingFiles.Add(item["veri_dosya_adi"]);                                                                                                                                            //
                    }                                                                                                                                                                                         //
                }                                                                                                                                                                                             //
                if (matchingFiles.Count == 0)                                                                                                                                                                 //
                {                                                                                                                                                                                             //
                    listBox4.Items.Clear();                                                                                                                                                                   //
                    listBox4.Items.Add("Kayıtlı Veri Bulunamadı!");                                                                                                                                           //
                    WriteToLogFile("Veri Tabanında Kayıtlı Veri Bulunamadı!");                                                                                                                                //
                }                                                                                                                                                                                             //
                else                                                                                                                                                                                          //
                {                                                                                                                                                                                             //
                    listBox4.Items.Clear();                                                                                                                                                                   //
                    listBox4.Items.AddRange(matchingFiles.ToArray());                                                                                                                                         //
                }                                                                                                                                                                                             //
            }                                                                                                                                                                                                 //
        }                                                                                                                                                                                                     //
                                                                                                                                                                                                              //
        // "VERİ TABANI DİZİNİ" BUTONU                                                                                                                                                                        //                          
        private void button12_Click(object sender, EventArgs e)                                                                                                                                               //
        {                                                                                                                                                                                                     //
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();                                                                                                                             //
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)                                                                                                                                         //
                textBox10.Text = folderBrowserDialog1.SelectedPath;                                                                                                                                           //
            string veri_tabani_klasor = textBox10.Text + "\\NetSaver_Veri_Tabani";                                                                                                                            //
            try                                                                                                                                                                                               //
            {                                                                                                                                                                                                 //
                if (!Directory.Exists(veri_tabani_klasor))                                                                                                                                                    //
                {                                                                                                                                                                                             //
                    Directory.CreateDirectory(veri_tabani_klasor);                                                                                                                                            //
                }                                                                                                                                                                                             //
            }                                                                                                                                                                                                 //
            catch (Exception ex)                                                                                                                                                                              //
            {                                                                                                                                                                                                 //
                MessageBox.Show("Veri Tabanı Oluşturulamadı: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);                                                                             //
                WriteToLogFile("Veri Tabanı Oluşturulamadı: " + ex.Message);                                                                                                                                  //
            }                                                                                                                                                                                                 //
            string dosyaYolu = "config.json";                                                                                                                                                                 //
            List<dynamic> veriler;                                                                                                                                                                            //
            if (!System.IO.File.Exists(dosyaYolu))                                                                                                                                                            //
            {                                                                                                                                                                                                 //
                try                                                                                                                                                                                           //
                {                                                                                                                                                                                             //
                    System.IO.File.WriteAllText(dosyaYolu, "[]");                                                                                                                                             //
                }                                                                                                                                                                                             //
                catch (Exception ex)                                                                                                                                                                          //
                {                                                                                                                                                                                             //
                    MessageBox.Show("Hata: " + ex.Message);                                                                                                                                                   //
                    WriteToLogFile("Hata: " + ex.Message);                                                                                                                                                    //
                    return;                                                                                                                                                                                   //
                }                                                                                                                                                                                             //
            }                                                                                                                                                                                                 //
            string json = System.IO.File.ReadAllText(dosyaYolu);                                                                                                                                              //
            veriler = JsonConvert.DeserializeObject<List<dynamic>>(json);                                                                                                                                     //
            bool updated = false;                                                                                                                                                                             //
            for (int i = 0; i < veriler.Count; i++)                                                                                                                                                           //
            {                                                                                                                                                                                                 //
                if (veriler[i].veri_tabani_yolu == veri_tabani_klasor)                                                                                                                                        //
                {                                                                                                                                                                                             //
                    veriler[i].veri_tabani_yolu = veri_tabani_klasor;                                                                                                                                         //
                    updated = true;                                                                                                                                                                           //
                    break;                                                                                                                                                                                    //
                }                                                                                                                                                                                             //
            }                                                                                                                                                                                                 //
            if (!updated)                                                                                                                                                                                     //
            {                                                                                                                                                                                                 //
                veriler.RemoveAll(item => item.veri_tabani_yolu == veri_tabani_klasor);                                                                                                                       //
                var yeniVeri = new                                                                                                                                                                            //
                {                                                                                                                                                                                             //
                    veri_tabani_yolu = veri_tabani_klasor                                                                                                                                                     //
                };                                                                                                                                                                                            //
                veriler.Add(yeniVeri);                                                                                                                                                                        //
            }                                                                                                                                                                                                 //
            string yeniJson = JsonConvert.SerializeObject(veriler.Where(item => item.veri_tabani_yolu == veri_tabani_klasor), Formatting.Indented);                                                           //
            try                                                                                                                                                                                               //
            {                                                                                                                                                                                                 //
                System.IO.File.WriteAllText(dosyaYolu, yeniJson);                                                                                                                                             //
            }                                                                                                                                                                                                 //
            catch (Exception ex)                                                                                                                                                                              //
            {                                                                                                                                                                                                 //
                MessageBox.Show("Hata: " + ex.Message);                                                                                                                                                       //
                WriteToLogFile("Hata: " + ex.Message);                                                                                                                                                        //
            }                                                                                                                                                                                                 //
        }                                                                                                                                                                                                     //
                                                                                                                                                                                                              //
        // "DİZİN SEÇ" BUTONU                                                                                                                                                                                 //
        private void button13_Click(object sender, EventArgs e)                                                                                                                                               //
        {                                                                                                                                                                                                     //
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();                                                                                                                             //
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)                                                                                                                                         //
                textBox5.Text = folderBrowserDialog1.SelectedPath;                                                                                                                                            //
        }                                                                                                                                                                                                     //
                                                                                                                                                                                                              //
        // "SİL" BUTONU                                                                                                                                                                                       //
        private void button14_Click(object sender, EventArgs e)                                                                                                                                               //
        {                                                                                                                                                                                                     //
            if (listBox2.SelectedItems.Count > 0)                                                                                                                                                             //
            {                                                                                                                                                                                                 //
                while (listBox2.SelectedItems.Count > 0)                                                                                                                                                      //
                    listBox2.Items.Remove(listBox2.SelectedItems[0]);                                                                                                                                         //
            }                                                                                                                                                                                                 //
            else                                                                                                                                                                                              //
                MessageBox.Show("Lütfen Silinecek Bir Öğe Seçin!");                                                                                                                                           //
        }                                                                                                                                                                                                     //
                                                                                                                                                                                                              //
        // "İPTAL" BUTONU                                                                                                                                                                                     //
        private void button15_Click(object sender, EventArgs e)                                                                                                                                               //
        {                                                                                                                                                                                                     //
                                                                                                                                                                                                              //
        }                                                                                                                                                                                                     //
                                                                                                                                                                                                              //
        // "VERİ ALIMINI BAŞLAT" BUTONU                                                                                                                                                                       //
        private void button16_Click_1(object sender, EventArgs e)                                                                                                                                             //
        {                                                                                                                                                                                                     //
            bool result2 = CheckComboBoxes(comboBox20, comboBox19, comboBox18, comboBox17, comboBox16, comboBox15, comboBox14, comboBox13, comboBox12, comboBox11);                                           //
            if (result2 && !string.IsNullOrEmpty(textBox9.Text))                                                                                                                                              //
            {                                                                                                                                                                                                 //
                DialogResult result = MessageBox.Show("Veri Alımını Başlatmak İstiyor musunuz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);                                                   //
                if (result == DialogResult.Yes)                                                                                                                                                               //
                {                                                                                                                                                                                             //
                    button3.Enabled = false;                                                                                                                                                                  //
                    string dosyaYolu = "veri_tabani.json";                                                                                                                                                    //
                    List<dynamic> veriler;                                                                                                                                                                    //
                    if (System.IO.File.Exists(dosyaYolu))                                                                                                                                                     //
                    {                                                                                                                                                                                         //
                        string json = System.IO.File.ReadAllText(dosyaYolu);                                                                                                                                  //
                        veriler = JsonConvert.DeserializeObject<List<dynamic>>(json);                                                                                                                         //
                    }                                                                                                                                                                                         //
                    else                                                                                                                                                                                      //
                        veriler = new List<dynamic>();                                                                                                                                                        //
                    string veriDosyaAdi = Path.GetFileName(veri);                                                                                                                                             //
                    int counter = 1;                                                                                                                                                                          //
                    while (VeriDosyaAdiVarMi(veriDosyaAdi, veriler))                                                                                                                                          //
                        veriDosyaAdi = $"{Path.GetFileNameWithoutExtension(veri)}_({counter++}){Path.GetExtension(veri)}";                                                                                    //
                    var yeniVeri = new                                                                                                                                                                        //
                    {                                                                                                                                                                                         //
                        veri_id = GenerateRandomString(32),                                                                                                                                                   //
                        veri_dosya_adi = veriDosyaAdi,                                                                                                                                                        //
                        veri_dosya_yolu = textBox10.Text + "\\NetSaver_Veri_Tabani",                                                                                                                          //
                        ucus_yapilan_il = comboBox20.SelectedItem.ToString(),                                                                                                                                 //
                        ucus_yapilan_havaalani = comboBox19.SelectedItem.ToString(),                                                                                                                          //
                        ucus_yapilan_hava_araci = comboBox18.SelectedItem.ToString(),                                                                                                                         //
                        hava_araci_kuyruk_numarasi = textBox9.Text,                                                                                                                                           //
                        ucus_kontrolu = comboBox17.SelectedItem.ToString(),                                                                                                                                   //
                        hava_durumu = comboBox16.SelectedItem.ToString(),                                                                                                                                     //
                        pist_durumu = comboBox15.SelectedItem.ToString(),                                                                                                                                     //
                        ani_manevra_yapildi_mi = comboBox14.SelectedItem.ToString(),                                                                                                                          //
                        veri_kaydediliyor_mu = comboBox13.SelectedItem.ToString(),                                                                                                                            //
                        teknik_sorun_oldu_mu = comboBox12.SelectedItem.ToString(),                                                                                                                            //
                        kaza_kirim_oldu_mu = comboBox11.SelectedItem.ToString(),                                                                                                                              //
                    };                                                                                                                                                                                        //
                    veriler.Add(yeniVeri);                                                                                                                                                                    //
                    string yeniJson = JsonConvert.SerializeObject(veriler, Formatting.Indented);                                                                                                              //
                    try                                                                                                                                                                                       //
                    {                                                                                                                                                                                         //
                        System.IO.File.WriteAllText(dosyaYolu, yeniJson);                                                                                                                                     //
                    }                                                                                                                                                                                         //
                    catch (Exception ex)                                                                                                                                                                      //
                    {                                                                                                                                                                                         //
                        MessageBox.Show("Hata: " + ex.Message);                                                                                                                                               //
                        WriteToLogFile("Hata: " + ex.Message);                                                                                                                                                //
                    }                                                                                                                                                                                         //
                    tabControl1.SelectedIndex = 0;                                                                                                                                                            //
                    label42.Text = "Veriler Alınıyor...";                                                                                                                                                     //
                    WriteToLogFile("Veriler Alınıyor...");                                                                                                                                                    //
                    veri_tabanina_kaydet();                                                                                                                                                                   //
                }                                                                                                                                                                                             //
            }                                                                                                                                                                                                 //
            else                                                                                                                                                                                              //
            {                                                                                                                                                                                                 //
                if (string.IsNullOrEmpty(textBox9.Text))                                                                                                                                                      //
                    textBox9.Focus();                                                                                                                                                                         //
            }                                                                                                                                                                                                 //
        }                                                                                                                                                                                                     //


        // LISTBOX İŞLEMLERİ ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                                                                                                                                                                                              //
        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)                                                                                                                                //
        {                                                                                                                                                                                                     //
            if (listBox2.SelectedItem != null)                                                                                                                                                                //
            {                                                                                                                                                                                                 //
                button14.Enabled = listBox2.SelectedItems.Count > 0;                                                                                                                                          //
                string selectedItem = listBox2.SelectedItem.ToString();                                                                                                                                       //
                int index = selectedItem.IndexOf("/");                                                                                                                                                        //
                if (index != -1)                                                                                                                                                                              //
                {                                                                                                                                                                                             //
                    string prefix = selectedItem.Substring(0, index).Trim();                                                                                                                                  //
                    textBox2.Text = prefix;                                                                                                                                                                   //
                    int secondIndex = selectedItem.IndexOf("/", index + 1);                                                                                                                                   //
                    if (secondIndex != -1)                                                                                                                                                                    //
                    {                                                                                                                                                                                         //
                        string middlePart = selectedItem.Substring(index + 1, secondIndex - index - 1).Trim();                                                                                                //
                        textBox3.Text = middlePart;                                                                                                                                                           //
                        string lastPart = selectedItem.Substring(secondIndex + 1).Trim();                                                                                                                     //
                        textBox1.Text = lastPart;                                                                                                                                                             //
                    }                                                                                                                                                                                         //
                }                                                                                                                                                                                             //
            }                                                                                                                                                                                                 //
            else                                                                                                                                                                                              //
                button14.Enabled = false;                                                                                                                                                                     //
        }                                                                                                                                                                                                     //
                                                                                                                                                                                                              //
        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)                                                                                                                                //
        {                                                                                                                                                                                                     //
            if (listBox3.SelectedItem != null)                                                                                                                                                                //
                veri = listBox3.SelectedItem.ToString();                                                                                                                                                      //
            else                                                                                                                                                                                              //
                veri = null;                                                                                                                                                                                  //
            button3.Enabled = listBox3.SelectedItems.Count > 0;                                                                                                                                               //
            button5.Enabled = listBox3.SelectedItems.Count > 0;                                                                                                                                               //
            //button9.Enabled = listBox3.SelectedItems.Count > 0;                                                                                                                                             //
        }                                                                                                                                                                                                     //
                                                                                                                                                                                                              //
        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)                                                                                                                                //
        {                                                                                                                                                                                                     //
            if (listBox4.SelectedItem != null && listBox4.SelectedItem.ToString() != "Kayıtlı Veri Bulunamadı!")                                                                                              //
                button6.Enabled = true;                                                                                                                                                                       //
        }                                                                                                                                                                                                     //
                                                                                                                                                                                                              //
        private void LoadListBoxData()                                                                                                                                                                        //
        {                                                                                                                                                                                                     //
            if (System.IO.File.Exists(ListBoxDataFile))                                                                                                                                                       //
            {                                                                                                                                                                                                 //
                string[] lines = System.IO.File.ReadAllLines(ListBoxDataFile);                                                                                                                                //
                listBox2.Items.AddRange(lines);                                                                                                                                                               //
            }                                                                                                                                                                                                 //
        }                                                                                                                                                                                                     //
                                                                                                                                                                                                              //
        private void SaveListBoxData()                                                                                                                                                                        //
        {                                                                                                                                                                                                     //
            List<string> items = new List<string>();                                                                                                                                                          //
            foreach (var item in listBox2.Items)                                                                                                                                                              //
                items.Add(item.ToString());                                                                                                                                                                   //
            System.IO.File.WriteAllLines(ListBoxDataFile, items);                                                                                                                                             //
        }                                                                                                                                                                                                     //


        // TEXTBOX İŞLEMLERİ ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                                                                                                                                                                                              //
        private void textBox1_TextChanged_1(object sender, EventArgs e)                                                                                                                                       //
        {                                                                                                                                                                                                     //
            UpdateButton1State();                                                                                                                                                                             //
            UpdateButton7State();                                                                                                                                                                             //
        }                                                                                                                                                                                                     //
                                                                                                                                                                                                              //
        private void textBox2_TextChanged_1(object sender, EventArgs e)                                                                                                                                       //
        {                                                                                                                                                                                                     //
            UpdateButton1State();                                                                                                                                                                             //
            UpdateButton7State();                                                                                                                                                                             //
        }                                                                                                                                                                                                     //
                                                                                                                                                                                                              //
        private void textBox3_TextChanged(object sender, EventArgs e)                                                                                                                                         //
        {                                                                                                                                                                                                     //
            UpdateButton7State();                                                                                                                                                                             //
        }                                                                                                                                                                                                     //
                                                                                                                                                                                                              //
        private void textBox7_TextChanged(object sender, EventArgs e)                                                                                                                                         //
        {                                                                                                                                                                                                     //
            if (listBox4.SelectedItem.ToString() != "Kayıtlı Veri Bulunamadı!")                                                                                                                               //
                button2.Enabled = true;                                                                                                                                                                       //
        }                                                                                                                                                                                                     //
                                                                                                                                                                                                              //
        private void textBox8_TextChanged_1(object sender, EventArgs e)                                                                                                                                       //
        {                                                                                                                                                                                                     //
            textBox8.PasswordChar = '●';                                                                                                                                                                      //
            UpdateButton1State();                                                                                                                                                                             //
            UpdateButton7State();                                                                                                                                                                             //
        }                                                                                                                                                                                                     //
                                                                                                                                                                                                              //
        private void textbox10_ilkdeger()                                                                                                                                                                     //
        {                                                                                                                                                                                                     //
            string dosyaYolu = "config.json";                                                                                                                                                                 //
            if (System.IO.File.Exists(dosyaYolu))                                                                                                                                                             //
            {                                                                                                                                                                                                 //
                string json = System.IO.File.ReadAllText(dosyaYolu);                                                                                                                                          //
                List<dynamic> veriler = JsonConvert.DeserializeObject<List<dynamic>>(json);                                                                                                                   //
                if (veriler.Count > 0)                                                                                                                                                                        //
                    textBox10.Text = veriler[0].veri_tabani_yolu;                                                                                                                                             //
            }                                                                                                                                                                                                 //
        }                                                                                                                                                                                                     //
                                                                                                                                                                                                              //
        private void AppendToRichTextBox(string text)                                                                                                                                                         //
        {                                                                                                                                                                                                     //
            if (richTextBox1.InvokeRequired)                                                                                                                                                                  //
                richTextBox1.Invoke(new Action<string>(AppendToRichTextBox), new object[] { text });                                                                                                          //
            else                                                                                                                                                                                              //
                richTextBox1.AppendText(text + Environment.NewLine);                                                                                                                                          //
        }                                                                                                                                                                                                     //
       

        // DOSYA İŞLEMLERİ /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                                                                                                                                                                                              //
        private void AddDirectories(TreeNode parentNode, string parentDirectory)                                                                                                                              //
        {                                                                                                                                                                                                     //
            var directories = sftpClient.ListDirectory(parentDirectory);                                                                                                                                      //
            foreach (var directory in directories)                                                                                                                                                            //
            {                                                                                                                                                                                                 //
                if (directory.IsDirectory && !directory.Name.StartsWith("."))                                                                                                                                 //
                {                                                                                                                                                                                             //
                    TreeNode node = new TreeNode(directory.Name);                                                                                                                                             //
                    parentNode.Nodes.Add(node);                                                                                                                                                               //
                    AddDirectories(node, parentDirectory + "/" + directory.Name);                                                                                                                             //
                }                                                                                                                                                                                             //
                else if (directory.IsRegularFile)                                                                                                                                                             //
                {                                                                                                                                                                                             //
                    TreeNode node = new TreeNode(directory.Name);                                                                                                                                             //
                    parentNode.Nodes.Add(node);                                                                                                                                                               //
                }                                                                                                                                                                                             //
            }                                                                                                                                                                                                 //
        }                                                                                                                                                                                                     //
                                                                                                                                                                                                              //
        public string dosyaYoluAl(string dosyaYolu, string hedefDosyaAdi)                                                                                                                                     //
        {                                                                                                                                                                                                     //
            string jsonText = File.ReadAllText(dosyaYolu);                                                                                                                                                    //
            JArray veriDizisi = JArray.Parse(jsonText);                                                                                                                                                       //
                                                                                                                                                                                                              //
            foreach (JObject veri in veriDizisi)                                                                                                                                                              //
            {                                                                                                                                                                                                 //
                if ((string)veri["veri_dosya_adi"] == hedefDosyaAdi)                                                                                                                                          //
                    return (string)veri["veri_dosya_yolu"] + "\\" + Path.GetFileName((string)veri["veri_dosya_adi"]);                                                                                         //
            }                                                                                                                                                                                                 //
            return null;                                                                                                                                                                                      //
        }                                                                                                                                                                                                     //
                                                                                                                                                                                                              //
        public void DosyaKopyala(string kaynakDosyaYolu, string hedefDizin)                                                                                                                                   //
        {                                                                                                                                                                                                     //
            try                                                                                                                                                                                               //
            {                                                                                                                                                                                                 //
                if (!File.Exists(kaynakDosyaYolu))                                                                                                                                                            //
                {                                                                                                                                                                                             //
                    MessageBox.Show("Kaynak Dosya Bulunamadı!\nDosya Yolu: " + kaynakDosyaYolu + "\nSilinmiş veya Değiştirilmiş Olabilir.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);              //
                    WriteToLogFile("Kaynak Dosya Bulunamadı: " + kaynakDosyaYolu);                                                                                                                            //
                    button2.Enabled = true;                                                                                                                                                                   //
                    button15.Enabled = false;                                                                                                                                                                 //
                    return;                                                                                                                                                                                   //
                }                                                                                                                                                                                             //
                if (!Directory.Exists(hedefDizin))                                                                                                                                                            //
                {                                                                                                                                                                                             //
                    MessageBox.Show("Hedef Dizin Bulunamadı!\nDizin Yolu: " + hedefDizin + "\nSilinmiş veya Değiştirilmiş Olabilir.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);                    //
                    WriteToLogFile("Hedef Dizin Bulunamadı: " + hedefDizin);                                                                                                                                  //
                    button2.Enabled = true;                                                                                                                                                                   //
                    button15.Enabled = false;                                                                                                                                                                 //
                    return;                                                                                                                                                                                   //
                }                                                                                                                                                                                             //
                string hedefDosyaYolu = Path.Combine(hedefDizin, Path.GetFileName(kaynakDosyaYolu));                                                                                                          //
                File.Copy(kaynakDosyaYolu, hedefDosyaYolu, true);                                                                                                                                             //
                MessageBox.Show("Dosya Başarıyla Aktarıldı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);                                                                                     //
                WriteToLogFile("Dosya Başarıyla Aktarıldı!");                                                                                                                                                 //
            }                                                                                                                                                                                                 //
            catch (Exception ex)                                                                                                                                                                              //
            {                                                                                                                                                                                                 //
                MessageBox.Show("Dosya Aktarılamadı: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);                                                                                     //
                WriteToLogFile("Dosya Aktarılamadı: " + ex.Message);                                                                                                                                          //
            }                                                                                                                                                                                                 //
        }                                                                                                                                                                                                     //
                                                                                                                                                                                                              //
        private void WriteToLogFile(string message)                                                                                                                                                           //
        {                                                                                                                                                                                                     //
            string logFileName = "log.txt";                                                                                                                                                                   //
            try                                                                                                                                                                                               //
            {                                                                                                                                                                                                 //
                using (StreamWriter sw = File.AppendText(logFileName))                                                                                                                                        //
                {                                                                                                                                                                                             //
                    sw.WriteLine($"{DateTime.Now} - {message}");                                                                                                                                              //
                }                                                                                                                                                                                             //
                AppendToRichTextBox($"{DateTime.Now} - {message}");                                                                                                                                           //
            }                                                                                                                                                                                                 //
            catch (Exception ex)                                                                                                                                                                              //
            {                                                                                                                                                                                                 //
                MessageBox.Show("Hata: " + ex.Message);                                                                                                                                                       //
                WriteToLogFile("Hata: " + ex.Message);                                                                                                                                                        //
            }                                                                                                                                                                                                 //
        }                                                                                                                                                                                                     //
                                                                                                                                                                                                              //
        private bool VeriDosyaAdiVarMi(string dosyaAdi, List<dynamic> veriler)                                                                                                                                //
        {                                                                                                                                                                                                     //
            foreach (var veri in veriler)                                                                                                                                                                     //
            {                                                                                                                                                                                                 //
                if (veri.veri_dosya_adi == dosyaAdi)                                                                                                                                                          //
                {                                                                                                                                                                                             //
                    return true;                                                                                                                                                                              //
                }                                                                                                                                                                                             //
            }                                                                                                                                                                                                 //
            return false;                                                                                                                                                                                     //
        }                                                                                                                                                                                                     //
                                                                                                                                                                                                              //
        private async void veri_tabanina_kaydet()                                                                                                                                                             //
        {                                                                                                                                                                                                     //
            try                                                                                                                                                                                               //
            {                                                                                                                                                                                                 //
                using (SftpClient client = new SftpClient(new PasswordConnectionInfo(textBox2.Text, textBox1.Text, textBox8.Text)))                                                                           //
                {                                                                                                                                                                                             //
                    client.Connect();                                                                                                                                                                         //
                    string serverFile = listBox3.SelectedItem.ToString();                                                                                                                                     //
                    string localDirectory = textBox10.Text + "\\NetSaver_Veri_Tabani";                                                                                                                        //
                    string localFilePath = Path.Combine(localDirectory, Path.GetFileName(serverFile));                                                                                                        //
                    if (!Directory.Exists(textBox10.Text))                                                                                                                                                    //
                    {                                                                                                                                                                                         //
                        MessageBox.Show("Veri Tabanı Dizini Bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);                                                                              //
                        label42.Text = "Veriler Alınamadı!";                                                                                                                                                  //
                        return;                                                                                                                                                                               //
                    }                                                                                                                                                                                         //
                    int counter = 1;                                                                                                                                                                          //
                    string originalFileName = Path.GetFileNameWithoutExtension(localFilePath);                                                                                                                //
                    string extension = Path.GetExtension(localFilePath);                                                                                                                                      //
                    while (File.Exists(localFilePath))                                                                                                                                                        //
                    {                                                                                                                                                                                         //
                        string newFileName = $"{originalFileName}_({counter}){extension}";                                                                                                                    //
                        localFilePath = Path.Combine(localDirectory, newFileName);                                                                                                                            //
                        counter++;                                                                                                                                                                            //
                    }                                                                                                                                                                                         //
                    await Task.Run(() =>                                                                                                                                                                      //
                    {                                                                                                                                                                                         //
                        using (Stream stream = System.IO.File.Open(localFilePath, FileMode.Create))                                                                                                           //
                        {                                                                                                                                                                                     //
                            client.DownloadFile(serverFile, stream, x => Console.WriteLine(x));                                                                                                               //
                        }                                                                                                                                                                                     //
                    });                                                                                                                                                                                       //
                    client.Disconnect();                                                                                                                                                                      //
                    label42.Text = "Veriler Alındı!";                                                                                                                                                         //
                    WriteToLogFile("Veriler Veritabanına Kaydedildi.");                                                                                                                                       //
                    button3.Enabled = true;                                                                                                                                                                   //
                }                                                                                                                                                                                             //
            }                                                                                                                                                                                                 //
            catch (Exception ex)                                                                                                                                                                              //
            {                                                                                                                                                                                                 //
                MessageBox.Show("ççHata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);                                                                                                 //
                WriteToLogFile("Hata: " + ex.Message);                                                                                                                                                        //
            }                                                                                                                                                                                                 //
        }                                                                                                                                                                                                     //


        // COMBOBOX İŞLEMLERİ //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                                                                                                                                                                                              //
        private void comboBox_KeyDown(object sender, KeyEventArgs e)                                                                                                                                          //
        {                                                                                                                                                                                                     //
            if (e.KeyCode == Keys.Enter)                                                                                                                                                                      //
            {                                                                                                                                                                                                 //
                ComboBox comboBox = sender as ComboBox;                                                                                                                                                       //
                AddItemToComboBox(comboBox);                                                                                                                                                                  //
            }                                                                                                                                                                                                 //
        }                                                                                                                                                                                                     //
                                                                                                                                                                                                              //
        private void AddItemToComboBox(ComboBox comboBox)                                                                                                                                                     //
        {                                                                                                                                                                                                     //
            string userInput = comboBox.Text.Trim();                                                                                                                                                          //
            if (!string.IsNullOrEmpty(userInput) && !comboBox.Items.Contains(userInput))                                                                                                                      //
            {                                                                                                                                                                                                 //
                DialogResult result = MessageBox.Show("Bu öğeyi eklemek istediğinizden emin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);                                             //
                if (result == DialogResult.Yes)                                                                                                                                                               //
                {                                                                                                                                                                                             //
                    comboBox.Items.Add(userInput);                                                                                                                                                            //
                    comboBox.SelectedIndex = comboBox.Items.IndexOf(userInput);                                                                                                                               //
                }                                                                                                                                                                                             //
            }                                                                                                                                                                                                 //
        }                                                                                                                                                                                                     //
                                                                                                                                                                                                              //
        private bool CheckComboBoxes(params ComboBox[] comboBoxes)                                                                                                                                            //
        {                                                                                                                                                                                                     //
            foreach (ComboBox comboBox in comboBoxes)                                                                                                                                                         //
            {                                                                                                                                                                                                 //
                if (comboBox.SelectedIndex == -1)                                                                                                                                                             //
                {                                                                                                                                                                                             //
                    comboBox.Focus();                                                                                                                                                                         //
                    MessageBox.Show("Lütfen Eksik Bilgileri Giriniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);                                                                                //
                    return false;                                                                                                                                                                             //
                }                                                                                                                                                                                             //
            }                                                                                                                                                                                                 //
            return true;                                                                                                                                                                                      //
        }                                                                                                                                                                                                     //


        // YARDIMCI FONKSİYONLAR ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                                                                                                                                                                                              //
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)                                                                                                                             //
        {                                                                                                                                                                                                     //
            if (tabControl1.SelectedIndex != 1)                                                                                                                                                               //
                tabPage4.Enabled = false;                                                                                                                                                                     //
        }                                                                                                                                                                                                     //
                                                                                                                                                                                                              //
        public static string GenerateRandomString(int length)                                                                                                                                                 //
        {                                                                                                                                                                                                     //
            StringBuilder stringBuilder = new StringBuilder(length);                                                                                                                                          //
            for (int i = 0; i < length; i++)                                                                                                                                                                  //
                stringBuilder.Append(chars[random.Next(chars.Length)]);                                                                                                                                       //
            return stringBuilder.ToString();                                                                                                                                                                  //
        }                                                                                                                                                                                                     //
                                                                                                                                                                                                              //
        private void ConnectToSftp()                                                                                                                                                                          //
        {                                                                                                                                                                                                     //
            string host = textBox2.Text;                                                                                                                                                                      //
            string username = textBox1.Text;                                                                                                                                                                  //
            string password = textBox8.Text;                                                                                                                                                                  //
            int port = Convert.ToInt32(textBox3.Text);                                                                                                                                                        //
            try                                                                                                                                                                                               //
            {                                                                                                                                                                                                 //
                sftpClient = new SftpClient(host, port, username, password);                                                                                                                                  //
                sftpClient.Connect();                                                                                                                                                                         //
                this.Invoke((MethodInvoker)delegate {                                                                                                                                                         //
                    PopulateTreeView("/home/" + username);                                                                                                                                                    //
                    PictureBox[] pictureBoxes = { pictureBox3, pictureBox4, pictureBox5, pictureBox6, pictureBox7, pictureBox8 };                                                                             //
                    for (int i = 0; i < pictureBoxes.Length; i++)                                                                                                                                             //
                        pictureBoxes[i].Image = Resource1.yesilnokta;                                                                                                                                         //
                    pictureBoxes[5].Image = null;                                                                                                                                                             //
                    timer.Stop();                                                                                                                                                                             //
                    WriteToLogFile("Sunucuya Bağlanıldı!");                                                                                                                                                   //
                    label28.Text = "Sunucuya Bağlanıldı!";                                                                                                                                                    //
                    label28.ForeColor = Color.Green;                                                                                                                                                          //
                });                                                                                                                                                                                           //
            }                                                                                                                                                                                                 //
            catch (Exception)                                                                                                                                                                                 //
            {                                                                                                                                                                                                 //
                this.Invoke((MethodInvoker)delegate {                                                                                                                                                         //
                    sftpClient?.Dispose();                                                                                                                                                                    //
                    PictureBox[] pictureBoxes = { pictureBox3, pictureBox4, pictureBox5, pictureBox6, pictureBox7, pictureBox8 };                                                                             //
                    for (int i = 0; i < pictureBoxes.Length; i++)                                                                                                                                             //
                        pictureBoxes[i].Image = Resource1.kirmizinokta;                                                                                                                                       //
                    pictureBoxes[5].Image = null;                                                                                                                                                             //
                    timer.Stop();                                                                                                                                                                             //
                    button8.Enabled = false;                                                                                                                                                                  //
                    WriteToLogFile("Sunucuya Bağlanılamadı!");                                                                                                                                                //
                    label28.Text = "Sunucuya Bağlanılamadı!";                                                                                                                                                 //
                    label28.ForeColor = Color.Red;                                                                                                                                                            //
                });                                                                                                                                                                                           //
            }                                                                                                                                                                                                 //
        }                                                                                                                                                                                                     //
                                                                                                                                                                                                              //
        private async Task Delay(int milliseconds)                                                                                                                                                            //
        {                                                                                                                                                                                                     //
            await Task.Delay(milliseconds);                                                                                                                                                                   //
        }                                                                                                                                                                                                     //
                                                                                                                                                                                                              //
        private void Timer_Tick(object sender, EventArgs e)                                                                                                                                                   //
        {                                                                                                                                                                                                     //
            pictureBoxes[pictureBoxIndex].Image = yeniResim;                                                                                                                                                  //
            pictureBoxIndex++;                                                                                                                                                                                //
            if (pictureBoxIndex == pictureBoxes.Length)                                                                                                                                                       //
            {                                                                                                                                                                                                 //
                for (int i = 0; i < pictureBoxes.Length; i++)                                                                                                                                                 //
                    pictureBoxes[i].Image = Resource1.beyaznokta;                                                                                                                                             //
                pictureBoxes[5].Image = null;                                                                                                                                                                 //
                pictureBoxIndex = 0;                                                                                                                                                                          //
            }                                                                                                                                                                                                 //
        }                                                                                                                                                                                                     //
                                                                                                                                                                                                              //
        private bool IsValidIPv4(string ipString)                                                                                                                                                             //
        {                                                                                                                                                                                                     //
            string pattern = @"^(\d{1,3})\.(\d{1,3})\.(\d{1,3})\.(\d{1,3})$";                                                                                                                                 //
            Regex regex = new Regex(pattern);                                                                                                                                                                 //
            return regex.IsMatch(ipString);                                                                                                                                                                   //
        }                                                                                                                                                                                                     //


        // TREEVIEW İŞLEMLERİ //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                                                                                                                                                                                              //
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)                                                                                                                                //
        {                                                                                                                                                                                                     //
            button4.Enabled = true;                                                                                                                                                                           //
        }                                                                                                                                                                                                     //
                                                                                                                                                                                                              //
        private void PopulateTreeView(string directory)                                                                                                                                                       //
        {                                                                                                                                                                                                     //
            treeView1.Nodes.Clear();                                                                                                                                                                          //
            TreeNode rootNode = new TreeNode(directory);                                                                                                                                                      //
            treeView1.Nodes.Add(rootNode);                                                                                                                                                                    //
            AddDirectories(rootNode, directory);                                                                                                                                                              //
        }                                                                                                                                                                                                     //


        // PROGRAM KAPATILIRKEN YAPILACAKLAR ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                                                                                                                                                                                              //
        private void ana_program_FormClosing_1(object sender, FormClosingEventArgs e)                                                                                                                         //
        {                                                                                                                                                                                                     //
            if (!exitButtonClicked)                                                                                                                                                                           //
            {                                                                                                                                                                                                 //
                DialogResult result = MessageBox.Show("Çıkış Yapmak İstediğinizden Emin Misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);                                                 //
                if (result == DialogResult.Yes)                                                                                                                                                               //
                {                                                                                                                                                                                             //
                    WriteToLogFile("Program Kapatıldı.");                                                                                                                                                     //
                    SaveListBoxData();                                                                                                                                                                        //
                }                                                                                                                                                                                             //
                else                                                                                                                                                                                          //
                    e.Cancel = true;                                                                                                                                                                          //
            }                                                                                                                                                                                                 //
            else                                                                                                                                                                                              //
                exitButtonClicked = false;                                                                                                                                                                    //
        }                                                                                                                                                                                                     //
    }
}
