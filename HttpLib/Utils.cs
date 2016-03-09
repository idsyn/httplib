using System;

namespace HttpLib
{
    public static class Utils
    {
        /// <summary>
        /// serialize an array of kv pairs into a url encoded query string
        /// </summary>
        /// <param name="parameters">kv pair array</param>
        /// <returns>url encoded query string</returns>
        public static string SerializeQueryString(object parameters)
        {
            var querystring = "";
            var i = 0;
            try
            {
                var properties = parameters.GetType().GetProperties();

                foreach (var property in properties)
                {
                    querystring += property.Name + "=" + Uri.EscapeDataString(property.GetValue(parameters, null).ToString());

                    if (++i < properties.Length) querystring += "&";
                }
            }
            catch (NullReferenceException e)
            {
                throw new ArgumentNullException("paramters cannot be null", e);
            }
            return querystring;
        }
    }
}
