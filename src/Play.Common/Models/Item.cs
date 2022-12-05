namespace Play.Common.Models;

public class Item : IEntity
{
    [BsonId, BsonRepresentation(BsonType.ObjectId), BsonElement("_id")]
    public string Id { get; set; } = null!;
    [BsonElement("name"), BsonRepresentation(BsonType.String)]
    public string Name { get; set; } = null!;
    [BsonElement("description"), BsonRepresentation(BsonType.String)]
    public string? Description { get; set; }
    [BsonElement("price"), BsonRepresentation(BsonType.Decimal128)]
    public decimal Price { get; set; }
    [BsonElement("created_date"), BsonRepresentation(BsonType.DateTime)]
    public DateTimeOffset CreatedDate { get; set; }
}