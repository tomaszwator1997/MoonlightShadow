using MoonlightShadow.Models;
using MoonlightShadow.Models.Products;
using MongoDB.Driver;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace MoonlightShadow.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _user;
        private readonly IMongoClient _client;

        public UserService(IDatabaseSettings settings)
        {
            _client = new MongoClient(settings.ConnectionString);
            var database = _client.GetDatabase(settings.DatabaseName);

            _user = database.GetCollection<User>(settings.UsersCollectionName);
        }

        public void Create(User userIn)
        {
            _user.InsertOne(userIn);
        }

        public List<User> Get()
        {
            return _user.Find(user => true).ToList();
        }

        public async Task<IAsyncCursor<User>> GetAsync()
        {
            return await _user.FindAsync(user => true);
        }

        public User GetByLogin(string login)
        {
            return _user.Find(user => user.Login == login).FirstOrDefault();
        }

        public User GetByEmail(string email)
        {
            return _user.Find(user => user.Email == email).FirstOrDefault();
        }

        public void Update(User userIn)
        {
            _user.ReplaceOne(user => user.Login == userIn.Login, userIn);
        }
    }
}