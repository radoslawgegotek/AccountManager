using AccountManager.Application.Authentication;
using AccountManager.Application.Core.Abstractions;
using AccountManager.Domain.Dto;
using AccountManager.Domain.Entities;
using AccountManager.Domain.IRepository;
using AccountManager.Domain.IServices;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AccountManager.Application.Services
{
    public class AppUserService : IAppUserService
    {
        private readonly IAppUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<RegisterRequestDTO> _validator;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IPasswordHasher<AppUser> _passwordHasher;

        public AppUserService(IAppUserRepository userRepository, 
            IMapper mapper,
            IValidator<RegisterRequestDTO> validator,
            ITokenGenerator tokenGenerator,
            IPasswordHasher<AppUser> passwordHasher)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _validator = validator;
            _tokenGenerator = tokenGenerator;
            _passwordHasher = passwordHasher;
        }

        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var users = await _userRepository.GetAllAsync();
                if (users == null)
                    return new NoContentResult();
                
                return new OkObjectResult(_mapper.Map<List<UserResponseDTO>>(users));
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.Message);
            }
        }

        public async Task<IActionResult> LoginAsync(LoginRequestDTO loginRequestDTO)
        {
            try
            {
                var user = await _userRepository.GetByEmailAsync(loginRequestDTO.Email);
                var correctPassword = _passwordHasher.VerifyHashedPassword(user, user.Password, loginRequestDTO.Password);
                if (correctPassword == PasswordVerificationResult.Success)
                {
                    LoginResponseDTO userResponse = new();
                    userResponse.User = _mapper.Map<UserResponseDTO>(user);
                    userResponse.Token = _tokenGenerator.GenerateJwtToken(user);
                    return new OkObjectResult(userResponse);
                }
                return new UnauthorizedObjectResult("Incorrect email or password");
            }
            catch (Exception)
            {
                return new UnauthorizedObjectResult("Incorrect email or password");
            }
        }

        public async Task<IActionResult> RegisterAsync(RegisterRequestDTO registerRequestDTO)
        {
            try
            {
                var result = _validator.Validate(registerRequestDTO);
                if (result.IsValid)
                {
                    if (registerRequestDTO.AvgPowerConsumption != null)
                    {
                        registerRequestDTO.AvgPowerConsumption = Convert.ToDouble(Math.Round((decimal)registerRequestDTO.AvgPowerConsumption, 3));
                    }

                    AppUser user = _mapper.Map<AppUser>(registerRequestDTO);
                    user.Password = _passwordHasher.HashPassword(user, registerRequestDTO.Password);
                    user.NormalizedEmail = user.Email.ToUpper();

                    await _userRepository.AddAsync(user);
                    return new OkResult();
                }
                return new BadRequestObjectResult(result.ToString());
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
