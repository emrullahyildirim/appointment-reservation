-- AuthService için kullanıcı ve veritabanı
CREATE USER authservice_user WITH PASSWORD 'authservice_pass';
CREATE DATABASE "AuthServiceDb" OWNER authservice_user;
GRANT ALL PRIVILEGES ON DATABASE "AuthServiceDb" TO authservice_user;

-- PatientService için kullanıcı ve veritabanı
CREATE USER patientservice_user WITH PASSWORD 'patientservice_pass';
CREATE DATABASE "PatientAppointmentDb" OWNER patientservice_user;
GRANT ALL PRIVILEGES ON DATABASE "PatientAppointmentDb" TO patientservice_user;

-- Her kullanıcının sadece kendi veritabanına erişebilmesi için
-- (PostgreSQL'de kullanıcılar varsayılan olarak sadece kendi veritabanlarına bağlanabilir)