using Livodyo.API.Services;

namespace Livodyo.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private StateManager State { get; }

        public AuthorsController([FromServices] StateManager state)
        {
            // assign state object, make it referable in class
            State = state;
        }

        [HttpGet]
        public IEnumerable<AuthorModel> GetAuthors()
        {
            // returns full list of authors
            return State.Authors.ToList();
        }

        [HttpGet("{authorId}/{maxResults}")]
        [HttpGet("{authorId}")]
        public IEnumerable<AudioBookModel> GetAudioBooksByAuthor(Guid authorId, int maxResults = 9999)
        {
            // returns new List of AudioBooks with maxlength of maxResults where author-id fits
            return State.AudioBooks.Where(c => c.AuthorId == authorId).Take(maxResults).ToList();
        }

        [HttpPost]
        public AuthorModel CreateAuthor(AuthorModel newModel)
        {
            // force overwrite model id
            newModel.Id = Guid.NewGuid();

            // add author to state
            State.Authors.Add(newModel);

            // state persistance
            State.SaveChanges();

            // returns model with new id
            return newModel;
        }
    }
}
