using System;
using System.Text;
using System.Text.Json;
using Uni_Manager.Entity;
using Uni_Manager.Interface;

namespace Uni_Manager.Repository;

public class LogRepository 
{
    public List<Log> LogList { get; set; } = [];

    public void ExportLogListToJson()
    {
        String fileName = "LogsList.json";
        String path = @"C:\Users\Desktop\wa\c#_corso\University\University\Log\";

        StringBuilder SaveLogs = new();
        string saveLogsJson = string.Empty;

        try
        {
            foreach (Log log in LogList)
            {
                SaveLogs.AppendLine($"{log.Message};{log.Date};{log.ErrorPlace}");
            }


            saveLogsJson = JsonSerializer.Serialize(LogList, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(Path.Combine(path, fileName), saveLogsJson.ToString());


        }
        catch (Exception ex)
        {
            Console.WriteLine(ILog.AddNewLog(ex.Message, "ExportLogListToJson").PrintLog());
        }
    }
}
