using FootBalLife.GameDB.Entities;
using FootBalLife.GameDB.Models;

namespace FootBalLife.GameDB.Repositories;
public class PositionRepository : _BaseRepository
{
    public List<EPosition> Retrive()
    {
        List<EPosition> result = new List<EPosition>();
        var listData = context.Positions;
        foreach (Position data in listData)
        {
            result.Add(mapping(data));
        }
        return result;
    }
    public EPosition Retrive(long Id)
    {
        Position? data = context.Positions.Find(Id);
        if (data != null)
        {
            return mapping(data);
        }
        return new EPosition();
    }

    public bool Modify(EPosition eData)
    {
        bool result = false;
        Position data = context.Positions.Find(eData.Id);

        if (data == null)
        {
            data = mapping(eData, data);
            context.Positions.Add(data);
            context.SaveChanges();
            result = true;
        }
        else
        {
            data = mapping(eData, data);
            context.Positions.Update(data);
            context.SaveChanges();
            result = true;
        }
        return result;

    }
    public bool Delete(long Id)
    {
        Position? data = context.Positions.Find(Id);
        if (data != null)
        {
            context.Positions.Remove(data);
            context.SaveChanges();
            return true;
        }
        return false;
    }

    internal static EPosition mapping(long Id)
    {
        if (Id == 0)
        {
            return null;
        }
        Position data = context.Positions.Find(Id);
        return mapping(data, true);
    }
    internal static EPosition mapping(Position data, bool noLoop = true)
    {
        if (data == null)
        {
            return null;
        }
        EPosition result = new EPosition();
        result.Id = data.Id;
        result.Name = data.Name;
        result.Location = data.Location;

        if (!noLoop)
        {
            foreach (Player one in data.Players)
            {
                result.Players.Add(PlayerRepository.mapping(one));
            }
            
        }
        return result;
    }
    internal static Position mapping(EPosition data, Position result = null)
    {
        if (data == null)
        {
            return null;
        }
        if (result == null)
        {
            result = new Position();
        }
        result.Id = data.Id;
        result.Name = data.Name;
        result.Location = data.Location;

        return result;
    }
}
