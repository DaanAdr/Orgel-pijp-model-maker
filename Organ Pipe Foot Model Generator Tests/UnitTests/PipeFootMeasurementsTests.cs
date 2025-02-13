using Organ_Pipe_Foot_Model_Generator.Entities;

namespace Organ_Pipe_Foot_Model_Generator_Tests.UnitTests
{
    public class PipeFootMeasurementsTests
    {
        public readonly double _topDiameter = 50;
        public readonly double _bottonDiameter = 150;
        public readonly double _height = 200;

        [Fact]
        public void CalculateMeasurements()
        {
            //Act
            PipeFootMeasurements measurements = new PipeFootMeasurements(_topDiameter, _bottonDiameter, _height);

            //Assert
            Assert.Equal(expected: 206.2, actual: measurements.LengthSlantedSide);
            Assert.Equal(expected: 157.1, actual: measurements.LengthInnerDiameter);
            Assert.Equal(expected: 471.2, actual: measurements.LengthOuterDiameter);
            Assert.Equal(expected: 103.1, actual: measurements.SmallRadius);
            Assert.Equal(expected: 309.3, actual: measurements.LargeRadius);
            Assert.Equal(expected: 87.3, actual: measurements.CornerInDegrees);
        }
    }
}
