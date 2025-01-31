﻿using Devon4Net.Infrastructure.LiteDb.LiteDb;
using Devon4Net.Infrastructure.Common;
using LiteDB;

namespace Devon4Net.Infrastructure.LiteDb.Repository
{
    public class LiteDbRepository<T> : ILiteDbRepository<T> where T : class
    {
        private const string ErrorMessage = "Please check the predicate is null and the deleteAllCheck param as well. The provided predicate is null and the input param deleteAllCheck is to false. If you want to delete all the collection please set the deleteAllCheck param to true.";

        private LiteDatabase LiteDb { get; }
        private string CollectionName { get; }

        public LiteDbRepository(ILiteDbContext liteDbContext)
        {
            LiteDb = liteDbContext?.Database ?? throw new ArgumentException("The context can not be null. Please check your DI container configuration and check if the container has declared the instance of the context");
            CollectionName = typeof(T).Name;
        }

        public BsonValue Create(T entity)
        {
            var result = LiteDb.GetCollection<T>(CollectionName).Insert(entity);

            return result;
        }

        public bool Update(T entity)
        {
            return LiteDb.GetCollection<T>(CollectionName).Update(entity);
        }

        public int Delete(BsonExpression predicate, bool deleteAllCheck = false)
        {
            try
            {
                if (predicate != null && !deleteAllCheck)
                {
                    return LiteDb.GetCollection<T>(CollectionName).DeleteMany(predicate);
                }

                if (predicate == null && deleteAllCheck)
                {
                    return LiteDb.GetCollection<T>(CollectionName).DeleteAll();
                }

                return int.MinValue;
            }
            catch (Exception ex)
            {
                Devon4NetLogger.Error(ex, ErrorMessage);
                throw;
            }
        }

        public IEnumerable<T> Get()
        {
            return LiteDb.GetCollection<T>(CollectionName).FindAll();
        }

        public IEnumerable<T> Get(BsonExpression predicate, int skip = 0, int limit = int.MaxValue)
        {
            return LiteDb.GetCollection<T>(CollectionName).Find(predicate,skip, limit);
        }

        public T GetFirstOrDefault(BsonExpression predicate)
        {
            return LiteDb.GetCollection<T>(CollectionName).FindOne(predicate);
        }
        public T GetFirstOrDefault(Query query)
        {
            return LiteDb.GetCollection<T>(CollectionName).FindOne(query);
        }
    }
}
