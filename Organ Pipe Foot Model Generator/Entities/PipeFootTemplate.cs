using ACadSharp;
using ACadSharp.Entities;
using ACadSharp.Tables;

namespace Organ_Pipe_Foot_Model_Generator.Entities
{
    public class PipeFootTemplate
    {
        public Line Bottomline { get; private set; }
        public Arc SmallArc { get; private set; }
        public Arc LargeArc { get; private set; }
        public Line Slantedline { get; private set; }
        public PipeFootMeasurements Measurements { get; private set; }
        private MText Key { get; set; }

        // TODO: Remove helper property?
        private Line HelperLine { get; set; }
        private Line LowerLabiaalLine { get; set; }
        private Line UpperLabiaalLine { get; set; }

        /// <summary>
        /// A representation for a CAD model for a pipe foot
        /// </summary>
        /// <param name="xOffsetFromOrigin">How far away from 0 on the X axis the model should be rendered</param>
        /// <param name="yOffsetFromOrigin">How far away from 0 on the Y axis the model should be rendered</param>
        /// <param name="topDiameter">The length of the top of the frustum. Labeled T in Images/frustum.png</param>
        /// <param name="bottomDiameter">The length of the bottom of the frustum. Labeled B in Images/frustum.png</param>
        /// <param name="height">The height of the frustum. Labeled H in Images/frustum.png</param>
        /// <param name="metalThickness">How thick the sheet metal for the pipe is, this has to be subtracted (twice) from the top- and bottomdiameter if the measurements are for the outerdiameter of the pipes</param>
        public PipeFootTemplate(double xOffsetFromOrigin, double yOffsetFromOrigin, double topDiameter, double bottomDiameter, double height, double metalThickness = 0, string key = "")
        {
            Measurements = new PipeFootMeasurements(topDiameter, bottomDiameter, height, metalThickness);
            
            double endAngleInRadians = Measurements.CornerInDegrees * (Math.PI / 180);

            DetermineBottomline(yOffsetFromOrigin, xOffsetFromOrigin);

            CSMath.XYZ centerpoint = new CSMath.XYZ(x: Math.Round(Bottomline.StartPoint.X - Measurements.SmallRadius, 1), y: yOffsetFromOrigin, z: 0);

            SmallArc = DetermineArc(Measurements.SmallRadius, endAngleInRadians, centerpoint);
            LargeArc = DetermineArc(Measurements.LargeRadius, endAngleInRadians, centerpoint);
            DetermineSlantedline(centerpoint);

            // Check if key should be added to the model
            if (!string.IsNullOrWhiteSpace(key))
            {
                DrawKey(key);
            }

            // TODO: Remove call to helper method?
            DetermineLabiaalCutouts(centerpoint);
        }

        /// <summary>
        /// Determine the Start and End coordinates for the bottomline of the CAD model. This line is parallel to the X axis.
        /// </summary>
        /// <param name="yOffsetFromOrigin">How far away from 0 on the Y axis the model should be rendered</param>
        /// <param name="xPositionForCenterpoint">The coordinates on the X axis for the centerpoint</param>
        private void DetermineBottomline(double yOffsetFromOrigin, double xOffsetFromOrigin)
        {
            double xStartPosition = xOffsetFromOrigin;
            double xEndPosition = xStartPosition + Measurements.LengthSlantedSide;

            Bottomline = new Line
            {
                StartPoint = new CSMath.XYZ(x: xStartPosition, y: yOffsetFromOrigin, z: 0),
                EndPoint = new CSMath.XYZ(xEndPosition, yOffsetFromOrigin, 0)
            };
        }

        /// <summary>
        /// Creates and Arc object that can be used in CAD models
        /// </summary>
        /// <param name="radius">The radius (straal in Dutch) for the arc</param>
        /// <param name="endAngleInRadians">The angle (in radians) at which the arc should stop. The arc automatically starts a 0</param>
        /// <param name="centerpoint">The point around which the arcs are drawn for the CAD model</param>
        /// <returns></returns>
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
        /// <param name="centerpoint">The point around which the arcs are drawn for the CAD model</param>
        private void DetermineSlantedline(CSMath.XYZ centerpoint)
        {
            double centerX = centerpoint.X;
            double centerY = centerpoint.Y;
            double angleInDegrees = Measurements.CornerInDegrees;
            double length = Measurements.LargeRadius;

            // Convert angle to radians
            double angleInRadians = angleInDegrees * (Math.PI / 180);

            // Calculate new start point
            double newStartX = centerX + Measurements.SmallRadius * Math.Cos(angleInRadians);
            double newStartY = centerY + Measurements.SmallRadius * Math.Sin(angleInRadians);
            double newStartXRounded = Math.Round(newStartX, 1);
            double newStartYRounded = Math.Round(newStartY, 1);

            // Calculate endpoint
            double endX = centerX + length * Math.Cos(angleInRadians);
            double endY = centerY + length * Math.Sin(angleInRadians);
            double endXRounded = Math.Round(endX, 1);
            double endYRounded = Math.Round(endY, 1);

            Slantedline = new Line
            {
                StartPoint = new CSMath.XYZ(x: newStartXRounded, y: newStartYRounded, z: 0),
                EndPoint = new CSMath.XYZ(endXRounded, endYRounded, 0)
            };
        }

