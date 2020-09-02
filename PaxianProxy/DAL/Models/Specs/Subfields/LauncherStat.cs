namespace StatHarvester.DAL.Models.Specs
{
    public class LauncherStat : ItemSpec
    {
        public int Stat { get; set; }

        public double Amount { get; set; }

        public bool IsFlat { get; set; }
    }
}