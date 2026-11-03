namespace GameStore.Api.Features.Baskets.UpsertBasket;


public record UpsertBasketDto(Guid Id, IEnumerable<UpsertBasketItemDto> Items);

public record UpsertBasketItemDto(Guid Id, int Quantity);