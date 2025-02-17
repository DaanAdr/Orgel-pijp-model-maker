namespace Organ_Pipe_Foot_Model_Generator.Entities
{
    public class PipeFootMeasurements
    {
        public double TopDiameter { get; }
        public double BottomDiameter { get; }
        public double Height { get; }
        public double LengthSlantedSide { get; private set; }
        public double LengthInnerDiameter { get; private set; }
        public double LengthOuterDiameter { get; private set; }
        public double SmallRadius { get; private set; }
        public double LargeRadius { get; private set; }
        public double CornerInDegrees { get; private set; }

        public PipeFootMeasurements(double topDiameter, double bottomDiameter, double height)
        {
            TopDiameter = Math.Round(topDiameter, 1);
            BottomDiameter = Math.Round(bottomDiameter, 1);
            Height = Math.Round(height, 1);

            LengthSlantedSide = CalculateLengthSlantedSide();
            LengthInnerDiameter = CalculateLengthInnerDiameter();
            LengthOuterDiameter = CalculateLengthOuterDiameter();
            SmallRadius = CalculateSmallRadius();
            LargeRadius = CalculateLargeRadius();
            CornerInDegrees = CalculateCornerDegrees();
        }

        private double CalculateLengthSlantedSide()
        {
            double bottomDiameterHalved = BottomDiameter / 2;
            double topDiameterHalved = TopDiameter / 2;
            double halvedBottomMinusHalvedTop = bottomDiameterHalved - topDiameterHalved;
            double halvedBottomMinusHalvedTopSquared = halvedBottomMinusHalvedTop * halvedBottomMinusHalvedTop;
            double heightSquared = Height * Height;
            double lengthSlantedSide = Math.Sqrt(halvedBottomMinusHalvedTopSquared + heightSquared);

            return Math.Round(lengthSlantedSide, 1); ;
        }

        private double CalculateLengthInnerDiameter()
        {
            double topDiameterTimesPi = TopDiameter * Math.PI;
            return Math.Round(topDiameterTimesPi, 1);
        }

        private double CalculateLengthOuterDiameter()
        {
            double bottomDiameterTimesPi = BottomDiameter * Math.PI;
            return Math.Round(bottomDiameterTimesPi, 1);
        }

        private double CalculateSmallRadius()
        {
            double lengthInnerDiameterTimesLengthSlantedSide = LengthInnerDiameter * LengthSlantedSide;
            double lengthOuterDiameterMinusLengthInnerDiameter = LengthOuterDiameter - LengthInnerDiameter;
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
            double lengthInnerDiameterDividedBySmallRadius = LengthInnerDiameter / SmallRadius;
            double sumOutcome = (lengthInnerDiameterDividedBySmallRadius * 180) / Math.PI;
            return Math.Round(sumOutcome, 1);
        }
    }
}
