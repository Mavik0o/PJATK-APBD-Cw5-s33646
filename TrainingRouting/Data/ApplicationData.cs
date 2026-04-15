using TrainingRouting.Models;

namespace TrainingRouting.Data;

public static class ApplicationData
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
        },
        new Room
        {
            Id = 3,
            Name = "Lab B204",
            BuildingCode = "B",
            Floor = 2,
            Capacity = 24,
            HasProjector = true,
            IsActive = true
        },
        new Room
        {
            Id = 4,
            Name = "Conference C301",
            BuildingCode = "C",
            Floor = 3,
            Capacity = 50,
            HasProjector = true,
            IsActive = false
        },
        new Room
        {
            Id = 5,
            Name = "Workshop B105",
            BuildingCode = "B",
            Floor = 1,
            Capacity = 16,
            HasProjector = false,
            IsActive = true
        }
    ];

    public static List<Reservation> Reservations { get; } =
    [
        new Reservation
        {
            Id = 1,
            RoomId = 1,
            OrganizerName = "Anna Kowalska",
            Topic = "REST Basics",
            Date = new DateOnly(2026, 5, 10),
            StartTime = new TimeOnly(9, 0, 0),
            EndTime = new TimeOnly(10, 30, 0),
            Status = "confirmed"
        },
        new Reservation
        {
            Id = 2,
            RoomId = 2,
            OrganizerName = "Jan Nowak",
            Topic = "ASP.NET Workshop",
            Date = new DateOnly(2026, 5, 10),
            StartTime = new TimeOnly(11, 0, 0),
            EndTime = new TimeOnly(13, 0, 0),
            Status = "planned"
        },
        new Reservation
        {
            Id = 3,
            RoomId = 3,
            OrganizerName = "Maria Zielinska",
            Topic = "HTTP and Postman",
            Date = new DateOnly(2026, 5, 11),
            StartTime = new TimeOnly(8, 30, 0),
            EndTime = new TimeOnly(10, 0, 0),
            Status = "confirmed"
        },
        new Reservation
        {
            Id = 4,
            RoomId = 5,
            OrganizerName = "Piotr Adamski",
            Topic = "Consultation Session",
            Date = new DateOnly(2026, 5, 12),
            StartTime = new TimeOnly(14, 0, 0),
            EndTime = new TimeOnly(15, 0, 0),
            Status = "cancelled"
        }
    ];
}