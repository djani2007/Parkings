using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Parking
{
    public int ParkingID { get; set; }
    public string Location { get; set; }
    public int TotalSpaces { get; set; }
    public int AvailableSpaces { get; set; }
    public List<string> Vehicles { get; set; }
}

class Program
{
    static List<Parking> parkings = new List<Parking>();
    const string filePath = "parkings.txt";

    static void Main()
    {
        LoadParkings();
        while (true)
        {
            Console.Clear();
            Console.WriteLine("М Е Н Ю");
            Console.WriteLine("1. Създаване на нов паркинг");
            Console.WriteLine("2. Регистрация на превозното средсво в паркинг");
            Console.WriteLine("3. Напускане на паркинг от превозното средство в паркинг");
            Console.WriteLine("4. Проверка на наличността на паркоместа");
            Console.WriteLine("5. Справка на всички паркинги");
            Console.WriteLine("X. Излизане");
            Console.Write("Избери опция: ");
            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    AddNewParking();
                    break;
                case "2":
                    RegisterVehicle();
                    break;
                case "3":
                    CheckoutVehicle();
                    break;
                case "4":
                    CheckAvailability();
                    break;
                case "5":
                    ViewAllParkings();
                    break;
                case "X" or "x":
                    return;
                default:
                    Console.WriteLine("Невалиден избор. Опитай пак.");
                    break;
            }
        }
    }

    static void LoadParkings()
    {
        if (!File.Exists(filePath))
        {
            File.Create(filePath).Close();
        }
        else
        {
            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                var parts = line.Split(',');
                var parking = new Parking
                {
                    ParkingID = int.Parse(parts[0]),
                    Location = parts[1],
                    TotalSpaces = int.Parse(parts[2]),
                    AvailableSpaces = int.Parse(parts[3]),
                    Vehicles = parts[4].Split('|').ToList()
                };
                parkings.Add(parking);
            }
        }
    }

    static void SaveParkings()
    {
        var lines = parkings.Select(p => $"{p.ParkingID},{p.Location},{p.TotalSpaces},{p.AvailableSpaces},{string.Join('|', p.Vehicles)}");
        File.WriteAllLines(filePath, lines);
    }

    static void AddNewParking()
    {
        Console.Write("Въведете Parking ID: ");
        int id = int.Parse(Console.ReadLine());
        Console.Write("Въведете местоположение: ");
        string location = Console.ReadLine();
        Console.Write("Въведете всики паркоместа: ");
        int totalSpaces = int.Parse(Console.ReadLine());
        var parking = new Parking
        {
            ParkingID = id,
            Location = location,
            TotalSpaces = totalSpaces,
            AvailableSpaces = totalSpaces,
            Vehicles = new List<string>()
        };
        parkings.Add(parking);
        SaveParkings();
        Console.WriteLine("Паркингът е добавен успешно.");
        Console.ReadLine();
    }

    static void RegisterVehicle()
    {
        Console.Write("Въведете Parking ID: ");
        int id = int.Parse(Console.ReadLine());
        var parking = parkings.FirstOrDefault(p => p.ParkingID == id);
        if (parking == null)
        {
            Console.WriteLine("Паркингът не е намерен.");
            Console.ReadLine();
            return;
        }
        if (parking.AvailableSpaces == 0)
        {
            Console.WriteLine("Няма свободни места.");
            Console.ReadLine();
            return;
        }
        Console.Write("Въведете регистрационния номер на превозното средство:");
        string vehicle = Console.ReadLine();
        parking.Vehicles.Add(vehicle);
        parking.AvailableSpaces--;
        SaveParkings();
        Console.WriteLine("Превозното средство е регистрирано успешно.");
        Console.ReadLine();
    }

    static void CheckoutVehicle()
    {
        Console.Write("Въведете Parking ID: ");
        int id = int.Parse(Console.ReadLine());
        var parking = parkings.FirstOrDefault(p => p.ParkingID == id);
        if (parking == null)
        {
            Console.WriteLine("Паркингът не е намерен.");
            Console.ReadLine();
            return;
        }
        Console.Write("Въведете регистрационния номер на превозното средство:");
        string vehicle = Console.ReadLine();
        if (parking.Vehicles.Remove(vehicle))
        {
            parking.AvailableSpaces++;
            SaveParkings();
            Console.WriteLine("Автомобилът е проверен успешно.");
        }
        else
        {
            Console.WriteLine("Превозното средство не е намерено.");
        }
        Console.ReadLine();
    }

    static void CheckAvailability()
    {
        Console.Write("Въведете местоположение или Parking ID: ");
        string input = Console.ReadLine();
        var parking = parkings.FirstOrDefault(p => p.Location.Equals(input, StringComparison.OrdinalIgnoreCase) || p.ParkingID.ToString() == input);
        if (parking == null)
        {
            Console.WriteLine("Паркингът не е намерен.");
        }
        else
        {
            Console.WriteLine($"Свободни места в {parking.Location}: {parking.AvailableSpaces}");
        }
        Console.ReadLine();
    }

    static void ViewAllParkings()
    {
        foreach (var parking in parkings)
        {
            Console.WriteLine($"ID: {parking.ParkingID}, Местоположение: {parking.Location}, Всички паркоместа: {parking.TotalSpaces}, Свободни места: {parking.AvailableSpaces}");
        }
        Console.ReadLine();
    }
    
}