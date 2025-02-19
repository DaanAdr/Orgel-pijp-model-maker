namespace Organ_Pipe_Foot_Model_Generator.Entities
{
    public class PipeFootMeasurements
    {
        public double TopDiameter { get; }
        public double BottomDiameter { get; }
        public double Height { get; }
        public double LengthSlantedSide { get; private set; }
        public double LengthBottomDiameter { get; private set; }
        public double LengthTopDiameter { get; private set; }
        public double SmallRadius { get; private set; }
        public double LargeRadius { get; private set; }
        public double CornerInDegrees { get; private set; }

        public PipeFootMeasurements(double topDiameter, double bottomDiameter, double height, double metalThickness)
        {
            TopDiameter = Math.Round(topDiameter - (metalThickness * 2), 1);
            BottomDiameter = Math.Round(bottomDiameter - (metalThickness * 2), 1);
            Height = Math.Round(height, 1);

            LengthSlantedSide = CalculateLengthSlantedSide();
            LengthBottomDiameter = CalculateLengthBottomDiameter();
            LengthTopDiameter = CalculateLengthTopDiameter();
            SmallRadius = CalculateSmallRadius();
            LargeRadius = CalculateLargeRadius();
            CornerInDegrees = CalculateCornerDegrees();
        }

        private double CalculateLengthSlantedSide()
        {
            double topDiameterHalved = TopDiameter / 2;
            double bottomDiameterHalved = BottomDiameter / 2;

            double halvedTopMinusHalvedBottom = topDiameterHalved - bottomDiameterHalved;
            double halvedTopMinusHalvedBottomSquared = halvedTopMinusHalvedBottom * halvedTopMinusHalvedBottom;

            double heightSquared = Height * Height;
            double lengthSlantedSide = Math.Sqrt(halvedTopMinusHalvedBottomSquared + heightSquared);

            return Math.Round(lengthSlantedSide, 1); ;
        }

        private double CalculateLengthBottomDiameter()
        {
            double bottomDiameterTimesPi = BottomDiameter * Math.PI;
            return Math.Round(bottomDiameterTimesPi, 1);
        }

        private double CalculateLengthTopDiameter()
        {
            double topDiameterTimesPi = TopDiameter * Math.PI;
            return Math.Round(topDiameterTimesPi, 1);
        }

        private double CalculateSmallRadius()
        {
            double lengthInnerDiameterTimesLengthSlantedSide = LengthBottomDiameter * LengthSlantedSide;
            double lengthOuterDiameterMinusLengthInnerDiameter = LengthTopDiameter - LengthBottomDiameter;
            double divide = lengthInnerDiameterTimesLengthSlantedSide / lengthOuterDiameterMinusLengthInnerDiameter;
            return Math.Round(divide, 1);
        }

        private double CalculateLargeRadius()
        {
            double smallRadiusPlusLengthSlantedSide = SmallRadius + LengthSlantedSide;
            return Math.Round(smallRadiusPlusLengthSlantedSide, 1);
        }

        private double CalculateCornerDegrees()
        {
            double lengthInnerDiameterDividedBySmallRadius = LengthBottomDiameter / SmallRadius;
            double sumOutcome = (lengthInnerDiameterDividedBySmallRadius * 180) / Math.PI;
            return Math.Round(sumOutcome, 1);
        }
    }
}
