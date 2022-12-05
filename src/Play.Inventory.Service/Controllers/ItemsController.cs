using Microsoft.AspNetCore.Mvc;
using Play.Inventory.Service.Clients;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemsController : ControllerBase
{
    private readonly MongoDBRepository<InventoryItem> _inventoryRepository;
    private readonly MongoDBRepository<CatalogItem> _catalogItemRepository;

    public ItemsController(
        MongoDBRepository<InventoryItem> inventoryRepository,
        MongoDBRepository<CatalogItem> catalogItemRepository)
    {
        _inventoryRepository = inventoryRepository;
        _catalogItemRepository = catalogItemRepository;
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<IEnumerable<InventoryItemResponse>>> Get(string userId)
    {
        var catalogItems = await _catalogItemRepository.GetAll();
        var inventoryItems = await _inventoryRepository.GetAll(i => i.UserId == userId); 
        
        var items = inventoryItems.AsQueryable()
            .Join(catalogItems.AsQueryable(), inventory => inventory.CatalogItemId, 
            catalogItem => catalogItem.Id, (inventory, catalogItem) => 
            inventory.ToDto(catalogItem.Name, catalogItem.Description!))
            .ToList();
        
        return Ok(items);
    }

    [HttpPost]
    public async Task<IActionResult> Post(GrantItemsRequest itemsRequest)
    {
        var inventoryItem = await _inventoryRepository.Get(
            i => i.UserId == itemsRequest.UserId 
            && i.CatalogItemId == itemsRequest.CatalogItemId);
        
        if(inventoryItem is null)
        {
            inventoryItem = new InventoryItem
            {
                CatalogItemId = itemsRequest.CatalogItemId,
                UserId = itemsRequest.UserId,
                Quantity = itemsRequest.Quantity,
                AcquiredDate = DateTimeOffset.UtcNow
            };

            var result = await _inventoryRepository.Create(inventoryItem);

            return CreatedAtAction(
                nameof(Get), 
                new { userId = result.UserId }, 
                result);
        }

        inventoryItem.Quantity += itemsRequest.Quantity;
        await _inventoryRepository.Update(inventoryItem.Id, inventoryItem);

        return Ok(new { Message = "Inventory has been modified." });
    }
}