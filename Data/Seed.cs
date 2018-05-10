using System.Collections.Generic;
using dotnetFun.API.Models;
using Newtonsoft.Json;

namespace dotnetFun.API.Data
{
    public class Seed
    {
        private readonly DataContext _context;
        public Seed(DataContext context) {
            _context = context;
        }

        public void SeedUsers()
        {
            // _context.Users.RemoveRange(_context.Users);
            // _context.SaveChanges();

            // seed user

            var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");
            
            var users = JsonConvert.DeserializeObject<List<User>>(userData);

            foreach(var user in users)
            {
                byte[] passwordHash, passwordSalt;

                CreatePasswordHash("password", out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.Username = user.Username.ToLower();

                _context.Users.Add(user);
            }

            _context.SaveChanges();
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmacEncrypt = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmacEncrypt.Key;
                passwordHash = hmacEncrypt.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}