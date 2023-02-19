using FootBalLife.Database.Entities;
using FootBalLife.Database.Models;

namespace FootBalLife.Database.Repositories;
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
    public EPlayer Retrive(string ID)
    {
        var data = context.Players.Find(ID);
        if (data != null)
        {
            return mapping(data);
        }
        return new EPlayer();
    }

    public bool Modify(EPlayer eData)
    {
        bool result = false;
        var data = context.Players.Find(eData.PersonID);

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
    public bool Delete(string ID)
    {
        Player? data = context.Players.Find(ID);
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

        result.PersonID = data.PersonID;
        result.Speed= data.Speed;
        result.KickCount = data.KickCount;
        result.Endurance= data.Endurance;
        result.Strike = data.Strike;
        result.Physics = data.Physics;
        result.Position = data.Position;
        result.Technique = data.Technique;
        result.Passing = data.Passing;

        if (!noLoop)
        {
            result.Person = PersonRepository.mapping(data.PersonID);
            result.Contract = ContractRepository.mapping(data.ContractID);
            result.PositionNavigation = PositionRepository.mapping(data.PositionID.Value);
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

        result.PersonID = data.PersonID;
        result.Speed = data.Speed;
        result.KickCount = data.KickCount;
        result.Endurance = data.Endurance;
        result.Strike = data.Strike;
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
