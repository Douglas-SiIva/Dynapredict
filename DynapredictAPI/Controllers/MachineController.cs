using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using DynapredictAPI.Data;
using DynapredictAPI.Models;
using DynapredictAPI.DTOs;

namespace DynapredictAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MachinesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MachinesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Machine>>> GetMachines()
        {
            return await _context.Machines.OrderBy(m => m.Name).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Machine>> GetMachine(int id)
        {
            var machine = await _context.Machines.FindAsync(id);

            if (machine == null)
            {
                return NotFound();
            }

            return machine;
        }

        [HttpPost]
        public async Task<ActionResult<Machine>> CreateMachine(CreateMachineDto createMachineDto)
        {
            if (await _context.Machines.AnyAsync(m => m.SerialNumber == createMachineDto.SerialNumber))
            {
                return Conflict(new { message = "Número de série já existe" });
            }

            var machine = new Machine
            {
                Name = createMachineDto.Name,
                SerialNumber = createMachineDto.SerialNumber,
                Description = createMachineDto.Description,
                Type = createMachineDto.Type,
                Status = "active",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Machines.Add(machine);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMachine), new { id = machine.Id }, machine);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMachine(int id, UpdateMachineDto updateMachineDto)
        {
            var machine = await _context.Machines.FindAsync(id);

            if (machine == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(updateMachineDto.SerialNumber) &&
                updateMachineDto.SerialNumber != machine.SerialNumber)
            {
                if (await _context.Machines.AnyAsync(m => m.SerialNumber == updateMachineDto.SerialNumber))
                {
                    return Conflict(new { message = "Número de série já existe" });
                }
            }

            if (!string.IsNullOrEmpty(updateMachineDto.Name))
                machine.Name = updateMachineDto.Name;

            if (!string.IsNullOrEmpty(updateMachineDto.SerialNumber))
                machine.SerialNumber = updateMachineDto.SerialNumber;

            if (!string.IsNullOrEmpty(updateMachineDto.Description))
                machine.Description = updateMachineDto.Description;

            if (!string.IsNullOrEmpty(updateMachineDto.Type))
                machine.Type = updateMachineDto.Type;

            if (!string.IsNullOrEmpty(updateMachineDto.Status))
                machine.Status = updateMachineDto.Status;

            machine.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MachineExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMachine(int id)
        {
            var machine = await _context.Machines.FindAsync(id);
            if (machine == null)
            {
                return NotFound();
            }

            _context.Machines.Remove(machine);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateMachineStatus(int id, [FromBody] string status)
        {
            var machine = await _context.Machines.FindAsync(id);
            if (machine == null)
            {
                return NotFound();
            }

            machine.Status = status;
            machine.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool MachineExists(int id)
        {
            return _context.Machines.Any(e => e.Id == id);
        }
    }
}