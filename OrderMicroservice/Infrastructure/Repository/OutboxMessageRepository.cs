using System.Text.Json;
using Domain.Entities;
using Domain.IRepository;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class OutboxMessageRepository : IOutboxMessageRepository
{
    private readonly AppDbContext _dbContext;

    public OutboxMessageRepository(AppDbContext context)
    {
        _dbContext = context;
    }

    public OutboxMessage GetOutboxMessage<T>(T command)
    {
        var outboxMessage = new OutboxMessage
        {
            Data = JsonSerializer.Serialize(command),
            EventType = command?.GetType().AssemblyQualifiedName,
            CreatedAt = DateTime.UtcNow
        };

        return outboxMessage;
    }

    public async Task AddAsync(OutboxMessage message, CancellationToken cancellationToken = default)
    {
        _dbContext.OutboxMessages.Add(message);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(OutboxMessage message, CancellationToken cancellationToken = default)
    {
        _dbContext.OutboxMessages.Update(message);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var msg = await GetByIdAsync(id, cancellationToken);
        if (msg != null)
        {
            _dbContext.OutboxMessages.Remove(msg);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<OutboxMessage?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var outboxMessage = await _dbContext.OutboxMessages.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        return outboxMessage;
    }

    public async Task<IEnumerable<OutboxMessage>?> GetUnprocessedAsync(int take = 100,
        CancellationToken cancellationToken = default)
    {
        var unprocessedMessages = await _dbContext.OutboxMessages
            .Where(x => !x.IsProcessed)
            .OrderBy(x => x.CreatedAt)
            .Take(take)
            .ToListAsync(cancellationToken);

        return unprocessedMessages;
    }
}