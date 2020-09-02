namespace StatHarvester.DAL.Models.Specs.Items
{
    using SpecAttributes;

    [ItemSpecType("Blueprint")]
    public class Blueprint : ItemSpec
    {
        public string BuildItem { get; set; }

        [LongField]
        public long Manhours { get; set; }

        [IntField]
        public int MaximumUsages { get; set; }
    }
}