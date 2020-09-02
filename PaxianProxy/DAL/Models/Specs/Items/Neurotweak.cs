namespace StatHarvester.DAL.Models.Specs.Items
{
    using System.Collections.Generic;
    using Dapper.Contrib.Extensions;
    using SpecAttributes;

    [ItemSpecType("Neurotweak")]
    internal class Neurotweak : ItemSpec
    {
        [IntField]
        public int Electricity { get; set; }

        [IntField]
        public int ChargeTime { get; set; }

        [DoubleField]
        public double ChargingRate { get; set; }

        [Write(false)]
        public ICollection<DurationAugStat> Tweaks { get; set; }
    }
}