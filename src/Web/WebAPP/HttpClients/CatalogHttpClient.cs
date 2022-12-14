﻿using BlazorStrap;
using Contracts.Services.Catalog;
using Microsoft.Extensions.Options;
using WebAPP.Abstractions.Http;
using WebAPP.DependencyInjection.Options;

namespace WebAPP.HttpClients;

public class CatalogHttpClient : ApplicationHttpClient, ICatalogHttpClient
{
    private readonly ECommerceHttpClientOptions _options;

    public CatalogHttpClient(HttpClient client, IOptionsSnapshot<ECommerceHttpClientOptions> optionsSnapshot, IBlazorStrap blazorStrap)
        : base(client, blazorStrap)
    {
        _options = optionsSnapshot.Value;
    }

    public Task<HttpResponse<PagedResult<Projection.CatalogItemListItem>>> GetAllItemsAsync(int limit, int offset, CancellationToken cancellationToken)
        => GetAsync<PagedResult<Projection.CatalogItemListItem>>($"{_options.CatalogEndpoint}/items?limit={limit}&offset={offset}", cancellationToken);

    public Task<HttpResponse<PagedResult<Projection.CatalogDetails>>> GetAsync(int limit, int offset, CancellationToken cancellationToken)
        => GetAsync<PagedResult<Projection.CatalogDetails>>($"{_options.CatalogEndpoint}?limit={limit}&offset={offset}", cancellationToken);

    public Task<HttpResponse> CreateAsync(Requests.CreateCatalog request, CancellationToken cancellationToken)
        => PostAsync($"{_options.CatalogEndpoint}", request, cancellationToken);

    public Task<HttpResponse> DeleteAsync(Guid catalogId, CancellationToken cancellationToken)
        => DeleteAsync($"{_options.CatalogEndpoint}/{catalogId}", cancellationToken);

    public Task<HttpResponse> ActivateAsync(Guid catalogId, CancellationToken cancellationToken)
        => PutAsync($"{_options.CatalogEndpoint}/{catalogId}/activate", cancellationToken);

    public Task<HttpResponse> DeactivateAsync(Guid catalogId, CancellationToken cancellationToken)
        => PutAsync($"{_options.CatalogEndpoint}/{catalogId}/deactivate", cancellationToken);

    public Task<HttpResponse> ChangeDescriptionAsync(Guid catalogId, string description, CancellationToken cancellationToken)
        => PutAsync($"{_options.CatalogEndpoint}/{catalogId}/description?description={description}", cancellationToken);

    public Task<HttpResponse> ChangeTitleAsync(Guid catalogId, string title, CancellationToken cancellationToken)
        => PutAsync($"{_options.CatalogEndpoint}/{catalogId}/title?title={title}", cancellationToken);

    public Task<HttpResponse> AddCatalogItemAsync(Guid catalogId, Requests.AddCatalogItem request, CancellationToken cancellationToken)
        => PostAsync($"{_options.CatalogEndpoint}/{catalogId}", request, cancellationToken);
}