using Microsoft.AspNetCore.Mvc;
using Play.Contracts;

namespace Play.Catalog.Service.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemsController : ControllerBase
{
    private readonly MongoDBRepository<Item> itemRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public ItemsController(
        MongoDBRepository<Item> itemRepository,
        IPublishEndpoint publishEndpoint)
    {
        this.itemRepository = itemRepository;
        _publishEndpoint = publishEndpoint;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ItemResponse>>> Get()
    {
        
        var items = await itemRepository.GetAll();

        return items.Select(i => i.ToItemDto()).ToList();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ItemResponse?>> GetById(string id)
    {
        return Ok((await itemRepository.GetById(id))?.ToItemDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateItemRequest createItem)
    {
        var createdItem = await itemRepository.Create(new Item 
            {
                Name = createItem.Name,
                Description = createItem.Description,
                Price = createItem.Price,
                CreatedDate = DateTimeOffset.UtcNow
            });
        
        await _publishEndpoint.Publish(new CatalogItemCreated(
            createdItem.Id,
            createdItem.Name,
            createdItem.Description!
        ));

        return CreatedAtAction(nameof(GetById), new { id = createdItem.Id }, createdItem.ToItemDto());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(UpdateItemRequest updateItem, string id)
    {
        var item = await itemRepository.GetById(id);

        if(item is null) return NotFound();

        item.Name = updateItem.Name;
        item.Description = updateItem.Description;
        item.Price = updateItem.Price;

        await itemRepository.Update(item.Id, item);
        
        await _publishEndpoint.Publish(new CatalogItemUpdated(
            item.Id,
            item.Name,
            item.Description!
        ));

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var item = await itemRepository.GetById(id);

        if(item is null) return NotFound();

        var result = await itemRepository.Delete(item.Id);

        await _publishEndpoint.Publish(new CatalogItemDeleted(item.Id));

        return result ? Ok() : BadRequest();
    }

    [HttpGet("name/{name}")]
    public async Task<ActionResult<Item>> GetByName(string name)
    {
        var item = await itemRepository.Get(i => i.Name == name);
        return Ok(item?.ToItemDto());
    }

}