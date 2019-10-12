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
        
        public Task<List<ApiProjection>> GetAllAlerts<ApiProjection>()
        {
            var excludeMongoID = Builders<Alert>.Projection.Exclude(x => x.MongoID);
            return Alerts.Find(x => true).Project(excludeMongoID).As<ApiProjection>().ToListAsync();
        }

        public Task<ApiProjection> GetAlert<ApiProjection>(int id)
        {
            var excludeMongoID = Builders<Alert>.Projection.Exclude(x => x.MongoID);
            return Alerts.Find(x => x.ID == id).Project(excludeMongoID).As<ApiProjection>().FirstOrDefaultAsync();
        }

        public Task InsertAlert(Alert item)
        {
            return GetAlert<Alert>(item.ID).ContinueWith((existing) =>
            {
                if (existing.Result != null)
                    throw new DuplicateException();

                try { Alerts.InsertOne(item); }
                catch (Exception e) { throw new PersistenceException(); }
            });
        }

        public Task DeleteAlert(int id)
        {
            return GetAlert<Alert>(id).ContinueWith((existing) =>
            {
                if (existing.Result == null)
                    throw new NotFoundException();

                try { Alerts.DeleteOne(x => x.MongoID == existing.Result.MongoID); }
                catch (Exception e) { throw new PersistenceException(); }
            });
        }

        public Task UpdateAlert(int existingItem, Alert update)
        {
            return GetAlert<Alert>(existingItem).ContinueWith((existing) =>
            {
                if (existing.Result == null)
                    throw new NotFoundException();

                try { Alerts.ReplaceOne(x => x.MongoID == existing.Result.MongoID, update); }
                catch (Exception e) { throw new PersistenceException(); }
            });
        }
    }
}
