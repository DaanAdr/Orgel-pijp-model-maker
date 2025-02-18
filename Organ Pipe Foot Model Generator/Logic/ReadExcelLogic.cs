using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration;
using CsvHelper;
using Organ_Pipe_Foot_Model_Generator.Entities;

namespace Organ_Pipe_Foot_Model_Generator.Logic
{
    public static class ReadExcelLogic
    {
        public static List<LabiaalPijpExcel> ReadLabiaalPijpCSVFile(string filePath, char delimiter)
        {
            List<LabiaalPijpExcel> records = new List<LabiaalPijpExcel>();

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = delimiter.ToString(),
                TrimOptions = TrimOptions.Trim
            }))
            {
                // Read the first row and discard it
                reader.ReadLine();

                // Read the headers from the second row
                csv.Read();
                csv.ReadHeader();

                // Map remaining rows to LabiaalPijpExcel entity
                // Cast the results of GetRecords to list so a human debugger can see the result properly
                records = csv.GetRecords<LabiaalPijpExcel>().ToList();
            }

            return records;
        }
    }
}
