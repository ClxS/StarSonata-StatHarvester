namespace StatHarvester.DAL.Models.Specs.Items
{
    using SpecAttributes;

    [ItemSpecType("Shield Charger")]
    internal class ShieldCharger : ItemSpec
    {
        [IntField]
        public int Electricity { get; set; }
    }
}