using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebServiceTask.DAL;
using WebServiceTask.DTO;
using WebServiceTask.FilterModels;
using WebServiceTask.Helpers;
using WebServiceTask.Interfaces;
using WebServiceTask.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServiceTask.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PersonalController : ControllerBase
    {


        private readonly IUrlHelper _urlHelper;
        private readonly AppDbContext _db;
        private readonly IDbContextAgent _dbagent;
        public PersonalController(IUrlHelper urlHelper, AppDbContext context, IDbContextAgent agent)
        {
            _urlHelper = urlHelper;
            _db = context;
            _dbagent = agent;
        }

        [HttpGet(Name = nameof(GetAllAddress))]
        public async Task<IActionResult> GetAllAddress(ApiVersion apiVersion, [FromQuery] BaseFilter filter)
        {

            List<PersonDTO> _personal = await _dbagent.GetAllPersonalAsync(filter);

            var _personalCount = await _dbagent.PersonalCountAsync(filter);
            var paginationMetadata = new
            {
                totaCount = _personalCount,
                pageSize = filter.PageCount,
                currentPage = filter.Page,
                totalPages = filter.GetTotalPages(_personalCount)
            };

            Response.Headers.Add("X-Pagination", CustomJsonConverter.Serialize(paginationMetadata));
            return Ok(_personal);
        }
    }
}
