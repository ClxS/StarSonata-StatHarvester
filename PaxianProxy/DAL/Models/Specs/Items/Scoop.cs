namespace StatHarvester.DAL.Models.Specs.Items
{
    using SpecAttributes;

    [ItemSpecType("Scoop")]
    internal class Scoop : ItemSpec
    {
        [IntField]
        public int ScoopRange { get; set; }

        [IntField]
        public int ScoopRate { get; set; }

        [IntField]
        public int Electricity { get; set; }

        [IntField]
        public int VacuumRange { get; set; }
    }
}