        private void DrawKey(string key)
        {
            double xPosition = Bottomline.EndPoint.X;
            double lowestYPosition = Bottomline.EndPoint.Y;
            double totalHeight = Measurements.LengthTopDiameter;
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

            Key = new MText
            {
                Height = letterHeight,
                Value = key,
                InsertPoint = new CSMath.XYZ(letterX, letterY, 0)
            };
        }

        private void DetermineLabiaalCutouts(CSMath.XYZ centerpoint)
        {
            double centerX = centerpoint.X;
            double centerY = centerpoint.Y;
            double length = Measurements.LargeRadius;

            // Convert angle to radians
            double angleInRadians = (Measurements.CornerInDegrees / 2) * (Math.PI / 180);

            // Calculate new start point
            double centerLineStartX = Math.Round(centerX + Measurements.SmallRadius * Math.Cos(angleInRadians), 1);
            double centerLineStartY = Math.Round(centerY + Measurements.SmallRadius * Math.Sin(angleInRadians), 1);

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
            double offsetDistance = 20.0;

            // Calculate the new start and end points for LowerLabiaalLine
            double upperLabiaalLineEndX = centerLineEndX + perpDirX * offsetDistance;
            double upperLabiaalLineEndY = centerLineEndY + perpDirY * offsetDistance;

            // Calculate the start point for UpperLabiaalLine by moving back 3 mm along the direction of HelperLine
            double upperLabiaalLineStartX = upperLabiaalLineEndX - (3 * unitDirX);
            double upperLabiaalLineStartY = upperLabiaalLineEndY - (3 * unitDirY);

            // Create the UpperLabiaalLine
            UpperLabiaalLine = new Line
            {
                StartPoint = new CSMath.XYZ(x: Math.Round(upperLabiaalLineStartX, 1), y: Math.Round(upperLabiaalLineStartY, 1), z: 0),
                EndPoint = new CSMath.XYZ(x: Math.Round(upperLabiaalLineEndX, 1), y: Math.Round(upperLabiaalLineEndY, 1), z: 0) // New endpoint parallel to HelperLine
            };

            // Calculate the new start and end points for LowerLabiaalLine
            double lowerLabiaalLineEndX = centerLineEndX - perpDirX * offsetDistance;
            double lowerLabiaalLineEndY = centerLineEndY - perpDirY * offsetDistance;

            // Calculate the start point for UpperLabiaalLine by moving back 3 mm along the direction of HelperLine
            double lowerLabiaalLineStartX = lowerLabiaalLineEndX - (3 * unitDirX);
            double lowerLabiaalLineStartY = lowerLabiaalLineEndY - (3 * unitDirY);

            // Create the UpperLabiaalLine
            LowerLabiaalLine = new Line
            {
                StartPoint = new CSMath.XYZ(x: Math.Round(lowerLabiaalLineStartX, 1), y: Math.Round(lowerLabiaalLineStartY, 1), z: 0),
                EndPoint = new CSMath.XYZ(x: Math.Round(lowerLabiaalLineEndX, 1), y: Math.Round(lowerLabiaalLineEndY, 1), z: 0) // New endpoint parallel to HelperLine
            };
        }

        public double GetFurthestXPosition()
        {
            return Bottomline.EndPoint.X;
        }

        public void AddToCadDocument(CadDocument doc)
        {
            BlockRecord block = new BlockRecord(Guid.NewGuid().ToString());
            block.Entities.Add(Bottomline);
            block.Entities.Add(SmallArc);
            block.Entities.Add(LargeArc);
            block.Entities.Add(Slantedline);

            // Check if the key need to be added
            if(Key !=  null)
            {
                block.Entities.Add(Key);
            }

            block.Entities.Add(LowerLabiaalLine);
            block.Entities.Add(UpperLabiaalLine);

            Insert insert = new Insert(block);

            doc.Entities.Add(insert);
        }
    }
}
