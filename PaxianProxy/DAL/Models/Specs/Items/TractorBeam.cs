namespace StatHarvester.DAL.Models.Specs.Items
{
    using SpecAttributes;

    [ItemSpecType("Tractor Beam")]
    internal class TractorBeam : ItemSpec
    {
        [IntField]
        public int Strength { get; set; }

        [IntField]
        public int DensityEnhancement { get; set; }

        [IntField]
        public int Range { get; set; }

        [IntField]
        public int RestingLength { get; set; }

        [IntField]
        public int Visibility { get; set; }

        [IntField]
        public int Electricity { get; set; }

        public string SideEffect { get; set; }

        [IntField]
        public int ConstantEnergy { get; set; }
    }
}