namespace Database.Repo.Interfaces;

public interface IBaseRepository<T>
{
    Task Add(T entity);

}