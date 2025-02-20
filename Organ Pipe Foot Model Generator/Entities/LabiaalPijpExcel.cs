using CsvHelper.Configuration.Attributes;


namespace Organ_Pipe_Foot_Model_Generator.Entities
{
    public class LabiaalPijpExcel
    {
        [Name("toets")]
        public required string Key { get; set; }
        //public string corpl { get; set; }

        [Name("uitw diam")]
        public required double TopDiameter { get; set; }
        //public string labbr { get; set; }
        //public string kernd { get; set; }
        //public string kernf { get; set; }
        //[Name("pl.brdt")]
        //public string PlaatBreedte { get; set; }
        //[Name("lab %")]
        //public string LabPercentage { get; set; }
        //public string opsn { get; set; }
        //public string voetg { get; set; }
        [Name("vt.lngt")]
        public required double Height { get; set; }
        [Name("pl.br.vt")]
        public required double PlateWidthFoot { get; set; }
        [Name("wd.vt")]
        public required double MetalThickness { get; set; }
        //[Name("wd.bo")]
        //public string WijdteBoven { get; set; }
        //[Name("wd.on")]
        //public string WijdteOnder { get; set; }
        //[Name("wel.lngt")]
        //public string WelLengte { get; set; }
    }   
}
