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
        private double XStandoffFromOrigin { get; set; }
        private double YStandoffFromOrigin { get; set; }
        private CSMath.XYZ CenterPointForRadii { get; set; }
        private readonly double EndAngleInDegrees = 180;
        private double EndAngleInRadians { get; set; }
        private readonly double StartAngleInRadians = 0;
        public PipeFootMeasurements Measurements { get; private set; }

        public PipeFootTemplate(double xStandoffFromOrigin, double yStandoffFromOrigin, double topDiameter, double bottomDiameter, double height, double metalThickness = 0)
        {
            Measurements = new PipeFootMeasurements(topDiameter, bottomDiameter, height, metalThickness);
            XStandoffFromOrigin = xStandoffFromOrigin;
            YStandoffFromOrigin = yStandoffFromOrigin;

            EndAngleInRadians = EndAngleInDegrees * (Math.PI / 180);

            DetermineBottomline();
            DetermineCenterPoint();
            DetermineEndAngle();
            DetermineSmallArc();
            DetermineLargeArc();
            DetermineSlantedline();
        }

        private void DetermineBottomline()
        {
            double totalLineDistance = XStandoffFromOrigin + Measurements.LengthSlantedSide;

            Bottomline = new Line
            {
                StartPoint = new CSMath.XYZ(x: XStandoffFromOrigin, y: YStandoffFromOrigin, z: 0),
                EndPoint = new CSMath.XYZ(totalLineDistance, YStandoffFromOrigin, 0)
            };
        }

        private void DetermineCenterPoint()
        {
            // The English word for "straal" is radius. So I don't have to halve the TopDiameter
            double xCoordinateForCenterPoint = Math.Round(Bottomline.StartPoint.X - Measurements.SmallRadius, 1);

            CenterPointForRadii = new CSMath.XYZ(x: xCoordinateForCenterPoint, y: YStandoffFromOrigin, z: 0);
        }

        private void DetermineEndAngle()
        {
            double endAngleInRadians = Measurements.CornerInDegrees * (Math.PI / 180);
            EndAngleInRadians = endAngleInRadians;
        }

        private void DetermineSmallArc()
        {
            SmallArc = new Arc
            {
                Center = CenterPointForRadii,
                Radius = Measurements.SmallRadius,
                StartAngle = StartAngleInRadians,
                EndAngle = EndAngleInRadians
            };
        }

        private void DetermineLargeArc()
        {
            LargeArc = new Arc
            {
                Center = CenterPointForRadii,
                Radius = Measurements.LargeRadius,
                StartAngle = StartAngleInRadians,
                EndAngle = EndAngleInRadians
            };
        }

        private void DetermineSlantedline()
        {
            double centerX = CenterPointForRadii.X;
            double centerY = CenterPointForRadii.Y;
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
