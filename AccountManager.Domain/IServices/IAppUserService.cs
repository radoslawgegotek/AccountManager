using AccountManager.Domain.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountManager.Domain.IServices
{
    public interface IAppUserService
    {
        Task<IActionResult> LoginAsync(LoginRequestDTO loginRequestDTO);
        Task<IActionResult> RegisterAsync(RegisterRequestDTO registerRequestDTO);
        Task<IActionResult> GetAllAsync();
    }
}
