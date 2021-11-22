using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PFM.Database.Entities;
using PFM.Models;
using PFM.Services;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using PFM.Mappings;
using AutoMapper;
using PFM.Commands;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace PFM.Controllers
{
    [ApiController]
    [Route("/transactions")]
    public class TransactionsController : ControllerBase{

       
        private readonly ILogger<TransactionsController> _logger;
        private readonly IMapper _mapper;
        private readonly ITransactionsService _transactionService;

        public TransactionsController(ILogger<TransactionsController> logger, ITransactionsService transactionService,  IMapper mapper)
        {
            _transactionService = transactionService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactions([FromQuery] QueryParams TransactionsParams)
        {
            var status = await _transactionService.GetPagedListTransactions(TransactionsParams);

            Response.AddPagination(status.CurrentPage, status.PageSize, status.TotalCount, status.TotalPages);

            return Ok(_mapper.Map<List<Transaction>>(status));
        }


        [HttpPost("/transactions/import")]
        public async Task<IActionResult> ImportTransactions()
        {
            var request = HttpContext.Request;
            if (!request.Body.CanSeek)
            {
                request.EnableBuffering();
            }

            var status = await _transactionService.AddTransactions(request);
            if (status) return StatusCode(201);

            return BadRequest("Error while inserting the transactions");
        }

        [HttpPost("/transaction/{id}/split")]
        public async Task<IActionResult> SplitTransactions([FromRoute] string id, [FromBody] SplitTransactionCommand split){
            var status = await _transactionService.SplitTransaction(id, split);
            if(status)
                return StatusCode(200);
            return BadRequest("Error while splitting transaction");
        }

        [HttpPost("/transaction/{id}/categorize")]
        public async Task<IActionResult> CategorizeTransaction([FromRoute] string id, [FromBody] CategorizeTransaction catTrans){
            var status = await _transactionService.CategorizeTransactions(id, catTrans);
            if(status)
                return StatusCode(200);
            return BadRequest("Error while categorizing transaction");
        }

        [HttpPost("/transaction/{id}/auto-categorize")]
        public IActionResult AutoCategorizeTransaction([FromRoute] string id){
            return Ok();
        }

    }
}