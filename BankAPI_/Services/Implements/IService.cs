namespace BankAPI_.Services.Implements;

public interface IService<T> where T : class
{
    Task<T> Create(T entity);
    Task Update(T entity);
    Task Delete(T entity);
    Task<T?> GetById(string id);
    Task<ICollection<T>> GetAll();
}