using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Windows.Controls;
using ACadSharp.IO;
using ACadSharp;
using System.Xml.Linq;

namespace Organ_Pipe_Foot_Model_Generator.Logic
{
    public class btnReadLogic
    {
        public void ReadFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == true) // Show the dialog and check if the user selected a file
            {
                string filePath = openFileDialog.FileName; // Get the file path
                //CadDocument doc = DwgReader.Read(filePath); // For DWG files
                CadDocument dxfDocument = DxfReader.Read(filePath);
                CadObjectCollection<ACadSharp.Entities.Entity> entities = dxfDocument.Entities;
                ACadSharp.Entities.Entity firstEntity = entities[0];

                var test = 3;
            }
        }
    }
}
