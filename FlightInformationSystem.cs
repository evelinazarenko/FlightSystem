using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using FlightSystem.Model;
using FlightSystem.Helpers;
using System.IO;

namespace FlightSystem
{
    // клас що представляє систему інформації про рейси
    public class FlightInformationSystem
    {
        #region Fields

        // шлях до файлу .json (графік польотів)
        private readonly string _flightsDatabaseFilePath;

        // шлях по файлу .json (результати графіку польотів)
        private readonly string _outputFlightsDatabaseFilePath;

        // приватне поле де зберігається інформація про рейси 
        private List<Flight> _flightSchedule;
        #endregion

        #region Constructors
        // конструктор класу, приймає шлях до файлу з графіком польотів та вхідного файлу
        public FlightInformationSystem(string flightsDatabaseFilePath,
                                       string outputFlightsDatabaseFilePath)
        {
            _flightsDatabaseFilePath = flightsDatabaseFilePath;
            _outputFlightsDatabaseFilePath = outputFlightsDatabaseFilePath;
            _flightSchedule = new List<Flight>();
            // читання графіку пльотів при створенні екземпляра класу
            ReadFlightsDb();
        }

        #endregion

        #region Methods
        // виведення інформації про всі рейси
        public string DisplayAllFlights()
        {
            var result = new StringBuilder();
            result.Append(Environment.NewLine);
            result.Append(Environment.NewLine);
            result.Append(Environment.NewLine);
            foreach (var flightInfo in _flightSchedule)
            {
                result.Append(flightInfo.Airline);
                result.Append(Environment.NewLine);

            }

            return result.ToString();
        }

        // додавання інформації про новий рейс
        public void AddFlightInfo(Flight newFlightInfo)
        {
            _flightSchedule.Add(newFlightInfo);
        }

        // видалення інформації про рейс
        public void RemoveFlightInfo(Flight flightInfoToRemove)
        {
            _flightSchedule.Remove(flightInfoToRemove);
        }

        // отримання всіх рейсів за назвою авіакомпанії
        public void GetAllFlightsByAirCompany(string airCompanyName)
        {
            var filteredData = _flightSchedule
                    .Where(flightInfo =>
            {
                if (flightInfo.Airline == airCompanyName)
                    return true;
                return false;
            })
                    .OrderBy(currentFlightInfo =>
                    {
                        return currentFlightInfo.DepartureTime;
                    })
                    .ToList();

            Console.WriteLine($"Filtered elements count:\t{filteredData.Count}");
            Console.WriteLine($"Located at:\t{_outputFlightsDatabaseFilePath}");

            // збереження відфільтрованих рейсів у вихідний файл
            SaveFlightDb(filteredData);

        }

        // отримання всіх рейсів за статусом
        public void GetAllFlightsByStatus(FlightStatus status)
        {
            var filteredData = _flightSchedule
                    .Where(flightInfo =>
            {
                if (flightInfo.Status == status)
                    return true;
                return false;
            })
                    .ToList();

            Console.WriteLine($"Filtered elements count:\t{filteredData.Count}");
            Console.WriteLine($"Located at:\t{_outputFlightsDatabaseFilePath}");

            SaveFlightDb(filteredData);

        }
        // отримання всіх рейсів за датою прибуття (заглушка, видає шлях до вихідного файлу)
        public string GetAllFlightsByDate(DateTime date)
        {
            var filteredFlights = _flightSchedule.Where(flightInfo =>
            {
                if (flightInfo.ArrivalTime == date)
                    return true;
                return false;
            }).ToList();
            // збереження всіх рейсів у вихідний файл (заглушка)
            SaveFlightDb();

            return _outputFlightsDatabaseFilePath;

        }

        // читання графіку польотів з файлу .json
        private void ReadFlightsDb()
        {
#pragma warning disable CS0168 // Переменная объявлена, но не используется
            try
            {
                var content = File.ReadAllText(_flightsDatabaseFilePath);

                var rootElem = JsonDocument.Parse(content).RootElement;

                if (rootElem.ValueKind == JsonValueKind.Null)
                    throw new ArgumentNullException(nameof(rootElem));

                // десеріалізація графіку польотів з JSON
                var flights = rootElem.EnumerateObject()
                                      .First()
                                      .Value
                                      .Deserialize<List<Flight>>(new JsonSerializerOptions()
                                      {
                                          Converters = {
                                            new TimeSpanJsonConverter(),
                                        },
                                      });

                _flightSchedule.Clear();
#pragma warning disable CS8604 // аргумент-силка, зі значенням NULL.
                _flightSchedule.AddRange(flights);
#pragma warning restore CS8604 // аргумент-силка, зі значенням NULL.
            }
            catch (FormatException formatException)
            {
                Console.WriteLine($"Incorrect JSON data format!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
#pragma warning restore CS0168 // переміна, не застосовується
        }

        private void SaveFlightDb(List<Flight> filteredFlightsInfo = null)
        {
            var jsonText = JsonSerializer.Serialize(
                filteredFlightsInfo ?? _flightSchedule,
            new JsonSerializerOptions()
            {
                WriteIndented = true,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault |
                                         System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
            });

            File.WriteAllText(_outputFlightsDatabaseFilePath, jsonText);

        }

        #endregion

    }
}