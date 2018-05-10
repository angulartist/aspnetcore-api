using System.Collections.Generic;
using System.Threading.Tasks;
using dotnetFun.API.Models;

namespace dotnetFun.API.Data
{
    public interface IDotnetAppRepository
    {
         void Add<T>(T entity) where T: class;

         void Delete<T>(T entity) where T: class;

         Task<bool> SaveAll();

         Task<IEnumerable<User>> GetUsers();

         Task<User> GetUser(string username);

         Task<Photo> GetPhoto(int id);
    }
}