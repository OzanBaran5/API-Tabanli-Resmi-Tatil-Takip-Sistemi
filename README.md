# API Tabanlı Türkiye Resmi Tatil Takip Sistemi 🇹🇷

Bu proje, C# Konsol Uygulaması olarak geliştirilmiş olup, **nager.at** API servisini kullanarak Türkiye'deki resmi tatil verilerini dinamik olarak çeker ve kullanıcıya çeşitli sorgulama imkanları sunar.

## 🎯 Proje Amacı
Bu uygulamanın temel amacı, `HttpClient` kullanarak dış bir kaynaktan (API) JSON formatında veri çekmek, bu veriyi C# nesnelerine (Class) dönüştürmek (Deserialize) ve LINQ sorguları ile bellekteki veriyi filtrelemektir.

## 🚀 Özellikler

* **Canlı Veri:** Uygulama açılışta 2023, 2024 ve 2025 yılları için API'ye istek atar.
* **JSON İşleme:** Gelen JSON verisi `System.Text.Json` kütüphanesi ile parse edilir.
* **Yıl Bazlı Listeleme:** Kullanıcı istediği yıla ait tatilleri görebilir.
* **Tarih Arama:** `gg-aa` (Örn: 29-10) formatında girilen tarihin tatil olup olmadığını kontrol eder.
* **İsim Arama:** Tatil ismine göre (Örn: "Zafer") arama yapar.
* **Null Safety:** Boş değerlere ve kullanıcı hatalarına karşı güvenli kod yapısı içerir.

## 🛠 Kullanılan Teknolojiler ve Kütüphaneler

* **Dil:** C# (.NET 6/7/8 uyumlu)
* **Platform:** Console Application
* **Kütüphaneler:**
    * `System.Net.Http` (API İstekleri için)
    * `System.Text.Json` (JSON Deserialization için)
    * `System.Linq` (Veri filtreleme ve sorgulama için)
    * `System.Threading.Tasks` (Asenkron işlemler için)

## ⚙️ Kodun Çalışma Mantığı

Proje 3 ana aşamadan oluşur:

1.  **Veri Çekme (Initialization):**
    * Program başladığında `LoadData()` metodu çalışır.
    * `HttpClient` nesnesi ile `https://date.nager.at/api/v3/PublicHolidays/{YIL}/TR` adresine asenkron (`async/await`) istek atılır.
    * Gelen veri `List<Holiday>` listesine dönüştürülüp hafızaya (RAM) kaydedilir.
    * *Not:* API'deki `fixed` değişken ismi C# için rezerve olduğundan, `[JsonPropertyName("fixed")]` özniteliği ile eşleştirme yapılmıştır.

2.  **Kullanıcı Etkileşimi (Menu Loop):**
    * Kullanıcıya 5 seçenekli bir menü sunulur.
    * Kullanıcı "Çıkış" diyene kadar `while` döngüsü çalışmaya devam eder.

3.  **Sorgulama (Business Logic):**
    * **Tarih Arama:** API'den gelen `yyyy-MM-dd` formatındaki tarih, `DateTime` nesnesine çevrilir ve kullanıcının girdiği `dd-MM` formatıyla karşılaştırılır.
    * **İsim Arama:** Kullanıcının girdiği metin, tatillerin hem yerel (`localName`) hem de uluslararası (`name`) adlarında `Contains` metodu ile aranır.

---
**Geliştirici:** Ozan Baran Karakurt  
