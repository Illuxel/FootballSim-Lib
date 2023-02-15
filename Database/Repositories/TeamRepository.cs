using FootBalLife.GameDB.Entities;
using FootBalLife.GameDB.Models;

namespace FootBalLife.GameDB.Repositories;

public class TeamRepository : _BaseRepository
{
    public List<ETeam> Retrive()
    {
        List<ETeam> result = new List<ETeam>();
        var listData = context.Teams;
        foreach (Team data in listData)
        {
            result.Add(mapping(data));
        }
        return result;
    }
    public ETeam Retrive(string Id)
    {
        Team? data = context.Teams.Find(Id);
        if (data != null)
        {
            return mapping(data);
        }
        return new ETeam();
    }

    public bool Modify(ETeam eData)
    {
        bool result = false;
        Team dbTeam = context.Teams.Find(eData.Id);
        
        if (dbTeam == null)
        {
            dbTeam = mapping(eData);
            dbTeam.Id = Guid.NewGuid().ToString();
            context.Teams.Add(dbTeam);
            context.SaveChanges();
            result = true;
        }
        else
        {
            dbTeam = mapping(eData, dbTeam);
            context.Teams.Update(dbTeam);
            context.SaveChanges();
            result = true;
        }
        return result;
    }
    public bool Delete(string Id)
    {
        bool result = false;
        Team? data = context.Teams.Find(Id);
        if (data != null)
        {
            context.Teams.Remove(data);
            context.SaveChanges();
            result = true;
        }
        return result;
    }


    internal static ETeam mapping(string Id)
    {
        if (string.IsNullOrEmpty(Id))
        {
            return null;
        }
        Team data = context.Teams.Find(Id);
        return mapping(data, true);
    }
    internal static ETeam mapping(Team data, bool noLoop = true)
    {
        if (data == null)
        {
            return null;
        }

        ETeam result = new ETeam();

        result.Id = data.Id;
        result.Name = data.Name;
        result.Strategy = data.Strategy;
        result.CoachId = data.CoachId;
        result.AgentId = data.AgentId;
        result.SportsDirectorId = data.SportsDirectorId;
        result.IsNationalTeam = data.IsNationalTeam;
        result.BaseColor= data.BaseColor;
        result.LeagueId = data.LeagueId;

        if(!noLoop)
        {
            foreach (Contract one in data.Contracts)
            {
                result.Contracts.Add(ContractRepository.mapping(one));
            }
            foreach (Match one in data.MatchTeam1Navigations)
            {
                result.MatchHomeTeamNavigations.Add(MatchRepository.mapping(one));
            }
            foreach (Match one in data.MatchTeam1Navigations)
            {
                result.MatchGuestTeamNavigations.Add(MatchRepository.mapping(one));
            }
            foreach (NationalResultTable one in data.NationalResultTables)
            {
                result.NationalResultTables.Add(NRTRepository.mapping(one));
            }
            result.League = LeagueRepository.mapping(data.LeagueId.Value);
        }
        
        return result;
    }
    internal static Team mapping(ETeam data, Team result = null)
    {
        if (data == null)
        {
            return null;
        }
        if (result == null)
        {
            result = new Team();
        }

        result.Id = data.Id;
        result.Name = data.Name;
        result.Strategy = data.Strategy;
        result.CoachId = data.CoachId;
        result.AgentId = data.AgentId;
        result.SportsDirectorId = data.SportsDirectorId;
        result.IsNationalTeam = data.IsNationalTeam;
        result.BaseColor = data.BaseColor;
        result.LeagueId = data.LeagueId;

        result.League = LeagueRepository.mapping(data.League, result.League);

        return result;
    }
}
