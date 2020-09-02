namespace StatHarvester.DAL.Models.Specs.Items
{
    using System.Collections.Generic;
    using Dapper.Contrib.Extensions;
    using SpecAttributes;

    [ItemSpecType("Overloader")]
    internal class Overloader : ItemSpec
    {
        [IntField]
        public int Electricity { get; set; }

        [DoubleField]
        public double FailureChance { get; set; }

        [IntField]
        public int ChargeTime { get; set; }

        [DoubleField]
        public double ChargingRate { get; set; }

        [Write(false)]
        public ICollection<DurationAugStat> WeaponEffects { get; set; }
    }
}