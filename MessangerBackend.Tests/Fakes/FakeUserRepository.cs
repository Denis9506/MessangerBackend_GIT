//using MessangerBackend.Core.Interfaces;
//using MessangerBackend.Core.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Text;
//using System.Threading.Tasks;

//namespace MessangerBackend.Tests.Fakes
//{
//    internal class FakeUserRepository : IRepository
//    {
//        private List<User> _users;
//        public async Task<T> Add<T>(T entity) where T : class
//        {
//            _users.Add(entity as User);
//            return entity;
//        }

//        public Task<T> Delete<T>(int id) where T : class
//        {
//            throw new NotImplementedException();
//        }

//        public IQueryable<T> GetAll<T>() where T : class
//        {
//            throw new NotImplementedException();
//        }

//        public async Task<T> GetById<T>(int id) where T : class
//        {
//            return _users.Single(x => x.Id == id) as T;
//        }

//        public Task<IEnumerable<T>> GetQuery<T>(Expression<Func<T, bool>> func) where T : class
//        {
//            return _users.AsEnumerable
//        }

//        public Task<T> Update<T>(T entity) where T : class
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
