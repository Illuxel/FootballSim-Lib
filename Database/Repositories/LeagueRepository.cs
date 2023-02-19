using FootBalLife.Database.Entities;
using FootBalLife.Database.Models;

namespace FootBalLife.Database.Repositories;
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
    public ELeague Retrive(long ID)
    {
        League? league = context.Leagues.Find(ID);
        if(league != null)
        {
            return mapping(league,false);
        }
        return new ELeague();
    }

    public bool Modify(ELeague eData)
    {
        bool result = false;
        League data = context.Leagues.Find(eData.ID);

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
    public bool Delete(long ID)
    {
        League? data = context.Leagues.Find(ID);
        if (data!=null)
        {
            context.Leagues.Remove(data);
            context.SaveChanges();
            return true;
        }
        return false;
    }
    internal static ELeague mapping(long ID)
    {
        if (ID == 0)
        {
            return null;
        }

        League? data = context.Leagues.Find(ID);

        return mapping(data, true);
    }
    internal static ELeague mapping(League? data, bool noLoop = true)
    {
        if (data == null)
        {
            return null;
        }

        ELeague result = new ELeague();

        result.ID = data.ID;
        result.Name = data.Name;
        result.CountryID = data.CountryID;
        result.CurrentRating = data.CurrentRating;

        if (!noLoop)
        {
            foreach (Team one in context.Teams.Where(x => x.LeagueID == data.ID))
            {
                result.Teams.Add(TeamRepository.mapping(one));
            }
        }

        return result;
    }
    internal static League mapping(ELeague? data, League? result = null)
    {
        if (data == null)
        {
            return null;
        }
        if (result == null)
        {
            result = new League();
        }

        result.ID = data.ID;
        result.Name = data.Name;
        result.CurrentRating = data.CurrentRating;
        result.CountryID = data.CountryID;
        
        return result;
    }
}
