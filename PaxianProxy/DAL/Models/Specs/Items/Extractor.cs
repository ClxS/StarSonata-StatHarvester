namespace StatHarvester.DAL.Models.Specs.Items
{
    using SpecAttributes;

    [ItemSpecType("Extractor")]
    public class Extractor : ItemSpec
    {
        public string ExtractedItem { get; set; }

        [DoubleField]
        public double Rate { get; set; }
    }
}