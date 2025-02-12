using ACadSharp;
using ACadSharp.Entities;
using ACadSharp.IO;
using ACadSharp.Tables;
using Line = ACadSharp.Entities.Line;


namespace Organ_Pipe_Foot_Model_Generator.Logic
{
    public class btnCreateSquareLogic
    {
        public void CreateSquareModel(int length, string filePath)
        {
            //Create a block record to use as a reference
            // a block record refers to a collection of objects that are grouped together to form a single object, known as a "block."
            BlockRecord record = new BlockRecord("my_block");

            int distanceFromOrigin = 100;
            int totalLength = distanceFromOrigin + length;

            Line bottomLine = new Line
            {
                StartPoint = new CSMath.XYZ(x: distanceFromOrigin, y: distanceFromOrigin, z: 0),
                EndPoint = new CSMath.XYZ(totalLength, distanceFromOrigin, 0)
            };

            Line topLine = new Line
            {
                StartPoint = new CSMath.XYZ(x: distanceFromOrigin, y: totalLength, z: 0),
                EndPoint = new CSMath.XYZ(totalLength, totalLength, 0)
            };

            Line leftLine = new Line
            {
                StartPoint = new CSMath.XYZ(x: distanceFromOrigin, y: distanceFromOrigin, z: 0),
                EndPoint = new CSMath.XYZ(distanceFromOrigin, totalLength, 0)
            };

            Line rightLine = new Line
            {
                StartPoint = new CSMath.XYZ(x: totalLength, y: distanceFromOrigin, z: 0),
                EndPoint = new CSMath.XYZ(totalLength, totalLength, 0)
            };

            // Add lines to blockrecord
            record.Entities.Add(bottomLine);
            record.Entities.Add(topLine);
            record.Entities.Add(leftLine);
            record.Entities.Add(rightLine);

            //Create an insert referencing the block record
            Insert insert = new Insert(record);

            //Add the insert into a document
            CadDocument doc = new CadDocument();

            //Add lines directly to the document
            //doc.Entities.Add(bottomLine);
            //doc.Entities.Add(topLine);
            //doc.Entities.Add(leftLine);
            //doc.Entities.Add(rightLine);

            //Add the insert into the model
            doc.Entities.Add(insert);

            // Save the document using DxfWriter

            using (DxfWriter writer = new DxfWriter(filePath, doc, false))
            {
                writer.Write();
            }
        }
    }
}
