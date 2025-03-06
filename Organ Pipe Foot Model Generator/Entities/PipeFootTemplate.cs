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
        //private Line LowerLabiumMarking { get; set; }
        //private Line UpperLabiumMarking { get; set; }

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
                Rectangle = new Rectangle(Bottomline, measurements.LengthTopDiameter);
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

        /// <summary>
        /// Determine the Start and End coordinates for both Labium markings
        /// </summary>
        private void DetermineLabiumMarkings(CSMath.XYZ centerpoint, double cornerInDegrees, double largeRadius, double smallRadius, double labiumWidth)
        {
            double centerX = centerpoint.X;
            double centerY = centerpoint.Y;
            double length = largeRadius;

            // Convert angle to radians
            double angleInRadians = (cornerInDegrees / 2) * (Math.PI / 180);

            // Calculate new start point
            double centerLineStartX = Math.Round(centerX + smallRadius * Math.Cos(angleInRadians), 1);
            double centerLineStartY = Math.Round(centerY + smallRadius * Math.Sin(angleInRadians), 1);

            // Calculate endpoint
            double centerLineEndX = Math.Round(centerX + length * Math.Cos(angleInRadians), 1);
            double centerLineEndY = Math.Round(centerY + length * Math.Sin(angleInRadians), 1);

            // LowerLabialLine
            // Calculate the direction vector of HelperLine
            double dirX = centerLineEndX - centerLineStartX;
            double dirY = centerLineEndY - centerLineStartY;

            // Normalize the direction vector using Pythagoras Theorum
            double lengthDir = Math.Sqrt(dirX * dirX + dirY * dirY);
            double unitDirX = dirX / lengthDir;
            double unitDirY = dirY / lengthDir;

            // Calculate the perpendicular direction vector (rotate by 90 degrees)
            double perpDirX = -unitDirY; // Rotate counter-clockwise
            double perpDirY = unitDirX;

            // Offset distance 
            double offsetDistance = Math.Round(labiumWidth / 2, 1);

            // Calculate the new start and end points for LowerLabiaalLine
            double upperLabiaalLineEndX = centerLineEndX + perpDirX * offsetDistance;
            double upperLabiaalLineEndY = centerLineEndY + perpDirY * offsetDistance;

            // Calculate the start point for UpperLabiaalLine by moving back 3 mm along the direction of HelperLine
            double upperLabiaalLineStartX = upperLabiaalLineEndX - (3 * unitDirX);
            double upperLabiaalLineStartY = upperLabiaalLineEndY - (3 * unitDirY);

            // Create the UpperLabiaalLine
            //UpperLabiumMarking = new Line
            //{
            //    StartPoint = new CSMath.XYZ(x: Math.Round(upperLabiaalLineStartX, 1), y: Math.Round(upperLabiaalLineStartY, 1), z: 0),
            //    EndPoint = new CSMath.XYZ(x: Math.Round(upperLabiaalLineEndX, 1), y: Math.Round(upperLabiaalLineEndY, 1), z: 0) // New endpoint parallel to HelperLine
            //};

            // Calculate the new start and end points for LowerLabiaalLine
            double lowerLabiaalLineEndX = centerLineEndX - perpDirX * offsetDistance;
            double lowerLabiaalLineEndY = centerLineEndY - perpDirY * offsetDistance;

            // Calculate the start point for UpperLabiaalLine by moving back 3 mm along the direction of HelperLine
            double lowerLabiaalLineStartX = lowerLabiaalLineEndX - (3 * unitDirX);
            double lowerLabiaalLineStartY = lowerLabiaalLineEndY - (3 * unitDirY);

            // Create the UpperLabiaalLine
            //LowerLabiumMarking = new Line
            //{
            //    StartPoint = new CSMath.XYZ(x: Math.Round(lowerLabiaalLineStartX, 1), y: Math.Round(lowerLabiaalLineStartY, 1), z: 0),
            //    EndPoint = new CSMath.XYZ(x: Math.Round(lowerLabiaalLineEndX, 1), y: Math.Round(lowerLabiaalLineEndY, 1), z: 0) // New endpoint parallel to HelperLine
            //};
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
            }
            else
            {
                block.Entities.Add(Rectangle.Topline);
                block.Entities.Add(Rectangle.Leftline);
                block.Entities.Add(Rectangle.Rightline);
            }

            // Check if the key need to be added
            if (Key != null)
            {
                block.Entities.Add(Key);
            }

            //// Check if labium markings need to be rendered
            //if(UpperLabiumMarking != null && LowerLabiumMarking != null)
            //{
            //    block.Entities.Add(LowerLabiumMarking);
            //    block.Entities.Add(UpperLabiumMarking);
            //}

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
