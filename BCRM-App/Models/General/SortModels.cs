using System.Collections.Generic;

namespace BCRM_App.Models.General
{
    public class SortModels
    {
        public SortModels()
        {
            Ordering = new List<SortModel>();
        }

        public List<SortModel> Ordering { get; set; }
    }
}
