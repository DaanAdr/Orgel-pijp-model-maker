using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Organ_Pipe_Foot_Model_Generator.Entities;

namespace Organ_Pipe_Foot_Model_Generator.Logic
{
    public static class CreateNestedDxfFileLogic
    {
        public static void CreateNestedDxfFile()
        {
            // Group all pipes by octave

            // Determine Y offset for next octave

            // Convert list of excel labiaal pijpen to list of PipeFootTemplate
        }

        private List<PipeFootTemplate> ConvertLabiaalPijpExcelToTemplateForOctave(List<LabiaalPijpExcel> excelPipes)
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
