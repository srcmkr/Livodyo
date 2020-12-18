/// <summary>
/// Pair programming session 1 (12.11.2020)
/// Authors: Deniz Ulu, Benjamin Bolzmann
/// BIS-268 Mobile Computing, WiSe 2020/21, Merz
/// </summary>

using Livodyo.API.Services;

namespace Livodyo.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private StateManager State { get; }

        public TagsController([FromServices] StateManager state)
        {
            // assign state object, make it referable in class
            State = state;
        }

        [HttpGet]
        public IEnumerable<TagModel> GetTags()
        {
            // returns full list of all tags
            return State.Tags.ToList();
        }

        [HttpDelete("{tagId}")]
        public bool DeleteTag(Guid tagId)
        {
            var toDel = State.Tags.SingleOrDefault(c => c.Id == tagId);
            if (toDel == null) return false;

            State.Tags.Remove(toDel);
            State.SaveChanges();
            return true;
        }

        [HttpGet("{tagId}/{maxResults}")]
        [HttpGet("{tagId}")]
        public IEnumerable<AudioBookModel> GetAudioBooksByTag(Guid tagId, int maxResults = 9999)
        {
            // returns new List of AudioBooks with maxlength of maxResults where tag-id fits
            return State.AudioBooks.Where(c => c.Tags.Contains(tagId)).Take(maxResults).ToList();
        }

        [HttpPost]
        public TagModel CreateAuthor(TagModel newModel)
        {
            // force overwrite model id
            newModel.Id = Guid.NewGuid();

            // add author to state
            State.Tags.Add(newModel);

            // state persistance
            State.SaveChanges();

            // returns model with new id
            return newModel;
        }
    }
}
