namespace StatHarvester.DAL.Models.Specs.Items
{
    using SpecAttributes;

    [ItemSpecType("Station Extension")]
    internal class StationExtension : ItemSpec
    {
        [IntField]
        public int ExtraSpace { get; set; }
    }
}