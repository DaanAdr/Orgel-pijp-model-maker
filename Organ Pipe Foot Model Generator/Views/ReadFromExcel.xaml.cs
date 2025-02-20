using System.Globalization;
using System.IO;
using System;
using System.Windows;
using System.Windows.Controls;
using CsvHelper;
using Microsoft.Win32;
using Organ_Pipe_Foot_Model_Generator.Entities;
using CsvHelper.Configuration;
using Organ_Pipe_Foot_Model_Generator.Logic;
using ACadSharp.IO;
using ACadSharp;

namespace Organ_Pipe_Foot_Model_Generator.Views
{
    /// <summary>
    /// Interaction logic for ReadFromExcel.xaml
    /// </summary>
    public partial class ReadFromExcel : UserControl
    {
        List<PipeFootTemplate> _pipeFootTemplates;

        public ReadFromExcel()
        {
            InitializeComponent();
        }

        private void btnReadXlsx_Click(object sender, RoutedEventArgs e)
        {
            string filePath = string.Empty;

            OpenFileDialog openFileDialog = new OpenFileDialog() 
            {
                Filter = "CSV Bestanden (*.csv)|*.csv",
                Title = "Selecteer een CSV bestand"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName;

                // Read CSV file
                List<LabiaalPijpExcel> records = ReadExcelLogic.ReadLabiaalPijpCSVFile(filePath, ';');

                // Convert to CAD objects
                _pipeFootTemplates = CreateNestedDxfFileLogic.CreateNestedDxfFile(records);

                btnSave.IsEnabled = true;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //Save file for model
            string filePath = string.Empty;

            //Create a file to put the square in
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "DXF files (*.dxf)|*.dxf|All files (*.*)|*.*",
                Title = "Save a DXF File",
                FileName = "Nested Pijp voet modellen.dxf" // Default file name
            };

            if ((bool)saveFileDialog.ShowDialog())
            {
                filePath = saveFileDialog.FileName; // Return the selected file path
            }

            //Add the insert into a document
            CadDocument doc = new CadDocument();

            // Loop through all templates and add them to the CadDocument
            foreach(PipeFootTemplate template in _pipeFootTemplates)
            {
                template.AddToCadDocument(doc);
            }

            // Save the document using DxfWriter

            using (DxfWriter writer = new DxfWriter(filePath, doc, false))
            {
                writer.Write();
            }
        }
    }
}
