using Organ_Pipe_Foot_Model_Generator.Entities;

namespace Organ_Pipe_Foot_Model_Generator.Logic
{
    public static class CreateNestedDxfFileLogic
    {
        public static List<PipeFootTemplate> CreateNestedDxfFile(List<LabiaalPijpExcel> excelPipes)
        {
            // Group all pipes by octave
            Dictionary<string, List<LabiaalPijpExcel>> octaves = GroupLabiaalPijpExcelByOctave(excelPipes);

            // TODO: Determine Y offset for next octave

            // Convert list of excel labiaal pijpen to list of PipeFootTemplate
            // Octaves might have to be added in reverse order for better identification
            List<LabiaalPijpExcel> rowToNest = octaves["MAJOR"]; // TODO: Remove, this is testing variable

            List<PipeFootTemplate> templatesForOctave = ConvertLabiaalPijpExcelToTemplateForOctave(rowToNest);

            return templatesForOctave;
        }

        private static Dictionary<string, List<LabiaalPijpExcel>> GroupLabiaalPijpExcelByOctave(List<LabiaalPijpExcel> excelPipes)
        {
            List<LabiaalPijpExcel> major = new List<LabiaalPijpExcel>();
            List<LabiaalPijpExcel> minor = new List<LabiaalPijpExcel>();
            List<LabiaalPijpExcel> first = new List<LabiaalPijpExcel>();
            List<LabiaalPijpExcel> second = new List<LabiaalPijpExcel>();
            List<LabiaalPijpExcel> third = new List<LabiaalPijpExcel>();
            List<LabiaalPijpExcel> fourth = new List<LabiaalPijpExcel>();

            // Loop through all pipes
            foreach (LabiaalPijpExcel excelPipe in excelPipes)
            {
                // Check if first char is upper case, if yes at to "MAJOR"
                if (char.IsUpper(excelPipe.Key[0]))
                {
                    major.Add(excelPipe);
                    continue;
                }

                // Check if last char is a number
                char lastCharOfKey = excelPipe.Key[excelPipe.Key.Length - 1];
                int number;

                if (int.TryParse(lastCharOfKey.ToString(), out number))
                {
                    switch(number)
                    {
                        case 1:
                            first.Add(excelPipe);
                            break;
                        case 2:
                            second.Add(excelPipe);
                            break;
                        case 3:
                            third.Add(excelPipe);
                            break;
                        case 4:
                            fourth.Add(excelPipe);
                            break;
                    }

                    continue;
                }

                // Add remaining to "MINOR"
                minor.Add(excelPipe);
            }

            // Create a dictionary to hold the lists
            Dictionary<string, List<LabiaalPijpExcel>> octaves = new Dictionary<string, List<LabiaalPijpExcel>>
            {
                { "MAJOR", major },
                { "MINOR", minor },
                { "FIRST", first },
                { "SECOND", second },
                { "THIRD", third },
                { "FOURTH", fourth }
            };

            return octaves;
        }

        private static List<PipeFootTemplate> ConvertLabiaalPijpExcelToTemplateForOctave(List<LabiaalPijpExcel> excelPipes)
        {
            List<PipeFootTemplate> octave = new List<PipeFootTemplate>();
            double previousObjectXPosition = 0;
            double objectSeperationDistance = 100;

            // Loop through all records
            foreach(LabiaalPijpExcel excelPipe in excelPipes)
            {
                // Determine X offset for next pipe
                double xStandoffFromOrigin = previousObjectXPosition + objectSeperationDistance;

                PipeFootTemplate template = ConvertLabiaalPijpExcelToTemplate(excelPipe, xStandoffFromOrigin);
                octave.Add(template);

                // Set x-position for last object 
                previousObjectXPosition = template.GetFurthestXPosition();
            }

            return octave;
        }

        private static PipeFootTemplate ConvertLabiaalPijpExcelToTemplate(LabiaalPijpExcel excelPipe, double xStandoffFromOrigin)
        {
            double yStandoffFromOrigin = 100;
            double bottomDiameter = Math.Round(excelPipe.PlateWidthFoot / Math.PI, 1);

            PipeFootTemplate template = new PipeFootTemplate(
                xStandoffFromOrigin, 
                yStandoffFromOrigin, 
                excelPipe.TopDiameter, 
                bottomDiameter, 
                excelPipe.Height, 
                excelPipe.MetalThickness
            );

            return template;
        }
    }
}
