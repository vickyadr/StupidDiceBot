namespace Dice_Bot.Models
{
    public class login : BaseDataObject
    {
        int site = -1;
        public int Site
        {
            get { return site; }
            set { SetProperty(ref site, value); }
        }

        int currencies = -1;
        public int Currencies
        {
            get { return currencies; }
            set { SetProperty(ref currencies, value); }
        }

        string username = string.Empty;
        public string Username
        {
            get { return username; }
            set { SetProperty(ref username, value); }
        }

        string loginas = string.Empty;
        public string LoginAs
        {
            get { return loginas; }
            set { SetProperty(ref loginas, value); }
        }

        string password = string.Empty;
        public string Password
        {
            get { return password; }
            set { SetProperty(ref password, value); }
        }

        string facode = string.Empty;
        public string FACode
        {
            get { return facode; }
            set { SetProperty(ref facode, value); }
        }

        string msg = string.Empty;
        public string Message
        {
            get { return msg; }
            set { SetProperty(ref msg, value); }
        }

        bool isLoading = false;
        public bool IsLoading
        {
            get { return isLoading; }
            set { SetProperty(ref isLoading, value); }
        }

        bool islogining = false;
        public bool IsLogining
        {
            get { return islogining; }
            set { SetProperty(ref islogining, value); }
        }

        bool ismsg = false;
        public bool IsMessage
        {
            get { return ismsg; }
            set { SetProperty(ref ismsg, value); }
        }

        bool useCurrencies = false;
        public bool UseCurrencies
        {
            get { return useCurrencies; }
            set { SetProperty(ref useCurrencies, value); }
        }

    }
}
