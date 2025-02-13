using Organ_Pipe_Foot_Model_Generator.Entities;

namespace Organ_Pipe_Foot_Model_Generator_Tests.UnitTests
{
    public class PipeFootMeasurementsTests
    {
        public readonly double _topDiameter = 50;
        public readonly double _bottonDiameter = 150;
        public readonly double _height = 200;

        [Fact]
        public void CalculateLengthSlantedSide_Returns206dot2()
        {
            //Arrange
            PipeFootMeasurements measurements = new PipeFootMeasurements(_topDiameter, _bottonDiameter, _height);

            //Act
            double lengthSlantedSide = measurements.CalculateLengthSlantedSide();
            double roundedLength = Math.Round(lengthSlantedSide, 1);

            //Assert
            Assert.Equal(expected: 206.2, actual: roundedLength);
        }

        [Fact]
        public void CalculateLengthInnerDiameter_Returns157dot1()
        {
            //Arrange
            PipeFootMeasurements measurements = new PipeFootMeasurements(_topDiameter, _bottonDiameter, _height);

            //Act
            double lengthSlantedSide = measurements.CalculateLengthInnerDiameter();
            double roundedLength = Math.Round(lengthSlantedSide, 1);

            //Assert
            Assert.Equal(expected: 157.1, actual: roundedLength);
        }
    }
}
