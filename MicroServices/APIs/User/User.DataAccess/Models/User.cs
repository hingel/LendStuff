namespace User.DataAccess.Models;

public class User
{
    public Guid Id { get; init; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public ICollection<Guid> ActiveOrders = new List<Guid>(); //detta är en lista med aktiva ordrar. Dessa tas bort när en order blir terminated.
    public int? Rating { get; set; }

    //lägg till adresser etc:
}