using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACadSharp.Entities;

namespace Organ_Pipe_Foot_Model_Generator.Entities
{
    public class PipeFootTemplate
    {
        public Line Bottomline { get; private set; }

        private double XStandoffFromOrigin { get; set; }
        private double YStandoffFromOrigin { get; set; }
        public PipeFootMeasurements Measurements { get; private set; }

        public PipeFootTemplate(double xStandoffFromOrigin, double yStandoffFromOrigin, double topDiameter, double bottomDiameter, double height)
        {
            Measurements = new PipeFootMeasurements(topDiameter, bottomDiameter, height);
            XStandoffFromOrigin = xStandoffFromOrigin;
            YStandoffFromOrigin = yStandoffFromOrigin;

            DetermineBottomline();
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
    }
}
