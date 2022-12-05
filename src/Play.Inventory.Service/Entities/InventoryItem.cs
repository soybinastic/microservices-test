using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Play.Common.Models;

namespace Play.Inventory.Service.Entities;

public class InventoryItem : IEntity
{
    [BsonId, BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;
    [BsonElement("user_id"), BsonRepresentation(BsonType.ObjectId)]
    public string UserId { get; set; } = null!;
    [BsonElement("catalog_item_id"), BsonRepresentation(BsonType.ObjectId)]
    public string CatalogItemId { get; set; } = null!;
    [BsonElement("quantity"), BsonRepresentation(BsonType.Int64)]
    public int Quantity { get; set; }
    [BsonElement("acquired_date"), BsonRepresentation(BsonType.DateTime)]
    public DateTimeOffset AcquiredDate { get; set; }
}