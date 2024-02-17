namespace LendStuff.Shared;

public interface IRepository<T>
{
	Task<IEnumerable<T>> GetAll();
	Task<IEnumerable<T>> FindByKey(Func<T, bool> findFunc);
	Task<T> AddItem(T item);
	Task<string> Delete(Guid id);
	Task<T> Update(T item);
}