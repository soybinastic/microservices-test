using MongoDB.Bson.Serialization.Attributes;
using Play.Common.Models;

namespace Play.Inventory.Service.Entities;


public class CatalogItem : IEntity
{
    [BsonElement("catalog_item_id"), BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string Id { get; set; } = null!;
    [BsonElement("name"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public string Name { get; set; } = null!;
    [BsonElement("description"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public string? Description { get; set; }
}