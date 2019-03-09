namespace Wallspace.Services
{
    using Wallspace.Models;

    public interface IJwtService
    {
        string WriteJwt(WallspaceUser wallspaceUser);
    }
}