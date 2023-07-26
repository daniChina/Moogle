using System.Text;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoogleEngine
{
    public class Initialize
    {
        public static string ContentPath = Path.Join("..", "Content");
        public static string[] files = Directory.GetFiles(ContentPath);
       
        public static Dictionary<string, Dictionary<string, float>> filesWords = FillFilesWords(files);
        public static Dictionary<string, float> idf = Fillidf(filesWords);
        public static Dictionary<string, Dictionary<string, float>> docTfIdf = DocWeight(filesWords, idf);



        public static string[] Read(string file)//*
        { //Este metodo lee los documentos e invoca al metodo Normalize que normaliza los documentos
            string[] normalized = Normalize(File.ReadAllText(file));
            return normalized;
        }

        public static string[] Normalize(string file)//*
        { // Este metodo recibe un string que son los textos en los documentos y los normaliza
            file = file.Normalize(NormalizationForm.FormD);
            string[] Normalized = Regex.Replace(file.ToLower(), @"[^\da-z ]", "").Split(" ", StringSplitOptions.RemoveEmptyEntries);
            return Normalized;
        }

        public static Dictionary<string, Dictionary<string, float>> FillFilesWords(string[] files)
        {
            Dictionary<string, Dictionary<string, float>> filesWords = new Dictionary<string, Dictionary<string, float>>();
            foreach (var i in files)
            {
                string[] normalized = Read(i);
                Dictionary<string, float> wordsInDoc = new Dictionary<string, float>();
                float maxValue = 0;
                foreach (var word in normalized)
                {
                    if (!wordsInDoc.ContainsKey(word)) wordsInDoc.Add(word, 1);
                    else wordsInDoc[word]++;

                    maxValue = Math.Max(maxValue, wordsInDoc[word]);
                }
                foreach (var item in wordsInDoc)
                {
                    wordsInDoc[item.Key] = wordsInDoc[item.Key] / maxValue;
                }
                filesWords.Add(i, wordsInDoc);
            }
            return filesWords;
        }
        public static Dictionary<string, float> Fillidf(Dictionary<string, Dictionary<string, float>> filesWords)
        {
            Dictionary<string, float> idf = new Dictionary<string, float>();
            int N = files.Length;
            foreach (var dict in filesWords.Values)
            {
                foreach (var kvp in dict.Keys)
                {
                    if (idf.ContainsKey(kvp)) idf[kvp] = idf[kvp] + 1;
                    else idf.Add(kvp, 1);
                }
            }
            foreach (var item in idf.Keys)
            {
                idf[item] = (float)Math.Log10(N / idf[item]);
            }
            return idf;

        }
        public static Dictionary<string, Dictionary<string, float>> DocWeight(Dictionary<string, Dictionary<string, float>> filesWords, Dictionary<string, float> idf)
        {
            Dictionary<string, Dictionary<string, float>> temp = new Dictionary<string, Dictionary<string, float>>();
            foreach (var item in filesWords.Keys)
            {
                temp.Add(item, new Dictionary<string, float>());

            }
            int index = 0;
            foreach (var item in filesWords.Values)
            {
                foreach (var kvp in item.Keys)
                {
                    temp.ElementAt(index).Value.Add(kvp, item[kvp] * idf[kvp]);
                }
                index++;
            }
            return temp;
        }

    }
}