<h3>Projekt został zrealizowany w frameworku .NET z wykorzystaniem:</h3>

- Enitity (ORM dla C# .NET)
- PostgreSQL (Baza danych)
- Docker (Konteneryzacja)
- Minio (Przechowywanie zdjęć produktów)
- RabbitMQ (Przesyłanie komunikatów między procesami)
- xUnit & Moq (testy jednostkowe)
- Service Workers (serwis do logowania zdarzeń)
- Scalar (Swagger na sterydach)
- Blazor Web Assembly (Aplikacja webowa)
- JWT (Autoryzacja użytkowników)

<h3>Szczegółowe wymagania:</h3>

- **Działający Spring Boot**: projekt napisany w .NET (ASP.NET Core)
- **Połączenie z bazą danych**: konfiguracja EF Core z PostgreSQL
- **CRUD dla encji**: pełny CRUD dla `Product` oraz dla `User`
- **Poprawna struktura warstwowa (Controller/Service/Repository)**: zrealizowane warstwy w folderach `Controllers`, `Services`, `Repository`
- **DTO + Walidacja danych**: DTO w `ztpai/DTO` z adnotacjami walidacji (np. `ProductRequestDTO`), obsługa błędów walidacji w `Program.cs`
- **Obsługa błędów**: globalny handler middleware w `Middleware/GlobalExceptionHandler` + zwroty 400/404 w kontrolerach
- **Security (JWT lub Basic Auth)**: JWT (accessToken & refreshToken) do autoryzacji w kontollerach i `AuthService`, zarejestrowane w `Program.cs`
- **Unit Testy**: projekt `ztpai.UnitTests` z testami usług (`AuthServiceTests`, `OrderServiceTests`, `ProductsServiceTests`) w sumie 14 testów jednostkowych
- **Events LUB Kolejki (Rabbit/Kafka)**: RabbitMQ logowanie w `Services/LoggingRabbitMQ`, konsument w `NotificationService/Worker`
- **Czysty kod**: zastosowany podział na warstwy, wykorzystywanie Dependency Injection oraz Mapperów i DTO
- **Frontend**: frontend jest w Blazor WebAssembly (`ztpai.WebApp`) i udostępnia podstawowe akcje za pomocą interfejsu graficznego

<h3>Film demontracyjny:</h3>
https://youtu.be/sFq2SyThDf4

<h3>Wymagania:</h3>

- .NET SDK 10
- PostgreSQL (connection string set in ztpai/appsettings*.json or user secrets)
- RabbitMQ (host set to localhost in ztpai/appsettings.json)
- MinIO (address/credentials set in API config)

<h3>Uruchomienie:</h3>

- `docker compose up` w folderze głównym
- `dotnet restore`
- Worker: `dotnet run --project NotificationService`
- API: `dotnet run --project ztpai --launch-profile https`
- WebApp: `dotnet run --project ztpai.WebApp --launch-profile https`

<h3>Przydatne porty i credentials:</h3>

- Scalar: https://localhost:7094/scalar/v1
- API: https://localhost:7094, http://localhost:5097
- WebApp: https://localhost:7222, http://localhost:5227
- Minio login: `admin`
- Minio password: `adminadmin`
- Minio web app port: `9001`
- RabbitMQ login: `admin`
- RabbitMQ password: `admin`
- RabbitMQ web app port: `15672`
- PostgreSQL login: `admin`
- PostgreSQL password: `admin`
- PostgreSQL port: `5432`

<h3>Roadmap:</h3>

- Atraktycja graficznie strona główna z produktami
- Wyświetlanie obrazków produktów
- Dodanie koszyka w Web App za pomocą Redis
- Możliość składania zamówień z Web App
- Implementacja refreshToken w Web App (funkcjonalność w api istnieje)
