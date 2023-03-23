namespace BankAPI_.Services.Implements;

public interface IService<T> where T : class
{
    Task<T> Create(T entity);
    Task Update(string id, T entity);
    Task Delete(string id);
    Task<T?> GetById(string id);
    Task<ICollection<T>> GetAll();
}