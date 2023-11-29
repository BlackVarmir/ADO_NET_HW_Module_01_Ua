using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace ADO_NET_ДЗ_Модуль_01_Ua
{
    internal partial class Program
    {
        
        static string connectionString = "Data Source=VageAndFruites.sqlite;";

        static async Task Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            await CreateDatabaseAndTableAsync();
            bool d = true;
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    Console.WriteLine("Підключено до бази даних.");

                    while (d)
                    {


                        Console.WriteLine("1. Вивести всю таблицю.");
                        Console.WriteLine("2. Вивести всі назви фруктів та овочів.");
                        Console.WriteLine("3. Вивести всі кольори.");
                        Console.WriteLine("4. Вивести максимальну калорійність.");
                        Console.WriteLine("5. Вивести мінімальну калорійність.");
                        Console.WriteLine("6. Вивести середню калорійність.");
                        Console.WriteLine("\n\n");
                        Console.WriteLine("7. Вивести кількість овочів.");
                        Console.WriteLine("8. Вивести кількість фруктів.");
                        Console.WriteLine("9. Вивести кількість фруктів та овочів за заданим кольором.");
                        Console.WriteLine("10. Вивести кількість фруктів та овочів кожного кольору.");
                        Console.WriteLine("11. Вивести калорійність фруктів та овочів меньшу за введене число.");
                        Console.WriteLine("12. Вивести калорійність фруктів та овочів вишю за введене число.");
                        Console.WriteLine("13. Вмвести в діапозоні калорійності.");
                        Console.WriteLine("14. Вивести усі овочі та фрукти жовтого або червоного кольору.");
                        Console.WriteLine("0. Вихід.");

                        int a = Convert.ToInt32(Console.ReadLine());
                        
                        if (a == 0)
                        {
                            d = false;
                            return;
                        }
                        else
                        {
                            switch (a)
                            {
                                case 1:
                                {
                                    await DisplayAllDataAsync(connection);
                                    Console.WriteLine("\n\n");
                                    break;
                                }

                                case 2:
                                {
                                    await DisplayAllNamesAsync(connection);
                                    Console.WriteLine("\n\n");
                                    break;
                                }

                                case 3:
                                {
                                    await DisplayAllColorsAsync(connection);
                                    Console.WriteLine("\n\n");
                                    break;
                                }

                                case 4:
                                {
                                    await DisplayMaxCaloriesAsync(connection);
                                    Console.WriteLine("\n\n");
                                    break;
                                }

                                case 5:
                                {
                                    await DisplayMinCaloriesAsync(connection);
                                    Console.WriteLine("\n\n");
                                    break;
                                }

                                case 6:
                                {
                                    await DisplayAvgCaloriesAsync(connection);
                                    Console.WriteLine("\n\n");
                                    break;
                                }

                                case 7:
                                {
                                    await DisplayAmountVeg(connection); 
                                    Console.WriteLine("\n\n");
                                    break;
                                }
                                case 8:
                                {
                                    await DisplayAmountFruit(connection);
                                    Console.WriteLine("\n\n");
                                    break;
                                }

                                case 9:
                                {
                                    Console.WriteLine("\nВведіть колір по якому хочете дізнатися кількість: ");
                                    string h = Console.ReadLine();
                                    await DisplayAmountVagAndFruitForGivenColor(connection, h);
                                    Console.WriteLine("\n\n");
                                    break;
                                }

                                case 10:
                                {
                                    await DisplayAmountByColor(connection);
                                    Console.WriteLine("\n\n");
                                    break;
                                }

                                case 11:
                                {
                                    Console.WriteLine("\nВведіть число калорійності меньше якого ви хочете побачити фрукти та овочі: ");
                                    string h = Console.ReadLine();
                                    await DisplayVegAndFruiteCaloriesIsIndicatedBelow(connection, h);
                                    Console.WriteLine("\n\n");
                                    break;
                                }

                                case 12:
                                {
                                    Console.WriteLine("\nВведіть число калорійності вище якого ви хочете побачити фрукти та овочі: ");
                                    string h = Console.ReadLine();
                                    await DisplayVegAndFruiteCaloriesIsAbove(connection, h);
                                    Console.WriteLine("\n\n");
                                    break;
                                }

                                case 13:
                                {
                                    Console.WriteLine("Введіть мінімальну калорійність: ");
                                    int h = Convert.ToInt32(Console.ReadLine());
                                    Console.WriteLine("Введіть максимальну калорійність: ");
                                    int j = Convert.ToInt32(Console.ReadLine());
                                    await DisplayVegAndFruiteCaloriesBetween(connection, h, j);
                                    Console.WriteLine("\n\n");
                                    break;
                                }

                                case 14:
                                {
                                    Console.WriteLine("1. Жовтий");
                                    Console.WriteLine("2. Червоний");
                                    string h;
                                    int j = Convert.ToInt32(Console.ReadLine());
                                    switch (j)
                                    {
                                        case 1:
                                        {
                                            h = "Жовтий";
                                            await DisplayItemsByColor(connection, h);
                                            break;
                                        }
                                        case 2:
                                        {
                                            h = "Червоний";
                                            await DisplayItemsByColor(connection, h);
                                            break;
                                        }
                                        default:
                                        {
                                            Console.WriteLine("Ви ввели неправельне значеня.");
                                            break;
                                        }
                                    }
                                    Console.WriteLine("\n\n");
                                    break;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Помилка підключення: {ex.Message}");
                }
            }
        }

        /*static async Task CreateDatabaseAndTableAsync()
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
        }*/
    }
}