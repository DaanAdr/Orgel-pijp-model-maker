using ACadSharp.Entities;

namespace Organ_Pipe_Foot_Model_Generator.Entities
{
    public class Rectangle
    {
        public Line Topline { get; set; }
        public Line Leftline { get; set; }
        public Line Rightline { get; set; }

        public Rectangle(Line bottomline, double height)
        {
            double bottomLeftX = bottomline.StartPoint.X;
            double bottomRightX = bottomline.EndPoint.X;

            double topY = bottomline.EndPoint.Y + height;

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
        }
    }
}
