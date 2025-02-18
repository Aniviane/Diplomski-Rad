using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models.DTO_s;
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

            foreach (var res in ret)
            {
                reservations.Add(new ReservationDTO(res));
            }
            return reservations;
        }

        [HttpGet("MyReservations/{id}")]
        public  ActionResult<IEnumerable<ReservationDTO>> GetMyReservations(Guid id)
        {
            var user =  _context.Users.Where(user => user.ID == id).Include(user => user.Reservations).FirstOrDefault();

            if (user == null) return BadRequest();

            List<ReservationDTO> reservations = new List<ReservationDTO>();

            foreach (var res in user.Reservations)
            {
                reservations.Add(new ReservationDTO(res));
            }
            return reservations.OrderBy(reservation => reservation.Day).ToList();
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
            

            var reservations =  _context.Reservations.Where(Reservation => Reservation.Day == date);

            List<int> Hours = new List<int>();

            foreach (var reservation in reservations)
            {
                Hours.Add(reservation.Hour);
            }
            return Ok(Hours);
        }

        // POST: api/Reservations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<List<ReservationDTO>>> PostReservation([FromBody] CreateReservationsDTO reservation)
        {
            var user = _context.Users.Find(reservation.UserId);
            if (user == null) return BadRequest();

            var createdReservations = new List<ReservationDTO>();

            foreach(var hour in reservation.Hours)
            {
                var date = reservation.Day.AddHours(2);

                var day = new DateTime(date.Year, date.Month, date.Day);
                var res = new Reservation(user, day, hour, reservation.UserId);

                if (!_context.Reservations.Any(r => r.Day == reservation.Day && r.Hour == hour))
                {
                    createdReservations.Add(new ReservationDTO(res));
                    _context.Reservations.Add(res);
                }



            }


            await _context.SaveChangesAsync();

            return createdReservations;
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
