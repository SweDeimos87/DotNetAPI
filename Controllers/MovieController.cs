using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;


namespace DotnetAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly HttpClient _client = new HttpClient();
        public MovieController(IHttpClientFactory clientFactory)
        {
            _client = clientFactory.CreateClient();
        }




        // Fetch all movies
        [HttpGet]
        public async Task<IActionResult> GetAllMovies()
        {
            var response = await _client.GetAsync("http://localhost:3000/movies");
            var movies = await response.Content.ReadAsStringAsync();
            return Ok(movies);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddMovie(string title, string director, string genre, int releaseYear)
        {
            var movieData = new
            {
                title = title,
                director = director,
                genre = genre,
                releaseYear = releaseYear
            };

            var response = await _client.PostAsync("http://localhost:3000/movies/add",
                    new StringContent(JsonSerializer.Serialize(movieData), Encoding.UTF8, "application/json"));



            if (response.IsSuccessStatusCode)
                return RedirectToAction("MovieUI");
            else
                return BadRequest("Failed to add movie");
        }

        [HttpPost("delete/{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var response = await _client.DeleteAsync($"http://localhost:3000/movies/delete/{id}");

            if (response.IsSuccessStatusCode)
                return RedirectToAction("MovieUI");
            else
                return BadRequest("Failed to delete movie");
        }



        // Serve the Movie UI
        [HttpGet("ui")]
        public IActionResult MovieUI()
        {
            return View("Index");
        }

        private IActionResult View(string v)
        {
            throw new NotImplementedException();
        }

        

    }
}
