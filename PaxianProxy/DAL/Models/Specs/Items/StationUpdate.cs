namespace StatHarvester.DAL.Models.Specs.Items
{
    using SpecAttributes;

    [ItemSpecType("Station Upgrade")]
    internal class StationUpdate : ItemSpec
    {
        [IntField]
        public int UpgradeTech { get; set; }

        [IntField]
        public int MinimumTech { get; set; }
    }
}