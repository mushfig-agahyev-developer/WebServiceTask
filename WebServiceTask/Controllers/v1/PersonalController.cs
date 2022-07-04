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
using WebServiceTask.Messages;

namespace WebServiceTask.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PersonalController : ControllerBase
    {
        public Message _message { get; set; }

        private readonly IDbContextAgent _dbagent;
        public PersonalController(AppDbContext context, IDbContextAgent agent)
        {
            _dbagent = agent;
            _message = new Message();
        }

        [HttpGet(Name = nameof(GetAll))]
        public async Task<IActionResult> GetAll(ApiVersion apiVersion, [FromQuery] GetAllRequest request)
        {
            List<PersonDTO> _personal = await _dbagent.GetAllPersonalAsync(request);

            var _personalCount = await _dbagent.PersonalCountAsync(request);
            var paginationMetadata = new
            {
                totaCount = _personalCount,
                pageSize = request.PageCount,
                currentPage = request.Page,
                totalPages = request.GetTotalPages(_personalCount)
            };

            Response.Headers.Add("X-Pagination", CustomJsonConverter.Serialize(paginationMetadata));
            return Ok(_personal);
        }

        [HttpPost(Name = nameof(Save))]
        public async Task<long> Save(ApiVersion version, [FromBody] PersonDTO personDTO)
        {
            if (personDTO == null)
                return -1;

            if (!ModelState.IsValid)
                return -1;

            long _response = await _dbagent.SaveAsync(personDTO);
            return _response;
        }
    }
}

