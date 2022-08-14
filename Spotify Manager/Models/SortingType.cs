using System;
using System.Collections.Generic;
using System.Text;

namespace Spotify_Manager.Models
{
    public class SortingType
    {
        public string Name { get; }
        public SortingType(string name)
        {
            Name = name;
        }
    }
}
