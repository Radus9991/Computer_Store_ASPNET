using ApplicationCore.Exceptions;
using ApplicationCore.Services;
using AutoMapper;
using ApplicationCore.Models;
using Infrastructure.Mongo.Models;
using ApplicationCore.Repositories;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Mongo.Sevices
{
    public class UserServiceMongo : IUserService
    {
        private readonly IBaseRepository<UserMongo, ObjectId?> userRepository;
        private readonly IPasswordAlgorithm passwordAlgorithm;
        private readonly IMapper mapper;

        public UserServiceMongo(IBaseRepository<UserMongo, ObjectId?> userRepository, IPasswordAlgorithm passwordAlgorithm, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.passwordAlgorithm = passwordAlgorithm;
            this.mapper = mapper;
        }

        private async Task ValidateUser(User user)
        {
            var u = await userRepository.FindFirst(x => x.Email == user.Email);

            if (u != null)
            {
                throw new InvalidUserException($"User with {user.Email} exists!");
            }

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
            return await userRepository.Get(ObjectId.Parse(id)) != null;
        }

        public async Task<User> AuthUser(string email, string password)
        {
            var user = await userRepository.FindFirst(x => x.Email == email);

            if (user == null || !passwordAlgorithm.ComparePasswordHash(password, user.Hash, user.Salt))
            {
                throw new InvalidUserException("Invalid credentials!");
            }

            return mapper.Map<User>(user);
        }

        public async Task<User> AddUser(User user, string password)
        {
            passwordAlgorithm.CreatePassword(password, out byte[] hash, out byte[] salt);

            user.Hash = hash;
            user.Salt = salt;

            await ValidateUser(user);

            UserMongo userEntity = mapper.Map<UserMongo>(user);

            return mapper.Map<User>(await userRepository.Add(userEntity));
        }

        public async Task<User> GetUser(string email)
        {
            return mapper.Map<User>(await userRepository.FindFirst(x => x.Email == email));
        }
    }
}
