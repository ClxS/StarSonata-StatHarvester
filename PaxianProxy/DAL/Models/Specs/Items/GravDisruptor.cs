namespace StatHarvester.DAL.Models.Specs.Items
{
    using SpecAttributes;

    [ItemSpecType("Gravometric Disruptor")]
    internal class GravDisruptor : ItemSpec
    {
        [IntField]
        public int Duration { get; set; }

        [IntField]
        public int ChargeTime { get; set; }

        [IntField]
        public int ChargingRate { get; set; }
    }
}