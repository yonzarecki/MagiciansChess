using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Security.Claims;

using System.Diagnostics;
using MagiciansChessDataAPI.Models;
using System.Configuration;

namespace MagiciansChessDataAPI.Controllers
{
    public class LeaderboardEntryController : ApiController
    {
        // Uncomment following lines for service principal authentication
        //private static string trustedCallerClientId = ConfigurationManager.AppSettings["todo:TrustedCallerClientId"];
        //private static string trustedCallerServicePrincipalId = ConfigurationManager.AppSettings["todo:TrustedCallerServicePrincipalId"];

        private static Dictionary<int, LeaderboardEntry> mockData = new Dictionary<int, LeaderboardEntry>();

        static LeaderboardEntryController()
        {
        }

        private static void CheckCallerId()
        {
            // Uncomment following lines for service principal authentication
            //string currentCallerClientId = ClaimsPrincipal.Current.FindFirst("appid").Value;
            //string currentCallerServicePrincipalId = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            //if (currentCallerClientId != trustedCallerClientId || currentCallerServicePrincipalId != trustedCallerServicePrincipalId)
            //{
            //    throw new HttpResponseException(new HttpResponseMessage { StatusCode = HttpStatusCode.Unauthorized, ReasonPhrase = "The appID or service principal ID is not the expected value." });
            //}
        }

        // GET: api/ToDoItemList
        public IEnumerable<LeaderboardEntry> Get()
        {
            CheckCallerId();

            return mockData.Values.ToList();
        }

        // GET: api/ToDoItemList/5
        public LeaderboardEntry GetById(string username, int id)
        {
            CheckCallerId();

            return mockData.Values.Where(m => (m.username == username || username == "*" ) && m.ID == id).First();
        }

        // POST: api/ToDoItemList
        public void Post(LeaderboardEntry entry)
        {
            CheckCallerId();

            entry.ID = mockData.Count > 0 ? mockData.Keys.Max() + 1 : 1;
            mockData.Add(entry.ID, entry);
        }

        public void Put(LeaderboardEntry entry)
        {
            CheckCallerId();

            LeaderboardEntry xentry = mockData.Values.First(a => (a.username == entry.username || entry.username == "*") && a.ID == entry.ID);
            if (entry != null && xentry != null)
            {
                xentry.username = entry.username;
            }
        }

        // DELETE: api/ToDoItemList/5
        public void Delete(string username, int id)
        {
            CheckCallerId();

            LeaderboardEntry todo = mockData.Values.First(a => (a.username == username || username == "*") && a.ID == id);
            if (todo != null)
            {
                mockData.Remove(todo.ID);
            }
        }
    }
}

