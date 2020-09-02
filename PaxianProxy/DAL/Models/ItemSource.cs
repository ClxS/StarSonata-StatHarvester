namespace StatHarvester.DAL.Models
{
    public class ItemSource
    {
        public int Id { get; set; }

        public ulong ItemId { get; set; }

        public ulong OriginItemId { get; set; }
    }
}