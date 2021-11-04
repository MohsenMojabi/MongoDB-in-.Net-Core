using MongoDB.Driver;
using MongoDBSample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDBSample.Services
{
    public class PersonService
    {
        private readonly IMongoCollection<Person> _people;
        public PersonService(IPersonDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _people = database.GetCollection<Person>(settings.PersonCollectionName);

        }

        public List<Person> Get()
        {
            List<Person> people;
            people = _people.Find(person => true).ToList();
            return people;
        }

        public Person Get(string id) =>
            _people.Find<Person>(person => person.Id == id).FirstOrDefault();

        public DeleteResult Delete(string id) =>
            _people.DeleteOne<Person>(person => person.Id == id);

        public Person Add(Person person)
        {
            _people.InsertOne(person);
            return person;
        }

        public Person Update(Person person)
        {
            _people.ReplaceOne<Person>(prs => prs.Id == person.Id, person);
            return person;
        }


    }
}
