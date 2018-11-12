using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Server.Data;
using Server.Models;
using ServiceStack.Redis;

namespace Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IRedisClient _redis;
        private readonly DataContext _context;
        const string SetKey = "values";
        public ValuesController(IRedisClientsManager redis, DataContext context)
        {
            _redis = redis.GetClient();
            _context = context;
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            return "Hi";
        }


        [HttpGet("all")]
        public ActionResult<string> GetAll()
        {
            var values = _context.Values.AsEnumerable();
            return Ok(values);
        }

        [HttpGet("current")]
        public IActionResult GetCurrent()
        {
            var result = new Dictionary<string, string>();
            var items = _redis.GetAllItemsFromSet(SetKey);
            foreach (var item in items)
            {
                var value = _redis.GetValue(item);
                var key = item.Replace(SetKey, "");
                result.Add(key, value);
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(dynamic request)
        {
            string index = Convert.ToString(request.index);
            if (!index.All(char.IsDigit))
            {
                return StatusCode(StatusCodes.Status422UnprocessableEntity, "Invalid Index!");
            }
            if (Convert.ToInt32(index) > 40)
            {
                return StatusCode(StatusCodes.Status422UnprocessableEntity, "Index too high!");
            }
            
            var redisKey = SetKey + index;
            _redis.SetValue(redisKey, "Nothing yet!");
            _redis.AddItemToSet(SetKey, redisKey);
            _redis.PublishMessage("message", index);

            _context.Values.Add(new Value {Number = Convert.ToInt32(index)});
            await _context.SaveChangesAsync();
            
            dynamic result = new JObject();
            result.working = true;
            return Ok(result);
        }


    }
}
