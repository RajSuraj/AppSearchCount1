using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Policy;
using System.Text.RegularExpressions;
using NUnit.Framework;
using TechTalk.SpecFlow;    

namespace AppSearchCount
{
    [Binding]
    public class AppSearchCountSteps
    {
        private string _theUrl;
        private string _theResponse;
        private string _search;
        private string _name;

        [Given(@"the alteryx service is running at ""(.*)""")]
        public void GivenTheAlteryxServiceIsRunningAt(string alteryxUrl)
        {
            _theUrl = alteryxUrl; 
        }
        
        [When(@"I search for application at ""(.*)"" with search term ""(.*)""")]
        public void WhenISearchForApplicationAtWithSearchTerm(string apiurl, string searchterm)
        {
            string search = Regex.Replace(searchterm, @"\s+", "+");
            string Url = _theUrl + "/" + apiurl + "?search=" + search;
            WebRequest webRequest = System.Net.WebRequest.Create(Url);
            WebResponse response = webRequest.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new System.IO.StreamReader(responseStream);
            string responseFromServer = reader.ReadToEnd();
            _theResponse = responseFromServer;
            
        }
     
        [Then(@"I see primaryapplication\.metainfo\.name contains ""(.*)""")]
        public void ThenISeePrimaryapplication_Metainfo_NameContains(string expectedname)
        {
            var dict =
                new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Dictionary<string, dynamic>>(
                    _theResponse);
            int count = dict["recordCount"];
            if (count == 1)
            {
                _name = dict["records"][0]["primaryApplication"]["metaInfo"]["name"];
            }
            else
            {
                int i = 0;
                for (i = 0; i <= count - 1; i++)
                {
                    if (dict["records"][i]["primaryApplication"]["metaInfo"]["name"] == expectedname)
                    {
                        _name = dict["records"][i]["primaryApplication"]["metaInfo"]["name"];
                        break;
                    }

                }
            }
            Assert.AreEqual(expectedname, _name);
        }

        [When(@"I search for application at ""(.*)"" with search multiple term ""(.*)""")]
        public void WhenISearchForApplicationAtWithSearchMultipleTerm(string apiurl, string searchterm)
        {
            string term = searchterm.TrimStart();
            string[] split = term.Split(new Char[] { ' ', ',', '.', ':', '\t' });
            if (split.Length > 0)
            {
                _search = split[0];
            }
            string Url = _theUrl + "/" + apiurl + "?search=" + _search;
            WebRequest webRequest = System.Net.WebRequest.Create(Url);
            WebResponse response = webRequest.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new System.IO.StreamReader(responseStream);
            string responseFromServer = reader.ReadToEnd();
            _theResponse = responseFromServer;

        }

        [Then(@"I see record-count is (.*)")]
        public void ThenISeeRecord_CountIs(int expectedcount)
        {
            var dict = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Dictionary<string, dynamic>>(_theResponse);
            int count = dict["recordCount"];
            Assert.AreEqual(count, expectedcount);

        }
    }
}
