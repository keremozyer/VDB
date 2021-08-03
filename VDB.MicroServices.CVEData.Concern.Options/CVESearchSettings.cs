namespace VDB.MicroServices.CVEData.Concern.Options
{
    public class CVESearchSettings
    {
        public string[] AllVersionsIndicators { get; set; }
        public string NodeOperatorAnd { get; set; }
        public string StandartVersionSeperator { get; set; }
        public string StandartVersionNumber { get; set; }
    }
}
