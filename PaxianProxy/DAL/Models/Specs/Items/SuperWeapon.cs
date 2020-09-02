namespace StatHarvester.DAL.Models.Specs.Items
{
    using System.Collections.Generic;
    using Dapper.Contrib.Extensions;
    using SpecAttributes;

    [ItemSpecType("Super Weapon")]
    internal class SuperWeapon : ItemSpec
    {
        [IntField]
        public int Damage { get; set; }

        [DoubleField]
        public double Dpe { get; set; }

        [IntField]
        public int Range { get; set; }

        [IntField]
        public int ChargingTime { get; set; }

        [DoubleField]
        public double ChargingRate { get; set; }

        [Write(false)]
        public ICollection<Action> Actions { get; set; }
    }
}