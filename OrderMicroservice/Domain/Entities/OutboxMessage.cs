namespace Domain.Entities;

public class OutboxMessage
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? EventType { get; set; }
    public string? Data { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ProcessedAt { get; set; }
    public bool IsProcessed { get; set; }
    public int RetryCount { get; set; }
    public string? ErrorMessage { get; set; }
    public string? CorrelationId { get; set; }
}