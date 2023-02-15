using FootBalLife.GameDB.Entities;
using FootBalLife.GameDB.Models;

namespace FootBalLife.GameDB.Repositories;
public class MatchRepository : _BaseRepository
{
    public List<EMatch> Retrive()
    {
        List<EMatch> result = new List<EMatch>();
        var listData = context.Matches;
        foreach (Match data in listData)
        {
            result.Add(mapping(data));
           
        }
        return result;
    }
    public EMatch Retrive(string Id)
    {
        Match? league = context.Matches.Find(Id);
        if (league != null)
        {
            return mapping(league);
        }
        return new EMatch();
    }

    public bool Modify(EMatch eData)
    {
        bool result = false;
        Match data = context.Matches.Find(eData.Id);

        if (data == null)
        {
            data = mapping(eData, data);
            Guid myuuid = Guid.NewGuid();
            string myuuidAsString = myuuid.ToString();
            data.Id = myuuidAsString;
            context.Matches.Add(data);
            context.SaveChanges();
            result = true;
        }
        else
        {
            data = mapping(eData, data);
            context.Matches.Update(data);
            context.SaveChanges();
            result = true;
        }
        return result;

    }
    public bool Delete(string Id)
    {
        Match? data = context.Matches.Find(Id);
        if (data != null)
        {
            context.Matches.Remove(data);
            context.SaveChanges();
            return true;
        }
        return false;
    }

    internal static EMatch mapping(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return null;
        }
        Match data = context.Matches.Find(id);
        return mapping(data, true);
    }
    internal static EMatch mapping(Match data, bool noLoop = true)
    {
        if (data == null)
        {
            return null;
        }

        EMatch result = new EMatch();

        result.Id = data.Id;
        result.HomeTeam = data.HomeTeam;
        result.GuestTeam = data.GuestTeam;
        result.Season = data.Season;
        result.WeekNumber = data.WeekNumber;
        result.HomeTeamGoals = data.HomeTeamGoals;
        result.GuestTeamGoals = data.GuestTeamGoals;

        if (!noLoop)
        {
            result.HomeTeamNavigation = TeamRepository.mapping(data.HomeTeam);
            result.GuestTeamNavigation = TeamRepository.mapping(data.GuestTeam);
        }

        return result;
    }
    internal static Match mapping(EMatch data, Match result = null)
    {
        if (data == null)
        {
            return null;
        }
        if (result == null)
        {
            result = new Match();
        }

        result.Id = data.Id;
        result.Id = data.Id;
        result.HomeTeam = data.HomeTeam;
        result.GuestTeam = data.GuestTeam;
        result.Season = data.Season;
        result.WeekNumber = data.WeekNumber;
        result.HomeTeamGoals = data.HomeTeamGoals;
        result.GuestTeamGoals = data.GuestTeamGoals;

        result.Team1Navigation = TeamRepository.mapping(data.HomeTeamNavigation, result.Team1Navigation);
        result.Team2Navigation = TeamRepository.mapping(data.GuestTeamNavigation, result.Team2Navigation);

        return result;
    }
}
