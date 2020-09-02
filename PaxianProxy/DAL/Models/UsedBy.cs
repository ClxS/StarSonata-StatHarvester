namespace StatHarvester.DAL.Models
{
    public class UsedBy
    {
        public int Id { get; set; }

        public ulong ItemId { get; set; }

        public ulong UsingItemId { get; set; }
    }
}