internal class Program_Temp 
{
    public static void MainTemp(string[] args) 
    {
        
        var taskServer = TempServer.Run();
        //var taskClient = TempClient.Run();

        //taskClient.Wait();
        taskServer.Wait();
    }
}

