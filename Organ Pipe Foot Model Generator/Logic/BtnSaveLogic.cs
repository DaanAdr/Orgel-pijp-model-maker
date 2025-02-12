using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Windows;

namespace Organ_Pipe_Foot_Model_Generator.Logic
{
    public class BtnSaveLogic
    {
        public void SaveFile()
        {
            // Create a SaveFileDialog instance
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*", // File type filter
                Title = "Save a Text File"
            };

            // Show the dialog and check if the user clicked 'Save'
            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName; // Get the selected file path

                // Write text to the selected file
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine("Hello, World!");
                    writer.WriteLine("This is a simple text file.");
                }

                MessageBox.Show("File created successfully at: " + filePath);
            }
        }
    }
}
