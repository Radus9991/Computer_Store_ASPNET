using ApplicationCore.Exceptions;
using ApplicationCore.Models;
using ApplicationCore.Repositories;
using ApplicationCore.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Memory.Services
{
    public class UserService : IUserService
    {
        private readonly IBaseRepository<User, string> userRepository;
        private readonly IPasswordAlgorithm passwordAlgorithm;

        public UserService(IBaseRepository<User, string> userRepository, IPasswordAlgorithm passwordAlgorithm)
        {
            this.userRepository = userRepository;
            this.passwordAlgorithm = passwordAlgorithm;
        }

        private void ValidateUser(User user)
        {
            ValidationContext validationContext = new ValidationContext(user);
            List<ValidationResult> validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(user, validationContext, validationResults);

            if (!isValid)
            {
                throw new InvalidUserException(string.Join(", ", validationResults.Select(x => x.ErrorMessage)));
            }
        }



        public async Task<bool> UserExists(string id)
        {
            return await userRepository.Get(id) != null;
        }

        public async Task<User> AuthUser(string email, string password)
        {
            var user = await userRepository.FindFirst(x => x.Email == email);

            if (user == null || !passwordAlgorithm.ComparePasswordHash(password, user.Hash, user.Salt))
            {
                throw new InvalidUserException("Invalid credentials!");
            }

            return user;
        }

        public async Task<User> AddUser(User user, string password)
        {
            passwordAlgorithm.CreatePassword(password, out byte[] hash, out byte[] salt);

            user.Hash = hash;
            user.Salt = salt;

            ValidateUser(user);
            return await userRepository.Add(user);
        }

        public async Task<User> GetUser(string email)
        {
            return await userRepository.FindFirst(x => x.Email == email);
        }
    }
}
