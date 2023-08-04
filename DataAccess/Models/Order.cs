using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LendStuff.Server.Models;
using LendStuff.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace LendStuff.DataAccess.Models;

public class Order
{
	[Key]
	public int OrderId { get; set; }
	[Required]
	public ApplicationUser Owner { get; set; }
	[Required]
	public string BorrowerId { get; set; }
	[Required]
	public BoardGame BoardGame { get; set; }
	[Required]
	public DateTime LentDate { get; set; }
	[Required]
	public DateTime ReturnDate { get; set; }
	[Required]
	public OrderStatus Status { get; set; }
	public ICollection<InternalMessage> OrderMessages { get; set; } = new List<InternalMessage>();
}