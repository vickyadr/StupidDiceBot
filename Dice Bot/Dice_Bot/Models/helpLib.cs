using System;

namespace Dice_Bot.Models
{
    public class HelpItem : BaseDataObject
	{
		string name = string.Empty;
		public string Name
		{
			get { return name; }
			set { SetProperty(ref name, value); }
		}
        
        string desc = string.Empty;
        public string Description
        {
            get { return desc; }
            set { SetProperty(ref desc, value); }
        }
    }
}
