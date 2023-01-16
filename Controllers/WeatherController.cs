using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace WeatherAPI
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<Weather>> GetWeather()
        {
            var weather = new List<Weather>();

            using (var connection = new SqlConnection("Server=localhost;Database=test;User Id=sa;Password=password;"))//Insert string
            {
                connection.Open();

                using (var command = new SqlCommand("SELECT * FROM Weather", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var user = new Weather
                            {
                                Date = reader.GetDateTime(0),
                                Temperature = reader.GetInt32(1),
                                Rain = reader.GetInt32(2),
                                Humidity = reader.GetInt32(3)
                            };

                            weather.Add(user);
                        }
                    }
                }
            }

            return Ok(weather);
        }
        [HttpPost]
        public void Post([FromBody] List<Weather> values, [FromHeader] string deviceId)
        {
            if(!(deviceId == "12345"))//Better Aut
            {
                return;
            }
            using (SqlConnection connection = new SqlConnection("Server=localhost;Database=test;User Id=sa;Password=password;"))//Insert string
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO LogTable (Date, DeviceId, Temperature, Rain, Humidity) VALUES (@deviceId, @date, @temperature)";
                    command.Parameters.Add("@deviceId", SqlDbType.NVarChar);
                    command.Parameters.Add("@date", SqlDbType.DateTime);
                    command.Parameters.Add("@temperature", SqlDbType.Int);
                    command.Parameters.Add("@rain", SqlDbType.Int);
                    command.Parameters.Add("@humidity", SqlDbType.Int);

                    foreach (var value in values)
                    {
                        command.Parameters["@deviceId"].Value = deviceId;
                        command.Parameters["@date"].Value = value.Date;
                        command.Parameters["@temperature"].Value = value.Temperature;
                        command.Parameters["@rain"].Value = value.Rain;
                        command.Parameters["@humidity"].Value = value.Humidity;
                        command.ExecuteNonQuery();
                    }
                }
            }
            doStuffWithData();
        }
        private void doStuffWithData()
        {
            throw new NotImplementedException();
        }
    }
}
