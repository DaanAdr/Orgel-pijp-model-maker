using System.Windows.Media.Media3D;
using Organ_Pipe_Foot_Model_Generator.Entities;

namespace Organ_Pipe_Foot_Model_Generator_Tests.UnitTests
{
    public class PipeFootMeasurementsTests
    {
        [Fact]
        public void CalculateMeasurements_TopAndBottomDiametersAreInnerDiameters()
        {
            //Arrange
            double topDiameter = 50;
            double bottomDiameter = 150;
            double height = 200;

            //Act
            PipeFootTemplate pipeFootTemplate = new PipeFootTemplate(100, 100, topDiameter, bottomDiameter, height);
            PipeFootMeasurements measurements = pipeFootTemplate.Measurements;

            //Assert
            //Measurements
            Assert.Equal(expected: 206.2, actual: measurements.LengthSlantedSide);
            Assert.Equal(expected: 157.1, actual: measurements.LengthInnerDiameter);
            Assert.Equal(expected: 471.2, actual: measurements.LengthOuterDiameter);
            Assert.Equal(expected: 103.1, actual: measurements.SmallRadius);
            Assert.Equal(expected: 309.3, actual: measurements.LargeRadius);
            Assert.Equal(expected: 87.3, actual: measurements.CornerInDegrees);

            //Model
            Assert.Equal(new CSMath.XYZ(x: 100, y: 100, z: 0), pipeFootTemplate.Bottomline.StartPoint);
            Assert.Equal(new CSMath.XYZ(x: 306.2, y: 100, z: 0), pipeFootTemplate.Bottomline.EndPoint);
        }

        [Fact]
        public void CalculateMeasurements_TopAndBottomDiametersAreOuterDiameters()
        {
            //Arrange
            double topDiameter = 50;
            double bottomDiameter = 150;
            double height = 200;
            double metalThickness = 1.5;

            //Act
            PipeFootTemplate pipeFootTemplate = new PipeFootTemplate(100, 100, topDiameter, bottomDiameter, height, metalThickness);
            PipeFootMeasurements measurements = pipeFootTemplate.Measurements;

            //Assert
            //Measurements
            Assert.Equal(expected: 206.2, actual: measurements.LengthSlantedSide);
            Assert.Equal(expected: 147.7, actual: measurements.LengthInnerDiameter);
            Assert.Equal(expected: 464.0, actual: measurements.LengthOuterDiameter);
            Assert.Equal(expected: 96.3, actual: measurements.SmallRadius);
            Assert.Equal(expected: 302.5, actual: measurements.LargeRadius);
            Assert.Equal(expected: 87.9, actual: measurements.CornerInDegrees);

            //Model
            //Assert.Equal(new CSMath.XYZ(x: 100, y: 100, z: 0), pipeFootTemplate.Bottomline.StartPoint);
            //Assert.Equal(new CSMath.XYZ(x: 306.2, y: 100, z: 0), pipeFootTemplate.Bottomline.EndPoint);
        }
    }
}
