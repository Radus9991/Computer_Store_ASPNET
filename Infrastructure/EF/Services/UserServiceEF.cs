using ApplicationCore.Exceptions;
using ApplicationCore.Services;
using AutoMapper;
using Infrastructure.EF.Models;
using ApplicationCore.Models;
using ApplicationCore.Repositories;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.EF.Sevices
{
    public class UserServiceEF : IUserService
    {
        private readonly IBaseRepository<UserEntity, int> userRepository;
        private readonly IPasswordAlgorithm passwordAlgorithm;
        private readonly IMapper mapper;

        public UserServiceEF(IBaseRepository<UserEntity, int> userRepository,
            IPasswordAlgorithm passwordAlgorithm,
            IMapper mapper)
        {
            this.userRepository = userRepository;
            this.passwordAlgorithm = passwordAlgorithm;
            this.mapper = mapper;
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
            return await userRepository.Get(int.Parse(id)) != null;
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

            ValidateUser(user);

            UserEntity userEntity = mapper.Map<UserEntity>(user);

            return mapper.Map<User>(await userRepository.Add(userEntity));
        }

        public async Task<User> GetUser(string email)
        {
            return mapper.Map<User>(await userRepository.FindFirst(x => x.Email == email));
        }
    }
}
