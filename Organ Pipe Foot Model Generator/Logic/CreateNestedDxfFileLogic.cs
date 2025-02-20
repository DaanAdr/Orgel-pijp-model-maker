using Organ_Pipe_Foot_Model_Generator.Entities;

namespace Organ_Pipe_Foot_Model_Generator.Logic
{
    public static class CreateNestedDxfFileLogic
    {
        /// <summary>
        /// Convert a list of LabiaalPijpExcel objects into a list of PipeFootTemplate objects, that can be used to populate a CAD file
        /// </summary>
        /// <param name="excelPipes">List of LabiaalPijpExcel</param>
        /// <param name="horizontalObjectSeperationDistance">The horizontal distance between PipeFootTemplate object in the CAD file</param>
        /// <param name="verticalObjectSeperationDistance">The vertical distance between PipeFootTemplate object in the CAD file</param>
        /// <returns></returns>
        public static List<PipeFootTemplate> CreateNestedDxfFile(
            List<LabiaalPijpExcel> excelPipes, 
            double horizontalObjectSeperationDistance,
            double verticalObjectSeperationDistance
        )
        {
            // Group all pipes by octave
            Dictionary<string, List<LabiaalPijpExcel>> octaves = GroupLabiaalPijpExcelByOctave(excelPipes);

            // Convert list of excel labiaal pijpen to list of PipeFootTemplate
            // TODO: Octaves might have to be added in reverse order for better identification
            List<PipeFootTemplate> templatesForOctave = ConvertLabiaalPijpExcelToTemplateForAllOctaves(
                octaves, 
                horizontalObjectSeperationDistance, 
                verticalObjectSeperationDistance
            );

            return templatesForOctave;
        }

        /// <summary>
        /// Group LabiaalPijpExcel objects by the octave they're in
        /// </summary>
        /// <param name="excelPipes">List of LabiaalPijpExcel</param>
        /// <returns></returns>
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

        /// <summary>
        /// Convert all grouped LabiaalPijpExcel objects to a list of PipeFootTemplate
        /// </summary>
        /// <param name="octaves">All LabiaalPijpExcel objects grouped by the octave they're in</param>
        /// <param name="horizontalObjectSeperationDistance">The horizontal distance between PipeFootTemplate object in the CAD file</param>
        /// <param name="verticalObjectSeperationDistance">The vertical distance between PipeFootTemplate object in the CAD file</param>
        /// <returns></returns>
        private static List<PipeFootTemplate> ConvertLabiaalPijpExcelToTemplateForAllOctaves(
            Dictionary<string, List<LabiaalPijpExcel>> octaves, 
            double horizontalObjectSeperationDistance,
            double verticalObjectSeperationDistance
        )
        {
            List<PipeFootTemplate> templates = new List<PipeFootTemplate>();
            double previousObjectYPosition = 0;

            // Loop through all octaves
            foreach (KeyValuePair<string, List<LabiaalPijpExcel>> octave in octaves)
            {
                List<LabiaalPijpExcel> excelPipesInOctave = octave.Value;

                // Check if octave has values, skip if not
                if (excelPipesInOctave.Count <= 0) continue;

                // Determine Y offset for next octave
                double yStandoffFromOrigin = previousObjectYPosition + verticalObjectSeperationDistance;

                List<PipeFootTemplate> templatesForOctave = ConvertLabiaalPijpExcelToTemplateForOctave(excelPipesInOctave, yStandoffFromOrigin, horizontalObjectSeperationDistance);
                templates.AddRange(templatesForOctave);

                // Update previousObjectYPosition
                previousObjectYPosition = templatesForOctave[0].Slantedline.EndPoint.Y;
            }

            return templates;
        }

        /// <summary>
        /// Convert a list of LabiaalPijpExcel to a list of PipeFootTemplate
        /// </summary>
        /// <param name="excelPipes">The list of LabiaalPijpExcel to convert</param>
        /// <param name="yOffsetFromOrigin">How far away from 0 on the Y axis the model should be rendered</param>
        /// <param name="horizontalObjectSeperationDistance">The horizontal distance between PipeFootTemplate object in the CAD file</param>
        /// <returns></returns>
        private static List<PipeFootTemplate> ConvertLabiaalPijpExcelToTemplateForOctave(
            List<LabiaalPijpExcel> excelPipes, 
            double yOffsetFromOrigin,
            double horizontalObjectSeperationDistance
        )
        {
            List<PipeFootTemplate> octave = new List<PipeFootTemplate>();
            double previousObjectXPosition = 0;

            // Loop through all records
            foreach(LabiaalPijpExcel excelPipe in excelPipes)
            {
                // Determine X offset for next pipe
                double xStandoffFromOrigin = previousObjectXPosition + horizontalObjectSeperationDistance;

                PipeFootTemplate template = ConvertLabiaalPijpExcelToTemplate(excelPipe, xStandoffFromOrigin, yOffsetFromOrigin);
                octave.Add(template);

                // Set x-position for last object 
                previousObjectXPosition = template.GetFurthestXPosition();
            }

            return octave;
        }

        /// <summary>
        /// Convert a LabiaalPijpExcel to PipeFootTemplate
        /// </summary>
        /// <param name="excelPipe">The LabiaalPijpExcel to convert</param>
        /// <param name="xOffsetFromOrigin">How far away from 0 on the X axis the model should be rendered</param>
        /// <param name="yOffsetFromOrigin">How far away from 0 on the Y axis the model should be rendered</param>
        /// <returns></returns>
        private static PipeFootTemplate ConvertLabiaalPijpExcelToTemplate(LabiaalPijpExcel excelPipe, double xOffsetFromOrigin, double yOffsetFromOrigin)
        {
            double bottomDiameter = Math.Round(excelPipe.PlateWidthFoot / Math.PI, 1);

            PipeFootTemplate template = new PipeFootTemplate(
                xOffsetFromOrigin, 
                yOffsetFromOrigin, 
                excelPipe.TopDiameter, 
                bottomDiameter, 
                excelPipe.Height, 
                excelPipe.MetalThickness
            );

            return template;
        }
    }
}
