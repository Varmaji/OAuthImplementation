using Newtonsoft.Json;
using OAuthImplementation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace OAuthImplementation.Controllers
{
    public class OAuthController : Controller
    {
       
      
        // GET: OAuth
        public ActionResult Index()
        {
          
            string Url = "https://app.vssps.visualstudio.com/oauth2/authorize?client_id={0}&response_type=Assertion &state=User1 &scope=vso.analytics vso.auditlog vso.build_execute vso.code_full vso.code_status vso.connected_server vso.dashboards_manage vso.environment_manage vso.extension.data_write vso.extension_manage vso.gallery_acquire vso.gallery_manage vso.graph_manage vso.identity_manage vso.loadtest_write vso.memberentitlementmanagement_write vso.notification_diagnostics vso.notification_manage vso.packaging_manage vso.profile_write vso.project_manage vso.release_manage vso.securefiles_manage vso.security_manage vso.serviceendpoint_manage vso.symbols_manage vso.taskgroups_manage vso.test_write vso.tokenadministration vso.tokens vso.variablegroups_manage vso.wiki_write vso.work_full&redirect_uri={1}";
            string RedirectUrl = System.Configuration.ConfigurationManager.AppSettings["RedirectURL"];
            string ClientId= System.Configuration.ConfigurationManager.AppSettings["ClientID"];
            Url = string.Format(Url, ClientId, RedirectUrl);
            return Redirect(Url);   

        }

        public ActionResult CallBack()

        {
            String code = Request.QueryString["Code"];
            Session["code"] = code;
            string data = GenerateAccessToken(code);//code);
            AccessDetails accessDetails = AccessDetails(data);

            AccessDetails accessToken = new AccessDetails();
            accessToken.access_token = accessDetails.access_token;
            Session["AT"] = accessToken.access_token;
            GetProjects(accessToken.access_token);

            return View(accessToken);
        }

        //client_assertion_type=urn:ietf:params:oauth:client-assertion-type:jwt-bearer&client_assertion={0}&grant_type=urn:ietf:params:oauth:grant-type:jwt-bearer&assertion={1}&redirect_uri={2}

        public string GenerateAccessToken(string code)
        {
            string RedirectUrl = System.Configuration.ConfigurationManager.AppSettings["RedirectURL"];
            return string.Format("client_assertion_type=urn:ietf:params:oauth:client-assertion-type:jwt-bearer&client_assertion={0}&grant_type=urn:ietf:params:oauth:grant-type:jwt-bearer&assertion={1}&redirect_uri={2}",
            HttpUtility.UrlEncode(System.Configuration.ConfigurationManager.AppSettings["ClientSecret"]),
            HttpUtility.UrlEncode(code),
            RedirectUrl
        );
        }
        public AccessDetails AccessDetails(string body)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://app.vssps.visualstudio.com");
            var request = new HttpRequestMessage(HttpMethod.Post, "/oauth2/token");
            var requestContent = body;
            request.Content = new StringContent(requestContent, Encoding.UTF8, "application/x-www-form-urlencoded");
            var response = client.SendAsync(request).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                AccessDetails details = Newtonsoft.Json.JsonConvert.DeserializeObject<AccessDetails>(result);
                return details;
            }
            return new AccessDetails();
        }

        public ActionResult GetProjects(string accessToken)
        {

            string pattoken = accessToken;


            ProjectModel model = new ProjectModel();
            try
            {


                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(
                        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                        Convert.ToBase64String(
                            System.Text.ASCIIEncoding.ASCII.GetBytes(
                                string.Format("{0}:{1}", "", pattoken))));

                    using (HttpResponseMessage response = client.GetAsync(
                                "https://dev.azure.com/ravivarmayaganti/_apis/projects").Result)
                    {
                        response.EnsureSuccessStatusCode();
                        string responseBody = response.Content.ReadAsStringAsync().Result;
                        model = JsonConvert.DeserializeObject<ProjectModel>(responseBody);
                        //Console.WriteLine(responseBody);
                    }
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }





            return View(model.value);

        }
    }
}