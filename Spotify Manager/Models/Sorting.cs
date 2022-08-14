using System;
using System.Collections.Generic;
using System.Text;

namespace Spotify_Manager.Models
{
    public class Sorting
    {
        public List<SortingType> Types { get;private set; }
        public Sorting()
        {
            Types = new List<SortingType>();
            LoadTypes();
        }

        private void LoadTypes()
        {
            Types.Add(new SortingType("Energy"));
            Types.Add(new SortingType("Tempo"));
        }

        public IEnumerable<SortingType> GetSortingTypes()
        {
            return Types;
        }
    }
}
