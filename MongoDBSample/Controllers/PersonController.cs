using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDBSample.Models;
using MongoDBSample.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDBSample.Controllers
{
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
}
