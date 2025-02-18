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

            if (openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName;

                // Read CSV file
                List<LabiaalPijpExcel> records = ReadExcelLogic.ReadLabiaalPijpCSVFile(filePath, ';');
            }
        }
    }
}
