using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeAutoClicker.Models
{
    public class Action
    {
        public Action(string type, string? clicktype , int? xpos, int? ypos , int? milliseconds , string? message , string? cmdChar )
        {
            Type = type;
            ClickType = clicktype;
            Xpos = xpos;
            Ypos = ypos;
            Milliseconds = milliseconds;
            Message = message;
            CmdChar = cmdChar;
        }

        public string? Type { get; set; }
        public string? ClickType { get; set; }
        public int? Xpos { get; set; }
        public int? Ypos { get; set; }
        public int? Milliseconds { get; set; }
        public string? Message { get; set; }
        public string? CmdChar { get; set; }
    }
}
