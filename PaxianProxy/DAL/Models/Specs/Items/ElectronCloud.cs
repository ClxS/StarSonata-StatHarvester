namespace StatHarvester.DAL.Models.Specs.Items
{
    using SpecAttributes;

    [ItemSpecType("Electron Cloud")]
    internal class ElectronCloud : ItemSpec
    {
        [IntField]
        public int Coverage { get; set; }

        [IntField]
        public int Duration { get; set; }

        [IntField]
        public int Illumination { get; set; }

        [IntField]
        public int Cooldown { get; set; }

        [IntField]
        public int Visibility { get; set; }
    }
}