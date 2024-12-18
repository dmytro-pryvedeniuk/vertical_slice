using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;
using TravelInspiration.API.Shared.Security;

namespace TravelInspiration.API.UnitTests.Shared.Security;


public class CurrentUserServiceTests
{
    [Fact]
    public void WhenGettingUser_WithNameIdentifierClaimInContext_NameIdentifierMustBeReturned()
    {
        var httpContextAccessor = new Mock<IHttpContextAccessor>();
        var identity = new ClaimsIdentity(
            [
                new (ClaimTypes.NameIdentifier, "Dmytro"),
            ],
            "Test",
            ClaimTypes.NameIdentifier,
            ClaimTypes.Role);
        var httpContext = new DefaultHttpContext()
        {
            User = new ClaimsPrincipal(identity)
        };
        httpContextAccessor.Setup(x => x.HttpContext)
            .Returns(httpContext);
        var currentUserService = new CurrentUserService(httpContextAccessor.Object);

        // Act
        var userId = currentUserService.UserId;

        Assert.Equal(identity.Name, userId);
    }    
}
