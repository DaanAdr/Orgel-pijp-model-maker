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
            PipeFootMeasurements measurements = new PipeFootMeasurements(topDiameter, bottomDiameter, height);

            // Assert
            // Measurements
            Assert.Equal(expected: 206.2, actual: measurements.LengthSlantedSide);
            Assert.Equal(expected: 157.1, actual: measurements.LengthBottomDiameter);
            Assert.Equal(expected: 471.2, actual: measurements.LengthTopDiameter);
            Assert.Equal(expected: 103.1, actual: measurements.SmallRadius);
            Assert.Equal(expected: 309.3, actual: measurements.LargeRadius);
            Assert.Equal(expected: 87.3, actual: measurements.CornerInDegrees);
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
            PipeFootMeasurements measurements = new PipeFootMeasurements(topDiameter, bottomDiameter, height, metalThickness);

            // Assert
            // Measurements
            Assert.Equal(expected: 206.2, actual: measurements.LengthSlantedSide);
            Assert.Equal(expected: 147.7, actual: measurements.LengthBottomDiameter);
            Assert.Equal(expected: 461.8, actual: measurements.LengthTopDiameter);
            Assert.Equal(expected: 97, actual: measurements.SmallRadius);
            Assert.Equal(expected: 303.2, actual: measurements.LargeRadius);
            Assert.Equal(expected: 87.2, actual: measurements.CornerInDegrees);
        }
    }
}
