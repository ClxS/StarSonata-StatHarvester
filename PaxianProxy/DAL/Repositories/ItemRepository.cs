namespace StatHarvester.DAL.Repositories
{
    using System.Data.Common;
    using System.Threading.Tasks;
    using Dapper.Contrib.Extensions;
    using Models;
    using Serilog;

    public class ItemRepository
    {
        public static async Task AddOrUpdateItemAsync(Item item)
        {
            await using var db = await StatConnection.Create();
            await db.OpenAsync();
            await using var transaction = await db.BeginTransactionAsync();
            var nameHash = CalculateNameHash(item.Name);
            await AddOrUpdateStatFieldAsync(item, nameHash, transaction);

            item.NameHash = nameHash;
            var existingItem = await db.GetAsync<Item>(item.NameHash, transaction);
            if (existingItem != null)
            {
                Log.Debug("Updating {Item} in database", item.Name);
                existingItem.Tech = item.Tech;
                existingItem.Description = item.Description;
                existingItem.Rarity = item.Rarity;
                existingItem.Type = item.Type;
                existingItem.StructuredDescription = item.StructuredDescription;
                existingItem.Weight = item.Weight;
                existingItem.Size = item.Size;
                await db.UpdateAsync(existingItem, transaction);
            }
            else
            {
                Log.Debug("Entering {Item} to database", item.Name);
                await db.InsertAsync(item, transaction);
            }
        }

        private static async Task AddOrUpdateStatFieldAsync(Item item, ulong nameHash, DbTransaction transaction)
        {
            await Task.CompletedTask;
        }

        private static ulong CalculateNameHash(string itemName)
        {
            // FNV-1 64 bit
            var result = 0xcbf29ce484222325;

            foreach (var c in itemName)
            {
                result *= 1099511628211;
                result ^= c;
            }

            return result;
        }
    }
}