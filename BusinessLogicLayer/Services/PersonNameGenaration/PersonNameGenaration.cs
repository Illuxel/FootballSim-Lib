using DatabaseLayer.Repositories;
using System;
using System.Collections.Generic;
using DatabaseLayer;
using System.Linq;

namespace BusinessLogicLayer.Services
{
    public class PersonNameGenaration
    {
        private Dictionary<int, List<PersonName>> _names;
        private Dictionary<int, List<PersonSurname>> _surnames;

        private PersonNameRepository _personNameRepository;
        internal Dictionary<int, List<PersonName>> Names
        {
            get
            {
                if (_names == null)
                {
                    _names = new Dictionary<int, List<PersonName>>();
                    var names = _personNameRepository.RetriveNames();
                    foreach (var name in names)
                    {
                        if (!_names.TryGetValue(name.CountryId, out List<PersonName> data))
                        {
                            data = new List<PersonName>();
                        }
                        data.Add(name);
                        _names[name.CountryId] = data;
                    }
                }

                return _names;
            }
        }
        internal Dictionary<int, List<PersonSurname>> Surnames
        { get
            {
                if (_surnames == null)
                {
                    _surnames = new Dictionary<int, List<PersonSurname>>();
                    var surnames = _personNameRepository.RetriveSurnames();
                    foreach (var surname in surnames)
                    {
                        if (!_surnames.TryGetValue(surname.CountryId, out List<PersonSurname> data))
                        {
                            data = new List<PersonSurname>();
                        }
                        data.Add(surname);
                        _surnames[surname.CountryId] = data;
                    }
                }

                return _surnames;
            }
        }

        public PersonNameGenaration()
        {
            this._personNameRepository = new PersonNameRepository();
        }


        public List<Person> CreatePersonNames(int countryId, int countOfPerson)
        {
            List<Person> people = new List<Person>();
            for(int t = 0; t < countOfPerson; t++)
            {
                people.Add(CreatePersonName(countryId));
            }
            return people;
        }
        public Person CreatePersonName(int countryId)
        {
            Person person = new Person();

            Random rnd = new Random();
            if (Names.TryGetValue(countryId, out List<PersonName> names))
            {
                person.Name = names.ElementAt(rnd.Next(0, names.Count - 1)).Name;
            }
            if (Surnames.TryGetValue(countryId, out List<PersonSurname> surnames))
            {
                person.Surname = surnames.ElementAt(rnd.Next(0, surnames.Count - 1)).Surname;
            }
            return person;
        }
    }

    
}
