namespace StatHarvester.DAL.Models.Specs.Items
{
    using SpecAttributes;

    [ItemSpecType("Warp Device")]
    internal class WarpDevice : ItemSpec
    {
        [IntField]
        public int MaxDistance { get; set; }

        [DoubleField]
        public double FailureRate { get; set; }

        [IntField]
        public int ChargeTime { get; set; }

        [DoubleField]
        public double ChargingRate { get; set; }
    }
}