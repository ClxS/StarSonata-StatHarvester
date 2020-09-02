namespace StatHarvester.DAL.Models.Specs.Items
{
    using SpecAttributes;

    [ItemSpecType("Energy")]
    public class Energy : ItemSpec
    {
        [IntField]
        public int Resevoir { get; set; }

        [IntField]
        public int Regeneration { get; set; }

        [IntField]
        public int Visibility { get; set; }

        public string FuelItem { get; set; }

        [IntField]
        public int FuelAmount { get; set; }

        [IntField]
        public int FuelPeriod { get; set; }
    }
}