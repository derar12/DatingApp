using System;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;
// checks data going in

namespace DatingApp.API.Data
{
    public class AuthRepository : IAuthoRepository
    {
        private readonly DataContext _context;

        public AuthRepository(DataContext context)
        {
            _context = context;

        }
// issue is here 
        public async Task<User> Login(string username, string password)
        {
            // first ore default will find an existing one or return default
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);

            if (user == null)
                return null;

            if(!VerifyPasswordHash(password,user.PasswordHash,user.PasswordSalt))
                return null;
            return user; 
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using( var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt)){         
                   var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                    for(int i = 0; i < computedHash.Length;i++)
                        {
                            if(computedHash[i] != password[i])
                                return false;
                        }
               }
               return true;
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user; 
        }

// uses system to create a password key to encrypt 
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
               using( var hmac = new System.Security.Cryptography.HMACSHA512()){
                   passwordSalt = hmac.Key;
                   passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
               }
        }
        public async Task<bool> UserExists(string username)
        {
            if(await _context.Users.AnyAsync(x=> x.Username == username))
                return true;
            return false;
        }
    }
}