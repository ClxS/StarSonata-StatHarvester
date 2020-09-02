namespace StatHarvester.DAL.Models.Specs.Items
{
    using SpecAttributes;

    [ItemSpecType("Scanner")]
    internal class Scanner : ItemSpec
    {
        [IntField]
        public int Range { get; set; }

        [IntField]
        public int Power { get; set; }

        [DoubleField]
        public double ChargeTime { get; set; }

        [DoubleField]
        public double ChargingRate { get; set; }

        [IntField]
        public int Electricity { get; set; }
    }
}