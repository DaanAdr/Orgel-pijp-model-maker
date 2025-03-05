namespace Organ_Pipe_Foot_Model_Generator.Entities
{
    public class PipeFootMeasurements
    {
        public double Height { get; private set; }
        public double LengthSlantedSide { get; private set; }
        public double LengthBottomDiameter { get; private set; }
        public double LengthTopDiameter { get; private set; }
        public double SmallRadius { get; private set; }
        public double LargeRadius { get; private set; }
        public double CornerInDegrees { get; private set; }
        public double LabiumWidth { get; private set; }

        /// <summary>
        /// Calculates all values necessary to make a PipeFootTemplate
        /// </summary>
        /// <param name="topDiameter">The length of the top of the frustum. Labeled T in Images/frustum.png</param>
        /// <param name="bottomDiameter">The length of the bottom of the frustum. Labeled B in Images/frustum.png</param>
        /// <param name="height">The height of the frustum. Labeled H in Images/frustum.png</param>
        /// <param name="metalThickness">How thick the sheet metal for the pipe is, this has to be subtracted (twice) from the top- and bottomdiameter if the measurements are for the outerdiameter of the pipes</param>
        public PipeFootMeasurements(double topDiameter, double bottomDiameter, double height, double metalThickness = 0, double labiumWidth = 0)
        {
            topDiameter = Math.Round(topDiameter - (metalThickness * 2), 1);
            bottomDiameter = Math.Round(bottomDiameter - (metalThickness * 2), 1);
            Height = Math.Round(height, 1);

            LengthSlantedSide = CalculateLengthSlantedSide(topDiameter, bottomDiameter);
            LengthBottomDiameter = CalculateLengthBottomDiameter(bottomDiameter);
            LengthTopDiameter = CalculateLengthTopDiameter(topDiameter);
            SmallRadius = CalculateSmallRadius();
            LargeRadius = CalculateLargeRadius();
            CornerInDegrees = CalculateCornerDegrees();
            LabiumWidth = labiumWidth;
        }

        /// <summary>
        /// Calculates the length for the slanted sides of the frustum. Labeled R in Images/frustum.png
        /// </summary>
        private double CalculateLengthSlantedSide(double topDiameter, double bottomDiameter)
        {
            double topDiameterHalved = topDiameter / 2;
            double bottomDiameterHalved = bottomDiameter / 2;

            double halvedTopMinusHalvedBottom = topDiameterHalved - bottomDiameterHalved;
            double halvedTopMinusHalvedBottomSquared = halvedTopMinusHalvedBottom * halvedTopMinusHalvedBottom;

            double heightSquared = Height * Height;
            double lengthSlantedSide = Math.Sqrt(halvedTopMinusHalvedBottomSquared + heightSquared);

            return Math.Round(lengthSlantedSide, 1); ;
        }

        /// <summary>
        /// Calculates the length of the SmallArc. Labeled L in Images/template.png
        /// </summary>
        private double CalculateLengthBottomDiameter(double bottomDiameter)
        {
            double bottomDiameterTimesPi = bottomDiameter * Math.PI;
            return Math.Round(bottomDiameterTimesPi, 1);
        }

        /// <summary>
        /// Calculates the length of the LargeArc. Labeled M in Images/template.png
        /// </summary>
        private double CalculateLengthTopDiameter(double topDiameter)
        {
            double topDiameterTimesPi = topDiameter * Math.PI;
            return Math.Round(topDiameterTimesPi, 1);
        }

        /// <summary>
        /// Calculates the radius (straal in Dutch) for the SmallArc. Labeled P in Images/template.png
        /// </summary>
        private double CalculateSmallRadius()
        {
            double lengthInnerDiameterTimesLengthSlantedSide = LengthBottomDiameter * LengthSlantedSide;
            double lengthOuterDiameterMinusLengthInnerDiameter = LengthTopDiameter - LengthBottomDiameter;
            double divide = lengthInnerDiameterTimesLengthSlantedSide / lengthOuterDiameterMinusLengthInnerDiameter;
            return Math.Round(divide, 1);
        }

        /// <summary>
        /// Calculates the radius (straal in Dutch) for the LargeArc. Labeled Q in Images/template.png
        /// </summary>
        private double CalculateLargeRadius()
        {
            double smallRadiusPlusLengthSlantedSide = SmallRadius + LengthSlantedSide;
            return Math.Round(smallRadiusPlusLengthSlantedSide, 1);
        }

        /// <summary>
        /// Calculates how many degrees the corner is at A in Images/template.png
        /// </summary>
        private double CalculateCornerDegrees()
        {
            double lengthInnerDiameterDividedBySmallRadius = LengthBottomDiameter / SmallRadius;
            double sumOutcome = (lengthInnerDiameterDividedBySmallRadius * 180) / Math.PI;
            return Math.Round(sumOutcome, 1);
        }
    }
}
