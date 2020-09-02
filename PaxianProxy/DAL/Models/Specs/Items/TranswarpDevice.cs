namespace StatHarvester.DAL.Models.Specs.Items
{
    using SpecAttributes;

    [ItemSpecType("Transwarp Device")]
    internal class TranswarpDevice : ItemSpec
    {
        [IntField]
        public int Range { get; set; }

        [IntField]
        public int ChargeTime { get; set; }

        [DoubleField]
        public double ChargingRate { get; set; }
    }
}