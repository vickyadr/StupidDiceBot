using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dice_Bot.Modules
{

    public class SiteData
    {
        public _site SiteScript { get; set; }
        public string Name { get; set; }
        public bool UseCurrency { get; set; }
        public Dictionary<string, string> Currencies { get; set; }
        public string loginAs { get; set; }
    }
}
