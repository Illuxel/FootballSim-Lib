using FootBalLife.GameDB.Entities;
using FootBalLife.GameDB.Models;

namespace FootBalLife.GameDB.Repositories;
public class LeagueRepository : _BaseRepository
{
    public List<ELeague> Retrive()
    {
        List<ELeague> result = new List<ELeague>();
        var listData = context.Leagues;
        foreach (League data in listData)
        {
            result.Add(mapping(data, false));
        }

        return result;
    }
    public ELeague Retrive(long Id)
    {
        League? league = context.Leagues.Find(Id);
        if(league != null)
        {
            return mapping(league,false);
        }
        return new ELeague();
    }

    public bool Modify(ELeague eData)
    {
        bool result = false;
        League data = context.Leagues.Find(eData.Id);

        if (data == null)
        {
            data = mapping(eData, data);
            context.Leagues.Add(data);
            context.SaveChanges();
            result = true;
        }
        else
        {
            data = mapping(eData, data);
            context.Leagues.Update(data);
            context.SaveChanges();
            result = true;
        }
        return result;
    }
    public bool Delete(long Id)
    {
        League? data = context.Leagues.Find(Id);
        if (data!=null)
        {
            context.Leagues.Remove(data);
            context.SaveChanges();
            return true;
        }
        return false;
    }
    internal static ELeague mapping(long Id)
    {
        if (Id == 0)
        {
            return null;
        }
        League data = context.Leagues.Find(Id);
        return mapping(data, true);
    }
    internal static ELeague mapping(League data, bool noLoop = true)
    {
        if (data == null)
        {
            return null;
        }
        ELeague result = new ELeague();
        result.Id = data.Id;
        result.Name = data.Name;
        result.CurrentRating = data.CurrentRating;
        result.CountryId = data.CountryId;
        if (!noLoop)
        {
            foreach (Team one in context.Teams.Where(x => x.LeagueId == data.Id))
            {
                result.Teams.Add(TeamRepository.mapping(one));
            }
        }

        return result;
    }
    internal static League mapping(ELeague data, League result = null)
    {
        if (data == null)
        {
            return null;
        }
        if (result == null)
        {
            result = new League();
        }
        result.Id = data.Id;
        result.Name = data.Name;
        result.CurrentRating = data.CurrentRating;
        result.CountryId = data.CountryId;
        
     
        return result;
    }
}
