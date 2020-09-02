namespace StatHarvester.DAL.Models.Specs.Items
{
    using System.Collections.Generic;
    using Dapper.Contrib.Extensions;
    using SpecAttributes;

    [ItemSpecType("Diffuser")]
    internal class Diffuser : ItemSpec
    {
        [IntField]
        public int Electricity { get; set; }

        [DoubleField]
        public double FailureChance { get; set; }

        [IntField]
        public int Visibility { get; set; }

        [Write(false)]
        public ICollection<ResistStatSpec> Resists { get; set; }

        public static Diffuser CreateFromStructuredInfo(Dictionary<string, object> structuredInfo)
        {
            var bp = new Diffuser
            {
                //Electricity = (string) structuredInfo["Electricity"],
                FailureChance = long.Parse((string) structuredInfo["FailureChance"]),
                Visibility = int.Parse((string) structuredInfo["Visibility"])
            };

            return bp;
        }
    }
}