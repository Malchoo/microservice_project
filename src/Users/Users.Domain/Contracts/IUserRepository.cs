using Users.Domain.Entities;
using Users.Domain.Ids;
using Users.Domain.ValueObjects.ReferenceType;

namespace Users.Domain.Contracts;
public interface IUserRepository
{
    Task AddUserAsync(User user);
    Task<bool> ExistsByEmailAsync(Email email);
    Task<bool> ExistsByMobileNumberAsync(MobileNumber mobileNumber);
    Task<bool> ExistsByUsernameAsync(Username username);
    Task<User?> GetUserByEmailAsync(Email email);
    Task<User?> GetUserByUserIdAsync(UserId userId);
    Task UpdateAsync(User user);
}