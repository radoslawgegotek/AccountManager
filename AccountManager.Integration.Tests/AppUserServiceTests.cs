using System.Net;
using AccountManager.Application.Services;
using AccountManager.Domain.IRepository;
using AccountManager.Domain.IServices;
using AccountManager.Persistence;
using AccountManager.Persistence.Repository;
using AutoMapper;
using Xunit;
using AccountManager.Application;
using AccountManager.Application.Validators;
using AccountManager.Application.Core.Abstractions;
using Moq;
using Microsoft.AspNetCore.Identity;
using AccountManager.Domain.Entities;
using AccountManager.Domain.Dto;
using Microsoft.AspNetCore.Mvc;
using AccountManager.Tests.Infrastructure;

namespace AccountManager.Tests
{
    public class AppUserServiceTests
    {
        private readonly MapperConfiguration _mapperConfig = new(cfg => cfg.AddProfile<MappingConfig>());
        private readonly AppDbContext _ctx;
        private readonly IAppUserService _appUserService;
        private readonly IAppUserRepository _userRepository;
        private readonly IPasswordHasher<AppUser> _passwordHasher;
        private readonly Mock<ITokenGenerator> _tokenGeneratorMock = new();

        public AppUserServiceTests()
        {
            _ctx = DbContextFactory.Create();
            _userRepository = new AppUserRepository(_ctx);
            _passwordHasher = new PasswordHasher<AppUser>();
            _tokenGeneratorMock.Setup(x => x.GenerateJwtToken(It.IsAny<AppUser>())).Returns("AuthJwtToken");
            
            _appUserService = new AppUserService(
                _userRepository,
				_mapperConfig.CreateMapper(),
				new RegisterRequestValidator(),
				_tokenGeneratorMock.Object,
				_passwordHasher);
		}

        [Fact]
        public async Task AppUserService_RegisterUser_Test()
        {
            RegisterRequestDTO registerDto = new()
            {
                Name = "Adam",
                LastName = "Nowak",
                Email = "user@example.com",
                Password = "1qaz@WSX",
                ConfirmPassword = "1qaz@WSX",
                PhoneNumber = "111222333",
                PESEL = "12345678901",
                Age = 22,
                AvgPowerConsumption = 14.99573421
            };

            await _appUserService.RegisterAsync(registerDto);
            var users = await _userRepository.GetAllAsync();
            Assert.NotNull(users);
            Assert.NotEmpty(users);
            Assert.Single(users);
            Assert.Equal("Adam", users.First().Name);
            Assert.NotEmpty(users.First().Password);
            Assert.NotEqual(registerDto.Password, users.First().Password);
            Assert.Equal(users.First().AvgPowerConsumption, 14.996);
        }

        public async Task AppUserService_Register_InvalidData_Test()
        {
            RegisterRequestDTO registerDto = new()
            {
                Name = "Adam",
                LastName = "Nowak",
                Email = " user@example.com",
                Password = "1qaze2",
                ConfirmPassword = "123",
                PhoneNumber = "111222333",
                PESEL = "12345678901",
                Age = 22,
                AvgPowerConsumption = 14.99573421
            };

            var result = await _appUserService.RegisterAsync(registerDto);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task AppUserService_GetAll_Test()
        {
            List<AppUser> users = new()
            {
                new AppUser()
                {
                    Name = "Adam",
                    LastName = "Nowak",
                    Email = "adam@example.com",
                    PhoneNumber = "999888222",
                    Password = "1qaz@WSX",
                    PESEL = "12322278901",
                    Age = 22
                },
                new AppUser()
                {
                    Name = "Maria",
                    LastName = "Kowalska",
                    Email = "maria@example.com",
                    PhoneNumber = "222333444",
                    Password = "1qaz@WSX",
                    PESEL = "12346428901",
                    Age = 32
                }
            };
            await _ctx.Users.AddRangeAsync(users);
            await _ctx.SaveChangesAsync();

            var result = await _appUserService.GetAllAsync();
            
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            var okObject = (OkObjectResult)result;
            Assert.NotNull(okObject.Value);
            Assert.IsType<List<UserResponseDTO>>(okObject.Value);
            

            Assert.Collection((okObject?.Value as List<UserResponseDTO>)!,
                r =>
                {
                    Assert.Equal("Adam", r.Name);
                    Assert.Equal("Nowak", r.LastName);
                    Assert.Equal("adam@example.com", r.Email);
                    Assert.Equal("12322278901", r.PESEL);
                    Assert.Equal("999888222", r.PhoneNumber);
                },
                r =>
                {
                    Assert.Equal("Maria", r.Name);
                    Assert.Equal("Kowalska", r.LastName);
                    Assert.Equal("maria@example.com", r.Email);
                    Assert.Equal("12346428901", r.PESEL);
                    Assert.Equal("222333444", r.PhoneNumber);
                });
        }

        [Fact]
        public async Task AppUserService_Login_Test()
        {
            var tempUser = new AppUser()
            {
                Name = "Adam",
                LastName = "Nowak",
                Email = "user@example.com",
				PhoneNumber = "111222333",
                PESEL = "12345678901",
                Age = 22,
                AvgPowerConsumption = 14.81,
            };
            tempUser.NormalizedEmail = tempUser.Email.ToUpper();
            tempUser.Password = _passwordHasher.HashPassword(tempUser, "1qaz@WSX");
            await _ctx.Users.AddAsync(tempUser);
            await _ctx.SaveChangesAsync();

            var loginReq = new LoginRequestDTO()
            {
                Email = tempUser.Email,
                Password = "1qaz@WSX",
            };
            
            var result = await _appUserService.LoginAsync(loginReq);

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            var resultType = (OkObjectResult)result;

            Assert.IsType<LoginResponseDTO>(resultType.Value);
            var resVal = (LoginResponseDTO)resultType.Value;
            Assert.NotNull(resVal);
            Assert.Equal(resVal.User.Name, tempUser.Name);
            Assert.Equal(resVal.User.Email, tempUser.Email);
            Assert.Equal(resVal.User.PESEL, tempUser.PESEL);
            Assert.Equal(resVal.User.Age, tempUser.Age);
        }
    }
}
