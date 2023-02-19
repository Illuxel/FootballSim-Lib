using FootBalLife.Database.Context;

namespace FootBalLife.Database.Repositories;
public abstract class _BaseRepository
{
    protected static FootbalLifeDbContext context = new FootbalLifeDbContext();
}