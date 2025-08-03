using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

// Marker Interface
public interface IInventoryEntity
{
    int Id { get; }
}

// Immutable Inventory Record
public record InventoryItem(int Id, string Name, int Quantity, DateTime DateAdded) : IInventoryEntity;

// Generic Inventory Logger
public class InventoryLogger<T> where T : IInventoryEntity
{
    private List<T> _log = new();
    private readonly string _filePath;

    public InventoryLogger(string filePath)
    {
        _filePath = filePath;
    }

    public void Add(T item)
    {
        _log.Add(item);
    }

    public List<T> GetAll()
    {
        return new List<T>(_log);
    }

    public void SaveToFile()
    {
        try
        {
            string json = JsonSerializer.Serialize(_log, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
            Console.WriteLine("Data saved to file successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving to file: {ex.Message}");
        }
    }

    public void LoadFromFile()
    {
        try
        {
            if (File.Exists(_filePath))
            {
                string json = File.ReadAllText(_filePath);
                _log = JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
                Console.WriteLine("Data loaded from file successfully.");
            }
            else
            {
                Console.WriteLine("File not found. No data loaded.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading from file: {ex.Message}");
        }
    }
}

// Inventory App Integration
public class InventoryApp
{
    private InventoryLogger<InventoryItem> _logger;

    public InventoryApp(string filePath)
    {
        _logger = new InventoryLogger<InventoryItem>(filePath);
    }

    public void SeedSampleData()
    {
        _logger.Add(new InventoryItem(1, "Laptop", 7, DateTime.Now));
        _logger.Add(new InventoryItem(2, "Mouse", 20, DateTime.Now));
        _logger.Add(new InventoryItem(3, "Keyboard", 22, DateTime.Now));
        _logger.Add(new InventoryItem(4, "IPad", 3, DateTime.Now));
        _logger.Add(new InventoryItem(5, "HDMI Cable", 19, DateTime.Now));
    }

    public void SaveData()
    {
        _logger.SaveToFile();
    }

    public void LoadData()
    {
        _logger.LoadFromFile();
    }

    public void PrintAllItems()
    {
        var items = _logger.GetAll();
        Console.WriteLine("\n--- Inventory Items ---");
        foreach (var item in items)
        {
            Console.WriteLine($"ID: {item.Id}, Name: {item.Name}, Quantity: {item.Quantity}, Date Added: {item.DateAdded}");
        }
    }
}

// Main Program
public class Program
{
    public static void Main()
    {
        string path = "inventory.json";

        // Session 1 - Seed and Save
        var app = new InventoryApp(path);
        app.SeedSampleData();
        app.SaveData();

        // Simulate new session
        Console.WriteLine("\nClearing in-memory data and loading from file...");

        // Session 2 - Load and Print
        var newApp = new InventoryApp(path);
        newApp.LoadData();
        newApp.PrintAllItems();
    }
}
