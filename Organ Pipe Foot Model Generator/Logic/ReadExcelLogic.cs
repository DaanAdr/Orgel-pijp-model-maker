using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using Organ_Pipe_Foot_Model_Generator.Entities;

namespace Organ_Pipe_Foot_Model_Generator.Logic
{
    public static class ReadExcelLogic
    {
        /// <summary>
        /// Read a CSV file and map it's values to LabiaalPijpExcel objects
        /// </summary>
        /// <param name="filePath">The path the file is located at</param>
        /// <param name="delimiter">What delimiter character to use for the CSV file</param>
        /// <returns></returns>
        public static List<LabiaalPijpExcel> ReadLabiaalPijpCSVFile(string filePath, char delimiter)
        {
            List<LabiaalPijpExcel> records = new List<LabiaalPijpExcel>();

            using (StreamReader reader = new StreamReader(filePath))
            using (CsvReader csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
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
                records = csv.GetRecords<LabiaalPijpExcel>()
                    .Where(record => !double.IsNaN(record.Height) &&
                                  !double.IsNaN(record.TopDiameter) &&
                                  !double.IsNaN(record.MetalThickness) &&
                                  !double.IsNaN(record.PlateWidthFoot))
                    .ToList();
            }

            return records;
        }
    }
}
