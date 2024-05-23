using Microsoft.AspNetCore.Mvc;
using Modul_7.Models;
using System;
using Newtonsoft.Json;


namespace Modul_7.Controllers
{
    public class PDetailController : ControllerBase
    {
        private readonly string _constr;
        private readonly HttpClient _httpClient;

        public PDetailController(IConfiguration configuration)
        {
            _constr = configuration.GetConnectionString("WebApiDatabase");
            _httpClient = new HttpClient();
        }

        [HttpGet("import-data")]
        public async Task<IActionResult> ImportPersonDetails()
        {
            string apiUrl = "https://dummy-user-tan.vercel.app/user";
            List<PersonDetailApi> personDetailsFromApi;

            try
            {
                var response = await _httpClient.GetStringAsync(apiUrl);
                personDetailsFromApi = JsonConvert.DeserializeObject<List<PersonDetailApi>>(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving data from API: {ex.Message}");
            }

            try
            {
                PDetailContext context = new PDetailContext(_constr);
                context.InsertPersonDetails(personDetailsFromApi);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error inserting data into database: {ex.Message}");
            }

            return Ok("Data imported successfully");
        }

    }
}

