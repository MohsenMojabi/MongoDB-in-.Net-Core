using MongoDB.Driver;
using MongoDBSample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDBSample.Services
{
    public class EmployeeService
    {
        private readonly IMongoCollection<Employee> _employees;
        public EmployeeService(IEmployeeDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _employees = database.GetCollection<Employee>(settings.EmployeesCollectionName);

        }

        public List<Employee> Get()
        {
            List<Employee> employees;
            employees = _employees.Find(emp => true).ToList();
            return employees;
        }

        public Employee Get(string id) =>
            _employees.Find<Employee>(emp => emp.Id == id).FirstOrDefault();

        public DeleteResult Delete(string id) =>
            _employees.DeleteOne<Employee>(emp => emp.Id == id);

        public Employee Add(Employee emp)
        {
            _employees.InsertOne(emp);
            return emp;
        }

        public Employee Update(Employee employee)
        {
            _employees.ReplaceOne<Employee>(emp => emp.Id == employee.Id, employee);
            return employee;
        }


    }
}
