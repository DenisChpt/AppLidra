//-----------------------------------------------------------------------
// <copiright file="JsonDataStore.cs">
//      Copyright (c) 2024 Damache Kamil, Ziani Racim, Chaput Denis. All rights reserved.
// </copyright>
// <author> Damache Kamil, Ziani Racim, Chaput Denis </author>
//-----------------------------------------------------------------------

namespace AppLidra.Server.Data
{
    using System.Text.Json;
    using AppLidra.Shared.Models;

    /// <summary>
    /// Represents a data store that handles JSON serialization and deserialization for projects, users, and expenses.
    /// </summary>
    public class JsonDataStore
    {
        private static readonly JsonSerializerOptions _deserializeOptions = new ()
        {
            PropertyNameCaseInsensitive = true,
        };

        private static readonly JsonSerializerOptions _serializeOptions = new ()
        {
            WriteIndented = true,
        };

        private readonly string _filePath;
        private readonly object _lock = new ();

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonDataStore"/> class.
        /// </summary>
        /// <param name="filePath">The file path to the JSON data store.</param>
        public JsonDataStore(string filePath)
        {
            this._filePath = filePath;

            if (!File.Exists(this._filePath))
            {
                this.SaveChanges();
            }

            this.LoadData();
        }

        /// <summary>
        /// Gets the list of projects.
        /// </summary>
        public List<Project> Projects { get; private set; } = [];

        /// <summary>
        /// Gets the list of users.
        /// </summary>
        public List<User> Users { get; private set; } = [];

        /// <summary>
        /// Gets the list of expenses.
        /// </summary>
        public List<Expense> Expenses { get; private set; } = [];

        /// <summary>
        /// Saves the changes to the JSON data store.
        /// </summary>
        public void SaveChanges()
        {
            lock (this._lock)
            {
                JsonData data = new (this.Projects, this.Users, this.Expenses);

                File.WriteAllText(this._filePath, JsonSerializer.Serialize(data, _serializeOptions));
            }
        }

        private void LoadData()
        {
            lock (this._lock)
            {
                string json = File.ReadAllText(this._filePath);
                JsonData? data = JsonSerializer.Deserialize<JsonData>(json, _deserializeOptions);

                this.Projects = data?.Projects ?? [];
                this.Users = data?.Users ?? [];
                this.Expenses = data?.Expenses ?? [];
            }
        }
    }
}