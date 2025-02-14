using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using ACadSharp.Entities;

namespace Organ_Pipe_Foot_Model_Generator.Entities
{
    public class PipeFootTemplate
    {
        public Line Bottomline { get; private set; }
        public Arc SmallArc { get; private set; }
        private double XStandoffFromOrigin { get; set; }
        private double YStandoffFromOrigin { get; set; }
        private CSMath.XYZ CenterPointForRadii { get; set; }
        private readonly double StartAngle = 90;
        private double EndAngle { get; set; }
        public PipeFootMeasurements Measurements { get; private set; }

        public PipeFootTemplate(double xStandoffFromOrigin, double yStandoffFromOrigin, double topDiameter, double bottomDiameter, double height)
        {
            Measurements = new PipeFootMeasurements(topDiameter, bottomDiameter, height);
            XStandoffFromOrigin = xStandoffFromOrigin;
            YStandoffFromOrigin = yStandoffFromOrigin;

            DetermineBottomline();
            DetermineCenterPoint();
            DetermineEndAngle();
            DetermineSmallArc();
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
            double heightTopDiameterToSlantedSidesIntersectionPoint = 
                (Measurements.BottomDiameter - Measurements.TopDiameter) * Measurements.Height / Measurements.BottomDiameter;

            double additionalLength = Math.Round(heightTopDiameterToSlantedSidesIntersectionPoint, 1);
            double xCoordinateForCenterPoint = Bottomline.EndPoint.X + additionalLength;
            CenterPointForRadii = new CSMath.XYZ(x: xCoordinateForCenterPoint, y: YStandoffFromOrigin, z: 0);
        }

        private void DetermineSmallArc()
        {
            double radius = 10; // Replace with your desired radius
            double startAngle = 0; // Start angle in radians
            double endAngle = Math.PI; // End angle in radians for a semi-circle

            SmallArc = new Arc
            {
                Center = CenterPointForRadii,
                Radius = Measurements.SmallRadius,
                StartAngle = 180,
                EndAngle = EndAngle
            };
        }

        private void DetermineEndAngle()
        {
            double endAngle = StartAngle - Measurements.CornerInDegrees;

            if(endAngle < 0)
            {
                endAngle += 360;
            }

            EndAngle = endAngle;
        }
    }
}
