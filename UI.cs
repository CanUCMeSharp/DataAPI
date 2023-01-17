using System.Configuration;
using System;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace DataAPI
{
    public static class UI
    {
        public static void InitUserDialogue()
        {
            WebApiConfig.DBConnectionString = ReadSetting("SQLConString");
            Console.WriteLine("Welcome to the TE2023 Server API!");
            int state = 0;
            bool finished = false;
            while (!finished)
            {
                switch (state)
                {
                    case 0:
                        Console.WriteLine("Do you want to change the default setting? [Y/N]");
                        var input = Console.ReadLine();
                        if (input == "Y")
                        {
                            state = 1;
                        }
                        else if(input == "N")
                        {
                            if(WebApiConfig.DBConnectionString == null)
                            {
                                Console.WriteLine("Sorry, you have to configure first!");
                                state = 1;
                                break;
                            }
                            state = -1;
                        }
                        break;
                    case 1:
                        Console.WriteLine("What's the IP of the SQL DB");
                        WebApiConfig.DBConnectionString += "Server=" + Console.ReadLine() + ";";
                        state++;
                        break;
                    case 2:
                        Console.WriteLine("What's the Database of the SQL DB");
                        WebApiConfig.DBConnectionString += "Database=" + Console.ReadLine() + ";";
                        state++;
                        break;
                    case 3:
                        Console.WriteLine("What's the User ID of the SQL DB");
                        WebApiConfig.DBConnectionString += "User Id=" + Console.ReadLine() + ";";
                        state++;
                        break;
                    case 4:
                        Console.WriteLine("What's the Password of the SQL DB");
                        WebApiConfig.DBConnectionString += "Password=" + Console.ReadLine() + ";";
                        AddUpdateAppSettings("SQLConString", WebApiConfig.DBConnectionString);
                        state++;
                        break;
                    default:
                        Console.WriteLine("Have fun with the API!");
                        finished = true;
                        break;
                }
            }
        }
        private static string? ReadSetting(string key)
        {
            string? result = null;
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                result = appSettings[key] ?? "Not Found";
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error reading app settings");
            }
            return result;
        }

        static void AddUpdateAppSettings(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }
        }
    }
}
