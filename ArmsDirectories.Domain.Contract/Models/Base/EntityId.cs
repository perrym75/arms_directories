using System.Diagnostics.CodeAnalysis;

namespace ArmsDirectories.Domain.Contract.Models.Base;

public readonly record struct EntityId<TEntity>(Guid Value) : IParsable<EntityId<TEntity>>
{
    public EntityId(string value) : this(Guid.Empty)
    {
        Value = Guid.Parse(value);
    }

    public static EntityId<TEntity> Empty => new(Guid.Empty);
    
    public static EntityId<TEntity> NewId() => new(Guid.CreateVersion7());

    public override string ToString() => Value.ToString();

    public static EntityId<TEntity> Parse(string s, IFormatProvider? provider)
    {
        return new EntityId<TEntity>(Guid.Parse(s, provider));
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out EntityId<TEntity> result)
    {
        if (Guid.TryParse(s, provider, out var guid))
        {
            result = new EntityId<TEntity>(guid);
            return true;
        }

        result = Empty;
        return false;
    }
}