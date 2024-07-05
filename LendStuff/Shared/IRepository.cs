namespace LendStuff.Shared;

public interface IRepository<T>
{
	Task<IEnumerable<T>> GetAll();
	Task<T?> GetById(Guid id);
	Task<T> AddItem(T item);
	Task<string> Delete(params Guid[] ids);
	Task<T?> Update(T item);
}