namespace Domain.IRepository;

public interface IUnitOfWork
{
    IOutboxMessageRepository OutboxMessageRepository { get; }
}