using System;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service;

public record GrantItemsRequest(
    string UserId,
    string CatalogItemId,
    int Quantity);

public record InventoryItemResponse(
    string CatalogItemId,
    string Name,
    string Description,
    int Quantity,
    DateTimeOffset AcquiredDate);

public record CatalogItemResponse(
    string Id,
    string Name,
    string Description
);

public static class DtoExtensions
{
    public static InventoryItemResponse ToDto(this InventoryItem item, string name, string description)
    {
        return new InventoryItemResponse(
            item.CatalogItemId, 
            name, 
            description, 
            item.Quantity, 
            item.AcquiredDate);
    }
}