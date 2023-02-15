using FootBalLife.GameDB.Entities;
using FootBalLife.GameDB.Models;

namespace FootBalLife.GameDB.Repositories;
public class PersonRepository : _BaseRepository
{
    public List<EPerson> Retrive()
    {
        List<EPerson> result = new List<EPerson>();
        var listData = context.People;
        foreach (Person data in listData)
        {
            result.Add(mapping(data, false));
        }
        return result;
    }
    public EPerson Retrive(string Id)
    {
        Person? data = context.People.Find(Id);
        if (data != null)
        {
            return mapping(data,false);
        }
        return new EPerson();
    }

    public bool Modify(EPerson eData)
    {
        bool result = false;
        Person data = context.People.Find(eData.Id);

        if (data == null)
        {
            data = mapping(eData, data);
            data.Id = Guid.NewGuid().ToString();
            context.People.Add(data);
            context.SaveChanges();
            result = true;
        }
        else
        {
            data = mapping(eData, data);
            context.People.Update(data);
            context.SaveChanges();
            result = true;
        }
        
        return result;

    }
    public bool Delete(string Id)
    {
        Person? data = context.People.Find(Id);
        if (data != null)
        {
            context.People.Remove(data);
            context.SaveChanges();
            return true;
        }
        return false;
    }

    internal static EPerson mapping(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return null;
        }
        Person data = context.People.Find(id);
        return mapping(data, true);
    }
    internal static EPerson mapping(Person data, bool noLoop = true)
    {
        if (data == null)
        {
            return null;
        }
        EPerson result = new EPerson();

        result.Id = data.Id;
        result.Name = data.Name;
        result.Surname = data.Surname;
        result.Birthday = data.Birthday;
        result.CurrentRoleId = data.CurrentRoleId;
        result.CountryId = data.CountryId;
        result.Icon = data.Icon;

        if (!noLoop)
        {
            result.Agent = AgentRepository.mapping(data.Id);
            result.Coach = CoachRepository.mapping(data.Id);
            result.Country = CountryRepository.mapping(data.CountryId);
            result.CurrentRole = RoleRepository.mapping(data.CurrentRoleId.Value);
            result.Director = DirectorRepository.mapping(data.Id);
            result.Scout = ScoutRepository.mapping(data.Id);
            result.Player = PlayerRepository.mapping(data.Id);
            foreach (Contract one in data.Contracts)
            {
                result.Contracts.Add(ContractRepository.mapping(one));
            }
        }

        return result;
    }
    internal static Person mapping(EPerson data, Person result = null)
    {
        if (data == null)
        {
            return null;
        }
        if (result == null)
        {
            result = new Person();
        }
        result.Id = data.Id;
        result.Name = data.Name;
        result.Surname = data.Surname;
        result.Birthday = data.Birthday;
        result.CurrentRoleId = data.CurrentRoleId;
        result.CountryId = data.CountryId;
        result.Icon = data.Icon;


        result.Agent = AgentRepository.mapping(data.Agent, result.Agent);
        result.Coach = CoachRepository.mapping(data.Coach, result.Coach);
        result.Country = CountryRepository.mapping(data.Country, result.Country);
        result.CurrentRole = RoleRepository.mapping(data.CurrentRole, result.CurrentRole);
        result.Director = DirectorRepository.mapping(data.Director, result.Director);
        result.Player = PlayerRepository.mapping(data.Player, result.Player);
        result.Scout = ScoutRepository.mapping(data.Scout, result.Scout);

        return result;
    }

   
}
