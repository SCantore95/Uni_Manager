using System;
using System.Text;
using System.Text.Json;
using Uni_Manager.Entity;
using Uni_Manager.Repository;

namespace Uni_Manager.Service;

public class LogService
{
    LogRepository logRepository = new LogRepository();

   public void PrintLogList()
    {
        logRepository.LogList.ForEach(l => Console.WriteLine(l.PrintLog() + "\n"));
    }


}
