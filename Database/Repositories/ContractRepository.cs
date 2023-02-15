using FootBalLife.GameDB.Entities;
using FootBalLife.GameDB.Models;

namespace FootBalLife.GameDB.Repositories;
public class ContractRepository : _BaseRepository
{
    public List<EContract> Retrive()
    {
        List<EContract> result = new List<EContract>();
        var listData = context.Contracts;
        foreach (Contract data in listData)
        {
            result.Add(mapping(data));
        }
        return result;
    }
    public EContract Retrive(string Id)
    {
        Contract? data = context.Contracts.Find(Id);
        if (data != null)
        {
            return mapping(data);
        }
        return new EContract();
    }

    public bool Modify(EContract eData)
    {
        bool result = false;
        Contract data = context.Contracts.Find(eData.Id);

        if (data == null)
        {
            data = mapping(eData, data);
            Guid myuuid = Guid.NewGuid();
            string myuuidAsString = myuuid.ToString();
            data.Id = myuuidAsString;
            context.Contracts.Add(data);
            context.SaveChanges();
            result = true;
        }
        else
        {
            data = mapping(eData, data);
            context.Contracts.Update(data);
            context.SaveChanges();
            result = true;
        }
        return result;

    }
    public bool Delete(string Id)
    {
        Contract? data = context.Contracts.Find(Id);
        if (data != null)
        {
            context.Contracts.Remove(data);
            context.SaveChanges();
            return true;
        }
        return false;
    }

    internal static EContract mapping(string Id)
    {
        if (Id == "")
        {
            return null;
        }
        Contract data = context.Contracts.Find(Id);
        return mapping(data, true);
    }
    internal static EContract mapping(Contract data, bool noLoop = true)
    {
        if (data == null)
        {
            return null;
        }
        EContract result = new EContract();
        result.Id = data.Id;
        result.SeasonFrom = data.SeasonFrom;
        result.SeasonTo = data.SeasonTo;
        result.TeamId= data.TeamId;
        result.PersonId= data.PersonId;
        result.Price= data.Price;

        if (!noLoop)
        {
            result.Person = PersonRepository.mapping(data.PersonId);
            result.Team = TeamRepository.mapping(data.TeamId);
            foreach (Player one in data.Players)
            {
                result.Players.Add(PlayerRepository.mapping(one));
            }
           
        }

        return result;
    }
    internal static Contract mapping(EContract data, Contract result = null)
    {
        if (data == null)
        {
            return null;
        }
        if (result == null)
        {
            result = new Contract();
        }
        result.Id = data.Id;
        result.Id = data.Id;
        result.SeasonFrom = data.SeasonFrom;
        result.SeasonTo = data.SeasonTo;
        result.TeamId = data.TeamId;
        result.PersonId = data.PersonId;
        result.Price = data.Price;

        result.Person = PersonRepository.mapping(data.Person, result.Person);
        result.Team = TeamRepository.mapping(data.Team, result.Team);
        return result;
    }
}
