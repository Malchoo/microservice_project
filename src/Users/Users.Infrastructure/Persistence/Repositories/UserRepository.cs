using Microsoft.EntityFrameworkCore;
using Users.Domain.Contracts;
using Users.Domain.Entities;
using Users.Domain.Ids;
using Users.Domain.ValueObjects.ReferenceType;

namespace Users.Infrastructure.Persistence.Repositories;

public class UserRepository(UserDbContext dbContext) : IUserRepository
{
    private readonly UserDbContext _dbContext = dbContext;

    public async Task AddUserAsync(User user)
    {
        await _dbContext.AddAsync(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> ExistsByEmailAsync(Email email)
    {
        return await _dbContext.Users.AnyAsync(
            user => user.Contacts.Email == email);
    }

    public async Task<bool> ExistsByMobileNumberAsync(MobileNumber mobileNumber)
    {
        return await _dbContext.Users.AnyAsync(
            user => user.Contacts.MobileNumber == mobileNumber);
    }

    public async Task<bool> ExistsByUsernameAsync(Username username)
    {
        return await _dbContext.Users.AllAsync(
            user => user.Profile.Username == username);
    }

    public async Task<User?> GetUserByEmailAsync(Email email)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(
            user => user.Contacts.Email == email);
    }

    public async Task<User?> GetUserByUserIdAsync(UserId userId)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(
            user => user.Id == userId);
    }

    public async Task UpdateAsync(User user)
    {
        _dbContext.Update(user);
        await _dbContext.SaveChangesAsync();
    }
}
