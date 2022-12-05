using System.ComponentModel.DataAnnotations;
using Play.Common.Models;

namespace Play.Catalog.Service;

public record ItemResponse(string Id, string Name, string Description, decimal Price, DateTimeOffset CreatedDate);
public record CreateItemRequest([Required] string Name, string Description, [Range(0, 1000)] decimal Price);
public record UpdateItemRequest([Required] string Name, string Description, [Range(0, 1000)] decimal Price);


public static class DtoExtensions
{
    public static ItemResponse ToItemDto(this Item item)
    {
        return new ItemResponse(item.Id, item.Name, item.Description!, item.Price, item.CreatedDate);
    }
}