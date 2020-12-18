/// <summary>
/// Pair programming session 1 (12.11.2020)
/// Authors: Deniz Ulu, Benjamin Bolzmann
/// BIS-268 Mobile Computing, WiSe 2020/21, Merz
/// </summary>

namespace Livodyo.API.Services
{
    public class StateManager
    {
        public List<AudioBookModel> AudioBooks { get; set; }
        public List<TagModel> Tags { get; set; }
        public List<AuthorModel> Authors { get; set; }

        public StateManager()
        {
            using (var db = new LiteDatabase(@"Livodyo.db"))
            {
                // Get audiobook collection
                var audioBooksCollection = db.GetCollection<AudioBookModel>("audiobooks");
                // store collection content to object list
                AudioBooks = audioBooksCollection.FindAll().ToList();

                // does same for tags
                var tagsCollection = db.GetCollection<TagModel>("tags");
                Tags = tagsCollection.FindAll().ToList();

                // does same for authors 
                var authorsCollection = db.GetCollection<AuthorModel>("authors");
                Authors = authorsCollection.FindAll().ToList();
            }
        }

        public void SaveChanges()
        {
            using (var db = new LiteDatabase(@"Livodyo.db"))
            {
                // get audiobook collection
                var audioBooksCollection = db.GetCollection<AudioBookModel>("audiobooks");
                // remove all the data is faster than foreach + check if changed xor still exists
                audioBooksCollection.DeleteAll();
                // add everything the state has back to persistence
                audioBooksCollection.InsertBulk(AudioBooks);

                // does same for tags
                var tagsCollection = db.GetCollection<TagModel>("tags");
                tagsCollection.DeleteAll();
                tagsCollection.InsertBulk(Tags);

                // does same for authors
                var authorsCollection = db.GetCollection<AuthorModel>("authors");
                authorsCollection.DeleteAll();
                authorsCollection.InsertBulk(Authors);
            }
        }
    }
}
