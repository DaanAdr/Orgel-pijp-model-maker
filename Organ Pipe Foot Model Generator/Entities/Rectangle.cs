using ACadSharp.Entities;

namespace Organ_Pipe_Foot_Model_Generator.Entities
{
    public class Rectangle
    {
        public Line Topline { get; private set; }
        public Line Leftline { get; private set; }
        public Line Rightline { get; private set; }

        public Line LowerLabiumMarking { get; private set; }
        public Line UpperLabiumMarking { get; private set; }

        public Rectangle(Line bottomline, double height, double labiumWidth)
        {
            double bottomLeftX = bottomline.StartPoint.X;
            double bottomRightX = bottomline.EndPoint.X;

            double bottomY = bottomline.EndPoint.Y;
            double topY = Math.Round(bottomY + height, 1);

            // Draw the lines of the rectangle
            Topline = new Line
            {
                StartPoint = new CSMath.XYZ(x: bottomLeftX, topY, 0),
                EndPoint = new CSMath.XYZ(bottomRightX, topY, 0)
            };

            Leftline = new Line
            {
                StartPoint = bottomline.StartPoint,
                EndPoint = new CSMath.XYZ(bottomLeftX, topY, 0)
            };

            Rightline = new Line
            {
                StartPoint = bottomline.EndPoint,
                EndPoint = new CSMath.XYZ(bottomRightX, topY, 0)
            };

            // Add additional information for labium cutouts
            double labiumStartX = Math.Round(bottomRightX - 3, 1);
            double verticalCenter = Math.Round(bottomY + (height / 2), 1);
            double labiumOffsetFromCenter = Math.Round(labiumWidth / 2, 1);
            double upperLabiumY = Math.Round(verticalCenter + labiumOffsetFromCenter, 1);
            double lowerLabiumY = Math.Round(verticalCenter - labiumOffsetFromCenter, 1);

            // Draw labium cutouts
            UpperLabiumMarking = new Line
            {
                StartPoint = new CSMath.XYZ(labiumStartX, upperLabiumY, 0),
                EndPoint = new CSMath.XYZ(bottomRightX, upperLabiumY, 0)
            };

            LowerLabiumMarking = new Line
            {
                StartPoint = new CSMath.XYZ(labiumStartX, lowerLabiumY, 0),
                EndPoint = new CSMath.XYZ(bottomRightX, lowerLabiumY, 0)
            };
        }
    }
}
