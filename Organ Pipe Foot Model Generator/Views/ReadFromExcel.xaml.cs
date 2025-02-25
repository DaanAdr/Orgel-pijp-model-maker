using System.Windows;
using System.Windows.Controls;
using ACadSharp;
using ACadSharp.IO;
using Microsoft.Win32;
using Organ_Pipe_Foot_Model_Generator.Entities;
using Organ_Pipe_Foot_Model_Generator.Logic;

namespace Organ_Pipe_Foot_Model_Generator.Views
{
    /// <summary>
    /// Interaction logic for ReadFromExcel.xaml
    /// </summary>
    public partial class ReadFromExcel : UserControl
    {
        List<LabiaalPijpExcel> _pipesInExcel;
        string _fileName;

        public ReadFromExcel()
        {
            InitializeComponent();
        }

        private void btnReadCsv_Click(object sender, RoutedEventArgs e)
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
                _fileName = System.IO.Path.GetFileNameWithoutExtension(filePath);

                // Read CSV file
                _pipesInExcel = ReadExcelLogic.ReadLabiaalPijpCSVFile(filePath, ';');

                btnSave.IsEnabled = true;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string filePath = string.Empty;
            double horizontalSeperatorDistance = Math.Round(double.Parse(txbSeperatorXAxis.Text), 1);
            double verticalSeperatorDistance = Math.Round(double.Parse(txbSeperatorYAxis.Text), 1);

            // Create models for CAD file
            List<PipeFootTemplate> cadModels = CreateNestedDxfFileLogic.CreateNestedDxfFile(_pipesInExcel, horizontalSeperatorDistance, verticalSeperatorDistance);

            //Create a file to put the square in
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "DXF files (*.dxf)|*.dxf|All files (*.*)|*.*",
                Title = "Save a DXF File",
                FileName = $"{_fileName}.dxf" // Default file name
            };

            if ((bool)saveFileDialog.ShowDialog())
            {
                filePath = saveFileDialog.FileName; // Return the selected file path
            }

            //Add the insert into a document
            CadDocument doc = new CadDocument();

            // Loop through all CADModels and add them to the CadDocument
            foreach(PipeFootTemplate cadModel in cadModels)
            {
                cadModel.AddToCadDocument(doc);
            }

            // Save the document using DxfWriter
            using (DxfWriter writer = new DxfWriter(filePath, doc, false))
            {
                writer.Write();
            }
        }
    }
}
