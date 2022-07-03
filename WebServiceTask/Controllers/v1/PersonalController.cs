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
        private readonly IDbContextAgent _context;
        public PersonalController(IUrlHelper urlHelper, AppDbContext context, IDbContextAgent agent)
        {
            _urlHelper = urlHelper;
            _db = context;
            _context = agent;
        }

        [HttpGet(Name = nameof(GetAllAddress))]
        public async Task<IActionResult> GetAllAddress(ApiVersion apiVersion, [FromQuery] BaseFilter filter)
        {

            List<Address> addresses = await _db.Addresses.ToListAsync();

            var addressesCount = addresses.Count();
            var paginationMetadata = new
            {
                totaCount = addressesCount,
                pageSize = filter.PageCount,
                currentPage = filter.Page,
                totalPages = filter.GetTotalPages(addressesCount)
            };

            Response.Headers.Add("X-Pagination", CustomJsonConverter.Serialize(paginationMetadata));
            return Ok(addresses);
        }
    }
}
