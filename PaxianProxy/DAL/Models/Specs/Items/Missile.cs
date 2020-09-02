namespace StatHarvester.DAL.Models.Specs.Items
{
    using SpecAttributes;

    [ItemSpecType("Missile")]
    internal class Missile : ItemSpec
    {
        [IntField]
        public int LaunchEnergy { get; set; }

        [IntField]
        public int TargetLaunchRange { get; set; }

        [IntField]
        public int FlightTime { get; set; }
    }
}