using FootBalLife.GameDB.Context;

namespace FootBalLife.GameDB.Repositories;
public abstract class _BaseRepository
{
    protected static FootbalLifeDbContext context = new FootbalLifeDbContext();
}