using System.Collections.Generic;
using System;

namespace FootBalLife.Database.Repositories
{
    public class MatchRepository
    {/*
        public List<Match> Retrive()
        {
            List<Match> result = new List<Match>();
            var listData = context.Matches;
            foreach (Match data in listData)
            {
                result.Add(mapping(data));

            }
            return result;
        }
        public Match Retrive(string ID)
        {
            Match? league = context.Matches.Find(ID);
            if (league != null)
            {
                return mapping(league);
            }
            return new Match();
        }

        public bool Modify(Match eData)
        {
            bool result = false;
            Match data = context.Matches.Find(eData.ID);

            if (data == null)
            {
                data = mapping(eData, data);
                Guid myuuid = Guid.NewGuid();
                string myuuidAsString = myuuid.ToString();
                data.ID = myuuidAsString;
                context.Matches.Add(data);
                context.SaveChanges();
                result = true;
            }
            else
            {
                data = mapping(eData, data);
                context.Matches.Update(data);
                context.SaveChanges();
                result = true;
            }
            return result;

        }
        public bool Delete(string ID)
        {
            Match? data = context.Matches.Find(ID);
            if (data != null)
            {
                context.Matches.Remove(data);
                context.SaveChanges();
                return true;
            }
            return false;
        }*/
    }
}

