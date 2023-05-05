using LendStuff.DataAccess.Models;
using LendStuff.DataAccess.Repositories.Interfaces;

namespace LendStuff.DataAccess.Services;

public class MessageService
{
	private readonly IRepository<InternalMessage> _messageRepository;

	public MessageService(IRepository<InternalMessage> messageRepository)
	{
		_messageRepository = messageRepository;
	}


}