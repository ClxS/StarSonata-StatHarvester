namespace StatHarvester.DAL.Models.Specs.SpecAttributes
{
    using System;

    internal class ItemSpecTypeAttribute : Attribute
    {
        public ItemSpecTypeAttribute(params string[] types)
        {
            this.Types = types;
        }

        public string[] Types { get; }
    }
}