using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PFM.Models;
using PFM.Services;

namespace PFM.Controllers
{
    [ApiController]
    [Route("/categories")]
    public class CategoriesController : ControllerBase{

        private readonly ICategoriesService _categoriesService;
        private readonly ILogger<CategoriesController> _logger;
        private readonly IMapper _mapper;


        public CategoriesController(IMapper mapper, ICategoriesService categoriesService, ILogger<CategoriesController> logger)
        {
            _mapper = mapper;
            _logger = logger;
            _categoriesService = categoriesService;
        }

        [HttpGet]
        public IActionResult GetCategories([FromQuery] string parentId){
            var status = _categoriesService.GetCategories(parentId);
            return Ok(_mapper.Map<List<Category>>(status));
        }

        [HttpPost("/categories/import")]
        public async Task<IActionResult> ImportCategories(){
             var request = HttpContext.Request;
            if (!request.Body.CanSeek)
            {
                request.EnableBuffering();
            }

            var status = await _categoriesService.AddCategories(request);
            if (status) return StatusCode(201);

            return BadRequest("Error while inserting the transactions");
        }
    }
}