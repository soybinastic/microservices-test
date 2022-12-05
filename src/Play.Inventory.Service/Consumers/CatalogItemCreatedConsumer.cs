using MassTransit;
using Play.Contracts;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Consumers;


public class CatalogItemCreatedConsumer : IConsumer<CatalogItemCreated>
{
    private readonly MongoDBRepository<CatalogItem> _catalogItemRepository;
    public CatalogItemCreatedConsumer(MongoDBRepository<CatalogItem> catalogItemRepository)
    {
        _catalogItemRepository = catalogItemRepository;
    }
    public async Task Consume(ConsumeContext<CatalogItemCreated> context)
    {
        var message = context.Message;

        var catalogItem = await _catalogItemRepository.GetById(message.ItemId);

        if(catalogItem is not null) return;

        catalogItem = new CatalogItem
        {
            Id = message.ItemId,
            Name = message.Name,
            Description = message.Description
        };

        await _catalogItemRepository.Create(catalogItem);
    }
}