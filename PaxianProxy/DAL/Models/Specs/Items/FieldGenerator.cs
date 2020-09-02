namespace StatHarvester.DAL.Models.Specs.Items
{
    using System.Collections.Generic;
    using Dapper.Contrib.Extensions;
    using SpecAttributes;

    [ItemSpecType("Field Generator")]
    internal class FieldGenerator : ItemSpec
    {
        [Write(false)]
        public ICollection<Aura> FieldsGenerated { get; set; }

        [IntField]
        public int Electricity { get; set; }

        [IntField]
        public int Visibility { get; set; }
    }
}