using Users.Domain.Contracts;

namespace Users.Domain.Ids;

public readonly record struct UserId : IEntityId
{
    public static UserId CreateUnique() => new(Guid.NewGuid());

    public UserId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentNullException("sloji nqkakvo pravilno suob6tenie"); //ToDo направи специална грешка

        Value = value;
    }

    public Guid Value { get; init; }
}