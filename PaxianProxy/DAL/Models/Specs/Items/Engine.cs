namespace StatHarvester.DAL.Models.Specs.Items
{
    using SpecAttributes;

    [ItemSpecType("Engine")]
    public class Engine : ItemSpec
    {
        [IntField]
        public int Thrust { get; set; }

        [IntField]
        public int Turning { get; set; }

        [IntField]
        public int Visibility { get; set; }
    }
}