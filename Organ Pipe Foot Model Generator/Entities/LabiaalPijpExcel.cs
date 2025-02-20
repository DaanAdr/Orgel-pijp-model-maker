using CsvHelper.Configuration.Attributes;


namespace Organ_Pipe_Foot_Model_Generator.Entities
{
    /// <summary>
    /// This class it to be used when reading Excel files for Labiaal pijpen
    /// </summary>
    public class LabiaalPijpExcel
    {
        [Name("toets")]
        public required string Key { get; set; }

        [Name("uitw diam")]
        public required double TopDiameter { get; set; }
        [Name("vt.lngt")]
        public required double Height { get; set; }
        [Name("pl.br.vt")]
        public required double PlateWidthFoot { get; set; }
        [Name("wd.vt")]
        public required double MetalThickness { get; set; }
    }   
}
