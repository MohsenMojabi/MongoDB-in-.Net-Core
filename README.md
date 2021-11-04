# Using MongoDB in .Net Core

MongoDB is a source-available cross-platform document-oriented database program. Classified as a NoSQL database program, MongoDB uses JSON-like documents with optional schemas. MongoDB is developed by MongoDB Inc. and licensed under the Server Side Public License (SSPL).

# Installing MongoDB and creating the database on windows
Download and install the MongoDB Server and Client from the below URL:
> https://www.mongodb.com/download-center/community

Remember to install the “MongoDB Compass Community” client as well.
You can use MongoDBCompass to create and manipulate your databases.

# Working with MongoDB database from an ASP.NET Core Web API application

1- Add the MongoDB.Driver NuGet package to youe Project. This will allow us to access the MongoDB database via friendly APIs.
```
MongoDB.Driver
```

2- Add MongoDB settings to the appsettings.config file :
```
"PersonDatabaseSettings": {
    "PersonCollectionName": "Person",
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "PersonDB"
  },
```

3- Create a new folder called “Models” and create the PersonDatabaseSettings.cs file with the following code inside it:
```
public class PersonDatabaseSettings : IPersonDatabaseSettings
    {
        public string PersonCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IPersonDatabaseSettings
    {
        public string PersonCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
```

4- Create the Person.cs file inside “Models” Folder with these code:
```
public class Person
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

    }
```

5- Create a folder called “Services” and add a new class called “PersonService.cs” to it as shown below:
```
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
```

6- Configure the required services in the Startup.cs file as shown below:
```
public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<PersonDatabaseSettings>(
                Configuration.GetSection(nameof(PersonDatabaseSettings)));

            services.AddSingleton<IPersonDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<PersonDatabaseSettings>>().Value);

            services.AddSingleton<PersonService>();
            services.AddControllers();
        }
```

7- Finally add a new controller in “Controllers” folder called “PersonController” as shown below:
```
[Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly PersonService _personService;

        public PersonController(PersonService personService)
        {
            _personService = personService;
        }

        [HttpGet]
        public ActionResult<List<Person>> Get() =>
            _personService.Get();

        [HttpGet("{id:length(24)}", Name = "GetPerson")]
        public ActionResult<Person> Get(string id)
        {
            var person = _personService.Get(id);

            if (person == null)
            {
                return NotFound();
            }

            return person;
        }

        [HttpDelete("{id:length(24)}", Name = "DeletePerson")]
        public ActionResult<Person> Delete(string id)
        {
            var person = _personService.Get(id);

            if (person == null)
            {
                return NotFound();
            }

            _ = _personService.Delete(id);

            return Ok(person);
        }

        [HttpPut]
        public ActionResult<Person> Add([FromBody] Person person)
        {
            _personService.Add(person);
            return Ok(person);
        }

        [HttpPost]
        public ActionResult<Person> Update([FromBody] Person person)
        {
            var existperson = _personService.Get(person.Id);

            if (existperson == null)
            {
                return NotFound();
            }

            _personService.Update(person);
            return Ok(person);
        }

    }
```

Now You can use PostMan to test your project.

