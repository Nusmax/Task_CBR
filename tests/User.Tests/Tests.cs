using Moq;
using User.Application.Commands;
using User.Application.Handlers;
using User.Application.Queries;
using User.Domain.Abstractions;
using User.Domain.Entities;

public class AppUserHandlerTests
{
    [Fact]
    public async Task Register_Ok()
    {
        var repo = new Mock<IUserRepository>();
        repo.Setup(x => x.GetByNameAsync("a", It.IsAny<CancellationToken>())).ReturnsAsync((AppUser?)null);
        var h = new RegisterUserHandler(repo.Object, new FakeHash());
        var r = await h.Handle(new RegisterUserCommand("a", "p"), default);
        Assert.True(r.IsSuccess);
    }
    [Fact]
    public async Task Register_Duplicate_Fails()
    {
        var repo = new Mock<IUserRepository>();
        repo.Setup(x => x.GetByNameAsync("a", It.IsAny<CancellationToken>())).ReturnsAsync(new AppUser());
        var h = new RegisterUserHandler(repo.Object, new FakeHash());
        var r = await h.Handle(new RegisterUserCommand("a", "p"), default);
        Assert.False(r.IsSuccess);
    }
    [Fact]
    public async Task Login_WrongPassword_Fails()
    {
        var repo = new Mock<IUserRepository>();
        repo.Setup(x => x.GetByNameAsync("a", It.IsAny<CancellationToken>())).ReturnsAsync(new AppUser { PasswordHash = "h" });
        var hasher = new Mock<IPasswordHasher>();
        hasher.Setup(x => x.Verify(It.IsAny<string>(), It.IsAny<string>())).Returns(false);
        var jwt = new Mock<IJwtProvider>();
        var h = new LoginUserHandler(repo.Object, hasher.Object, jwt.Object);
        var r = await h.Handle(new LoginUserQuery("a", "bad"), default);
        Assert.False(r.IsSuccess);
    }
    class FakeHash : IPasswordHasher
    {
        public string Hash(string p) => "h:" + p;
        public bool Verify(string p, string h) => h == "h:" + p;
    }
}

