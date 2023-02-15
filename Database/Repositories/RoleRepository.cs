using FootBalLife.GameDB.Entities;
using FootBalLife.GameDB.Models;

namespace FootBalLife.GameDB.Repositories;
public class RoleRepository : _BaseRepository
{
    public List<ERole> Retrive()
    {
        List<ERole> result = new List<ERole>();
        var listData = context.Roles;
        foreach (Role data in listData)
        {
            result.Add(mapping(data));
        }
        return result;
    }
    public ERole Retrive(long Id)
    {
        Role? data = context.Roles.Find(Id);
        if (data != null)
        {
            return mapping(data);
        }
        return new ERole();
    }

    public bool Modify(ERole eData)
    {
        bool result = false;
        Role data = context.Roles.Find(eData.Id);

        if (data == null)
        {
            data = mapping(eData, data);
            context.Roles.Add(data);
            context.SaveChanges();
            result = true;
        }
        else
        {
            data = mapping(eData, data);
            context.Roles.Update(data);
            context.SaveChanges();
            result = true;
        }
        return result;

    }
    public bool Delete(long Id)
    {
        Role? data = context.Roles.Find(Id);
        if (data != null)
        {
            context.Roles.Remove(data);
            context.SaveChanges();
            return true;
        }
        return false;
    }

    internal static ERole mapping(long Id)
    {
        if (Id == 0)
        {
            return null;
        }
        Role data = context.Roles.Find(Id);
        return mapping(data, true);
    }
    internal static ERole mapping(Role data, bool noLoop = true)
    {
        if (data == null)
        {
            return null;
        }
        ERole result = new ERole();
        result.Id = data.Id;
        result.Name = data.Name;
        result.IsNpc = data.IsNpc;
        result.Icon = data.Icon;

        if (!noLoop)
        {
            foreach (Person one in data.People)
            {
                result.People.Add(PersonRepository.mapping(one));
            }
        }

        return result;
    }
    internal static Role mapping(ERole data, Role result = null)
    {
        if (data == null)
        {
            return null;
        }
        if (result == null)
        {
            result = new Role();
        }
        result.Id = data.Id;
        result.Name = data.Name;
        result.IsNpc = data.IsNpc;
        result.Icon = data.Icon;

        return result;
    }
}
