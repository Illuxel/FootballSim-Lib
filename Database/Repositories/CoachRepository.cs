using FootBalLife.GameDB.Entities;
using FootBalLife.GameDB.Models;

namespace FootBalLife.GameDB.Repositories;
public class CoachRepository : _BaseRepository
{
    public List<ECoach> Retrive()
    {
        List<ECoach> result = new List<ECoach>();
        var listData = context.Coaches;
        foreach (Coach data in listData)
        {
            result.Add(mapping(data));
        }
        return result;
    }
    public ECoach Retrive(string Id)
    {
        Coach? data = context.Coaches.Find(Id);
        if (data != null)
        {
            return mapping(data);
        }
        return new ECoach();
    }

    public bool Modify(ECoach eData)
    {
        bool result = false;
        Coach data = context.Coaches.Find(eData.PersonId);
        if (data == null)
        {
            data = mapping(eData, data);
            context.Coaches.Add(data);
            context.SaveChanges();
            result = true;
        }
        else
        {
            data = mapping(eData, data);
            context.Coaches.Update(data);
            context.SaveChanges();
            result = true;
        }
        return result;

    }
    public bool Delete(string Id)
    {
        Coach? data = context.Coaches.Find(Id);
        if (data != null)
        {
            context.Coaches.Remove(data);
            context.SaveChanges();
            return true;
        }
        return false;
    }
    internal static ECoach mapping(string Id)
    {
        if (Id == "")
        {
            return null;
        }
        Coach data = context.Coaches.Find(Id);
        return mapping(data, true);
    }
    internal static ECoach mapping(Coach data, bool noLoop = true)
    {
        if (data == null)
        {
            return null;
        }
        ECoach result = new ECoach();
        result.PersonId = data.PersonId;
        if (!noLoop)
        {
            result.Person = PersonRepository.mapping(data.PersonId);
        }

        return result;
    }
    internal static Coach mapping(ECoach data, Coach result = null)
    {
        if (data == null)
        {
            return null;
        }
        if (result == null)
        {
            result = new Coach();
        }
        result.PersonId = data.PersonId;
        result.Person = PersonRepository.mapping(data.Person, result.Person);

        return result;
    }
}
