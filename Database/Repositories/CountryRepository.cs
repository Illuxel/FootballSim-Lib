using FootBalLife.GameDB.Entities;
using FootBalLife.GameDB.Models;

namespace FootBalLife.GameDB.Repositories;
public class CountryRepository : _BaseRepository
{
    public List<ECountry> Retrive()
    {
        List<ECountry> result = new List<ECountry>();
        var listData = context.Countries;
        foreach (Country data in listData)
        {
            Console.WriteLine(data.Leagues);
            result.Add(mapping(data));
        }
        return result;
    }
    public ECountry Retrive(long Id)
    {
        Country? data = context.Countries.Find(Id);
        if (data != null)
        {
            return mapping(data);
        }
        return new ECountry();
    }

    public bool Modify(ECountry eData)
    {
        bool result = false;
        Country data = context.Countries.Find(eData.Id);

        if (data == null)
        {
            data = mapping(eData, data);
            context.Countries.Add(data);
            context.SaveChanges();
            result = true;
        }
        else
        {
            data = mapping(eData, data);
            context.Countries.Update(data);
            context.SaveChanges();
            result = true;
        }
        return result;

    }
    public bool Delete(long Id)
    {
        Country? data = context.Countries.Find(Id);
        if (data != null)
        {
            context.Countries.Remove(data);
            context.SaveChanges();
            return true;
        }
        return false;
    }

    internal static ECountry mapping(long Id)
    {
        if (Id == 0)
        {
            return null;
        }
        Country data = context.Countries.Find(Id);
        return mapping(data, true);
    }

    internal static ECountry mapping(Country data, bool noLoop = true)
    {
        if (data == null)
        {
            return null;
        }
        ECountry result = new ECountry();
        result.Id = data.Id;
        result.Name = data.Name;
        result.Icon = data.Icon;
        if (!noLoop)
        {
            foreach (League one in context.Leagues.Where(x => x.CountryId == data.Id))
            {
                result.Leagues.Add(LeagueRepository.mapping(one));
            }
            foreach (Person one in context.People.Where(x => x.CountryId == data.Id))
            {
                result.People.Add(PersonRepository.mapping(one));
            }
        }

        return result;
    }
    internal static Country mapping(ECountry data, Country result = null)
    {
        if (data == null)
        {
            return null;
        }
        if (result == null)
        {
            result = new Country();
        }
        result.Id = data.Id;
        result.Name = data.Name;
        result.Icon = data.Icon;
        return result;
    }
}
