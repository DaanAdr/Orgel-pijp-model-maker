﻿using ACadSharp.Entities;

namespace Organ_Pipe_Foot_Model_Generator.Entities
{
    public class Frustum
    {
        public Arc SmallArc { get; private set; }
        public Arc LargeArc { get; private set; }
        public Line Slantedline { get; private set; }

        public Frustum(PipeFootMeasurements measurements, Line bottomline)
        {
            // Determine information for helper variables
            double endAngleInRadians = measurements.CornerInDegrees * (Math.PI / 180);
            CSMath.XYZ centerpointForArcs = new CSMath.XYZ(x: Math.Round(bottomline.StartPoint.X - measurements.SmallRadius, 1), y: bottomline.StartPoint.Y, z: 0);

            // Determine information for the CAD model elements
            SmallArc = DetermineArc(measurements.SmallRadius, endAngleInRadians, centerpointForArcs);
            LargeArc = DetermineArc(measurements.LargeRadius, endAngleInRadians, centerpointForArcs);
            Slantedline = DetermineSlantedline(centerpointForArcs, measurements.CornerInDegrees, measurements.LargeRadius, measurements.SmallRadius);
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
    }
}
