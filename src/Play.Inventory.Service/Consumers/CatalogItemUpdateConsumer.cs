using MassTransit;
using Play.Contracts;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Consumers;


public class CatalogItemUpdateConsumer : IConsumer<CatalogItemUpdated>
{
    private readonly MongoDBRepository<CatalogItem> _catalogItemRepository;

    public CatalogItemUpdateConsumer(MongoDBRepository<CatalogItem> catalogItemRepository)
    {
        _catalogItemRepository = catalogItemRepository;
    }
    public async Task Consume(ConsumeContext<CatalogItemUpdated> context)
    {
        var catalogItemMessage = context.Message;

        var catalogItem = await _catalogItemRepository.GetById(catalogItemMessage.ItemId);

        if(catalogItem is null) 
        {
            catalogItem = new CatalogItem
            {
                Id = catalogItemMessage.ItemId,
                Name = catalogItemMessage.Name,
                Description = catalogItemMessage.Description
            };

            await _catalogItemRepository.Create(catalogItem);
        }
        else
        {
            catalogItem.Name = catalogItemMessage.Name;
            catalogItem.Description = catalogItemMessage.Description;

            await _catalogItemRepository.Update(catalogItem.Id, catalogItem);
        }
        
    }
}