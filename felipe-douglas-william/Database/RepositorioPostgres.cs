using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class PostgresRepository<T> : IDatabaseRepositorio<T>
    {
        private readonly string connectionString = "Server=localhost;User Id=postgres;Password=postgres;Database=produtos_precos;Port=5432;Pooling=true;";

        public PostgresRepository()
        {
        }

        public T GetById<T>(int id)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                string tableName = typeof(T).Name.ToLower() + "s"; // Nome da tabela, adicionando 's' no final

                string query = $"SELECT * FROM {tableName} WHERE id = @Id";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("Id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Criar uma instância do tipo T e preencher os valores
                            T instance = Activator.CreateInstance<T>();

                            foreach (var property in typeof(T).GetProperties())
                            {
                                object value = reader[property.Name];
                                property.SetValue(instance, value);
                            }

                            return instance;
                        }
                    }
                }
            }

            return default(T);
        }

        public List<T> Get<T>()
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                string tableName = typeof(T).Name.ToLower() + "s"; // Nome da tabela, adicionando 's' no final

                string query = $"SELECT * FROM {tableName}";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        List<T> entities = new List<T>();

                        while (reader.Read())
                        {
                            T instance = Activator.CreateInstance<T>();

                            foreach (var property in typeof(T).GetProperties())
                            {
                                object value = reader[property.Name];
                                property.SetValue(instance, value);
                            }

                            entities.Add(instance);
                        }

                        return entities;
                    }
                }
            }
        }

        public int Insert<T>(T entity)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                string tableName = typeof(T).Name.ToLower() + "s"; // Nome da tabela, adicionando 's' no final

                var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                string fieldNames = string.Join(", ", properties.Where(p => p.Name != "Id").Select(p => p.Name));
                string paramNames = string.Join(", ", properties.Where(p => p.Name != "Id").Select(p => $"@{p.Name}"));

                string query = $"INSERT INTO {tableName} ({fieldNames}) VALUES ({paramNames}); SELECT lastval();"; // Consulta para obter o último ID inserido

                using (var command = new NpgsqlCommand(query, connection))
                {
                    foreach (var property in properties)
                    {
                        if (property.Name != "Id")
                        {
                            var paramName = "@" + property.Name;
                            var paramValue = property.GetValue(entity);

                            command.Parameters.AddWithValue(paramName, paramValue);
                        }
                    }

                    // Obtém o último ID inserido
                    object insertedId = command.ExecuteScalar();
                    if (insertedId != null && int.TryParse(insertedId.ToString(), out int id))
                    {
                        return id;
                    }
                }
            }
            return 0;
        }

        public void Update<T>(T entity)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                string tableName = typeof(T).Name.ToLower() + "s"; // Nome da tabela, adicionando 's' no final

                var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                var updateFields = string.Join(", ", properties.Select(p => $"{p.Name} = @{p.Name}"));

                string query = $"UPDATE {tableName} SET {updateFields} WHERE id = @Id";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    foreach (var property in properties)
                    {
                        var paramName = "@" + property.Name;
                        var paramValue = property.GetValue(entity);

                        command.Parameters.AddWithValue(paramName, paramValue);
                    }

                    var idProperty = typeof(T).GetProperty("Id");
                    var idValue = idProperty.GetValue(entity);

                    command.Parameters.AddWithValue("@Id", idValue);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete<T>(int id)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                string tableName = typeof(T).Name.ToLower() + "s"; // Nome da tabela, adicionando 's' no final

                string query = $"DELETE FROM {tableName} WHERE id = @Id";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("Id", id);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
