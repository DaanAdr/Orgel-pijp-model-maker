using ACadSharp;
using ACadSharp.Entities;
using ACadSharp.Tables;

namespace Organ_Pipe_Foot_Model_Generator.Entities
{
    public class PipeFootTemplate
    {
        public Line Bottomline { get; private set; }
        public Arc SmallArc { get; private set; }
        public Arc LargeArc { get; private set; }
        public Line Slantedline { get; private set; }
        public PipeFootMeasurements Measurements { get; private set; }

        public PipeFootTemplate(double xStandoffFromOrigin, double yStandoffFromOrigin, double topDiameter, double bottomDiameter, double height, double metalThickness = 0)
        {
            Measurements = new PipeFootMeasurements(topDiameter, bottomDiameter, height, metalThickness);
            CSMath.XYZ centerpoint = new CSMath.XYZ(x: xStandoffFromOrigin, y: yStandoffFromOrigin, z: 0);
            double endAngleInRadians = Measurements.CornerInDegrees * (Math.PI / 180);

            DetermineBottomline(yStandoffFromOrigin, centerpoint.X);
            SmallArc = DetermineArc(Measurements.SmallRadius, endAngleInRadians, centerpoint);
            LargeArc = DetermineArc(Measurements.LargeRadius, endAngleInRadians, centerpoint);
            DetermineSlantedline(centerpoint);
        }

        private void DetermineBottomline(double yStandoffFromOrigin, double xPositionForCenterpoint)
        {
            double xStartPosition = Math.Round(xPositionForCenterpoint + Measurements.SmallRadius, 1);
            double xEndPosition = xStartPosition + Measurements.LengthSlantedSide;

            Bottomline = new Line
            {
                StartPoint = new CSMath.XYZ(x: xStartPosition, y: yStandoffFromOrigin, z: 0),
                EndPoint = new CSMath.XYZ(xEndPosition, yStandoffFromOrigin, 0)
            };
        }

        private Arc DetermineArc(double radius, double endAngleInRadians, CSMath.XYZ centerpoint)
        {
            return new Arc
            {
                Center = centerpoint,
                Radius = radius,
                StartAngle = 0,
                EndAngle = endAngleInRadians
            };
        }

        private void DetermineSlantedline(CSMath.XYZ centerpoint)
        {
            double centerX = centerpoint.X;
            double centerY = centerpoint.Y;
            double angleInDegrees = Measurements.CornerInDegrees;
            double length = Measurements.LargeRadius;

            // Convert angle to radians
            double angleInRadians = angleInDegrees * (Math.PI / 180);

            // Calculate new start point
            double newStartX = centerX + Measurements.SmallRadius * Math.Cos(angleInRadians);
            double newStartY = centerY + Measurements.SmallRadius * Math.Sin(angleInRadians);
            double newStartXRounded = Math.Round(newStartX, 1);
            double newStartYRounded = Math.Round(newStartY, 1);

            // Calculate endpoint
            double endX = centerX + length * Math.Cos(angleInRadians);
            double endY = centerY + length * Math.Sin(angleInRadians);
            double endXRounded = Math.Round(endX, 1);
            double endYRounded = Math.Round(endY, 1);

            Slantedline = new Line
            {
                StartPoint = new CSMath.XYZ(x: newStartXRounded, y: newStartYRounded, z: 0),
                EndPoint = new CSMath.XYZ(endXRounded, endYRounded, 0)
            };
        }

        public double GetFurthestXPosition()
        {
            return Bottomline.EndPoint.X;
        }

        public void AddToCadDocument(CadDocument doc)
        {
            BlockRecord block = new BlockRecord(Guid.NewGuid().ToString());
            block.Entities.Add(Bottomline);
            block.Entities.Add(SmallArc);
            block.Entities.Add(LargeArc);
            block.Entities.Add(Slantedline);

            Insert insert = new Insert(block);

            doc.Entities.Add(insert);
        }
    }
}
