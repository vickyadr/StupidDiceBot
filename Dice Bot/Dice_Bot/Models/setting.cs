namespace Dice_Bot.Models
{
    public class setting : BaseDataObject
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
            private set { SetProperty(ref desc, value); }
        }

        string format = string.Empty;
        public string Format
        {
            get { return format; }
            set { SetProperty(ref format, value); }
        }

        object val = null;
        public object Val
        {
            get { return val; }
            set { SetProperty(ref val, value); Description = string.Format(Format, value); }
        }

        public object[] piker { get; set; }
    }
}
