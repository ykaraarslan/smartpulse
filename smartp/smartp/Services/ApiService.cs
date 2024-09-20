using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace smartp.Services
{
    public class TransactionHistoryData
    {
        public string Date { get; set; }
        public string Hour { get; set; }
        public string ContractName { get; set; }
        public double Price { get; set; }
        public double Quantity { get; set; }
        public long Id { get; set; }
    }

    public class TransactionHistoryResponseData
    {
        public List<TransactionHistoryData> Items { get; set; }
    }

    public class ApiService
    {
        private readonly HttpClient _client;

        public ApiService()
        {
            _client = new HttpClient();
        }

        public void GetTransactionHistory(string tgtToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://seffaflik.epias.com.tr/electricity-service/v1/markets/idm/data/transaction-history");

            request.Headers.Add("TGT", tgtToken);
            var content = new StringContent("{\"startDate\":\"2024-09-19T00:00:00+03:00\",\"endDate\":\"2024-09-20T00:00:00+03:00\",\"page\":{\"number\":1,\"size\":50}}",
                Encoding.UTF8,
                "application/json");
            request.Content = content;

            var response = _client.Send(request);

            var responseContent = response.Content.ReadAsStringAsync().Result;

            var transactionHistoryResponse = JsonConvert.DeserializeObject<TransactionHistoryResponseData>(responseContent);
            ProcessTransactionData(transactionHistoryResponse.Items);
        }

        private void ProcessTransactionData(List<TransactionHistoryData> items)
        {
            var grupData = items.GroupBy(x => x.ContractName)
                .Select(g => new
                {
                    ContractName = g.Key,
                    TotalQuantity = g.Sum(x => x.Quantity) / 10,
                    TotalAmount = g.Sum(x => (x.Price * x.Quantity) / 10),
                    WeightedAveragePrice = g.Sum(x => (x.Price * x.Quantity) / 10) / (g.Sum(x => x.Quantity) / 10),
                    Date = DateTime.Parse(g.First().Date)
                });

            foreach (var data in grupData)
            {
                Console.WriteLine($"Contract: {data.ContractName}," +
                    $" Toplam İşlem Miktarı: {data.TotalQuantity}," +
                    $" Toplam İşlem Tutarı: {data.TotalAmount}," +
                    $" Ağırlıklı Ortalama Fiyat: {data.WeightedAveragePrice}," +
                    $" Tarih: {data.Date}");
            }
        }
    }
}
