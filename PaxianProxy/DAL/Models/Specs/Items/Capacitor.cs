namespace StatHarvester.DAL.Models.Specs.Items
{
    using System.Collections.Generic;
    using SpecAttributes;

    [ItemSpecType("Capacitor")]
    internal class Capacitor : ItemSpec
    {
        [IntField]
        public int Visibility { get; set; }

        public static Capacitor CreateFromStructuredInfo(Dictionary<string, object> structuredInfo)
        {
            var bp = new Capacitor
            {
                Visibility = int.Parse((string) structuredInfo["Visibility"])
            };

            return bp;
        }
    }
}