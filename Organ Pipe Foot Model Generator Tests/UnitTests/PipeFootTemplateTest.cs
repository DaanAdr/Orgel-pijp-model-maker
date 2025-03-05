using Organ_Pipe_Foot_Model_Generator.Entities;

namespace Organ_Pipe_Foot_Model_Generator_Tests.UnitTests
{
    public class PipeFootTemplateTest
    {
        [Fact]
        public void DrawCadModel()
        {
            // Arrange
            double topDiameter = 150;
            double bottomDiameter = 50;
            double height = 200;
            PipeFootMeasurements measurements = new PipeFootMeasurements(topDiameter, bottomDiameter, height);

            // Act
            PipeFootTemplate pipeFootTemplate = new PipeFootTemplate(100, 100, measurements);

            // Assert
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
    }
}
