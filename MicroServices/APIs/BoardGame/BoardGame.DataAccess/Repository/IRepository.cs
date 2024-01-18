namespace BoardGame.DataAccess.Repository;

public interface IRepository<T>
{
	Task<IEnumerable<T>> GetAll(); //Kanske inte behöver denna om jag använder den nedan enbart letar efter tom sträng eller liknande?
	Task<IEnumerable<T>> FindByKey(Func<T, bool> findFunc);
	Task<T> AddItem(T item);
	Task<string> Delete(string id);
	Task<T> Update(T item);
}