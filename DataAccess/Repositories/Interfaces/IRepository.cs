namespace LendStuff.DataAccess.Repositories.Interfaces;

public interface IRepository<T> 
{
	Task<IEnumerable<T>> GetAll();
	Task<IEnumerable<T>> FindByKey(Func<T, bool> findFunc);
	Task<T> AddItem(T item);
	Task<string> Delete(string id);
	Task<T> Update(T item);
}