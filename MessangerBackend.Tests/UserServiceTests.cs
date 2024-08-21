
using MessangerBackend.Core.Interfaces;
using MessangerBackend.Core.Models;
using MessangerBackend.Core.Models.Exceptions;
using MessangerBackend.Core.Services;
using MessangerBackend.Storage;
using Microsoft.EntityFrameworkCore;

namespace MessangerBackend.Tests
{
    public class UserServiceTests
    {
        [Fact]
        public async Task UserSerice_Login_CorrectInput()
        {
            /// AAA Assign Act Assert 
            var userService = CreateUserService();
            var expectedUser = new User()
            {
                Nickname = "DenisNEW",
                Password = "796059de"
            };
            var user = await userService.Login("DenisNEW", "796059de");
            Assert.Equal(expectedUser, user, new UserComparer());
        }

        [Theory]
        [InlineData("")]
        [InlineData("     ")]
        [InlineData(null)]
        public async Task UserService_Register_ThrowsExceptionWhenIncorrectData(string data)
        {
            // Assign
            var service = CreateUserService();

            // Act
            var exceptionNicknameHandler = async () =>
            {
                await service.Register(data, "1234");
            };
            var exceptionPasswordHandler = async () =>
            {
                await service.Register("nick", data);
            };
            // Assert
            await Assert.ThrowsAsync<UserServiceException>(exceptionNicknameHandler);
            await Assert.ThrowsAsync<UserServiceException>(exceptionPasswordHandler);
        }

        [Theory]
        [InlineData("")]
        [InlineData("     ")]
        [InlineData(null)]
        public async Task UserService_Login_ThrowsExceptionWhenEmptyField(string data)
        {
            // Assign
            var service = CreateUserService();

            // Act
            var exceptionNicknameHandler = async () =>
            {
                await service.Login(data, "1234");
            };
            var exceptionPasswordHandler = async () =>
            {
                await service.Login("nick", data);
            };

            // Assert
            await Assert.ThrowsAsync<UserServiceException>(exceptionNicknameHandler);
            await Assert.ThrowsAsync<UserServiceException>(exceptionPasswordHandler);
        }
        private IUserService CreateUserService()
        {
            var options = new DbContextOptionsBuilder<MessangerContext>().UseSqlServer("Server=.\\SQLEXPRESS;Database=MessangerDb;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=False;").Options;
            var context = new MessangerContext(options);
            var repository = new Repository(context);
            var service = new UserService(repository);

            return service;

        }


        //---------------------------------------



        //SearchUsers
        [Theory]
        [InlineData("DenisNEW")]//// якщо нікнейм коректний 
        [InlineData("NEW")]//// якщо нікнейм є частиною чиєгось нікнейму 
        [InlineData("DENISNEW")]//// різні регістри
        [InlineData("denisnew")]//// різні регістри с регистрами реализована логика в сервсе как в телеграмме,
                                //// что при любом регистре находит того или иных пользователей
        public async Task UserSerice_SearchUser_CorrectInput(string data)
        {
            var userService = CreateUserService();
            var expectedUsers = new List<User>()
            {
                new User{Nickname = "DenisNEW" },
            };
            var user = userService.SearchUsers(data);
            Assert.Equal(expectedUsers, user, new UserComparer());
        }

        [Theory]
        [InlineData("")]//// якщо нікнейм пустий 

        [InlineData("ABC")]//// якщо немає такого нікнейму

        [InlineData(null)]////якщо nulll
        public async Task UserService_SearchUsers_ThrowsExceptionWhenIncorrectData(string data)
        {
            // Assign
            var service = CreateUserService();

            // Act
            var exceptionNicknameHandler = async () =>
            {
                service.SearchUsers(data);
            };
            // Assert
            await Assert.ThrowsAsync<UserServiceException>(exceptionNicknameHandler);
        }

        //---------------------------------------

        [Fact]
        public async Task UserService_GetUserById_CorrectInput()
        {
            // Assign
            var service = CreateUserService();
            var expectedUser = new User { Nickname = "Denis" };

            // Act
            var user = await service.GetUserById(1);

            // Assert
            Assert.Equal(expectedUser, user, new UserComparer());
        }

        [Theory]
        [InlineData(-2)] 
        [InlineData(999)] 
        [InlineData(null)]
        public async Task UserService_GetUserById_InCorrectInput(int data)
        {
            // Assign
            var service = CreateUserService();

            // Act
            var user = await service.GetUserById(data);

            // Assert
            Assert.Null(user);
        }


        [Fact]
        public void UserService_GetUsers_ReturnsEmptyListWhenNoUsers()
        {
            // Assign
            var service = CreateUserService();

            // Act
            var users = service.GetUsers(2, 10); 
            // Assert
            Assert.Empty(users);
        }
    }


    class UserComparer : IEqualityComparer<User>
    {
        public bool Equals(User x, User y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.Nickname == y.Nickname;
        }

        public int GetHashCode(User obj)
        {
            return HashCode.Combine(obj.Nickname);
        }
    }
}
