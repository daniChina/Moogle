namespace MoogleEngine;


public static class Moogle
{
    public static SearchResult Query(string query)
    {    
        Dictionary<string,float>queryNormalizedTf=ProcessQuery.FillQueryWordsTf(query);//este diccionario tiene la palabra de la query y su tf
        //Dictionary<string, float> querytf = ProcessQuery.FillQueryWordsTf(query);
        Dictionary<string,float> queryTfIdf=ProcessQuery.TfIdfQuery(queryNormalizedTf,Initialize.idf);
        Dictionary<string,float> fileScore = FillScore.Score(Initialize.docTfIdf,queryTfIdf);

       
         SearchItem[] items = new SearchItem[3] {
            new SearchItem(fileScore.ElementAt(fileScore.Count-1).Key , " ", fileScore.ElementAt(fileScore.Count-1).Value),
            new SearchItem(fileScore.ElementAt(fileScore.Count-2).Key, "Lorem ipsum dolor sit amet", fileScore.ElementAt(fileScore.Count-2).Value),
            new SearchItem(fileScore.ElementAt(fileScore.Count-3).Key, "Lorem ipsum dolor sit amet", fileScore.ElementAt(fileScore.Count-3).Value),
        };

        return new SearchResult(items, query);
    }
}
