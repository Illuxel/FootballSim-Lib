using FootBalLife.GameDB.Entities;
using FootBalLife.GameDB.Models;

namespace FootBalLife.GameDB.Repositories;
public class AgentRepository : _BaseRepository
{
    public List<EAgent> Retrive()
    {
        List<EAgent> result = new List<EAgent>();
        var listData = context.Agents;
        foreach (Agent data in listData)
        {
            result.Add(mapping(data));
        }
        return result;
    }
    public EAgent Retrive(string Id)
    {
        Agent? data = context.Agents.Find(Id);
        if (data != null)
        {
            return mapping(data);
        }
        return new EAgent();
    }

    public bool Modify(EAgent eData)
    {
        bool result = false;
        Agent data = context.Agents.Find(eData.PersonId);
        if (data == null)
        {
            data = mapping(eData, data);
            context.Agents.Add(data);
            context.SaveChanges();
            result = true;
        }
        else
        {
            data = mapping(eData, data);
            context.Agents.Update(data);
            context.SaveChanges();
            result = true;
        }
        return result;

    }
    public bool Delete(string Id)
    {
        Agent? data = context.Agents.Find(Id);
        if (data != null)
        {
            context.Agents.Remove(data);
            context.SaveChanges();
            return true;
        }
        return false;
    }

    internal static EAgent mapping(string Id)
    {
        if (Id == "")
        {
            return null;
        }
        Agent data = context.Agents.Find(Id);
        return mapping(data, true);
    }
    internal static EAgent mapping(Agent data, bool noLoop = true)
    {
        if (data == null)
        {
            return null;
        }
        EAgent result = new EAgent();
        result.PersonId = data.PersonId;
        
        if (!noLoop)
        {
            result.Person = PersonRepository.mapping(data.PersonId);
        }
        return result;
    }
    internal static Agent mapping(EAgent data, Agent result = null)
    {
        if (data == null)
        {
            return null;
        }
        if (result == null)
        {
            result = new Agent();
        }
        result.PersonId = data.PersonId;


        result.Person = PersonRepository.mapping(data.Person, result.Person);

        return result;
    }
}
