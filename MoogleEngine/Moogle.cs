namespace MoogleEngine;
using System.Text;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


public static class Moogle
{
    public static SearchResult Query(string query)
    {
        Dictionary<string, float> queryNormalizedTf = ProcessQuery.FillQueryWordsTf(query);//este diccionario tiene la palabra de la query y su tf
		   //Dictionary<string, float> querytf = ProcessQueryFillQueryWordsTf(query);
        Dictionary<string, float> queryTfIdf = ProcessQuery.TfIdfQuery(queryNormalizedTf, Initialize.idf);//Este dicc tiene el peso de la query
        Dictionary<string, float> fileScore = FillScore.Score(Initialize.docTfIdf, queryTfIdf);//Este es el dicc final donde esta el documento y el score que le corresponde 

        // Creo el array del tamaño de los documentos q tengo
        var items = new SearchItem[fileScore.Count];

        // le asigno a cada documento su score y su snippet
        GetItems(items, fileScore, queryNormalizedTf);

        // retorno el array de los documentos con su score
		 var finalItems= new SearchItem[4];

		// Results(finalItems);
		
        return new SearchResult(items, query);
    }

    public static void GetItems(SearchItem[] items, Dictionary<string, float> scores, Dictionary<string, float> queryNormalized)
    {
        // por cada documento q tenga en el dic de scores  se le asigna su score, titulo y snippet y lo guardardo
        // en el array de items
        int i = items.Length - 1; // creo el contador va a ir guiando donde agregar el elemento en el array
        foreach (var docInfo in scores)
        {
            items[i--] = new SearchItem(GetTittle(docInfo.Key), ObtenerFragmentoDocumento(docInfo.Key, queryNormalized), docInfo.Value);
        }
    }
    //Este metodo recibe el documento y devuelve un substring con solo el titulo
    public static string GetTittle(string document)
    {
        string s = document.Substring(11);
        return s;

    }
	// public static void Results (SearchItem[]items){
	// 	SearchItem[] finalItems = new SearchItem[4];
	// 	int counter = 0; 
	// 	for (int i =0;i<finalItems.Length;i++){
	// 		finalItems[i]=items[i];
	// 	}
	// }


    public static string ObtenerFragmentoDocumento(string documento, Dictionary<string, float> queryNormalizedTf)
    {
        //Leer el documento
        documento = File.ReadAllText(documento).Normalize(System.Text.NormalizationForm.FormD);
        // Dividir el documento en palabras
        string[] palabrasDocumento = documento.Split(' ');


        // Buscar una palabra de la entrada en el documento
        foreach (string palabra in queryNormalizedTf.Keys)
        {


            //  busco el indice de la palabra en el documento
            // si la palabra no existe entonces el numero q devuelve es -1
            int indicePalabra = Array.IndexOf(palabrasDocumento, palabra);
            // por tanto para saber si la parabra existe en el documento solo se comprueba q el indice sea 
            // distinto de -1 :)
            if (indicePalabra == -1) continue;

            // Construir el fragmento del documento que contiene al menos una palabra de la entrada
            int inicioFragmento = Math.Max(0, indicePalabra - 5); // Incluir 5 palabras antes de la encontrada
            int finFragmento
                = Math.Min(palabrasDocumento.Length - 1, indicePalabra + 30); // Incluir 30 palabras después de la encontrada

            string[] fragmentoArray = new string[finFragmento - inicioFragmento + 1];
            Array.Copy(palabrasDocumento, inicioFragmento, fragmentoArray, 0, fragmentoArray.Length);

            return string.Join(" ", fragmentoArray);
        }

        // Si no se encuentra ninguna palabra de la entrada en el documento, se retorna una cadena vacía
        return string.Empty;
    }

}