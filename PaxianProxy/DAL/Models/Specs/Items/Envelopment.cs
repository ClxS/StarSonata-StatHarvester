namespace StatHarvester.DAL.Models.Specs.Items
{
    using SpecAttributes;

    [ItemSpecType("Envelopment")]
    public class Envelopment : ItemSpec
    {
        [IntField]
        public int Protection { get; set; }

        [IntField]
        public int Duration { get; set; }

        [IntField]
        public int Cooldown { get; set; }

        [DoubleField]
        public double ChargeTime { get; set; }

        [DoubleField]
        public double ChargingRate { get; set; }
    }
}