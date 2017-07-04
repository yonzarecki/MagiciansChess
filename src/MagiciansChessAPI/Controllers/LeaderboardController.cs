using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using MagiciansChessDataAPI.Filters;
using System.Net.Http.Headers;
using System.Configuration;
using MagiciansChessAPI.Models;
using MagiciansChessAPI;
using ChessLibrary;
using MagiciansChessAPI.QuickChess;
using System.IO;
using System.Text;

namespace MagiciansChessDataAPI.Filters
{
    using System.Web.Http.Filters;

    public class HttpOperationExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is Microsoft.Rest.HttpOperationException)
            {
                var ex = (Microsoft.Rest.HttpOperationException)context.Exception;
                context.Response = new HttpResponseMessage(ex.Response.StatusCode);
            }
        }
    }
}

namespace MagiciansChessDataAPI.Controllers
{
    [HttpOperationExceptionFilterAttribute]
    public class LeaderboardController : ApiController
    {

        private static MagiciansChessAPI.MagiciansChessDataAPI NewDataAPIClient()
        {
            var client = new MagiciansChessAPI.MagiciansChessDataAPI(new Uri(ConfigurationManager.AppSettings["MagiciansChessDataAPIURL"]));
            // Uncomment following line and entire ServicePrincipal.cs file for service principal authentication of calls to ToDoListDataAPI
            //client.HttpClient.DefaultRequestHeaders.Authorization =
            //    new AuthenticationHeaderValue("Bearer", ServicePrincipal.GetS2SAccessTokenForProdMSA().AccessToken);
            return client;
        }
        // TODO - here we define all function for our controller, using the DataAPI client we defined above


        // GET: api/LeaderboardEntryList
        public async Task<IEnumerable<LeaderboardEntry>> GetLeaderboardEntries()
        {
            using (var client = NewDataAPIClient())
            {
                var results = await client.LeaderboardEntryOperations.GetWithHttpMessagesAsync();
                return results.Body.Select(m => new LeaderboardEntry
                {
                    GameTime = m.GameTime,
                    ID = (int)m.ID,
                    Username = m.Username,
                    HumanWon = m.HumanWon
                });
            }
        }

        // POST: api/LeaderboardEntry
        public async Task PostLeaderboardEntry(LeaderboardEntry entry)
        {
            using (var client = NewDataAPIClient())
            {
                await client.LeaderboardEntryOperations.PostAsync(new LeaderboardEntry
                {
                    Username = entry.Username,
                    ID = entry.ID,
                    GameTime = entry.GameTime,
                    HumanWon = entry.HumanWon
                });
            }
        }



        // GET: api/ToDoItemList/5
        /*public async Task<ToDoItem> GetByID(int id)
        {
            //owner = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value;
            using (var client = NewDataAPIClient())
            {
                var result = await client.ToDoList.GetByIdByOwnerAndIdAsync(owner, id);
                return new ToDoItem
                {
                    Description = result.Description,
                    ID = (int)result.ID,
                    Owner = result.Owner
                };
            }
        }

        // POST: api/ToDoItemList
        public async Task Post(ToDoItem todo)
        {
            //owner = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value;
            todo.Owner = owner;
            using (var client = NewDataAPIClient())
            {
                await client.ToDoList.PostByTodoAsync(new ToDoItem
                {
                    Description = todo.Description,
                    ID = todo.ID,
                    Owner = todo.Owner
                });
            }
        }

        public async Task Put(ToDoItem todo)
        {
            //owner = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value;
            todo.Owner = owner;
            using (var client = NewDataAPIClient())
            {
                await client.ToDoList.PutByTodoAsync(new ToDoItem
                {
                    Description = todo.Description,
                    ID = todo.ID,
                    Owner = todo.Owner
                });
            }
        }

        // DELETE: api/ToDoItemList/5
        public async Task Delete(int id)
        {
            //owner = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value;
            using (var client = NewDataAPIClient())
            {
                await client.ToDoList.DeleteByOwnerAndIdAsync(owner, id);
            }
        }*/
    }
}

