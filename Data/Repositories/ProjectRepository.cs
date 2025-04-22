using System.Linq.Expressions;
using Data.Contexts;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public interface IProjectRepository : IBaseRepository<ProjectEntity>
{
}

public class ProjectRepository(DataContext context) : BaseRepository<ProjectEntity>(context), IProjectRepository
{
    public override async Task<IEnumerable<ProjectEntity>> GetAllAsync()
    {
        var entities = await _context.Projects
            .Include(x => x.Client)
            .Include(x => x.User)
            .Include(x => x.Status)
            .ToListAsync();

        return entities;
    }

    public override async Task<ProjectEntity?> GetAsync(Expression<Func<ProjectEntity, bool>> findBy)
    {
        if (findBy == null)
            return null!;

        var entity = await _context.Projects
            .Include(x => x.Client)
            .Include(x => x.User)
            .Include(x => x.Status)
            .FirstOrDefaultAsync(findBy);

        return entity;
    }
}
