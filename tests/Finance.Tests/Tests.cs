using Finance.Application.Handlers;
using Finance.Application.Queries;
using Finance.Domain.Abstractions;
using Finance.Domain.Entities;
using Moq;

public class FinanceHandlerTests
{
    [Fact]
    public async Task GetFavorites_Empty()
    {
        var repo = new Mock<IFinanceRepository>();
        repo.Setup(x => x.GetFavoritesAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync([]);
        var h = new GetFavoritesHandler(repo.Object);
        var r = await h.Handle(new GetFavoritesQuery(Guid.NewGuid()), default);
        Assert.True(r.IsSuccess);
        Assert.Empty(r.Value!);
    }
    [Fact]
    public async Task GetFavorites_ReturnsRate()
    {
        var repo = new Mock<IFinanceRepository>();
        repo.Setup(x => x.GetFavoritesAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([new Currency { Id = "USD", Name = "Доллар", Rate = 90.5m }]);
        var h = new GetFavoritesHandler(repo.Object);
        var r = await h.Handle(new GetFavoritesQuery(Guid.NewGuid()), default);
        Assert.Equal(90.5m, r.Value![0].Rate);
    }
}

