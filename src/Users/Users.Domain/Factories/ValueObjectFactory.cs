using ErrorOr;

namespace Users.Domain.Factories;

public static class ValueObjectFactory
{
    public static ErrorOr<TValueObject> ChangeProperty<TValueObject, TProperty>(
        TValueObject currentValueObject,
        TProperty currentValue,
        TProperty newValue,
        Func<TProperty, Error> errorFunc,
        Func<TProperty, TValueObject> changeFunc)
        where TValueObject : notnull
    {
        if (EqualityComparer<TProperty>.Default.Equals(currentValue, newValue))
            return errorFunc(newValue);

        return changeFunc(newValue);
    }
}