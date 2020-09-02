namespace StatHarvester.DAL.Models.Specs.Items
{
    using System.Collections.Generic;
    using Dapper.Contrib.Extensions;
    using SpecAttributes;

    [ItemSpecType("Ship")]
    internal class Ship : ItemSpec
    {
        [IntField]
        public int HullSpace { get; set; }

        [IntField]
        public int MaxSpeed { get; set; }

        [IntField]
        public int Augmenters { get; set; }

        [IntField]
        public int Weapons { get; set; }

        [IntField]
        public int EnergyGeneration { get; set; }

        [IntField]
        public int Visibility { get; set; }

        [IntField]
        public int Reflectivity { get; set; }

        [IntField]
        public int WeightReduction { get; set; }

        [Write(false)]
        public ICollection<BuiltInItem> BuildInItems { get; set; }

        [Write(false)]
        public ICollection<ResistStatSpec> Resists { get; set; }
    }
}