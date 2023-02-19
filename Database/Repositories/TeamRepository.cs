using FootBalLife.Database.Entities;
using FootBalLife.Database.Models;

namespace FootBalLife.Database.Repositories;

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
    public ETeam Retrive(string ID)
    {
        Team? data = context.Teams.Find(ID);
        if (data != null)
        {
            return mapping(data);
        }
        return new ETeam();
    }

    public bool Modify(ETeam eData)
    {
        bool result = false;
        Team dbTeam = context.Teams.Find(eData.ID);
        
        if (dbTeam == null)
        {
            dbTeam = mapping(eData);
            dbTeam.ID = Guid.NewGuid().ToString();
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
    public bool Delete(string ID)
    {
        bool result = false;
        Team? data = context.Teams.Find(ID);
        if (data != null)
        {
            context.Teams.Remove(data);
            context.SaveChanges();
            result = true;
        }
        return result;
    }

    internal static ETeam mapping(string ID)
    {
        if (string.IsNullOrEmpty(ID))
        {
            return null;
        }
        Team data = context.Teams.Find(ID);
        return mapping(data, true);
    }
    internal static ETeam mapping(Team data, bool noLoop = true)
    {
        if (data == null)
        {
            return null;
        }

        ETeam result = new ETeam();

        result.ID = data.ID;
        result.Name = data.Name;
        result.Strategy = data.Strategy;
        result.CoachID = data.CoachID;
        result.AgentID = data.AgentID;
        result.SportsDirectorID = data.SportsDirectorID;
        result.IsNationalTeam = data.IsNationalTeam;
        result.BaseColor= data.BaseColor;
        result.LeagueID = data.LeagueID;

        if(!noLoop)
        {
            foreach (Contract one in data.Contracts)
            {
                result.Contracts.Add(ContractRepository.mapping(one));
            }
            foreach (Match one in data.MatchHomeTeamNavigations)
            {
                result.MatchHomeTeamNavigations.Add(MatchRepository.mapping(one));
            }
            foreach (Match one in data.MatchHomeTeamNavigations)
            {
                result.MatchGuestTeamNavigations.Add(MatchRepository.mapping(one));
            }
            foreach (NationalResultTable one in data.NationalResultTables)
            {
                result.NationalResultTables.Add(NRTRepository.mapping(one));
            }
            result.League = LeagueRepository.mapping(data.LeagueID);
        }
        
        return result;
    }
    internal static Team mapping(ETeam? data, Team result = null)
    {
        if (data == null)
        {
            return null;
        }
        if (result == null)
        {
            result = new Team();
        }

        result.ID = data.ID;
        result.Name = data.Name;
        result.Strategy = data.Strategy;
        result.CoachID = data.CoachID;
        result.AgentID = data.AgentID;
        result.SportsDirectorID = data.SportsDirectorID;
        result.IsNationalTeam = data.IsNationalTeam;
        result.BaseColor = data.BaseColor;
        result.LeagueID = data.LeagueID;

        result.League = LeagueRepository.mapping(data.League, result.League);

        return result;
    }
}
