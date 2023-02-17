using FootBalLife.Database.Entities;
using FootBalLife.Database.Models;

namespace FootBalLife.Database.Repositories;
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
    public ECoach Retrive(string ID)
    {
        Coach? data = context.Coaches.Find(ID);
        if (data != null)
        {
            return mapping(data);
        }
        return new ECoach();
    }

    public bool Modify(ECoach eData)
    {
        bool result = false;
        Coach data = context.Coaches.Find(eData.PersonID);
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
    public bool Delete(string ID)
    {
        Coach? data = context.Coaches.Find(ID);
        if (data != null)
        {
            context.Coaches.Remove(data);
            context.SaveChanges();
            return true;
        }
        return false;
    }
    internal static ECoach mapping(string ID)
    {
        if (ID == "")
        {
            return null;
        }
        Coach data = context.Coaches.Find(ID);
        return mapping(data, true);
    }
    internal static ECoach mapping(Coach data, bool noLoop = true)
    {
        if (data == null)
        {
            return null;
        }
        ECoach result = new ECoach();
        result.PersonID = data.PersonID;
        if (!noLoop)
        {
            result.Person = PersonRepository.mapping(data.PersonID);
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
        result.PersonID = data.PersonID;
        result.Person = PersonRepository.mapping(data.Person, result.Person);

        return result;
    }
}
