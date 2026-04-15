using TrainingRouting.Models;

namespace TrainingRouting.Data;

public class ApplicationData
{
    public static List<Room> Rooms { get; } =
    [
        new Room
        {
            Id = 1,
            Name = "Room A101",
            BuildingCode = "A",
            Floor = 1,
            Capacity = 20,
            HasProjector = true,
            IsActive = true
        },
        new Room
        {
            Id = 2,
            Name = "Room A202",
            BuildingCode = "A",
            Floor = 2,
            Capacity = 30,
            HasProjector = false,
            IsActive = true
        }
    ];
}