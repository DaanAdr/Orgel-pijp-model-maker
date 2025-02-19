using System.Windows.Media.Media3D;
using Organ_Pipe_Foot_Model_Generator.Entities;

namespace Organ_Pipe_Foot_Model_Generator_Tests.UnitTests
{
    public class PipeFootMeasurementsTests
    {
        [Fact]
        public void CalculateMeasurements_TopAndBottomDiametersAreInnerDiameters()
        {
            // Arrange
            double topDiameter = 150;
            double bottomDiameter = 50;
            double height = 200;

            // Act
            PipeFootTemplate pipeFootTemplate = new PipeFootTemplate(100, 100, topDiameter, bottomDiameter, height);
            PipeFootMeasurements measurements = pipeFootTemplate.Measurements;

            // Assert
            // Measurements
            Assert.Equal(expected: 206.2, actual: measurements.LengthSlantedSide);
            Assert.Equal(expected: 157.1, actual: measurements.LengthInnerDiameter);
            Assert.Equal(expected: 471.2, actual: measurements.LengthOuterDiameter);
            Assert.Equal(expected: 103.1, actual: measurements.SmallRadius);
            Assert.Equal(expected: 309.3, actual: measurements.LargeRadius);
            Assert.Equal(expected: 87.3, actual: measurements.CornerInDegrees);

            // Model
            // Bottomline
            Assert.Equal(new CSMath.XYZ(x: 100, y: 100, z: 0), pipeFootTemplate.Bottomline.StartPoint);
            Assert.Equal(new CSMath.XYZ(x: 306.2, y: 100, z: 0), pipeFootTemplate.Bottomline.EndPoint);

            // Small Arc
            Assert.Equal(0, pipeFootTemplate.SmallArc.StartAngle);
            var radiansSmallArc = 87.3 * (Math.PI / 180);
            Assert.Equal(radiansSmallArc, pipeFootTemplate.SmallArc.EndAngle);
            Assert.Equal(new CSMath.XYZ(x: -3.1, y: 100, z: 0), pipeFootTemplate.SmallArc.Center);
            Assert.Equal(103.1, pipeFootTemplate.SmallArc.Radius);

            // Large Arc
            Assert.Equal(0, pipeFootTemplate.LargeArc.StartAngle);
            var radiansLargeArc = 87.3 * (Math.PI / 180);
            Assert.Equal(radiansLargeArc, pipeFootTemplate.LargeArc.EndAngle);
            Assert.Equal(new CSMath.XYZ(x: -3.1, y: 100, z: 0), pipeFootTemplate.LargeArc.Center);
            Assert.Equal(309.3, pipeFootTemplate.LargeArc.Radius);

            // Slanted line
            Assert.Equal(new CSMath.XYZ(x: 1.8, y: 203, z: 0), pipeFootTemplate.Slantedline.StartPoint);
            Assert.Equal(new CSMath.XYZ(x: 11.5, y: 409, z: 0), pipeFootTemplate.Slantedline.EndPoint);
        }

        [Fact]
        public void CalculateMeasurements_TopAndBottomDiametersAreOuterDiameters()
        {
            // Arrange
            double topDiameter = 150;
            double bottomDiameter = 50;
            double height = 200;
            double metalThickness = 1.5;

            // Act
            PipeFootTemplate pipeFootTemplate = new PipeFootTemplate(100, 100, topDiameter, bottomDiameter, height, metalThickness);
            PipeFootMeasurements measurements = pipeFootTemplate.Measurements;

            // Assert
            // Measurements
            Assert.Equal(expected: 206.2, actual: measurements.LengthSlantedSide);
            Assert.Equal(expected: 147.7, actual: measurements.LengthInnerDiameter);
            Assert.Equal(expected: 461.8, actual: measurements.LengthOuterDiameter);
            Assert.Equal(expected: 97, actual: measurements.SmallRadius);
            Assert.Equal(expected: 303.2, actual: measurements.LargeRadius);
            Assert.Equal(expected: 87.2, actual: measurements.CornerInDegrees);

            // Model
            // Bottomline
            Assert.Equal(new CSMath.XYZ(x: 100, y: 100, z: 0), pipeFootTemplate.Bottomline.StartPoint);
            Assert.Equal(new CSMath.XYZ(x: 306.2, y: 100, z: 0), pipeFootTemplate.Bottomline.EndPoint);

            // Small Arc
            Assert.Equal(0, pipeFootTemplate.SmallArc.StartAngle);
            var radiansSmallArc = 87.2 * (Math.PI / 180);
            Assert.Equal(radiansSmallArc, pipeFootTemplate.SmallArc.EndAngle);
            Assert.Equal(new CSMath.XYZ(x: 3, y: 100, z: 0), pipeFootTemplate.SmallArc.Center);
            Assert.Equal(97, pipeFootTemplate.SmallArc.Radius);

            // Large Arc
            Assert.Equal(0, pipeFootTemplate.LargeArc.StartAngle);
            var radiansLargeArc = 87.2 * (Math.PI / 180);
            Assert.Equal(radiansLargeArc, pipeFootTemplate.LargeArc.EndAngle);
            Assert.Equal(new CSMath.XYZ(x: 3, y: 100, z: 0), pipeFootTemplate.LargeArc.Center);
            Assert.Equal(303.2, pipeFootTemplate.LargeArc.Radius);

            // Slanted line
            Assert.Equal(new CSMath.XYZ(x: 7.7, y: 196.9, z: 0), pipeFootTemplate.Slantedline.StartPoint);
            Assert.Equal(new CSMath.XYZ(x: 17.8, y: 402.8, z: 0), pipeFootTemplate.Slantedline.EndPoint);
        }
    }
}
