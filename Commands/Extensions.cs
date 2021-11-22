using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace PFM.Commands
{
     public static class Extensions
    {

        /// <summary>
        /// Global pagination Header to return with every Collection and filter request
        /// </summary>
        /// <param name="response"></param>
        /// <param name="currentPage"></param>
        /// <param name="itemsPerPage"></param>
        /// <param name="totalItems"></param>
        /// <param name="totalPages"></param>
        public static void AddPagination(this HttpResponse response, int currentPage, int itemsPerPage, int totalItems, int totalPages)
        {
            var paginationHeader = new PaginationHeader(currentPage, itemsPerPage, totalItems, totalPages);
            var camelCase = new JsonSerializerSettings();
            camelCase.ContractResolver = new CamelCasePropertyNamesContractResolver();

            response.Headers.Add("Pagination", JsonConvert.SerializeObject(paginationHeader, camelCase));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }

        public static void AddCounterHeader(this HttpResponse response, int countInbox, int countDeleted, int countArchived, int countImportant)
        {
            //var countersHeader = new CountersHeader(countInbox, countDeleted, countArchived, countImportant);
            //var camelCase = new JsonSerializerSettings();
            //camelCase.ContractResolver = new CamelCasePropertyNamesContractResolver();

            //response.Headers.Add("Counters", JsonConvert.SerializeObject(countersHeader, camelCase));
            //response.Headers.Add("Access-Control-Expose-Headers", "Counters");
        }

        /// <summary>
        /// Global exeption Handler return of Header for entire application
        /// </summary>
        /// <param name="response"></param>
        /// <param name="message"></param>
        public static void AddApplicationerror(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }

        /// <summary>
        /// Global exception Handler 
        /// </summary>
        /// <param name="app"></param>
       
       

        /// <summary>
        /// Custom Midlware 
        /// </summary>
        /// <param name="app"></param>
 
    }
}