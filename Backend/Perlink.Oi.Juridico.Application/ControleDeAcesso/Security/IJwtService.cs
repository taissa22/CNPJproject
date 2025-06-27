namespace Perlink.Oi.Juridico.Application.Security {
    public interface IJwtService {
        JsonWebToken CreateJsonWebToken(User user);
    }
}