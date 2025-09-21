# ZimmetApp – Zimmet Yönetim Sistemi (ASP.NET Core MVC • .NET 6 • SQL Server)

İki sayfalık, basit ve pratik bir **Zimmet Yönetim** uygulaması. Giriş (login) yoktur.
- **Zimmet Listesi**: Tüm zimmetleri görüntüle, ara, düzenle, sil
- **Zimmet Giriş**: Yeni zimmet ekle / mevcut zimmeti düzenle, aynı ekranda **Kişi Ekle (Hızlı)**

> Amaç: Telefon/Tablet/Hat gibi ekipmanların personele zimmetlenmesini kolayca yönetmek.

---

##  Özellikler
- Kişi ekleme (hızlı form) + kişi seçimi
- Zimmet CRUD (ekle, listele, düzenle, sil)
- Arama (kişi/cihaz/seri-hat metnine göre)
- EF Core Code-First (migration ile tablo oluşturma)
- .NET 6 LTS, ASP.NET Core MVC, SQL Server




##  Teknolojiler
- .NET 6 (ASP.NET Core MVC)
- Entity Framework Core (SqlServer)
- SQL Server (LocalDB veya kurumsal instance)

---


### 1) Depoyu klonlayın
```bash
git clone https://github.com/<kullanici-adi>/ZimmetApp.git
cd ZimmetApp
```

### 2) Bağımlılıkları yükleyin (NuGet)
> Visual Studio yönetir; CLI isterseniz:
```bash
dotnet restore
```

### 3) Bağlantı dizesi (appsettings.json)
`appsettings.json` → `ConnectionStrings:DefaultConnection` değerini **hedef SQL Server**ınıza göre ayarlayın.

**Windows Auth (kurumsal):**
```json
"DefaultConnection": "Server=SQLSRV01\\MSSQL2019;Database=ZimmetDb;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True"
```

**SQL kullanıcı/şifre:**
```json
"DefaultConnection": "Server=SQLSRV01;Database=ZimmetDb;User Id=zimmet_user;Password=***;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True"
```

> Geliştirme için LocalDB:
> `Server=(localdb)\MSSQLLocalDB;Database=ZimmetDb;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True`

### 4) Veritabanını oluşturun (Migration)
**Visual Studio – Package Manager Console:**
```powershell
Add-Migration Init
Update-Database
```

**CLI:**
```bash
dotnet ef migrations add Init
dotnet ef database update
```

> Proje, açılışta `db.Database.Migrate()` çağrısıyla migration’ları otomatik de uygulayabilir (Program.cs içinde).

### 5) Çalıştırın
**Visual Studio:** F5 / IIS Express  
**CLI:**
```bash
dotnet run
```
Uygulama varsayılan olarak `https://localhost:xxxx/` üzerinde açılır.

---

## 🧭 Sayfalar / Rotalar
- **Zimmet Listesi:** `/Assignments/Index` (varsayılan ana sayfa)
- **Zimmet Giriş:** `/Assignments/Manage`  
  - Aynı sayfada **Kişi Ekle (Hızlı)** bölümü vardır.



##  Proje Yapısı (özet)
```
ZimmetApp/
├─ Controllers/
│  └─ AssignmentsController.cs
├─ Data/
│  └─ AppDbContext.cs
├─ Models/
│  ├─ Assignment.cs
│  ├─ ItemType.cs
│  └─ Person.cs
├─ Views/
│  └─ Assignments/
│     ├─ Index.cshtml   ← Zimmet Listesi
│     └─ Manage.cshtml  ← Zimmet Giriş (+ Kişi Ekle)
├─ appsettings.json
└─ Program.cs
```

---


### “Unable to resolve service for AppDbContext”
- `Program.cs` içinde:
  - `builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(...))` var mı?
  - `using ZimmetApp.Data;` eklendi mi?
- `AppDbContext` **public** mi ve tek bir sürümü var mı?

### “Invalid object name 'Persons' / 'Assignments'”
- Migration uygulanmamış olabilir → `Add-Migration` + `Update-Database`.
- Doğru veritabanına bağlı mısınız? `DefaultConnection` değerini kontrol edin.

### “Cannot insert the value NULL into column 'PersonId'”
- Eski kayıtlarda `PersonId` NULL olabilir. Geçici olarak `PersonId`’i nullable yapıp doldurun veya varsayılan bir “Bilinmiyor” kişisine bağlayın.

### Yanlış veritabanına bağlanma şüphesi
- `Program.cs`’de tanılama log’u alın: `db.Database.GetDbConnection().ConnectionString` ve `SELECT @@SERVERNAME, DB_NAME()`.


