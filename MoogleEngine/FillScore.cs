using System.Text;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoogleEngine
{
    public class FillScore
    {
        public static Dictionary<string, float> Score(Dictionary<string, Dictionary<string, float>> docTfIdf, Dictionary<string, float> queryTfIdf)
        {
            Dictionary<string, float> fileScore = new Dictionary<string, float>();
            float longQuery = 0;
            int index = 0;

            foreach (var word in queryTfIdf)
            {
                longQuery += (float)Math.Pow(word.Value, 2);
            }
            foreach (var item in docTfIdf.Values)
            {
                float num = 0;
                float longDoc = 0;

                foreach (var kvp in queryTfIdf)
                {
                    if (item.ContainsKey(kvp.Key))
                    {
                        num += item[kvp.Key] * kvp.Value;
                    }
                }

                foreach (var word in item)
                {
                    longDoc += (float)Math.Pow(word.Value, 2);
                }

                float score = num / (float)(Math.Sqrt(longDoc) * Math.Sqrt(longQuery));

                fileScore.Add(docTfIdf.ElementAt(index).Key, score);
                index++;
            }
            fileScore = fileScore.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            return fileScore;
        }



    }
}



