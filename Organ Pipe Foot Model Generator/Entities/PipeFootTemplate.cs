using ACadSharp;
using ACadSharp.Entities;
using ACadSharp.Tables;

namespace Organ_Pipe_Foot_Model_Generator.Entities
{
    public class PipeFootTemplate
    {
        private Frustum Frustum { get; set; }
        private Rectangle Rectangle { get; set; }


        public Line Bottomline { get; private set; }
        
        private MText Key { get; set; }

        /// <summary>
        /// A representation for a CAD model for a pipe foot
        /// </summary>
        /// <param name="xOffsetFromOrigin">How far away from 0 on the X axis the model should be rendered</param>
        /// <param name="yOffsetFromOrigin">How far away from 0 on the Y axis the model should be rendered</param>
        /// <param name="measurements">The PipeFootMeasurements that can be used to render the model</param>
        /// <param name="key">The musical key the pipe is intended to play. This wil be written in the lower right corner of the model</param>
        public PipeFootTemplate(double xOffsetFromOrigin, double yOffsetFromOrigin, PipeFootMeasurements measurements, string key = "")
        {
            // Determine bottom line, as this is present in both frustum and rectangle modes
            Bottomline = DetermineBottomline(yOffsetFromOrigin, xOffsetFromOrigin, measurements.LengthSlantedSide);

            // Check if Top- and BottomDiameter are the same length, if not create a frustum
            if(measurements.LengthTopDiameter != measurements.LengthBottomDiameter)
            {
                Frustum = new Frustum(measurements, Bottomline);
            }
            else
            {
                Rectangle = new Rectangle(Bottomline, measurements.LengthTopDiameter, measurements.LabiumWidth);
            }

            // Check if key should be added to the model
            if (!string.IsNullOrWhiteSpace(key))
            {
                Key = DrawKey(key, measurements.LengthTopDiameter);
            }

            //// Check if markings for labiaum cutouts need to be rendered
            //if (measurements.LabiumWidth > 0.0)
            //{
            //    DetermineLabiumMarkings(centerpoint, measurements.CornerInDegrees, measurements.LargeRadius, measurements.SmallRadius, measurements.LabiumWidth);
            //}
        }

        /// <summary>
        /// Determine the Start and End coordinates for the bottomline of the CAD model. This line is parallel to the X axis.
        /// </summary>
        private Line DetermineBottomline(double yOffsetFromOrigin, double xOffsetFromOrigin, double lengthSlantedSide)
        {
            double xStartPosition = xOffsetFromOrigin;
            double xEndPosition = xStartPosition + lengthSlantedSide;

            return new Line
            {
                StartPoint = new CSMath.XYZ(x: xStartPosition, y: yOffsetFromOrigin, z: 0),
                EndPoint = new CSMath.XYZ(xEndPosition, yOffsetFromOrigin, 0)
            };
        }

        /// <summary>
        /// Determine where the key should be drawn on the model
        /// </summary>
        private MText DrawKey(string key, double lengthTopDiameter)
        {
            double xPosition = Bottomline.EndPoint.X;
            double lowestYPosition = Bottomline.EndPoint.Y;
            double totalHeight = lengthTopDiameter;
            double letterHeight;
            double letterY = lowestYPosition + 1;

            // Lock letter height to 1 cm if the total height 
            if(totalHeight >= 17)
            {
                letterHeight = 15;
            }
            else
            {
                letterHeight = totalHeight - 2;
            }

            // Estimate text width (average width per character can vary)
            double averageCharacterWidth = letterHeight * 0.25; // Adjust this factor as needed
            double estimatedWidth = averageCharacterWidth * key.Length;
            double letterX = Math.Round(xPosition - (estimatedWidth + 1), 1);

            return new MText
            {
                Height = letterHeight,
                Value = key,
                InsertPoint = new CSMath.XYZ(letterX, letterY, 0)
            };
        }

        public double GetFurthestXPosition()
        {
            return Bottomline.EndPoint.X;
        }

        public double GetHighestYPosition()
        {
            // Determine what models needs to be draw
            if (Rectangle != null)
            {
                return Rectangle.Topline.StartPoint.Y;
            }

            return Frustum.Slantedline.EndPoint.Y;
        }

        public void AddToCadDocument(CadDocument doc)
        {
            BlockRecord block = new BlockRecord(Guid.NewGuid().ToString());
            block.Entities.Add(Bottomline);

            // Determine what models needs to be draw
            if (Frustum != null)
            {
                block.Entities.Add(Frustum.SmallArc);
                block.Entities.Add(Frustum.LargeArc);
                block.Entities.Add(Frustum.Slantedline);

                // Check if labium markings need to be rendered
                if (Frustum.UpperLabiumMarking != null && Frustum.LowerLabiumMarking != null)
                {
                    block.Entities.Add(Frustum.UpperLabiumMarking);
                    block.Entities.Add(Frustum.LowerLabiumMarking);
                }
            }
            else
            {
                block.Entities.Add(Rectangle.Topline);
                block.Entities.Add(Rectangle.Leftline);
                block.Entities.Add(Rectangle.Rightline);

                // Check if labium markings need to be rendered
                if (Rectangle.UpperLabiumMarking != null && Rectangle.LowerLabiumMarking != null)
                {
                    block.Entities.Add(Rectangle.UpperLabiumMarking);
                    block.Entities.Add(Rectangle.LowerLabiumMarking);
                }
            }

            // Check if the key need to be added
            if (Key != null)
            {
                block.Entities.Add(Key);
            }

            Insert insert = new Insert(block);

            doc.Entities.Add(insert);
        }

        public object GetModelInformationForTesting()
        {
            if (Rectangle != null) return Rectangle;

            return Frustum;
        }
    }
}
