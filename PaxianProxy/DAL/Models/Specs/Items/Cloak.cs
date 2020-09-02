namespace StatHarvester.DAL.Models.Specs.Items
{
    using System.Collections.Generic;
    using SpecAttributes;

    [ItemSpecType("Cloaking Device")]
    internal class Cloak : ItemSpec
    {
        [DoubleField]
        public double DetectionCloaking { get; set; }

        [DoubleField]
        public double VisibilityCloaking { get; set; }

        [IntField]
        public int Electricity { get; set; }

        public static Cloak CreateFromStructuredInfo(Dictionary<string, object> structuredInfo)
        {
            var bp = new Cloak
            {
                Electricity = int.Parse((string) structuredInfo["Electricity"]),
                VisibilityCloaking = double.Parse((string) structuredInfo["VisibilityCloaking"]),
                DetectionCloaking = double.Parse((string) structuredInfo["DetectionCloaking"])
            };

            return bp;
        }
    }
}