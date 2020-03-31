using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace M3Service.Util
{
    public class RestClientUtil
    {
        public static string GetInputParameters(object dataObject)
        {
            if (dataObject == null)
            {
                return "";
            }
            else
            {
                var builder = new StringBuilder();
                builder.Append("?");

                Type type = dataObject.GetType();

                foreach (var prop in type.GetProperties())
                {
                    bool value = true;
                    object[] attrs = prop.GetCustomAttributes(true);
                    foreach (object attr in attrs)
                    {
                        MapToM3 attribute = attr as MapToM3;
                        if (attribute != null)
                        {
                            value = attribute.Include;
                            break;
                        }
                    }

                    if (value)
                    {
                        var tempValue = prop.GetValue(dataObject, null);
                        if (tempValue != null && tempValue.ToString() != "")
                        {
                            builder.Append(prop.Name + "=" + prop.GetValue(dataObject, null));

                            if (prop.Name != type.GetProperties().Last().Name)
                            {
                                builder.Append("&");
                            }
                        }
                    }
                }
                return builder.ToString();
            }
        }

        public static string GetInputParametersFromJson(string dataObject)
        {
            if (String.IsNullOrEmpty(dataObject))
            {
                return "";
            }
            else
            {

                var jObj = (JObject)JsonConvert.DeserializeObject(dataObject);

                var query = "?" + String.Join("&",
                                jObj.Children().Cast<JProperty>()
                                .Select(jp => jp.Name + "=" + HttpUtility.UrlEncode(jp.Value.ToString())));

                return query;
            }
        }

        public static string GetOutputParameters(Type type)
        {

            var builder = new StringBuilder();

            foreach (var prop in type.GetProperties())
            {
                bool value = true;
                object[] attrs = prop.GetCustomAttributes(true);
                foreach (object attr in attrs)
                {
                    MapToM3 attribute = attr as MapToM3;
                    if (attribute != null)
                    {
                        value = attribute.Include;
                        break;
                    }
                }

                if (value)
                {
                    builder.Append(prop.Name);

                    if (prop.Name != type.GetProperties().Last().Name)
                    {
                        builder.Append(",");
                    }
                }
            }
            return builder.ToString();
        }

        public static List<string> GetProperties(Type type)
        {
            List<string> propertyList = new List<string>();

            foreach (var prop in type.GetProperties())
            {
                propertyList.Add(prop.Name);
            }

            return propertyList;
        }
    }
}
