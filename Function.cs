using System;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace ADO_NET_ДЗ_Модуль_01_Ua
{


    internal partial class Program
    {
        //static string connectionString = "Data Source=VageAndFruites.sqlite;";

        static async Task CreateDatabaseAndTableAsync()
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    Console.WriteLine("Connection to the database established.");

                    // Create the database (if not exists)
                    await CreateDatabaseAsync(connection);


                    // Create the table
                    await CreateTableAsync(connection);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating database and table: {ex.Message}");
                }
            }
        }

        static async Task CreateDatabaseAsync(SqliteConnection connection)
        {
            string createDatabaseQuery = "ATTACH DATABASE 'VageAndFruites.sqlite' AS NewDB";

            using (SqliteCommand command = new SqliteCommand(createDatabaseQuery, connection))
            {
                await command.ExecuteNonQueryAsync();
                Console.WriteLine("Database attached successfully.");
            }
        }

        static async Task CreateTableAsync(SqliteConnection connection)
        {
            await connection.OpenAsync();

            SqliteCommand checkTableCommand = new SqliteCommand()
            {
                CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name='VageAndFruites';",
                Connection = connection,
            };

            var result = await checkTableCommand.ExecuteScalarAsync();

            if (result == null)
            {
                SqliteCommand createTableCommand = new SqliteCommand()
                {
                    CommandText =
                        "CREATE TABLE VageAndFruites (ID INTEGER PRIMARY KEY AUTOINCREMENT,Title TEXT NOT NULL,Type TEXT NOT NULL,Color TEXT NOT NULL,Calories INTEGER NOT NULL)",
                    Connection = connection,
                };

                await createTableCommand.ExecuteNonQueryAsync();

                await InsertDataAsync(connection);
            }
        }

        static async Task InsertDataAsync(SqliteConnection connection)
        {
            string insertDataQuery = "INSERT OR IGNORE INTO VageAndFruites (Title, Type, Color, Calories) VALUES " +
                                     "('Яблуко', 'Фрукт', 'Жовтий', 52), " +
                                     "('Авокадо', 'Фрукт', 'Зелений', 160), " +
                                     "('Банан', 'Фрукт', 'Жовтий', 89), " +
                                     "('Помідор', 'Овочь', 'Червоний', 20), " +
                                     "('Груші', 'Фрукт', 'Жовтий', 57), " +
                                     "('Лимон', 'Фрукт', 'Жовтий', 29), " +
                                     "('Картофель', 'Овочь', 'Коричневий', 74), " +
                                     "('Цибуля', 'Овочь', 'Біла', 40), " +
                                     "('Огірок', 'Овочь', 'Зелений', 15), " +
                                     "('Буряк', 'Овочь', 'Червоний', 43)";
            using (SqliteCommand command = new SqliteCommand(insertDataQuery, connection))
            {
                await command.ExecuteNonQueryAsync();
                Console.WriteLine("Table insert complete");
            }
        }

        static async Task DisplayAllDataAsync(SqliteConnection connection)
        {
            string query = "SELECT * FROM VageAndFruites";

            using (SqliteCommand command = new SqliteCommand(query, connection))
            {
                using (SqliteDataReader reader = await command.ExecuteReaderAsync())
                {
                    Console.WriteLine("\nУсі дані з таблиці:");

                    while (await reader.ReadAsync())
                    {
                        Console.WriteLine($"ID: {reader["ID"]}, Назва: {reader["Title"]}, Тип: {reader["Type"]}, Колір: {reader["Color"]}, Калорійність: {reader["Calories"]}");
                    }
                }
            }
        }

        static async Task DisplayAllNamesAsync(SqliteConnection connection)
        {
            string query = "SELECT Title FROM VageAndFruites";

            using (SqliteCommand command = new SqliteCommand(query, connection))
            {
                using (SqliteDataReader reader = await command.ExecuteReaderAsync())
                {
                    Console.WriteLine("\nУсі назви овочів і фруктів:");

                    while (await reader.ReadAsync())
                    {
                        Console.WriteLine($"{reader["Title"]}");
                    }
                }
            }
        }

        static async Task DisplayAllColorsAsync(SqliteConnection connection)
        {
            string query = "SELECT DISTINCT Color FROM VageAndFruites";

            using (SqliteCommand command = new SqliteCommand(query, connection))
            {
                using (SqliteDataReader reader = await command.ExecuteReaderAsync())
                {
                    Console.WriteLine("\nУсі кольори:");

                    while (await reader.ReadAsync())
                    {
                        Console.WriteLine($"{reader["Color"]}");
                    }
                }
            }
        }

        static async Task DisplayMaxCaloriesAsync(SqliteConnection connection)
        {
            string query = "SELECT MAX(Calories) FROM VageAndFruites";

            using (SqliteCommand command = new SqliteCommand(query, connection))
            {
                object result = await command.ExecuteScalarAsync();
                if (result != DBNull.Value)
                {
                    int maxCalories = Convert.ToInt32(result);
                    Console.WriteLine($"\nМаксимальна калорійність: {maxCalories}");
                }
                else
                {
                    Console.WriteLine("\nНемає даних.");
                }
            }
        }

        static async Task DisplayMinCaloriesAsync(SqliteConnection connection)
        {
            string query = "SELECT MIN(Calories) FROM VageAndFruites";

            using (SqliteCommand command = new SqliteCommand(query, connection))
            {
                object result = await command.ExecuteScalarAsync();
                if (result != DBNull.Value)
                {
                    int minCalories = Convert.ToInt32(result);
                    Console.WriteLine($"\nМінімальна калорійність: {minCalories}");
                }
                else
                {
                    Console.WriteLine("\nНемає даних.");
                }
            }
        }

        static async Task DisplayAvgCaloriesAsync(SqliteConnection connection)
        {
            string query = "SELECT AVG(Calories) FROM VageAndFruites";

            using (SqliteCommand command = new SqliteCommand(query, connection))
            {
                object result = await command.ExecuteScalarAsync();
                if (result != DBNull.Value)
                {
                    int avgCalories = Convert.ToInt32(result);
                    Console.WriteLine($"\nСередня калорійність: {avgCalories}");
                }
                else
                {
                    Console.WriteLine("\nНемає даних.");
                }
            }
        }

        static async Task DisplayAmountVeg(SqliteConnection connection)
        {
            string typeToSearch = "Овочь";

            string query = $"SELECT Type, COUNT(*) as Count FROM VageAndFruites WHERE Type = '{typeToSearch}' COLLATE NOCASE";

            using (SqliteCommand command = new SqliteCommand(query, connection))
            {
                SqliteDataReader reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                {
                    string Type = reader["Type"].ToString();
                    int count = Convert.ToInt32(reader["Count"]);

                    Console.WriteLine($"Type: {Type}, Count: {count}");
                }
            }
        }

        static async Task DisplayAmountFruit(SqliteConnection connection)
        {
            string typeToSearch = "Фрукт";

            string query = $"SELECT Type, COUNT(*) as Count FROM VageAndFruites WHERE Type = '{typeToSearch}' COLLATE NOCASE";

            using (SqliteCommand command = new SqliteCommand(query, connection))
            {
                SqliteDataReader reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                {
                    string Type = reader["Type"].ToString();
                    int count = Convert.ToInt32(reader["Count"]);

                    Console.WriteLine($"Type: {Type}, Count: {count}");
                }
            }
        }

        static async Task DisplayAmountVagAndFruitForGivenColor(SqliteConnection connection, string colorToSearch)
        {
            string query = $"SELECT Color, COUNT(*) AS Count FROM VageAndFruites WHERE Color = '{colorToSearch}' COLLATE NOCASE";

            using (SqliteCommand command = new SqliteCommand(query, connection))
            {
                SqliteDataReader reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                {
                    string color = reader["Color"].ToString();
                    int count = Convert.ToInt32(reader["Count"]);

                    Console.WriteLine($"Color: {color}, Count: {count}");
                }
            }
        }

        static async Task DisplayAmountByColor(SqliteConnection connection)
        {
            string query = "SELECT Color, COUNT(*) as Count FROM VageAndFruites GROUP BY Color COLLATE NOCASE";

            using (SqliteCommand command = new SqliteCommand(query, connection))
            {
                SqliteDataReader reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                {
                    string color = reader["Color"].ToString();
                    int count = Convert.ToInt32(reader["Count"]);

                    Console.WriteLine($"Color: {color}, Count: {count}");
                }
            }
        }

        static async Task DisplayVegAndFruiteCaloriesIsIndicatedBelow(SqliteConnection connection, string caloriesToSearch)
        {
            string query = $"SELECT Title, Type, Calories FROM VageAndFruites WHERE Calories < {caloriesToSearch}";

            using (SqliteCommand command = new SqliteCommand(query, connection))
            {
                SqliteDataReader reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                {
                    string title = reader["Title"].ToString();
                    string type = reader["Type"].ToString();
                    int calories = Convert.ToInt32(reader["Calories"]);

                    Console.WriteLine($"Title: {title}, Type: {type}, Calories: {calories}");
                }
            }
        }

        static async Task DisplayVegAndFruiteCaloriesIsAbove(SqliteConnection connection, string caloriesToSearch)
        {
            string query = $"SELECT Title, Type, Calories FROM VageAndFruites WHERE Calories > {caloriesToSearch}";

            using (SqliteCommand command = new SqliteCommand(query, connection))
            {
                SqliteDataReader reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                {
                    string title = reader["Title"].ToString();
                    string type = reader["Type"].ToString();
                    int calories = Convert.ToInt32(reader["Calories"]);

                    Console.WriteLine($"Title: {title}, Type: {type}, Calories: {calories}");
                }
            }
        }

        static async Task DisplayVegAndFruiteCaloriesBetween(SqliteConnection connection, int mincaloriesToSearch, int maxcaloriesToSearch)
        {
            string query = $"SELECT * FROM VageAndFruites WHERE Calories BETWEEN {mincaloriesToSearch} AND {maxcaloriesToSearch}";

            using (SqliteCommand command = new SqliteCommand(query, connection))
            {
                SqliteDataReader reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                {
                    Console.WriteLine($"ID: {reader["ID"]}, Title: {reader["Title"]}, Type: {reader["Type"]}, Color: {reader["Color"]}, Calories: {reader["Calories"]}");
                }
            }
        }

        static async Task DisplayItemsByColor(SqliteConnection connection, string color)
        {
            string query = $"SELECT * FROM VageAndFruites WHERE Color = '{color}' COLLATE NOCASE";

            using (SqliteCommand command = new SqliteCommand(query, connection))
            {
                SqliteDataReader reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                {
                    Console.WriteLine($"ID: {reader["ID"]}, Title: {reader["Title"]}, Type: {reader["Type"]}, Color: {reader["Color"]}, Calories: {reader["Calories"]}");
                }
            }
        }
    }
}
