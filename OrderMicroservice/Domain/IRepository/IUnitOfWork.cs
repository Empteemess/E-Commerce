namespace Domain.IRepository;

public interface IUnitOfWork
{
    Task SaveChangesAsync();
    IOutboxMessageRepository OutboxMessageRepository { get; }
}