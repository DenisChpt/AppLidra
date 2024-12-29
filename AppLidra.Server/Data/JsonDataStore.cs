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

    private static readonly JsonSerializerOptions _deserializeOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private static readonly JsonSerializerOptions _serializeOptions = new()
    {
        WriteIndented = true
    };

    public JsonDataStore(string filePath)
    {
        _filePath = filePath;

        if (!File.Exists(_filePath))
        {
            SaveChanges();
        }

        LoadData();
    }

    private void LoadData()
    {
        lock (_lock)
        {
            string json = File.ReadAllText(_filePath);
            JsonData? data = JsonSerializer.Deserialize<JsonData>(json, _deserializeOptions);

            Projects = data?.Projects ?? [];
            Users = data?.Users ?? [];
            Expenses = data?.Expenses ?? [];
        }
    }

    public void SaveChanges()
    {
        lock (_lock)
        {
            JsonData data = new()
            {
                Projects = Projects,
                Users = Users,
                Expenses = Expenses
            };

            File.WriteAllText(_filePath, JsonSerializer.Serialize(data, _serializeOptions));
        }
    }
}
