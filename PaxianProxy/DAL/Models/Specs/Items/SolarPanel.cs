namespace StatHarvester.DAL.Models.Specs.Items
{
    using SpecAttributes;

    [ItemSpecType("Solar Panel")]
    internal class SolarPanel : ItemSpec
    {
        [IntField]
        public int Charge { get; set; }
    }
}