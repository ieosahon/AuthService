namespace Authentication.Application.Contracts.RepositoryContracts;

public interface IDbQuery
{
    Task<LgaAndState> GetLgaAndStateByLgaId(uint lgaId);
}