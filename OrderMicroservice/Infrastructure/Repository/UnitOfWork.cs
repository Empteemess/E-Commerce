using Domain.IRepository;
using Infrastructure.Data;

namespace Infrastructure.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context,
        IOutboxMessageRepository outboxMessageRepository)
    {
        _context = context;
        OutboxMessageRepository = outboxMessageRepository;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public IOutboxMessageRepository OutboxMessageRepository { get; }
}