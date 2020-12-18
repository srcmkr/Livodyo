/// <summary>
/// Pair programming session 2 (23.11.2020)
/// Authors: Deniz Ulu, Benjamin Bolzmann
/// BIS-268 Mobile Computing, WiSe 2020/21, Merz
/// </summary>

using System;

namespace Livodyo.Models
{
    public class TagModel
    {
        // primary key
        public Guid Id { get; set; }

        // Tag data
        public string Tag { get; set; }
    }
}
