namespace StatHarvester.DAL.Models.Specs.Items
{
    using SpecAttributes;

    [ItemSpecType("Weapon")]
    internal class Weapon : ItemSpec
    {
        [IntField]
        public int MinDamage { get; set; }

        [IntField]
        public int MaxDamage { get; set; }

        [IntField]
        public int Pellets { get; set; }

        [DoubleField]
        public double Dps { get; set; }

        [DoubleField]
        public double Dpe { get; set; }

        [DoubleField]
        public double Recoil { get; set; }

        [IntField]
        public int Range { get; set; }

        [IntField]
        public int Speed { get; set; }

        [IntField]
        public int DamageType { get; set; }

        [IntField]
        public int Electricity { get; set; }

        [IntField]
        public int FiringVisibility { get; set; }

        [IntField]
        public int FiringVisibilityDuration { get; set; }

        [IntField]
        public int MaxSpeed { get; set; }

        [IntField]
        public int Turning { get; set; }

        public bool DegradingTurn { get; set; }

        [IntField]
        public int Tracking { get; set; }

        [DoubleField]
        public double EngineDelay { get; set; }

        [DoubleField]
        public double BurnTime { get; set; }
    }
}