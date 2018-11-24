using System;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext db;
        public AuthRepository(DataContext db)
        {
            this.db=db;
        }
        public async Task<bool> Exists(string username)
        {
            if (await db.Users.AnyAsync(i=>i.Username==username))
                return true;
            return false;
        }

        public async Task<User> Login(string username, string password)
        {
            var user=await db.Users.FirstOrDefaultAsync(p=>p.Username==username);
            if (user==null)
                return null;
            if (!VerifyPassword(password,user.PasswordHash,user.PasswordSalt))
                return null;

            return user;
        }

        private bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using(var hm=new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash=hm.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i=0;i<computedHash.Length;i++)
                {
                    if (computedHash[i]!=passwordHash[i])
                        return false;
                }
            }
           return true;
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash,passwordSalt;
            CreatePassword(password,out passwordHash,out passwordSalt);

            user.PasswordSalt=passwordSalt;
            user.PasswordHash=passwordHash;
            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();

            return user;
        }

        private void CreatePassword(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac=new System.Security.Cryptography.HMACSHA512())
            {
                 passwordSalt=hmac.Key;
                 passwordHash=hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));  
            }
        }
    }
}