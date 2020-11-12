using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Livodyo.API.Services;
using Livodyo.Models;
using System.Linq;
using System;

namespace Livodyo.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AudioBooksController : ControllerBase
    {
        private StateManager State { get; }

        public AudioBooksController([FromServices] StateManager state)
        {
            // assign state object, make it referable in class
            State = state;
        }

        [HttpGet]
        public IEnumerable<AudioBookModel> GetAllAudioBooks()
        {
            // returns full list of all audiobooks in system
            return State.AudioBooks.ToList();
        }

        [HttpDelete("{audiobookId}")]
        public bool DeleteAudioBook(Guid audiobookId)
        {
            var toDel = State.AudioBooks.FirstOrDefault(c => c.Id == audiobookId);
            if (toDel == null) return false;

            State.AudioBooks.Remove(toDel);
            State.SaveChanges();
            return true;
        }

        [HttpPost]
        public AudioBookModel CreateAudioBook(AudioBookModel newModel)
        {
            // validate first
            if (State.Authors.All(c => c.Id != newModel.AuthorId)) return null;
            if (newModel.Tags.Any(tag => State.Tags.All(c => c.Id != tag)))
            {
                return null;
            }               

            // override the given id anyway (force recreate)
            newModel.Id = Guid.NewGuid();

            // add new model to state
            State.AudioBooks.Add(newModel);

            // state persistance
            State.SaveChanges();

            // returns model with new guid
            return newModel;
        }
        
    }
}
