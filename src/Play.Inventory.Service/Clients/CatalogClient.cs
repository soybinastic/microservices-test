namespace Play.Inventory.Service.Clients;

public class CatalogClient
{
    private readonly HttpClient _httpClient;

    public CatalogClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyCollection<CatalogItemResponse>> GetCatalogItems()
    {
        var items = await _httpClient
            .GetFromJsonAsync<IReadOnlyCollection<CatalogItemResponse>>("/api/items");
        
        return items!;
    }
}