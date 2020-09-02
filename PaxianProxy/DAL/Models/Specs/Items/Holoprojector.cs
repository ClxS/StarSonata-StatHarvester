namespace StatHarvester.DAL.Models.Specs.Items
{
    using SpecAttributes;

    [ItemSpecType("Holoprojector")]
    internal class Holoprojector : ItemSpec
    {
        [IntField]
        public int Duration { get; set; }
    }
}