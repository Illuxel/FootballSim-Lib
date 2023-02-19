using FootBalLife.Database.Entities;
using FootBalLife.Database.Models;

namespace FootBalLife.Database.Repositories;
public class DirectorRepository : _BaseRepository
{
    public List<EDirector> Retrive()
    {
        List<EDirector> result = new List<EDirector>();
        var listData = context.Directors;
        foreach (Director data in listData)
        {
            result.Add(mapping(data));
        }
        return result;
    }
    public EDirector Retrive(string ID)
    {
        Director? data = context.Directors.Find(ID);
        if (data != null)
        {
            return mapping(data);
        }
        return new EDirector();
    }

    public bool Modify(EDirector eData)
    {
        bool result = false;
        Director data = context.Directors.Find(eData.PersonID);

        if (data == null)
        {
            data = mapping(eData);
            context.Directors.Add(data);
            context.SaveChanges();
            result = true;
        }
        else
        {
            data = mapping(eData, data);
            context.Directors.Update(data);
            context.SaveChanges();
            result = true;
        }
        return result;

    }
    public bool Delete(string ID)
    {
        Director? data = context.Directors.Find(ID);
        if (data != null)
        {
            context.Directors.Remove(data);
            context.SaveChanges();
            return true;
        }
        return false;
    }

    internal static EDirector mapping(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return null;
        }
        Director data = context.Directors.Find(id);
        return mapping(data, true);
    }
    internal static EDirector mapping(Director data, bool noLoop = true)
    {
        if (data == null)
        {
            return null;
        }
        EDirector result = new EDirector();
        result.PersonID = data.PersonID;
        if (!noLoop)
        {
            result.Person = PersonRepository.mapping(data.PersonID);
        }

        return result;
    }
    internal static Director mapping(EDirector data, Director result = null)
    {
        if (data == null)
        {
            return null;
        }
        if (result == null)
        {
            result = new Director();
        }
        result.PersonID = data.PersonID;


        result.Person = PersonRepository.mapping(data.Person, result.Person);

        return result;
    }
}
