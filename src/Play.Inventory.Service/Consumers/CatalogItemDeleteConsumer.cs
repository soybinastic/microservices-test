using MassTransit;
using Play.Contracts;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Consumers;


public class CatalogItemDeleteConsumer : IConsumer<CatalogItemDeleted>
{
    private readonly MongoDBRepository<CatalogItem> _catalogItemRepository;

    public CatalogItemDeleteConsumer(MongoDBRepository<CatalogItem> catalogItemRepository)
    {
        _catalogItemRepository = catalogItemRepository;
    }
    public async Task Consume(ConsumeContext<CatalogItemDeleted> context)
    {
        var catalogItemMessage = context.Message;

        var catalogItemToBeDelete = await _catalogItemRepository.GetById(catalogItemMessage.ItemId);

        if(catalogItemToBeDelete is null) return;

        await _catalogItemRepository.Delete(catalogItemToBeDelete.Id);
    }
}