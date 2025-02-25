using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace Organ_Pipe_Foot_Model_Generator.Logic
{
    public class NullableDoubleConverter : ITypeConverter
    {
        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return double.NaN;
            }

            return double.TryParse(text, out double result) ? result : double.NaN;
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            return value?.ToString() ?? string.Empty;
        }
    }
}
