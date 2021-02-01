using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DividniApi.Services;
using Microsoft.AspNetCore.Cors;

namespace DividniApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DividniController : ControllerBase
    {
        private readonly DividniService _service;
        public DividniController() {
            _service = new DividniService();
        }

        // POST: api/CompileQuestion
        [EnableCors("DividniPolicy")]
        [HttpPost]
        public string CompileQuestion(string name, string question) 
        {
            return _service.compileQuestion(Request.Form["name"], Request.Form["question"]);
        }
    }
}