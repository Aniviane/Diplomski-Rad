using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models.Framework.Models;
using WebApplication1.Models.Framework.Models.DTOs;
using WebApplication1.Models.Framework_Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly DataContext _context;

        public ReservationsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Reservations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReservationDTO>>> GetReservations()
        {
            var ret = await _context.Reservations.ToListAsync();

            List<ReservationDTO> reservations = new List<ReservationDTO>();

            foreach(var res in ret)
            {
                reservations.Add(new ReservationDTO(res));
            }
            return reservations;
        }

        // GET: api/Reservations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ReservationDTO>> GetReservation(Guid id)
        {
            var reservation = await _context.Reservations.FindAsync(id);

            if (reservation == null)
            {
                return NotFound();
            }

            return new ReservationDTO(reservation);
        }

        // PUT: api/Reservations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReservation(Guid id, Reservation reservation)
        {
            if (id != reservation.ID)
            {
                return BadRequest();
            }

            _context.Entry(reservation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpGet("Hours")]
        public ActionResult<List<int>> GetTakenHours(DateTime Date)
        {
            var date = new DateTime(Date.Year,Date.Month,Date.Day);
            

            var ret =  _context.Reservations.Where(Reservation => Reservation.Day == date);
            List<int> Hours = new List<int>();
            foreach (var res in ret)
            {
                Hours.Add(res.Hour);
            }
            return Ok(Hours);
        }

        // POST: api/Reservations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ReservationDTO>> PostReservation(ReservationDTO reservation)
        {
            var user = _context.Users.Find(reservation.UserId);
            if (user == null) return BadRequest();
            var day = new DateTime(reservation.Day.Year, reservation.Day.Month, reservation.Day.Day);
            var res = new Reservation(reservation.ID, user, day, reservation.Hour, reservation.UserId);

            _context.Reservations.Add(res);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReservation", new { id = res.ID }, res);
        }

        // DELETE: api/Reservations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservation(Guid id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReservationExists(Guid id)
        {
            return _context.Reservations.Any(e => e.ID == id);
        }

       
    }
}
