using System.Globalization;
using System.IO;
using System;
using System.Windows;
using System.Windows.Controls;
using CsvHelper;
using Microsoft.Win32;
using Organ_Pipe_Foot_Model_Generator.Entities;
using CsvHelper.Configuration;

namespace Organ_Pipe_Foot_Model_Generator.Views
{
    /// <summary>
    /// Interaction logic for ReadFromExcel.xaml
    /// </summary>
    public partial class ReadFromExcel : UserControl
    {
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

            if (openFileDialog.ShowDialog() == true) filePath = openFileDialog.FileName;

            try
            {
                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ";", // Set the delimiter to semicolon
                    //MissingFieldFound = null // Ignore missing fields
                    TrimOptions = TrimOptions.Trim
                }))
                {
                    // Read the first row and discard it
                    reader.ReadLine();

                    // Read the headers from the second row
                    csv.Read();
                    csv.ReadHeader();

                    // Cast the results of GetRecords to list so a human debugger can see the result properly
                    List<LabiaalPijpExcel> records = csv.GetRecords<LabiaalPijpExcel>().ToList();

                    var test1 = 1;
                }
            }
            catch (CsvHelperException ex)
            {
                var test = 3;
            }





        }
    }
}
