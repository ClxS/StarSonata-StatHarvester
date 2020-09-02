namespace StatHarvester.DAL.Models.Specs.Items
{
    using System.Collections.Generic;
    using Dapper.Contrib.Extensions;
    using SpecAttributes;

    [ItemSpecType("Super Item", "Super Drone Item")]
    internal class SuperItem : ItemSpec
    {
        [IntField]
        public int ChargeTime { get; set; }

        [DoubleField]
        public double ChargingRate { get; set; }

        [Write(false)]
        public ICollection<Action> Actions { get; set; }
    }
}