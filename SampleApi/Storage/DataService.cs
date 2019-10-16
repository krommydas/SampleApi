using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace SampleApi.Storage
{
    public class DataService
    {
        public DataService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            Alerts = database.GetCollection<Alert>(settings.AlertsCollectionName);
        }

        IMongoCollection<Alert> Alerts { get; set; }

        public delegate ReturnType ProjectTo<ReturnType>(Alert item);

        public Task<List<ApiProjection>> GetAllAlerts<ApiProjection>(ProjectTo<ApiProjection> projectDel)
        {
            var excludeMongoID = Builders<Alert>.Projection.Exclude(x => x.MongoID);
            return Alerts.Find(x => true).Project(x => projectDel(x)).ToListAsync();
        }

        public Task<ApiProjection> GetAlert<ApiProjection>(int id, ProjectTo<ApiProjection> projectDel)
        {
            var excludeMongoID = Builders<Alert>.Projection.Exclude(x => x.MongoID);
            return Alerts.Find(x => x.ID == id).Project(x => projectDel(x)).FirstOrDefaultAsync();
        }

        public Task InsertAlert(Alert item)
        {
            return Alerts.CountDocumentsAsync(x => x.ID == item.ID).ContinueWith((existing) =>
            {
                if (existing.Result != 0)
                    throw new DuplicateException();

                try { Alerts.InsertOne(item); }
                catch (Exception e) { throw new PersistenceException(); }
            });
        }

        public async Task InsertAlerts(IEnumerable<Alert> items)
        {
            if (items == null)
                throw new ArgumentNullException();

            if (!items.Any()) return;

            var itemIDs = items.Select(x => x.ID).ToHashSet();
            var existingItemsCount = await this.Alerts.CountDocumentsAsync(x => itemIDs.Contains(x.ID));

            if (existingItemsCount > 0)
                throw new DuplicateException();

            await this.Alerts.InsertManyAsync(items);
        }

        public Task DeleteAlert(int id)
        {
            return Alerts.FindOneAndDeleteAsync(x => x.ID == id).ContinueWith((existing) =>
            {
                if (existing.Result == null)
                    throw new NotFoundException();
            });
        }

        public Task UpdateAlert(int existingItem, Alert update)
        {
            return Alerts.FindOneAndReplaceAsync(x => x.ID == existingItem, update);
        }
    }
}
