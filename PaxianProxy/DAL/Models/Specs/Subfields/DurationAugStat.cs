namespace StatHarvester.DAL.Models.Specs
{
    internal class DurationAugStat : ItemSpec
    {
        public int Stat { get; set; }

        public double Amount { get; set; }

        public bool IsFlat { get; set; }

        public double Duration { get; set; }
    }
}