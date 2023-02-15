using FootBalLife.GameDB.Entities;
using FootBalLife.GameDB.Models;

namespace FootBalLife.GameDB.Repositories;
public class PlayerRepository : _BaseRepository
{
    public List<EPlayer> Retrive()
    {
        List<EPlayer> result = new List<EPlayer>();
        var listData = context.Players;
        foreach(var data in listData)
        {
            result.Add(mapping(data));
        }
        return result;
    }
    public EPlayer Retrive(string Id)
    {
        var data = context.Players.Find(Id);
        if (data != null)
        {
            return mapping(data);
        }
        return new EPlayer();
    }

    public bool Modify(EPlayer eData)
    {
        bool result = false;
        var data = context.Players.Find(eData.PersonId);

        if (data == null)
        {
            data = mapping(eData);
            context.Players.Add(data);
            context.SaveChanges();
            result = true;
        }
        else
        {
            data = mapping(eData, data);
            context.Players.Update(data);
            context.SaveChanges();
            result = true;
        }
        return result;

    }
    public bool Delete(string Id)
    {
        Player? data = context.Players.Find(Id);
        if (data != null)
        {
            context.Players.Remove(data);
            context.SaveChanges();
            return true;
        }
        return false;
    }
    internal static EPlayer? mapping(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return null;
        }
        Player data = context.Players.Find(id);
        return mapping(data, true);
    }
    internal static EPlayer? mapping(Player? data, bool noLoop = true)
    {
        if (data == null)
        {
            return null;
        }
        EPlayer result = new EPlayer();

        result.PersonId = data.PersonId;
        result.Speed= data.Speed;
        result.KickCount = data.KickCount;
        result.Endurance= data.Endurance;
        result.Reflex = data.Reflex;
        result.Physics = data.Physics;
        result.Position = data.Position;
        result.Technique = data.Technique;
        result.Passing = data.Passing;

        if (!noLoop)
        {
            result.Person = PersonRepository.mapping(data.PersonId);
            result.Contract = ContractRepository.mapping(data.ContractId);
            result.PositionNavigation = PositionRepository.mapping(data.PositionId.Value);
        }

        

        return result;
    }
    internal static Player? mapping(EPlayer data, Player result = null)
    {
        if (data == null)
        {
            return null;
        }
        if (result == null)
        {
            result = new Player();
        }
        result.PersonId = data.PersonId;
        result.Speed = data.Speed;
        result.KickCount = data.KickCount;
        result.Endurance = data.Endurance;
        result.Reflex = data.Reflex;
        result.Physics = data.Physics;
        result.Position = data.Position;
        result.Technique = data.Technique;
        result.Passing = data.Passing;


        result.Person = PersonRepository.mapping(data.Person, result.Person);
        result.Contract = ContractRepository.mapping(data.Contract, result.Contract);
        result.PositionNavigation = PositionRepository.mapping(data.PositionNavigation, result.PositionNavigation);

        return result;
    }
}
