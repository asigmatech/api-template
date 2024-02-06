namespace AsigmaApiTemplate.Api.Models;

public abstract class BaseModel : IEntity
{
    public Guid Id { get; set; }

    public DateTime DateCreated { get; set; } = DateTime.UtcNow;

    public DateTime DateUpdated { get; set; } = DateTime.UtcNow;
    
    public bool IsDeleted { get; set; }
}