using FootBalLife.Database.Entities;
using FootBalLife.Database.Models;

namespace FootBalLife.Database.Repositories;
public class NRTRepository : _BaseRepository
{
    public List<ENationalResultTable> Retrive()
    {
        List<ENationalResultTable> result = new List<ENationalResultTable>();
        var listData = context.NationalResultTables;
        foreach (NationalResultTable data in listData)
        {
            result.Add(mapping(data));
        }
        return result;
    }
    public ENationalResultTable Retrive(string TeamID, string seasonID)
    {
        NationalResultTable? league = context.NationalResultTables.Where(b => b.TeamID == TeamID && b.Season == seasonID).FirstOrDefault();
        if (league != null)
        {
            return mapping(league);
        }
        return new ENationalResultTable();
    }

    public bool Modify(ENationalResultTable eData)
    {
        bool result = false;
        NationalResultTable? data = context.NationalResultTables.Where(b => b.TeamID == eData.TeamID && b.Season == eData.Season).FirstOrDefault();

        if (data == null)
        {
            data = mapping(eData, data);
            context.NationalResultTables.Add(data);
            context.SaveChanges();
            result = true;
        }
        else
        {
            data = mapping(eData, data);
            context.NationalResultTables.Update(data);
            context.SaveChanges();
            result = true;
        }
        return result;

    }
    public bool Delete(string TeamID, string seasonID)
    {
        NationalResultTable? data = context.NationalResultTables.Where(b => b.TeamID == TeamID && b.Season == seasonID).FirstOrDefault();
        if (data != null)
        {
            context.NationalResultTables.Remove(data);
            context.SaveChanges();
            return true;
        }
        return false;
    }

    internal static ENationalResultTable mapping(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return null;
        }
        NationalResultTable data = context.NationalResultTables.Find(id);
        return mapping(data, true);
    }
    internal static ENationalResultTable mapping(NationalResultTable data, bool noLoop = true)
    {
        if (data == null)
        {
            return null;
        }

        ENationalResultTable result = new ENationalResultTable();

        result.Season = data.Season;
        result.TeamID = data.TeamID;
        result.Wins= data.Wins;
        result.Draws= data.Draws;
        result.Loses= data.Loses;
        result.ScoredGoals= data.ScoredGoals;
        result.MissedGoals= data.MissedGoals;
        result.TotalPosition= data.TotalPosition;
        
        if (!noLoop)
        {
            result.Team = TeamRepository.mapping(data.TeamID);
        }

        return result;
    }
    internal static NationalResultTable mapping(ENationalResultTable data, NationalResultTable result = null)
    {
        if (data == null)
        {
            return null;
        }
        if (result == null)
        {
            result = new NationalResultTable();
        }

        result.Season = data.Season;
        result.TeamID = data.TeamID;
        result.Wins = data.Wins;
        result.Draws = data.Draws;
        result.Loses = data.Loses;
        result.ScoredGoals = data.ScoredGoals;
        result.MissedGoals = data.MissedGoals;
        result.TotalPosition = data.TotalPosition;

        result.Team = TeamRepository.mapping(data.Team, result.Team);

        return result;
    }
}
