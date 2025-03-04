using QaseTestCaseGenerator.Models;
using System.Diagnostics;
using System.Net;

namespace QaseTestCaseGenerator.Static
{
    public class StaticObjects
    {
        public static HttpClient confluenceHttpClient = new HttpClient();
        public static HttpClient openAiHttpClient = new HttpClient();
        public static HttpClient qaseHttpClient = new HttpClient();
        public static List<Command> commands = new List<Command>();
        public static Stopwatch programStopwatch = new Stopwatch();
        public static CookieContainer cookieContainer = new CookieContainer(); 
    }
}
