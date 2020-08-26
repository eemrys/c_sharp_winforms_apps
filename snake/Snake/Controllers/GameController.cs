using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Snake.Models;
using Snake.BusinessLogic;
using Snake.DataAccessLayer;

namespace Snake.Controllers 
{    
    [ApiController]
    public class BoardController : ControllerBase
    {
        private ISnakeRepository _repository;

        public BoardController(ISnakeRepository repository)
        {
            _repository = repository;
        }

        [Route("api/gameboard")]
        [HttpGet]
        [ProducesResponseType(typeof(Board), 200)]
        [ProducesResponseType(400)]
        public IActionResult Get()
        {
            try
            {
                return Ok(_repository.Get());
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [Route("api/direction")]
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult Post([FromBody]Board.Direction newDir) 
        {
            try
            {
                _repository.UpdateDir(newDir);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        
    }
}
