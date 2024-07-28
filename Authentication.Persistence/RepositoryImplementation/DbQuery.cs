using Authentication.Application.DTOs.ResponseDto;

namespace Authentication.Persistence.RepositoryImplementation;

public class DbQuery : IDbQuery
{
    private readonly AppDbContext _context;
    public DbQuery(AppDbContext context)
    {
        _context = context;
    }

    public async Task<LgaAndState> GetLgaAndStateByLgaId(uint lgaId)
    {
        var query = await (from state in _context.States
            join lga in _context.LGAs on state.Id equals lga.StateId
            where lga.Id == lgaId
            select new LgaAndState
            {
                StateName = state.Name,
                LgaName = lga.Name
            }).FirstOrDefaultAsync();

        return query;
    }
}