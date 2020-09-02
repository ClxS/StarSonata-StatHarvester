namespace StatHarvester.DAL.Models.Specs.SpecAttributes
{
    using System.Collections.Generic;
    using System.Reflection;

    public interface ISpecLoaderAttribute
    {
        void Load(Dictionary<string, object> structuredInfo, ItemSpec targetSpec, PropertyInfo property);
    }
}