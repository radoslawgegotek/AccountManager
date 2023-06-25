using AccountManager.Domain.Entities;
using AccountManager.Domain.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountManager.Persistence.Repository
{
    public class AppUserRepository : Repository<AppUser>, IAppUserRepository
    {
        private readonly AppDbContext _dbContext;

        public AppUserRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _dbContext = appDbContext;
        }

        public async Task<AppUser> GetByEmailAsync(string email)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.NormalizedEmail == email.ToUpper());
            if (user == null)
            {
                throw new Exception($"Cannot find entity with email: {email}");
            }
            return user;
        }
    }
}
