using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;

namespace Dictionary_Lookup
{
    static class DictionaryAPI
    {
        //Helper variable 
        public static string GetData(string word)
        {
            string data = "";
            //Requests the corresponding word data from the API and gets the JSON response as a string 

            RestClient MyClient = new RestClient("https://api.dictionaryapi.dev/api/v2/entries/");
            RestRequest MyReq = new RestRequest("en_US/" + word);

            try {
            string result = MyClient.Get(MyReq).Content;
            }
            catch {
                Console.WriteLine("Fetch failed");
            }


            //Every property in the JSON response object is conceptualized as a class
            //Deserialize method maps data to corresponding class properties

            List<MyObj> output = new List<MyObj>();

            //try-catch block deals with HTTP 4XX status codes (bad request error i.e invalid word)
            
            try
            {
                output = JsonConvert.DeserializeObject<List<MyObj>>(result);
            }
            catch
            {
                return "Invalid word or expression";
            }
            
            string synonyms = "Synonyms = ";
            foreach (var meaning in output[0].meanings)
            {
                data += "Part of speech = "+ meaning.partOfSpeech + Environment.NewLine;
                foreach (var def in meaning.definitions)
                {
                    data += "Definition = " + def.definition + Environment.NewLine;
                    if (def.example != null)
                    {
                        data += "Example = " + def.example + Environment.NewLine;
                    }
                    if (def.synonyms != null)
                    {
                        foreach (var synonym in def.synonyms)
                        {
                            synonyms += synonym + " / ";
                        }
                        synonyms = synonyms.Substring(0, synonyms.Length - 3);
                        data += synonyms + Environment.NewLine;
                        synonyms = "Synonyms= ";
                    }
                }
                data += "-----------------------------------------------------" + Environment.NewLine;
            }
            return data;
        }

        public class MyObj
        {
            public string word { get; set; }
            public List<Phonetic> phonetics { get; set; }
            public List<Meaning> meanings { get; set; }
        }
        public class Definition
        {
            public string definition { get; set; }
            public List<string> synonyms { get; set; }
            public string example { get; set; }
        }

        public class Meaning
        {
            public string partOfSpeech { get; set; }
            public List<Definition> definitions { get; set; }
        }
        public class Phonetic
        {
            public string text { get; set; }
            public string audio { get; set; }
        }
    }

}