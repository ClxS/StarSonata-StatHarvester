namespace StatHarvester.DAL.Models.Specs.Items
{
    using System.Collections.Generic;
    using Dapper.Contrib.Extensions;
    using SpecAttributes;

    [ItemSpecType("Drone", "Permanent Drone")]
    internal class Drone : ItemSpec
    {
        [IntField]
        public int Dps { get; set; }

        [IntField]
        public int Visibility { get; set; }

        public string SuperItem { get; set; }

        public string Weapon1 { get; set; }

        public string Weapon2 { get; set; }

        public string Weapon3 { get; set; }

        public string Weapon4 { get; set; }

        public string Weapon5 { get; set; }

        [IntField]
        public int Lifespan { get; set; }

        [IntField]
        public int ChargeTime { get; set; }

        [IntField]
        public int ChargingRate { get; set; }

        public bool IsPermanent { get; set; }

        [Write(false)]
        public ICollection<ResistStatSpec> Resists { get; set; }
    }
}