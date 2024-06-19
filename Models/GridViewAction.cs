using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeAutoClicker.Models
{
    public class GridViewAction
    {
        public string Type;
        public string Description;

        public GridViewAction(string type, string description)
        {
            Type = type;
            Description = description;
        }
    }
}
