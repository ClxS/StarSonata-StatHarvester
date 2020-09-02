namespace StatHarvester.DAL.Models.Specs.Items
{
    using SpecAttributes;

    [ItemSpecType("Exterminator")]
    public class Exterminator : ItemSpec
    {
        [IntField]
        public int ActivationEnergy { get; set; }

        [IntField]
        public int Douses { get; set; }

        public bool Fire { get; set; }

        [IntField]
        public int Frequency { get; set; }

        [IntField]
        public int Dps { get; set; }

        [IntField]
        public int Dpe { get; set; }
    }
}