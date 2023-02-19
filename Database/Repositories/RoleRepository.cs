using FootBalLife.Database.Entities;
using FootBalLife.Database.Models;

namespace FootBalLife.Database.Repositories;
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
    public ERole Retrive(long ID)
    {
        Role? data = context.Roles.Find(ID);
        if (data != null)
        {
            return mapping(data);
        }
        return new ERole();
    }

    public bool Modify(ERole eData)
    {
        bool result = false;
        Role data = context.Roles.Find(eData.ID);

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
    public bool Delete(long ID)
    {
        Role? data = context.Roles.Find(ID);
        if (data != null)
        {
            context.Roles.Remove(data);
            context.SaveChanges();
            return true;
        }
        return false;
    }

    internal static ERole mapping(long ID)
    {
        if (ID == 0)
        {
            return null;
        }
        Role data = context.Roles.Find(ID);
        return mapping(data, true);
    }
    internal static ERole mapping(Role data, bool noLoop = true)
    {
        if (data == null)
        {
            return null;
        }
        ERole result = new ERole();
        result.ID = data.ID;
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
        result.ID = data.ID;
        result.Name = data.Name;
        result.IsNpc = data.IsNpc;
        result.Icon = data.Icon;

        return result;
    }
}
