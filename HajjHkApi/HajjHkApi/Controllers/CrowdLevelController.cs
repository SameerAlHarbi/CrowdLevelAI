using HajjHkApi.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HajjHkApi.Controllers
{
    [EnableCors("Any")]
    [Route("api/[controller]")]
    public class CrowdLevelController  :Controller
    {
        protected readonly ILogger _logger;
        protected readonly CrowdLevelsManager dataManager;

        public CrowdLevelController(ILogger<CrowdLevelController> logger, CrowdLevelsManager dataMgr)
        {
            _logger = logger;
            this.dataManager = dataMgr;
        }

        [HttpGet]
        [Produces("application/json")]//only json
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                return Ok(dataManager.CrowdLevels);
            }
            catch (Exception)
            { }

            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CrowdLevel model)
        {
            try
            {
                this.dataManager.CrowdLevels.Add(model);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
