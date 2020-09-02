namespace StatHarvester.DAL.Models.Specs
{
    internal class ResistStatSpec : ItemSpec
    {
        public int Stat { get; set; }

        public double Amount { get; set; }

        public bool IsFlat { get; set; }

        public int Soak { get; set; }
    }
}