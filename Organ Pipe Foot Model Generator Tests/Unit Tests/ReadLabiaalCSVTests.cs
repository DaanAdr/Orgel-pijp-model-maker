using Organ_Pipe_Foot_Model_Generator.Entities;
using Organ_Pipe_Foot_Model_Generator.Logic;

namespace Organ_Pipe_Foot_Model_Generator_Tests.UnitTests
{
    public class ReadLabiaalCSVTests
    {
        [Fact]
        public void ReadLabiaalCSVFile()
        {
            // Arrange
            List<LabiaalPijpExcel> expectedPipes = new List<LabiaalPijpExcel>()
            {
                // Major
                new LabiaalPijpExcel
                {
                    Key = "C",
                    TopDiameter = 136.0,
                    PlateWidthFoot = 68.7,
                    Height = 210,
                    MetalThickness = 1.4
                },
                new LabiaalPijpExcel
                {
                    Key = "F#",
                    TopDiameter = 105.6,
                    PlateWidthFoot = 61.5,
                    Height = 200,
                    MetalThickness = 1.29
                },
                // Minor
                new LabiaalPijpExcel
                {
                    Key = "e",
                    TopDiameter = 68.8,
                    PlateWidthFoot = 51,
                    Height = 175,
                    MetalThickness = 1.09
                },
                new LabiaalPijpExcel
                {
                    Key = "f*",
                    TopDiameter = 65.9,
                    PlateWidthFoot = 50.1,
                    Height = 175,
                    MetalThickness = 1.06
                },
                new LabiaalPijpExcel
                {
                    Key = "f#",
                    TopDiameter = 63.1,
                    PlateWidthFoot = 49.2,
                    Height = 175,
                    MetalThickness = 1.04
                },
                new LabiaalPijpExcel
                {
                    Key = "g#*",
                    TopDiameter = 57.8,
                    PlateWidthFoot = 47.4,
                    Height = 175,
                    MetalThickness = 0.99
                },
                // 1st
                new LabiaalPijpExcel
                {
                    Key = "c1",
                    TopDiameter = 48.5,
                    PlateWidthFoot = 44,
                    Height = 175,
                    MetalThickness = 0.9
                },
                new LabiaalPijpExcel
                {
                    Key = "c#1",
                    TopDiameter = 46.4,
                    PlateWidthFoot = 43.1,
                    Height = 175,
                    MetalThickness = 0.89
                },
                // 2nd
                new LabiaalPijpExcel
                {
                    Key = "f2",
                    TopDiameter = 23.5,
                    PlateWidthFoot = 31.9,
                    Height = 175,
                    MetalThickness = 0.75
                },
                new LabiaalPijpExcel
                {
                    Key = "f#2",
                    TopDiameter = 22.6,
                    PlateWidthFoot = 31.5,
                    Height = 175,
                    MetalThickness = 0.74
                },
                // 3rd
                new LabiaalPijpExcel
                {
                    Key = "d3",
                    TopDiameter = 15.6,
                    PlateWidthFoot = 28.5,
                    Height = 175,
                    MetalThickness = 0.69
                },
                new LabiaalPijpExcel
                {
                    Key = "d#3",
                    TopDiameter = 14.5,
                    PlateWidthFoot = 28.1,
                    Height = 175,
                    MetalThickness = 0.68
                },
            };

            // Get the base directory of the test project
            string baseDirectory = AppContext.BaseDirectory;

            // Construct the path to the file using a relative path
            string filePath = Path.Combine(baseDirectory, @"..\..\..\Unit Tests\testing documents\Labiaal Pipe CSV.csv");

            // Normalize the path, resolving any relative segments
            filePath = Path.GetFullPath(filePath);

            // Act
            List<LabiaalPijpExcel> pipesInCSV = ReadExcelLogic.ReadLabiaalPijpCSVFile(filePath, ';');

            //Assert
            Assert.Equal(expectedPipes.Count(), pipesInCSV.Count());

            for (int i = 0; i < pipesInCSV.Count; i++ )
            {
                Assert.Equal(expectedPipes[i].Key, pipesInCSV[i].Key);
                Assert.Equal(expectedPipes[i].TopDiameter, pipesInCSV[i].TopDiameter);
                Assert.Equal(expectedPipes[i].MetalThickness, pipesInCSV[i].MetalThickness);
                Assert.Equal(expectedPipes[i].Height, pipesInCSV[i].Height);
                Assert.Equal(expectedPipes[i].PlateWidthFoot, pipesInCSV[i].PlateWidthFoot);
            }
        }
    }
}
