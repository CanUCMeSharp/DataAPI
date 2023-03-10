using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace DataAPI
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<Weather>> GetWeather()
        {
            var weather = new List<Weather>();
            //weather.Add(new Weather());
            //weather[0].Date = DateTime.Now;
            //"Server=localhost;Database=test;User Id=sa;Password=password;"
            try
            {
                using (var connection = new SqlConnection(WebApiConfig.DBConnectionString))
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
                                    Date = reader.GetDateTime(1),
                                    Temperature = reader.GetInt32(2),
                                    Rain = reader.GetInt32(3),
                                    Humidity = reader.GetInt32(4)
                                };

                                weather.Add(user);
                            }
                        }
                    }
                }
                Console.WriteLine("HTTPGET: Forwaded Data to User");
            }
            catch
            {
                Console.WriteLine("HTTPGET: UNKOWN ERROR");
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
            using (SqlConnection connection = new SqlConnection(WebApiConfig.DBConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO Weather (MDate, Temperature, Rain, Humidity) VALUES (@date, @temperature, @rain, @humidity)";
                    command.Parameters.Add("@date", SqlDbType.DateTime);
                    command.Parameters.Add("@temperature", SqlDbType.Int);
                    command.Parameters.Add("@rain", SqlDbType.Int);
                    command.Parameters.Add("@humidity", SqlDbType.Int);

                    foreach (var value in values)
                    {
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
            //do stuff
        }
    }
}
