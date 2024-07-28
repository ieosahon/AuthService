namespace Authentication.Application.Contracts.RepositoryContracts;

public interface IGenericRepository <T> where T : class
{
    
    Task<IReadOnlyList<T>> GetAsync();
    
    Task<T> GetByIdAsync(Expression<Func<T, bool>> predicate);
    
    void Add(T entity);
    
    void Update(T entity);
    
    void Delete(T entity);
    
    void DeleteRange(List<T> entities);

    Task<T> GetSingle(Expression<Func<T, bool>> predicate);
    Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate);


}