namespace StatHarvester.DAL.Models.Specs.Items
{
    using System.Collections.Generic;
    using Dapper.Contrib.Extensions;
    using SpecAttributes;

    [ItemSpecType("Launcher")]
    internal class Launcher : ItemSpec
    {
        [DoubleField]
        public double LaunchSpeed { get; set; }

        [IntField]
        public int ChargeTime { get; set; }

        [DoubleField]
        public double ChargingRate { get; set; }

        [Write(false)]
        public ICollection<LauncherStat> LauncherStats { get; set; }
    }
}