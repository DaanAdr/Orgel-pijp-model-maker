using ACadSharp.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Organ_Pipe_Foot_Model_Generator.Entities
{
    public class Frustum
    {
        public Arc SmallArc { get; private set; }
        public Arc LargeArc { get; private set; }
        public Line Slantedline { get; private set; }

        public Line LowerLabiumMarking { get; private set; }
        public Line UpperLabiumMarking { get; private set; }

        public Frustum(PipeFootMeasurements measurements, Line bottomline)
        {
            // Determine information for helper variables
            double endAngleInRadians = measurements.CornerInDegrees * (Math.PI / 180);
            CSMath.XYZ centerpointForArcs = new CSMath.XYZ(x: Math.Round(bottomline.StartPoint.X - measurements.SmallRadius, 1), y: bottomline.StartPoint.Y, z: 0);

            // Determine information for the CAD model elements
            SmallArc = DetermineArc(measurements.SmallRadius, endAngleInRadians, centerpointForArcs);
            LargeArc = DetermineArc(measurements.LargeRadius, endAngleInRadians, centerpointForArcs);
            Slantedline = DetermineSlantedline(centerpointForArcs, measurements.CornerInDegrees, measurements.LargeRadius, measurements.SmallRadius);

            // Check if labium markings need to be drawn
            if (measurements.LabiumWidth > 0)
            {
                DetermineLabiumMarkings(centerpointForArcs, measurements.CornerInDegrees, measurements.LargeRadius, measurements.SmallRadius, measurements.LabiumWidth);
            }
        }

        /// <summary>
        /// Creates and Arc object that can be used in CAD models
        /// </summary>
        private Arc DetermineArc(double radius, double endAngleInRadians, CSMath.XYZ centerpoint)
        {
            return new Arc
            {
                Center = centerpoint,
                Radius = radius,
                StartAngle = 0, // Arcs always start at 0
                EndAngle = endAngleInRadians
            };
        }

        /// <summary>
        /// Determine the Start and End coordinates for the slanted line of the CAD model.
        /// </summary>
        private Line DetermineSlantedline(CSMath.XYZ centerpoint, double cornerInDegrees, double largeRadius, double smallRadius)
        {
            double centerX = centerpoint.X;
            double centerY = centerpoint.Y;
            double angleInDegrees = cornerInDegrees;
            double length = largeRadius;

            // Convert angle to radians
            double angleInRadians = angleInDegrees * (Math.PI / 180);

            // Calculate new start point
            double newStartX = centerX + smallRadius * Math.Cos(angleInRadians);
            double newStartY = centerY + smallRadius * Math.Sin(angleInRadians);
            double newStartXRounded = Math.Round(newStartX, 1);
            double newStartYRounded = Math.Round(newStartY, 1);

            // Calculate endpoint
            double endX = centerX + length * Math.Cos(angleInRadians);
            double endY = centerY + length * Math.Sin(angleInRadians);
            double endXRounded = Math.Round(endX, 1);
            double endYRounded = Math.Round(endY, 1);

            return new Line
            {
                StartPoint = new CSMath.XYZ(x: newStartXRounded, y: newStartYRounded, z: 0),
                EndPoint = new CSMath.XYZ(endXRounded, endYRounded, 0)
            };
        }

        /// <summary>
        /// Determine the Start and End coordinates for both Labium markings
        /// </summary>
        private void DetermineLabiumMarkings(CSMath.XYZ centerpoint, double cornerInDegrees, double largeRadius, double smallRadius, double labiumWidth)
        {
            double centerX = centerpoint.X;
            double centerY = centerpoint.Y;

            // Determine cutout length
            int cutoutLength = 3;
            int test = (int)(cornerInDegrees / 10);

            if (test > cutoutLength) cutoutLength = test;

            // Convert angle to radians
            double angleInRadians = (cornerInDegrees / 2) * (Math.PI / 180);

            #region Centerline
            // Calculate new start point
            double centerLineStartX = Math.Round(centerX + smallRadius * Math.Cos(angleInRadians), 1);
            double centerLineStartY = Math.Round(centerY + smallRadius * Math.Sin(angleInRadians), 1);

            // Calculate endpoint
            double centerLineEndX = Math.Round(centerX + largeRadius * Math.Cos(angleInRadians), 1);
            double centerLineEndY = Math.Round(centerY + largeRadius * Math.Sin(angleInRadians), 1);
            #endregion

            // Calculate the direction vector of Centerline
            double vectorXAxis = centerLineEndX - centerLineStartX;
            double vectorYAxis = centerLineEndY - centerLineStartY;

            // Normalize the direction vector using Pythagoras Theorum
            double lengthDir = Math.Sqrt(vectorXAxis * vectorXAxis + vectorYAxis * vectorYAxis); // Length of the centerline
            double unitDirX = vectorXAxis / lengthDir;      // Have a length of 1 but the direction of the centerline
            double unitDirY = vectorYAxis / lengthDir;

            // Calculate the perpendicular direction vector (rotate by 90 degrees)
            double perpDirX = -unitDirY; // Rotate counter-clockwise
            double perpDirY = unitDirX;

            // Offset distance 
            double offsetDistance = Math.Round(labiumWidth / 2, 1);

            // TODO: Determine if the lines need to be + or -

            // Calculate the new start and end points for LowerLabiaalLine
            double upperLabiaalLineEndX = centerLineEndX + perpDirX * offsetDistance;
            double upperLabiaalLineEndY = centerLineEndY + perpDirY * offsetDistance;

            // Calculate the start point for UpperLabiaalLine by moving back 3 mm along the direction of HelperLine
            double upperLabiaalLineStartX = upperLabiaalLineEndX - (cutoutLength * unitDirX);
            double upperLabiaalLineStartY = upperLabiaalLineEndY - (cutoutLength * unitDirY);

            // Create the UpperLabiaalLine
            UpperLabiumMarking = new Line
            {
                StartPoint = new CSMath.XYZ(x: Math.Round(upperLabiaalLineStartX, 1), y: Math.Round(upperLabiaalLineStartY, 1), z: 0),
                EndPoint = new CSMath.XYZ(x: Math.Round(upperLabiaalLineEndX, 1), y: Math.Round(upperLabiaalLineEndY, 1), z: 0) // New endpoint parallel to HelperLine
            };

            // Calculate the new start and end points for LowerLabiaalLine
            double lowerLabiaalLineEndX = centerLineEndX - perpDirX * offsetDistance;
            double lowerLabiaalLineEndY = centerLineEndY - perpDirY * offsetDistance;

            // Calculate the start point for UpperLabiaalLine by moving back 3 mm along the direction of HelperLine
            double lowerLabiaalLineStartX = lowerLabiaalLineEndX - (cutoutLength * unitDirX);
            double lowerLabiaalLineStartY = lowerLabiaalLineEndY - (cutoutLength * unitDirY);

            // Create the UpperLabiaalLine
            LowerLabiumMarking = new Line
            {
                StartPoint = new CSMath.XYZ(x: Math.Round(lowerLabiaalLineStartX, 1), y: Math.Round(lowerLabiaalLineStartY, 1), z: 0),
                EndPoint = new CSMath.XYZ(x: Math.Round(lowerLabiaalLineEndX, 1), y: Math.Round(lowerLabiaalLineEndY, 1), z: 0) // New endpoint parallel to HelperLine
            };
        }
    }
}
