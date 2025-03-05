using System.Text.RegularExpressions;

namespace Organ_Pipe_Foot_Model_Generator.Logic
{
    public static class InputValidationLogic
    {
        /// <summary>
        /// Check if the given string contains only numbers. Unfortunately, this still allows whitespace to pass through
        /// </summary>
        public static bool InputIsNumericOnly(string input)
        {
            Regex _regex = new Regex("[^0-9.-]+");
            return !_regex.IsMatch(input);
        }
    }
}
