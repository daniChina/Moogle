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

        // public static string GetSnippet(string document,Dictionary<string, float> queryTfIdf)
        // {
        //     document=File.ReadAllText(document).ToLower();
        //     string[] sentences = document.Split(".", StringSplitOptions.RemoveEmptyEntries);
        //     char[] separator = { ',', ' ', ';', ':', '"', '-', '+', '=', ')', '(', '|', '.', '#', '@', '%', '&', '/', '/', '{', '}', '[', ']' };
        //     //Este diccionario tiene las oraciones , las palabras de esa oracion y su frecuencia  
        //     Dictionary<string, Dictionary<string, float>> paragraph = new Dictionary<string, Dictionary<string, float>>();
        //     foreach (var item in sentences)
        //     {
        //         Dictionary<string, float> temp = new Dictionary<string, float>();
        //         string[] wordsInDocument = item.Split(separator, StringSplitOptions.RemoveEmptyEntries);
        //         float maxValue= 0;
        //         foreach (var kvp in wordsInDocument)
        //         {
        //             if (!temp.ContainsKey(kvp)) temp.Add(kvp, 1);
        //             else temp[kvp]++;

        //             maxValue=Math.Max(temp[kvp],maxValue);
        //         }
        //         foreach (var elmt in temp)
        //         {
        //             temp[elmt.Key]= temp[elmt.Key]/maxValue;
        //         }
        //         paragraph.TryAdd(item, temp);
        //     }
        //     // Dictionary<string,Dictionary<string,float>> SentencesWeight=Initialize.DocWeight(paragraph,Initialize.idf);
        //     // Dictionary<string,float> SentencesScore = Score(SentencesWeight,queryTfIdf);

        //     return paragraph.ElementAt(paragraph.Count-1).Key;
        // }

    }
}



