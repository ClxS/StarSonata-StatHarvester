namespace StatHarvester.DAL.Models.Specs.Items
{
    using SpecAttributes;

    [ItemSpecType("Shield")]
    internal class Shield : ItemSpec
    {
        [IntField]
        public int Protection { get; set; }

        [IntField]
        public int RegenElectricity { get; set; }

        [IntField]
        public int Electricity { get; set; }

        [DoubleField]
        public double Regen { get; set; }

        [IntField]
        public int Visiblity { get; set; }
    }
}