namespace StatHarvester.DAL.Models.Specs.Items
{
    using SpecAttributes;

    [ItemSpecType("Escape Pod")]
    public class EscapePod : ItemSpec
    {
        [IntField]
        public int MinSpeed { get; set; }

        [IntField]
        public int MaxSpeed { get; set; }

        [IntField]
        public int Shields { get; set; }

        [IntField]
        public int Turning { get; set; }
    }
}