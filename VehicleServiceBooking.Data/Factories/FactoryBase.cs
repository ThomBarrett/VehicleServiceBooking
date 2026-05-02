using Microsoft.EntityFrameworkCore;
using VehicleServiceBooking.Data.DbContexts;

namespace VehicleServiceBooking.Data.Factories;

public abstract class FactoryBase<T> where T : class
{
    protected abstract DbSet<T> GetDbSet(DataContext db);
    public abstract T Create(dynamic data);
    
    public virtual async Task<T> BuildAsync(DataContext db, T entity)
    {
        if (db is null)
            throw new ArgumentNullException(nameof(db));

        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        var set = GetDbSet(db);
        set.Add(entity);
        
        await db.SaveChangesAsync();

        return entity;
    }
}