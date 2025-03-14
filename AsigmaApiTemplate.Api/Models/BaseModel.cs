using System.Text.Json.Serialization;

namespace AsigmaApiTemplate.Api.Models;

public abstract class BaseModel : IEntity
{
    public Guid Id { get; set; }

    [JsonIgnore]
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;

    [JsonIgnore]
    public DateTime DateUpdated { get; set; } = DateTime.UtcNow;
    [JsonIgnore]
    public bool IsDeleted { get; set; }
}

