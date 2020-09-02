namespace StatHarvester.DAL.Models.Specs.SpecAttributes
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    internal class DoubleFieldAttribute : Attribute, ISpecLoaderAttribute
    {
        private readonly string propName;

        public DoubleFieldAttribute(string propName = null)
        {
            this.propName = propName;
        }

        public void Load(Dictionary<string, object> structuredInfo, ItemSpec targetSpec, PropertyInfo property)
        {
            property.SetValue(targetSpec, double.Parse((string) structuredInfo[this.propName ?? property.Name]));
        }
    }
}