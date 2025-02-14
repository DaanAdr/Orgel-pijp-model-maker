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
        private readonly double EndAngleInDegrees = 180;
        private double EndAngleInRadians { get; set; }
        private double StartAngleInRadians { get; set; }
        public PipeFootMeasurements Measurements { get; private set; }

        public PipeFootTemplate(double xStandoffFromOrigin, double yStandoffFromOrigin, double topDiameter, double bottomDiameter, double height)
        {
            Measurements = new PipeFootMeasurements(topDiameter, bottomDiameter, height);
            XStandoffFromOrigin = xStandoffFromOrigin;
            YStandoffFromOrigin = yStandoffFromOrigin;

            EndAngleInRadians = EndAngleInDegrees * (Math.PI / 180);

            DetermineBottomline();
            DetermineCenterPoint();
            DetermineStartAngle();
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
            double xCoordinateForCenterPoint = Bottomline.EndPoint.X + Measurements.SmallRadius;

            CenterPointForRadii = new CSMath.XYZ(x: xCoordinateForCenterPoint, y: YStandoffFromOrigin, z: 0);
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

        private void DetermineStartAngle()
        {
            double startAngleInDegrees = EndAngleInDegrees - Measurements.CornerInDegrees;

            //TODO: Determine if the wrap around logic is necessary
            //if (endAngle < 0)
            //{
            //    endAngle += 360;
            //}

            double startAngleInRadians = startAngleInDegrees * (Math.PI / 180);
            StartAngleInRadians = startAngleInRadians;
        }

        //TODO: Remove after testing
        public Point GetCenterPoint()
        {
            return new Point(CenterPointForRadii);
        }
    }
}
