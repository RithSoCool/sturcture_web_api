namespace BCRM_App.Extentions
{
    public static class GetValueFromPropertyExtention
    {
        public static string GetValueFromProperty_By_Key<T>(this T obj, string key)
        {
            var value = obj.GetType().GetProperty(key).GetValue(obj);
            if (value == null) return "";
            return value.ToString();
        }

        public static string GetValueFromProperty_By_Index<T>(this T obj, int index)
        {
            try
            {
                var value = obj.GetType().GetProperties()[index].GetValue(obj);
                return value.ToString();
            }
            catch
            {
                return "";
            }
        }
    }
}
