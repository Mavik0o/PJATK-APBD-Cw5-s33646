using Microsoft.AspNetCore.Mvc;
using TrainingRouting.Data;
using TrainingRouting.Models;

namespace TrainingRouting.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Room>> GetAll(
        [FromQuery] int? minCapacity,
        [FromQuery] bool? hasProjector,
        [FromQuery] bool? activeOnly)
    {
        var rooms = ApplicationData.Rooms.AsEnumerable();

        if (minCapacity.HasValue)
        {
            rooms = rooms.Where(r => r.Capacity >= minCapacity.Value);
        }

        if (hasProjector.HasValue)
        {
            rooms = rooms.Where(r => r.HasProjector == hasProjector.Value);
        }

        if (activeOnly.HasValue && activeOnly.Value)
        {
            rooms = rooms.Where(r => r.IsActive);
        }

        return Ok(rooms);
    }

    [HttpGet("{id:int}")]
    public ActionResult<Room> GetById(int id)
    {
        var room = ApplicationData.Rooms.FirstOrDefault(r => r.Id == id);

        if (room is null)
        {
            return NotFound(new { message = "Room not found." });
        }

        return Ok(room);
    }

    [HttpGet("building/{buildingCode}")]
    public ActionResult<IEnumerable<Room>> GetByBuildingCode(string buildingCode)
    {
        var rooms = ApplicationData.Rooms
            .Where(r => r.BuildingCode.Equals(buildingCode, StringComparison.OrdinalIgnoreCase))
            .ToList();

        return Ok(rooms);
    }

    [HttpPost]
    public ActionResult<Room> Create([FromBody] Room room)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        room.Id = ApplicationData.Rooms.Any() ? ApplicationData.Rooms.Max(r => r.Id) + 1 : 1;
        ApplicationData.Rooms.Add(room);

        return CreatedAtAction(nameof(GetById), new { id = room.Id }, room);
    }

    [HttpPut("{id:int}")]
    public ActionResult<Room> Update(int id, [FromBody] Room updatedRoom)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingRoom = ApplicationData.Rooms.FirstOrDefault(r => r.Id == id);

        if (existingRoom is null)
        {
            return NotFound(new { message = "Room not found." });
        }

        existingRoom.Name = updatedRoom.Name;
        existingRoom.BuildingCode = updatedRoom.BuildingCode;
        existingRoom.Floor = updatedRoom.Floor;
        existingRoom.Capacity = updatedRoom.Capacity;
        existingRoom.HasProjector = updatedRoom.HasProjector;
        existingRoom.IsActive = updatedRoom.IsActive;

        return Ok(existingRoom);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var room = ApplicationData.Rooms.FirstOrDefault(r => r.Id == id);

        if (room is null)
        {
            return NotFound(new { message = "Room not found." });
        }

        var hasReservations = ApplicationData.Reservations.Any(r => r.RoomId == id);
        if (hasReservations)
        {
            return Conflict(new { message = "Cannot delete room with existing reservations." });
        }

        ApplicationData.Rooms.Remove(room);
        return NoContent();
    }
}