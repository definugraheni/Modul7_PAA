using Microsoft.AspNetCore.Mvc;
using Modul_7.Models;


namespace Modul_7.Controllers
{
    public class PersonController : Controller
    {
        private string __constr;
        public PersonController(IConfiguration configuration)
        {
            __constr = configuration.GetConnectionString("WebApiDatabase");
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("person")]
        public ActionResult<Person> ListPerson()
        {
            PersonContext context = new PersonContext(this.__constr);
            List<Person> ListPerson = context.ListPerson();
            return Ok(ListPerson);
        }
        [HttpGet("person/{id}")]
        public ActionResult<Person> getPersonById(int id)
        {
            PersonContext context = new PersonContext(this.__constr);
            Person person = context.getPersonById(id);
            return Ok(person);
        }
        [HttpGet("person/detail/{id}")]
        public ActionResult<PersonDetail> getPersonDetail(int id)
        {
            PDetailContext context = new PDetailContext(this.__constr);
            PersonDetail personDetail = context.getPersonDetail(id);
            return Ok(personDetail);
        }
    }
}
