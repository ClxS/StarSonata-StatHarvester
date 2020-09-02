namespace StatHarvester.Extensions.Autofac
{
    using global::Autofac;
    using StarSonata.Api;

    public static class BuilderExtensions
    {
        public static void AddStarSonata(this ContainerBuilder builder)
        {
            builder.RegisterType<StarSonataClient>().SingleInstance();
        }
    }
}