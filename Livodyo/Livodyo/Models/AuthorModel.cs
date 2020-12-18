/// <summary>
/// Pair programming session 2 (23.11.2020)
/// Authors: Deniz Ulu, Benjamin Bolzmann
/// BIS-268 Mobile Computing, WiSe 2020/21, Merz
/// </summary>

using System;

namespace Livodyo.Models
{
    public class AuthorModel
    {
        // primary key
        public Guid Id { get; set; }

        // Author data
        public string Name { get; set; }
    }
}
