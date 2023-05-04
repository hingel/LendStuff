using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LendStuff.Server.Models;
using LendStuff.Shared;
using Microsoft.VisualBasic;

namespace LendStuff.DataAccess.Models;

public class Order
{
	public int OrderId { get; set; }
	public ApplicationUser Owner { get; set; }
	public ApplicationUser Borrower { get; set; }
	public BoardGame BoardGame { get; set; }
	public DateTime LentDate { get; set; }
	public DateTime ReturnDate { get; set; }
	public OrderStatus Status { get; set; }
}