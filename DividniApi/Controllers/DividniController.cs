using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DividniApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DividniController : ControllerBase
    {
        // POST: api/CompileQuestion
        [HttpPost]
        public string CompileQuestion() //Task<ActionResult>
        {
            return "Success";
        }

        // POST: api/GenerateStandard
        [HttpPost]
        public string GenerateStandard() //Task<ActionResult>
        {
            return "Success";
        }

        // POST: api/GenerateLMS
        [HttpPost]
        public string GenerateLMS() //Task<ActionResult>
        {
            return "Success";
        }

    }
}