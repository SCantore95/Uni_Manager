using Uni_Manager.AppMenuManager;
using Uni_Manager.Entity;
using Uni_Manager.Service;



namespace Uni_Manager
{
   
public class Program
    {
      
        static AppMenuService appMenùService = new AppMenuService();

        static void Main(string[] args)
        {
            appMenùService.ImportAll();

            //AppMenu.Show();  
        }
    }
}
        