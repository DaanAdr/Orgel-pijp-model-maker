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

        public PipeFootMeasurements(double topDiameter, double bottomDiameter, double height)
        {
            TopDiameter = topDiameter;
            BottomDiameter = bottomDiameter;
            Height = height;
        }

        public double CalculateLengthSlantedSide()
        {
            double bottomDiameterHalved = BottomDiameter / 2;
            double topDiameterHalved = TopDiameter / 2;
            double halvedBottomMinusHalvedTop = bottomDiameterHalved - topDiameterHalved;
            double halvedBottomMinusHalvedTopSquared = halvedBottomMinusHalvedTop * halvedBottomMinusHalvedTop;
            double heightSquared = Height * Height;
            double lengthSlantedSide = Math.Sqrt(halvedBottomMinusHalvedTopSquared + heightSquared);

            return lengthSlantedSide;
        }

        public void SetCalculatedMeasurements()
        {
            this.LengthSlantedSide = CalculateLengthSlantedSide();
            this.LengthInnerDiameter = CalculateLengthInnerDiameter();
            this.LengthOuterDiameter = CalculateLengthOuterDiameter();
            this.SmallRadius = CalculateSmallRadius();
            this.LargeRadius = CalculateLargeRadius();
        }

        public double CalculateLengthInnerDiameter()
        {
            return TopDiameter * Math.PI;
        }

        public double CalculateLengthOuterDiameter()
        {
            return BottomDiameter * Math.PI;
        }

        public double CalculateSmallRadius()
        {
            double lengthInnerDiameterTimesLengthSlantedSide = LengthInnerDiameter * LengthSlantedSide;
            double lengthOuterDiameterMinusLengthInnerDiameter = LengthOuterDiameter - LengthInnerDiameter;

            return lengthInnerDiameterTimesLengthSlantedSide / lengthOuterDiameterMinusLengthInnerDiameter;
        }

        public double CalculateLargeRadius()
        {
            return SmallRadius + LengthSlantedSide;
        }
    }
}
