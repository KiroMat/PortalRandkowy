using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalRandkowy.API.Data;
using PortalRandkowy.API.Models;

namespace PortalRandkowy.API.Controllers {
    [Authorize]
    [Route ("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase 
    {
        private readonly DataContext _context;
        public ValuesController (DataContext context) 
        {
            _context = context;
        }
        // GET api/values
        [HttpGet]
        public async Task<IActionResult> GetValues () {
            return Ok(await _context.Values.ToListAsync());
        }

        // GET api/values/5
        [AllowAnonymous]      
        [HttpGet ("{id}")]
        public async Task<IActionResult> GetValue (int id) {
            return Ok(await _context.Values.FirstOrDefaultAsync( x => x.Id == id));
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> AddValue ([FromBody] Value value) 
        { 
            await _context.Values.AddAsync(value);
            _context.SaveChanges();
            return Ok(value);
        }

        // PUT api/values/5
        [HttpPut ("{id}")]
        public async Task<IActionResult> EditValue (int id, [FromBody] Value value) 
        { 
            var data = await _context.Values.FindAsync(id);
            if(data != null)
            {
                data.Name = value.Name;
                _context.Values.Update(data);
                await _context.SaveChangesAsync();
                return Ok(data);
            }

            return NotFound();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVAlue (int id) 
        {
            var data = await _context.Values.FindAsync(id);
            
            if(data == null)
                return NoContent();

            _context.Values.Remove(data);
            await _context.SaveChangesAsync();
            return Ok(data);
        }
    }
}