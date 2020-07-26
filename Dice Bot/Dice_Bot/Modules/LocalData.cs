using Android.App;

namespace Dice_Bot.Modules
{
    public class LocalData
    {
        Android.Content.ISharedPreferencesEditor prefEdit { get; set; }
        Android.Content.ISharedPreferences pref { get; set; }

        public LocalData()
        {
            pref = Application.Context.GetSharedPreferences("setting", Android.Content.FileCreationMode.Private);
            prefEdit = pref.Edit();
        }

        public void SavePref(string sKey, bool val)
        {
            prefEdit.PutBoolean(sKey, val);
            prefEdit.Commit();
        }
        public void SavePref(string sKey, float val)
        {
            prefEdit.PutFloat(sKey, val);
            prefEdit.Commit();
        }
        public void SavePref(string sKey, long val)
        {
            prefEdit.PutLong(sKey, val);
            prefEdit.Commit();
        }
        public void SavePref(string sKey, int val)
        {
            prefEdit.PutInt(sKey, val);
            prefEdit.Commit();
        }
        public void SavePref(string sKey, string val)
        {
            prefEdit.PutString(sKey, val);
            prefEdit.Commit();
        }

        public string LoadPref(string sKey, string def = null)
        {
            return pref.GetString(sKey,def);
        }
        public int LoadPref(string sKey, int def = int.MinValue)
        {
            return pref.GetInt(sKey, def);
        }
        public long LoadPref(string sKey, long def = long.MinValue)
        {
            return pref.GetLong(sKey, def);
        }
        public float LoadPref(string sKey, float def = float.MinValue)
        {
            return pref.GetFloat(sKey, def);
        }
        public bool LoadPref(string sKey, bool def = false)
        {
            return pref.GetBoolean(sKey, def);
        }
    }
}
