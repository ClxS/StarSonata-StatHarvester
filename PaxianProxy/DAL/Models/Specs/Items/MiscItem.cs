namespace StatHarvester.DAL.Models.Specs.Items
{
    using SpecAttributes;

    [ItemSpecType("Item")]
    internal class MiscItem : ItemSpec
    {
        [IntField]
        public int ChargingRate { get; set; }

        [IntField]
        public int Electricity { get; set; }
    }
}