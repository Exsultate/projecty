using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using GTANetworkAPI;
using MongoDB.Driver.Core.Clusters;

namespace ProjectYServer.Database
{
    public class DbManager
    {
        public static MongoClient dbClient = null;
        public static IMongoDatabase db = null;
        public DbManager()
        {
            try
            {
                dbClient = new MongoClient("mongodb://localhost:27017");
                if (dbClient.Cluster.Description.State == ClusterState.Connected)
                {
                    NAPI.Util.ConsoleOutput($"Succesfully connected to {dbClient.Cluster}");
                }
                db = dbClient.GetDatabase("projecty");
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput(ex.ToString());
            }
        }
    }
}
