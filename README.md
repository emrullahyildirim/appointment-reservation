# Appointment Reservation System

Full-stack microservice tabanlı bir randevu rezervasyon sistemi:

- **Frontend**: React + Vite + TypeScript + Tailwind (hasta web uygulaması)
- **Backend**:
  - **AuthService** (kimlik doğrulama ve kullanıcı yönetimi)
  - **PatientService** (hasta, randevu, doktor, slot ve bekleme listesi yönetimi)
- **Gateway**: Nginx reverse proxy (frontend + backend’leri tek domain altında toplar)
- **Database**: PostgreSQL (Docker içinde, init script ile seed edilmiş)
- **Orkestrasyon**: Docker Compose

---

## 1. Mimari Genel Bakış

### 1.1. Servisler

- **AuthService**
  - Giriş / kayıt / refresh token
  - Kullanıcı ve role-based yetkilendirme
  - Dış servisler (ör. S2S token, external API ayarları)
  - Projeler:
    - `backend/Services/AuthService/AuthService.API`
    - `backend/Services/AuthService/AuthService.Business`
    - `backend/Services/AuthService/AuthService.DataAccess`
    - `backend/Services/AuthService/AuthService.Entities`

- **PatientService**
  - Hastalar (`Patients`)
  - Doktorlar ve ünvanları (`Doctors`, `DoctorTitles`)
  - Randevular (`Appointments`)
  - Randevu slotları (`AppointmentSlots`)
  - Bekleme listesi (`WaitlistEntries`)
  - Arka plan job’ı: slot üretimi (`SlotGenerationBackgroundService`)
  - Projeler:
    - `backend/Services/PatientService/PatientService.API`
    - `backend/Services/PatientService/PatientService.Business`
    - `backend/Services/PatientService/PatientService.DataAccess`
    - `backend/Services/PatientService/PatientService.Entities`

- **Shared Core**
  - `backend/Shared/Core`
  - Cross-cutting concerns:
    - Logging (Serilog uyumlu arayüz)
    - Caching (Memory/Redis)
    - Autofac AOP (validation, caching, logging, performance, transaction aspect’leri)
    - JWT security, claims yardımcıları
    - Result pattern (`IDataResult`, `SuccessResult`, vb.)
    - EF Core generic repository (`EfEntityRepositoryBase`)

- **Frontend (patient_website)**
  - Konum: `frontend/patient`
  - Stack:
    - React 18 / TypeScript
    - Vite
    - React Router
    - React Query
    - Tailwind CSS
  - Sayfalar:
    - Auth (login/register)
    - Home (doktor/slot arama, randevu alma)
  - API’ler:
    - Nginx üzerinden `/api/...` endpoint’lerine istek atar.

- **Nginx Gateway**
  - Konum: `gateway/nginx/nginx.conf`
  - Proxy kuralları:
    - Frontend: `/` → `patient_website`
    - Auth API: `/api/auth` → `auth_service`
    - Patient API:
      - `/api/patients`, `/api/appointments`, `/api/appointment-slots`, `/api/doctors`, `/api/waitlists` → `patient_service`
    - Swagger:
      - Auth swagger: `/swagger/auth/`
      - Patient swagger: `/swagger/patient/`  
        (`/swagger/patient/v1/swagger.json` istekleri backend’de `/swagger/v1/swagger.json`’a rewrite edilir.)

---

## 2. Çalıştırma (Docker Compose)

Proje kök dizininde:

docker compose down            # varsa eski container’ları durdur
docker compose up -d --build   # tüm sistemi ayağa kaldır### 2.1. Servisler ve portlar

- **Nginx Gateway**: `http://localhost/`
  - Frontend: `http://localhost/`
  - Auth Swagger: `http://localhost/swagger/auth/`
  - Patient Swagger: `http://localhost/swagger/patient/`
- **Postgres**: `localhost:5432` (development için dışa açılmış)

Docker Compose konumu: `docker-compose.yml`

Önemli özellikler:

- **Postgres healthcheck**:
  - `pg_isready -U postgres` ile hazır olduğunda `healthy`
- **Bağımlılıklar**:
  - `auth_service` ve `patient_service`, `postgres_db` `service_healthy` olmadan başlamaz.
  - `nginx_gateway`, auth/patient/frontend servisleri healthy olmadan trafik almaya başlamaz.
- **Volume’lar**:
  - `postgres_data` → veritabanı kalıcı depolama
  - `nginx_logs` → Nginx log dosyaları

---

## 3. Veritabanı Kurulumu

### 3.1. PostgreSQL init script

