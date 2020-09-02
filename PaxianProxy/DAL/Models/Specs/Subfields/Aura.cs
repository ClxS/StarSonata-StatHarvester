namespace StatHarvester.DAL.Models.Specs
{
    internal class Aura : ItemSpec
    {
        public int Stat { get; set; }

        public double Amount { get; set; }

        public bool IsFlat { get; set; }
    }
}