namespace StatHarvester.DAL.Models.Specs.Items
{
    using SpecAttributes;

    [ItemSpecType("Radar")]
    internal class Radar : ItemSpec
    {
        [DoubleField]
        public double Vision { get; set; }

        [IntField]
        public int Detection { get; set; }

        [IntField]
        public int Electricity { get; set; }

        [IntField]
        public int Visibility { get; set; }

        [DoubleField]
        public double ChargeTime { get; set; }

        [DoubleField]
        public double ChargingRate { get; set; }

        [IntField]
        public int PingBonus { get; set; }

        [IntField]
        public int PingTime { get; set; }

        [IntField]
        public int PingVisibility { get; set; }
    }
}