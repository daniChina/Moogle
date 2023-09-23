using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoogleEngine
{
    public class ProcessQuery
    {

        public Dictionary<string, float> idfQuery = new Dictionary<string, float>();

        public static string[] NormalizeQuery(string query)//*
        {
            query = query.ToLower();
            char[] delimiters = new char[] { ' ', '.', ';', ':', '!', '?', ',', '+', '/', '(', ')', '<', '>', '{', '}', '[', ']', '~', '*', '&', '%', '$', '#', '@' };
            string[] QueryNormalized = query.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            return QueryNormalized;

        }

        public static Dictionary<string, float> FillQueryWordsTf(string query)//*
        {
            string[] Normalized = NormalizeQuery(query);
            Dictionary<string, float> queryWords = new Dictionary<string, float>();
            float maxValue = 0;
            foreach (var elm in Normalized)
            {
                if (!(queryWords.ContainsKey(elm)))
                {
                    queryWords.Add(elm, 1);
                }
                else queryWords[elm]++;
                maxValue = Math.Max(maxValue, queryWords[elm]);
            }
            foreach (var item in queryWords)
            {
                queryWords[item.Key] = queryWords[item.Key] / maxValue;
            }
            return queryWords;

        }
        public static Dictionary<string, float> TfIdfQuery(Dictionary<string, float> QueryNormalized, Dictionary<string, float> idf)
        {
           Dictionary<string,float> tempTfIdf = new Dictionary<string, float>();
           foreach (var item in QueryNormalized)
           {
            if (!(idf.ContainsKey(item.Key))) idf.Add(item.Key,1);
           }
           foreach (var kvp in QueryNormalized)
           {
             tempTfIdf.Add(kvp.Key,idf[kvp.Key]*QueryNormalized[kvp.Key]);
           }

            return tempTfIdf;
        }
        
    }
}

