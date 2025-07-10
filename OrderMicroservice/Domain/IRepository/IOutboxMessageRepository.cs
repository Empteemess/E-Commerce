using Domain.Entities;

namespace Domain.IRepository;

public interface IOutboxMessageRepository
{
    OutboxMessage MapToOutboxMessage<T>(T command);
    Task AddAsync(OutboxMessage message, CancellationToken cancellationToken = default);
    Task UpdateAsync(OutboxMessage message, CancellationToken cancellationToken = default);
    Task RemoveAsync(Guid id, CancellationToken cancellationToken = default);
    Task<OutboxMessage?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IEnumerable<OutboxMessage>?>
        GetUnprocessedAsync(int take = 100, CancellationToken cancellationToken = default);
}