using AppLidra.Shared.Models;
using AppLidra.Shared.Services;
using System.Text.Json;

namespace AppLidra.Server.Data;

public class JsonDataStore
{
    private readonly string _filePath;
    private readonly object _lock = new();

    public List<Project> Projects { get; private set; } = [];
    public List<User> Users { get; private set; } = [];
    public List<Expense> Expenses { get; private set; } = [];

    public JsonDataStore(string filePath)
    {
        _filePath = filePath;

        if (!File.Exists(_filePath))
        {
            SaveChanges(); // Crée un fichier JSON vide
        }

        LoadData();
    }

    private void LoadData()
    {
        lock (_lock)
        {
            var json = File.ReadAllText(_filePath);
            var data = JsonSerializer.Deserialize<JsonData>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Projects = data?.Projects ?? [];
            Users = data?.Users ?? [];
            Expenses = data?.Expenses ?? [];
        }
    }

    public void SaveChanges()
    {
        lock (_lock)
        {
            var data = new JsonData
            {
                Projects = Projects,
                Users = Users,
                Expenses = Expenses
            };

            File.WriteAllText(_filePath, JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true }));
        }
    }
}
