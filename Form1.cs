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

namespace Nioh_2_Save_Converter
{
    public partial class Form1 : Form
    {
        Byte[] loadedOldSaveGame;
        Byte[] loadedNewSaveGame;
        string saveFolder;
        public Form1()
        {
            InitializeComponent();
            MessageBox.Show("Informations: \nYou should have created a new savegame (and be sure to backed up the old one!!!) \n The program refer Original Save as the savegame you want to convert, and New Save as the new file it'll be converted from, you imperatively need to create a new save (and BACKUP ORIGINAL SAVE) before using this program.\nConverted Save will erase newly created savegame. \n You should do that for SAVEDATAXX (XX=Slot Number) and for SYSTEMSAVEDATAXX.");
        }

        private void LoadOriginalSave_Click(object sender, EventArgs e)
        {
            var filePath = string.Empty;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                string DocumentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                openFileDialog.InitialDirectory = DocumentsFolder + "\\KoeiTecmo\\NIOH2\\SaveData";
                openFileDialog.Filter = "SAVEDATA.BIN (*.BIN)|*.bin";
                openFileDialog.FilterIndex = 2;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;
                    loadedOldSaveGame = File.ReadAllBytes(filePath);
                    //Checking if it a valid safe file by comparing first and second byte
                    if (loadedOldSaveGame[0] != 0x7F && loadedOldSaveGame[1] != 0x9C)
                    {
                        MessageBox.Show("Error, this isn't a valid save file!");
                        Array.Clear(loadedOldSaveGame, 0, loadedOldSaveGame.Length);
                    }
                    else
                    {
                        originalSaveFileName.Text = openFileDialog.FileName;
                    }
                }
            }
        }

        private void ConvertButton_Click(object sender, EventArgs e)
        {
            if (loadedOldSaveGame?.Length > 0 && loadedNewSaveGame?.Length > 0)
            {
                DialogResult confirmDialog = MessageBox.Show($"The save will convert \n {originalSaveFileName.Text} (Your true save game)\n " +
                    $"to\n{newSaveFileName.Text} (New save)\nAre you sure you want to proceed ?", "Confirm ?", MessageBoxButtons.YesNoCancel);
                if (confirmDialog == DialogResult.Yes)
                {
                    Array.Copy(loadedNewSaveGame, 0x10, loadedOldSaveGame, 0x10, 4);
                    try
                    {
                        File.WriteAllBytes(saveFolder, loadedOldSaveGame);
                        MessageBox.Show("Complete!");
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show($"{exception.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Error, one of the file have not been loaded !");
            }
        }

        private void LoadNewSave_Click(object sender, EventArgs e)
        {
            var filePath = string.Empty;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                string DocumentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                openFileDialog.InitialDirectory = DocumentsFolder + "\\KoeiTecmo\\NIOH2\\SaveData";
                openFileDialog.Filter = "SAVEDATA.BIN (*.BIN)|*.bin";
                openFileDialog.FilterIndex = 2;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;
                    loadedNewSaveGame = File.ReadAllBytes(filePath);
                    //Checking if it a valid safe file by comparing first and second byte
                    if (loadedNewSaveGame[0] != 0x7F && loadedNewSaveGame[1] != 0x9C) 
                    {
                        MessageBox.Show("Error, this isn't a valid save file!");
                        Array.Clear(loadedNewSaveGame, 0, loadedNewSaveGame.Length);
                    }
                    else
                    {
                        newSaveFileName.Text = openFileDialog.FileName;
                    }
                    saveFolder = filePath;
                }
            }
        }

    }
}
