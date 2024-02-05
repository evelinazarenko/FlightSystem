using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using FlightSystem.Model;
using FlightSystem.Helpers;
using System.IO;

namespace FlightSystem
{
    // ���� �� ����������� ������� ���������� ��� �����
    public class FlightInformationSystem
    {
        #region Fields

        // ���� �� ����� .json (������ �������)
        private readonly string _flightsDatabaseFilePath;

        // ���� �� ����� .json (���������� ������� �������)
        private readonly string _outputFlightsDatabaseFilePath;

        // �������� ���� �� ���������� ���������� ��� ����� 
        private List<Flight> _flightSchedule;
        #endregion

        #region Constructors
        // ����������� �����, ������ ���� �� ����� � �������� ������� �� �������� �����
        public FlightInformationSystem(string flightsDatabaseFilePath,
                                       string outputFlightsDatabaseFilePath)
        {
            _flightsDatabaseFilePath = flightsDatabaseFilePath;
            _outputFlightsDatabaseFilePath = outputFlightsDatabaseFilePath;
            _flightSchedule = new List<Flight>();
            // ������� ������� ������ ��� �������� ���������� �����
            ReadFlightsDb();
        }

        #endregion

        #region Methods
        // ��������� ���������� ��� �� �����
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

        // ��������� ���������� ��� ����� ����
        public void AddFlightInfo(Flight newFlightInfo)
        {
            _flightSchedule.Add(newFlightInfo);
        }

        // ��������� ���������� ��� ����
        public void RemoveFlightInfo(Flight flightInfoToRemove)
        {
            _flightSchedule.Remove(flightInfoToRemove);
        }

        // ��������� ��� ����� �� ������ ���������
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

            // ���������� �������������� ����� � �������� ����
            SaveFlightDb(filteredData);

        }

        // ��������� ��� ����� �� ��������
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
        // ��������� ��� ����� �� ����� �������� (��������, ���� ���� �� ��������� �����)
        public string GetAllFlightsByDate(DateTime date)
        {
            var filteredFlights = _flightSchedule.Where(flightInfo =>
            {
                if (flightInfo.ArrivalTime == date)
                    return true;
                return false;
            }).ToList();
            // ���������� ��� ����� � �������� ���� (��������)
            SaveFlightDb();

            return _outputFlightsDatabaseFilePath;

        }

        // ������� ������� ������� � ����� .json
        private void ReadFlightsDb()
        {
#pragma warning disable CS0168 // ���������� ���������, �� �� ������������
            try
            {
                var content = File.ReadAllText(_flightsDatabaseFilePath);

                var rootElem = JsonDocument.Parse(content).RootElement;

                if (rootElem.ValueKind == JsonValueKind.Null)
                    throw new ArgumentNullException(nameof(rootElem));

                // ������������ ������� ������� � JSON
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
#pragma warning disable CS8604 // ��������-�����, � ��������� NULL.
                _flightSchedule.AddRange(flights);
#pragma warning restore CS8604 // ��������-�����, � ��������� NULL.
            }
            catch (FormatException formatException)
            {
                Console.WriteLine($"Incorrect JSON data format!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
#pragma warning restore CS0168 // �������, �� �������������
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