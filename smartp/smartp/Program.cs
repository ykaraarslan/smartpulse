using System;
using smartp.Services;
using smartp.Services;

namespace smartp
{
    class Program
    {
        static void Main(string[] args)
        {
            var tgtService = new TgtService();
            var apiService = new ApiService();

            try
            {

                string tgtToken = tgtService.GetTgt("ykaraarslan39@gmail.com", "748159263Yk.");
                apiService.GetTransactionHistory(tgtToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata: " + ex.Message);
            }
        }
    }
}