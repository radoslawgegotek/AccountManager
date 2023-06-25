using AccountManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountManager.Application.Core.Abstractions
{
    public interface ITokenGenerator
    {
        public string GenerateJwtToken(AppUser user);
    }
}
