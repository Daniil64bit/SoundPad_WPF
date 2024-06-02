using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPITEst.Entity;

namespace WebAPITEst.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WriterController : ControllerBase
    {
        private readonly DataContext _context;

        public WriterController(DataContext context)
        {
            _context = context;
        }

        // GET: Writers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Writer>>> GetWriters()
        {
            return Ok(await _context.Writers.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult<Writer>>
            PostWriter(Writer writer)
        {
            _context.Writers.Add(writer);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