- Konum: `infra/postgres/init.sql`
- Ne yapar:
  - `AuthService` için:
    - Kullanıcı: `authservice_user`
    - DB: `AuthServiceDb`
  - `PatientService` için:
    - Kullanıcı: `patientservice_user`
    - DB: `PatientAppointmentDb`
  - İlgili user’lara DB owner ve tüm yetkiler atanır.

Bu script **sadece ilk DB yaratımında** (volume boşken) çalışır.  
Sonraki açılışlarda EF Core migration’ları devreye girer.

### 3.2. EF Core Migration / Otomatik Şema

`PatientService.API/Program.cs` içinde dev ortamda:
sharp
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;

    var db = services.GetRequiredService<PatientAppointmentContext>();
    // Veritabanını silmeden, migration'ları uygular.
    db.Database.Migrate();
}- İlk container açılışında:
  - `PatientAppointmentContext` için tüm tablolar (`Appointments`, `AppointmentSlots`, `Patients`, `Doctors`, `DoctorTitles`, `WaitlistEntries`, `__EFMigrationsHistory`) oluşturulur.
- `docker compose down` + `up -d --build` yaptığında:
  - Volume durduğu sürece veritabanı **silinmez**, sadece eksik migration varsa uygulanır.

---

## 4. Önemli Backend Ayrıntıları

### 4.1. PatientService API

- `Program.cs`:
  - `AddControllers()`, `AddEndpointsApiExplorer()`, `AddSwaggerGen(...)`
  - `UseCors("AllowAll")`
  - `UseSwagger()`, `UseSwaggerUI(...)`
    - Swagger endpoint:  
      `c.SwaggerEndpoint("/swagger/patient/v1/swagger.json", "PatientService API V1");`
  - DB config:
    - `UseNpgsql(builder.Configuration.GetConnectionString("PatientAppointmentDb"))`
    - Migration assembly: `"PatientService.DataAccess"`

- Örnek endpoint’ler:
  - `GET /api/Patients/all`
  - `POST /api/Patients`
  - `GET /api/Appointments/all`
  - `GET /api/AppointmentSlots/all`
  - `GET /api/Doctors/all`
  - `GET /api/Waitlists/all`, `POST /api/Waitlists`, `DELETE /api/Waitlists`

### 4.2. AuthService API

- JWT tabanlı kimlik doğrulama
- Örnek endpoint’ler:
  - `POST /api/Auth/login`
  - `POST /api/Auth/register`
  - `POST /api/Auth/refresh-token`
  - `GET /api/Users` vs.

---

## 5. Frontend (Patient Website)

- Konum: `frontend/patient`
- Önemli dosyalar:
  - `index.html` → root `div#root`, entrypoint `/src/index.tsx`
  - `src/index.tsx` → `PageRouter` ile React Router kurulumunu yapar.
  - `src/routes/index.tsx`:
    - `/auth` → Auth sayfası
    - `/` → `Layout` (topbar + sidebar + outlet)
  - `src/pages/Home` → ana hasta ana sayfası (doktor/slot arama, randevu al)

Başlatma (sadece frontend, development):

cd frontend/patient
npm install
npm run dev(Not: Docker içinde zaten build edilip Nginx ile serve ediliyor.)

---

## 6. Geliştirme Notları

### 6.1. Ortam Değişkenleri

`docker-compose.yml` içinde:

- `AuthService`:
  - `ConnectionStrings__AuthDb=Host=postgres_db;Database=AuthServiceDb;Username=authservice_user;Password=authservice_pass`
  - `TokenOptions__SecurityKey`, `TokenOptions__Issuer`, `TokenOptions__Audience`
  - `PatientService__Path=http://patient_service:8080/api/Patients`
- `PatientService`:
  - `ConnectionStrings__PatientAppointmentDb=Host=postgres_db;Database=PatientAppointmentDb;Username=patientservice_user;Password=patientservice_pass`
  - `ASPNETCORE_ENVIRONMENT=Development`
  - `DOTNET_ENVIRONMENT=Development`

### 6.2. Loglama

- AuthService için log dosyaları: `backend/Services/AuthService/AuthService.API/logs`
- Nginx logları: Docker volume `nginx_logs` (`/var/log/nginx`)

---


## 7. Komut Özeti

- Tüm sistemi ayağa kaldır:

docker compose up -d --build- Sistemi durdur:

docker compose down- Sadece veritabanını sıfırlamak (gerekirse):

docker compose down
docker volume rm appointment_postgres_data
docker compose up -d --build- PatientService DB tablolarını kontrol etmek:

docker exec appointment_postgres psql -U patientservice_user -d PatientAppointmentDb -c "\dt"---