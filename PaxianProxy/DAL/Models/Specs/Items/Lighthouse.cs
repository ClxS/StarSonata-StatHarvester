namespace StatHarvester.DAL.Models.Specs.Items
{
    using SpecAttributes;

    [ItemSpecType("Lighthouse")]
    internal class Lighthouse : ItemSpec
    {
        [IntField]
        public int Electricity { get; set; }

        [IntField]
        public int Visiblity { get; set; }
    }
}