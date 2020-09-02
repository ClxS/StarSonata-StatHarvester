namespace StatHarvester.DAL.Models.Specs.Items
{
    using SpecAttributes;

    [ItemSpecType("Fighter")]
    internal class Fighter : ItemSpec
    {
        [IntField]
        public int Dps { get; set; }

        [IntField]
        public int LaunchEnergy { get; set; }

        [IntField]
        public int FlightTime { get; set; }
    }
}