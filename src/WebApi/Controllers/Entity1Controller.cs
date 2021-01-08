using System.Threading;
using System.Threading.Tasks;
using EfProblemSample.WebApi.Repository;
using EfProblemSample.WebApi.Repository.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EfProblemSample.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Entity1Controller : ControllerBase
    {
        private readonly Context _context;

        public Entity1Controller(Context context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get([FromRoute] long id, CancellationToken cancellationToken)
        {
            var result = await _context.Entity1s.SingleAsync(x => x.Id == id, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Invoke this endpoint to force a unique constraint violation in the database
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult> CauseTrouble(CancellationToken cancellationToken)
        {
            await Task.WhenAll(InsertHardCoded(cancellationToken), InsertHardCoded(cancellationToken));
            await _context.SaveChangesAsync(cancellationToken);
            return Ok();
        }

        private async Task InsertHardCoded(CancellationToken cancellationToken)
        {
            var entity = new Entity1 { AUniqueProperty = "NotAsUniqueAsWeNeed", SomeProperty = "Something" }; 
            await _context.Entity1s.AddAsync(entity, cancellationToken);
        }
    }
}