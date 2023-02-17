using FootBalLife.Database.Entities;
using FootBalLife.Database.Models;

namespace FootBalLife.Database.Repositories;
public class ScoutRepository : _BaseRepository
{
    public List<EScout> Retrive()
    {
        List<EScout> result = new List<EScout>();
        var listData = context.Scouts;
        foreach (Scout data in listData)
        {
            result.Add(mapping(data));
        }
        return result;
    }
    public EScout? Retrive(string ID)
    {
        Scout? data = context.Scouts.Find(ID);
        if (data != null)
        {
            return mapping(data);
        }
        return new EScout();
    }

    public bool Modify(EScout eData)
    {
        bool result = false;
        var data = context.Scouts.Find(eData.PersonID);

        if (data == null)
        {
            data = mapping(eData);
            context.Scouts.Add(data);
            context.SaveChanges();
            result = true;
        }
        else
        {
            data = mapping(eData, data);
            context.Scouts.Update(data);
            context.SaveChanges();
            result = true;
        }

        return result;

    }
    public bool Delete(string scoutID)
    {
        var data = context.Scouts.Find(scoutID);

        if (data != null)
        {
            context.Scouts.Remove(data);
            context.SaveChanges();
            return true;
        }

        return false;
    }

    internal static EScout? mapping(string scoutID)
    {
        if (string.IsNullOrEmpty(scoutID))
        {
            return null;
        }

        var data = context.Scouts.Find(scoutID);
        return mapping(data, true);
    }

    internal static EScout? mapping(Scout? data, bool noLoop = true)
    {
        if (data == null)
        {
            return null;
        }

        var result = new EScout();
        result.PersonID = data.PersonID;

        if (!noLoop)
        {
            result.Person = PersonRepository.mapping(data.PersonID);
        }

        return result;
    }

    internal static Scout? mapping(EScout? data, Scout result = null)
    {
        if (data == null)
        {
            return null;
        }
        if (result == null)
        {
            result = new Scout();
        }
        result.PersonID = data.PersonID;
        result.Person = PersonRepository.mapping(data.Person, result.Person);
        return result;
    }
}
