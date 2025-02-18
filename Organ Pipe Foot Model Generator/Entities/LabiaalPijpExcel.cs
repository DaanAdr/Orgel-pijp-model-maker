using CsvHelper.Configuration.Attributes;


namespace Organ_Pipe_Foot_Model_Generator.Entities
{
    public class LabiaalPijpExcel
    {
        public string toets { get; set; }
        public string corpl { get; set; }

        [Name("uitw diam")]
        public string UitwendigeDiameter { get; set; }
        public string labbr { get; set; }
        public string kernd { get; set; }
        public string kernf { get; set; }
        [Name("pl.brdt")]
        public string PlaatBreedte { get; set; }
        [Name("lab %")]
        public string LabPercentage { get; set; }
        public string opsn { get; set; }
        public string voetg { get; set; }
        [Name("vt.lngt")]
        public string VoetLengte { get; set; }
        [Name("pl.br.vt")]
        public string PlaatBreedteVoet { get; set; }
        [Name("wd.vt")]
        public string WijdteVoet { get; set; }
        [Name("wd.bo")]
        public string WijdteBoven { get; set; }
        [Name("wd.on")]
        public string WijdteOnder { get; set; }
        [Name("wel.lngt")]
        public string WelLengte { get; set; }
    }   
}
