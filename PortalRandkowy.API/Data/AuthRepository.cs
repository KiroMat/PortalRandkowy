using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PortalRandkowy.API.Models;

namespace PortalRandkowy.API.Data {
    public class AuthRepository : IAuthRepository {
        private readonly DataContext _context;

        #region public
        public AuthRepository (DataContext context) 
        {
            _context = context;
        }
        public async Task<User> Login (string userName, string passwort) 
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == userName);
            if(user == null)
            {
                return null;
            }

            if(!VerifypasswordHash(passwort, user.HashPassword, user.SaltPassword))
                return null;

            return user;
        }

        public async Task<User> Register (User user, string password) 
        {
            byte[] hashPassword, saltPassword;
            CreatePasswordHashSatl(password, out hashPassword, out saltPassword);

            user.HashPassword = hashPassword;
            user.SaltPassword = saltPassword;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> UserExist (string userName) 
        {
            if(await _context.Users.AnyAsync(x => x.UserName == userName))
                return true;
            else
                return false;
        }
        #endregion;

        #region private
        private void CreatePasswordHashSatl(string password, out byte[] hashPassword, out byte[] saltPassword)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                saltPassword = hmac.Key;
                hashPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }           
        }

        private bool VerifypasswordHash(string password, byte[] hashPassword, byte[] saltPassword)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(saltPassword))
            {
                var computedHash = hashPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < computedHash.Length; i++)
                { 
                    if(computedHash[i] != hashPassword[i])
                        return false;
                }
                return true;
            } 
        }
        #endregion
    }
}