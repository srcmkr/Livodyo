using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Livodyo.API.Services;
using Livodyo.Models;
using System.Linq;
using System;

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
