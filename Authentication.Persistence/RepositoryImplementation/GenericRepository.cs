// ReSharper disable ClassWithVirtualMembersNeverInherited.Global

namespace Authentication.Persistence.RepositoryImplementation;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly AppDbContext _context;

    public GenericRepository(AppDbContext context)
    {
        _context = context;
    }

    public void Add(T entity) =>
        _context.Set<T>().Add(entity);
    

    public void Delete(T entity) =>
        _context.Set<T>().Remove(entity);

    public void DeleteRange(List<T> entities) =>
        _context.Set<T>().RemoveRange(entities);

    public virtual async Task<T> GetByIdAsync(Expression<Func<T, bool>> predicate) =>
        await _context.Set<T>().FindAsync(predicate);
    

    public virtual async Task<IReadOnlyList<T>> GetAsync()
        => await _context.Set<T>().AsNoTracking().ToListAsync();
    

    public void Update(T entity) =>
        _context.Set<T>().Update(entity);

    public virtual async Task<T> GetSingle(Expression<Func<T, bool>> predicate)
        => await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(predicate);

    public virtual async Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate)
        => await _context.Set<T>().Where(predicate).AsNoTracking().ToListAsync();
    
    
}