using Microsoft.AspNetCore.Mvc;
using TrainingRouting.Data;
using TrainingRouting.Models;

namespace TrainingRouting.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Reservation>> GetAll(
        [FromQuery] DateOnly? date,
        [FromQuery] string? status,
        [FromQuery] int? roomId)
    {
        var reservations = ApplicationData.Reservations.AsEnumerable();

        if (date.HasValue)
        {
            reservations = reservations.Where(r => r.Date == date.Value);
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            reservations = reservations.Where(r =>
                r.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
        }

        if (roomId.HasValue)
        {
            reservations = reservations.Where(r => r.RoomId == roomId.Value);
        }

        return Ok(reservations);
    }

    [HttpGet("{id:int}")]
    public ActionResult<Reservation> GetById(int id)
    {
        var reservation = ApplicationData.Reservations.FirstOrDefault(r => r.Id == id);

        if (reservation is null)
        {
            return NotFound(new { message = "Reservation not found." });
        }

        return Ok(reservation);
    }

    [HttpPost]
    public ActionResult<Reservation> Create([FromBody] Reservation reservation)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var room = ApplicationData.Rooms.FirstOrDefault(r => r.Id == reservation.RoomId);

        if (room is null)
        {
            return BadRequest(new { message = "Room does not exist." });
        }

        if (!room.IsActive)
        {
            return BadRequest(new { message = "Cannot create reservation for inactive room." });
        }

        var hasConflict = ApplicationData.Reservations.Any(r =>
            r.RoomId == reservation.RoomId &&
            r.Date == reservation.Date &&
            !r.Status.Equals("cancelled", StringComparison.OrdinalIgnoreCase) &&
            reservation.StartTime < r.EndTime &&
            reservation.EndTime > r.StartTime);

        if (hasConflict)
        {
            return Conflict(new { message = "Reservation conflicts with an existing reservation." });
        }

        reservation.Id = ApplicationData.Reservations.Any()
            ? ApplicationData.Reservations.Max(r => r.Id) + 1
            : 1;

        ApplicationData.Reservations.Add(reservation);

        return CreatedAtAction(nameof(GetById), new { id = reservation.Id }, reservation);
    }

    [HttpPut("{id:int}")]
    public ActionResult<Reservation> Update(int id, [FromBody] Reservation updatedReservation)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingReservation = ApplicationData.Reservations.FirstOrDefault(r => r.Id == id);

        if (existingReservation is null)
        {
            return NotFound(new { message = "Reservation not found." });
        }

        var room = ApplicationData.Rooms.FirstOrDefault(r => r.Id == updatedReservation.RoomId);

        if (room is null)
        {
            return BadRequest(new { message = "Room does not exist." });
        }

        if (!room.IsActive)
        {
            return BadRequest(new { message = "Cannot assign reservation to inactive room." });
        }

        var hasConflict = ApplicationData.Reservations.Any(r =>
            r.Id != id &&
            r.RoomId == updatedReservation.RoomId &&
            r.Date == updatedReservation.Date &&
            !r.Status.Equals("cancelled", StringComparison.OrdinalIgnoreCase) &&
            updatedReservation.StartTime < r.EndTime &&
            updatedReservation.EndTime > r.StartTime);

        if (hasConflict)
        {
            return Conflict(new { message = "Updated reservation conflicts with an existing reservation." });
        }

        existingReservation.RoomId = updatedReservation.RoomId;
        existingReservation.OrganizerName = updatedReservation.OrganizerName;
        existingReservation.Topic = updatedReservation.Topic;
        existingReservation.Date = updatedReservation.Date;
        existingReservation.StartTime = updatedReservation.StartTime;
        existingReservation.EndTime = updatedReservation.EndTime;
        existingReservation.Status = updatedReservation.Status;

        return Ok(existingReservation);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var reservation = ApplicationData.Reservations.FirstOrDefault(r => r.Id == id);

        if (reservation is null)
        {
            return NotFound(new { message = "Reservation not found." });
        }

        ApplicationData.Reservations.Remove(reservation);
        return NoContent();
    }
}