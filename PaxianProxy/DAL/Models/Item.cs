namespace StatHarvester.DAL.Models
{
    using System.Collections.Generic;
    using Dapper.Contrib.Extensions;
    using Specs;

    public class Item
    {
        [ExplicitKey]
        public ulong NameHash { get; set; }

        public string Name { get; set; }

        public int Tech { get; set; }

        public string Description { get; set; }

        public string Rarity { get; set; }

        public string Type { get; set; }

        public string StructuredDescription { get; set; }

        public long Weight { get; set; }

        public long Size { get; set; }

        [Write(false)]
        public ICollection<AugStat> AugMods { get; set; }

        [Write(false)]
        public ICollection<RequirementsSpec> Requirements { get; set; }
    }
}