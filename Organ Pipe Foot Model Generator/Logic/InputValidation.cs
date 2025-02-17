using System.Text.RegularExpressions;

namespace Organ_Pipe_Foot_Model_Generator.Logic
{
    public static class InputValidation
    {
        public static bool InputIsNumericOnly(string input)
        {
            Regex _regex = new Regex("[^0-9.-]+");
            return !_regex.IsMatch(input);
        }
    }
}
