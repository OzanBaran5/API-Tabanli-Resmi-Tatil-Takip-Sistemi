using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PublicHolidayTracker
{

    public class Holiday
    {
        public string date { get; set; }
        public string localName { get; set; }
        public string name { get; set; }
        public string countryCode { get; set; }


        [JsonPropertyName("fixed")]
        public bool @fixed { get; set; }

        public bool global { get; set; }
    }

    class Program
    {

        static List<Holiday> allHolidays = new List<Holiday>();
        static readonly HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            Console.WriteLine("Veriler API'den çekiliyor, lütfen bekleyiniz...");

 
            await LoadData();

            Console.Clear();
            Console.WriteLine("Veriler başarıyla yüklendi!");


            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\n===== PublicHolidayTracker =====");
                Console.WriteLine("1. Tatil listesini göster (yıl seçmeli)");
                Console.WriteLine("2. Tarihe göre tatil ara (gg-aa formatı)");
                Console.WriteLine("3. İsme göre tatil ara");
                Console.WriteLine("4. Tüm tatilleri 3 yıl boyunca göster (2023–2025)");
                Console.WriteLine("5. Çıkış");
                Console.Write("Seçiminiz: ");

                string choice = Console.ReadLine() ?? "";

                switch (choice)
                {
                    case "1":
                        ShowHolidaysByYear();
                        break;
                    case "2":
                        SearchByDate();
                        break;
                    case "3":
                        SearchByName();
                        break;
                    case "4":
                        ShowAllHolidays();
                        break;
                    case "5":
                        exit = true;
                        Console.WriteLine("Çıkış yapılıyor...");
                        break;
                    default:
                        Console.WriteLine("Geçersiz seçim, lütfen tekrar deneyin.");
                        break;
                }
            }
        }

        static async Task LoadData()
        {
            int[] years = { 2023, 2024, 2025 };

            foreach (var year in years)
            {
                try
                {
                    string url = $"https://date.nager.at/api/v3/PublicHolidays/{year}/TR";


                    var holidays = await client.GetFromJsonAsync<List<Holiday>>(url);

                    if (holidays != null)
                    {
                        allHolidays.AddRange(holidays);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{year} yılı verileri çekilirken hata oluştu: {ex.Message}");
                }
            }
        }

        static void ShowHolidaysByYear()
        {
            Console.Write("Listelenecek yılı girin (2023, 2024, 2025): ");
            string inputYear = Console.ReadLine() ??"";

            var filteredHolidays = allHolidays
                .Where(h => h.date.StartsWith(inputYear))
                .ToList();

            PrintList(filteredHolidays);
        }

        static void SearchByDate()
        {
            Console.Write("Tarih girin (Örn: 29-10): ");
            string inputDate = Console.ReadLine() ??""; 


            var foundHolidays = allHolidays.Where(h =>
            {

                if (DateTime.TryParse(h.date, out DateTime dt))
                {
                    string formattedDate = dt.ToString("dd-MM");
                    return formattedDate == inputDate;
                }
                return false;
            }).ToList();

            if (foundHolidays.Count > 0)
                PrintList(foundHolidays);
            else
                Console.WriteLine("Bu tarihte resmi bir tatil bulunamadı.");
        }

        static void SearchByName()
        {
            Console.Write("Tatil isminden bir parça girin (Örn: Zafer): ");
            string searchKey = (Console.ReadLine() ?? "").ToLower();

            var foundHolidays = allHolidays
                .Where(h => h.localName.ToLower().Contains(searchKey) || h.name.ToLower().Contains(searchKey))
                .ToList();

            if (foundHolidays.Count > 0)
                PrintList(foundHolidays);
            else
                Console.WriteLine("Eşleşen tatil bulunamadı.");
        }

        static void ShowAllHolidays()
        {
            Console.WriteLine("\n--- 2023-2025 Tüm Tatiller ---");
            PrintList(allHolidays);
        }

        static void PrintList(List<Holiday> list)
        {
            Console.WriteLine($"\nToplam {list.Count} kayıt bulundu:\n");
            Console.WriteLine("{0,-12} {1,-35} {2,-35}", "Tarih", "Yerel Ad", "Uluslararası Ad");
            Console.WriteLine(new string('-', 85));

            foreach (var h in list)
            {
                Console.WriteLine("{0,-12} {1,-35} {2,-35}", h.date, h.localName, h.name);
            }
            Console.WriteLine(new string('-', 85));
        }
    }
}