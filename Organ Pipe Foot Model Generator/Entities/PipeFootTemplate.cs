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
        private readonly double StartAngle = 0;
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
            //The English word for "straal" is radius. So I don't have to halve the TopDiameter
            double xCoordinateForCenterPoint = Bottomline.StartPoint.X - Measurements.SmallRadius;

            CenterPointForRadii = new CSMath.XYZ(x: xCoordinateForCenterPoint, y: YStandoffFromOrigin, z: 0);
        }

        private void DetermineSmallArc()
        {
            SmallArc = new Arc
            {
                Center = CenterPointForRadii,
                Radius = Measurements.SmallRadius,
                StartAngle = StartAngle,
                EndAngle = EndAngle
            };
        }

        private void DetermineEndAngle()
        {
            double endAngle = StartAngle + Measurements.CornerInDegrees;

            //if (endAngle < 0)
            //{
            //    endAngle += 360;
            //}

            EndAngle = endAngle;
        }

        //TODO: Remove after testing
        public Point GetCenterPoint()
        {
            return new Point(CenterPointForRadii);
        }
    }
}
