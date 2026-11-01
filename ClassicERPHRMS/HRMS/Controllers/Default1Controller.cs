using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRMS.Controllers
{
    [AllowAnonymous]
    public class Default1Controller : Controller
    {
        public ActionResult Index()
        {
            var sas  = new Person { Id = 1, Name = "John Doe", Age = 25 };
            sas.Children = GetPeopleList();
            sas.GrandChild = GetPeopleList();
            List<Person> peopleList = GetPeopleList(); // Replace with your data retrieval logic
            return View(sas);
        }

        [HttpPost]
        //public ActionResult ProcessPeople(List<Person> people)
        public ActionResult ProcessPeople(Person people)
        {
            if (ModelState.IsValid)
            {
                // Validations passed, process the list of people
                //foreach (var person in people)
                //{
                //    // Save to the database or perform other actions
                //}

                return RedirectToAction("Index"); // Redirect to a success page
            }

            // ModelState is not valid, return to the form with errors
            return View("Index", people);
        }

        private List<Person> GetPeopleList()
        {
            // Replace this with your actual data retrieval logic
            List<Person> peopleList = new List<Person>
        {
            new Person { Id = 1, Name = "John Doe", Age = 25 },
            new Person { Id = 2, Name = "Jane Smith", Age = 30 },
            // Add more persons as needed
        };

            return peopleList;
        }
    }
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public List<Person> Children { get; set; }
        public List<Person> GrandChild { get; set; }
    }
}