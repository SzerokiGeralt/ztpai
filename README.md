Projekt **Web Api + Web App** został zrealizowany w frameworku **ASP .NET Core** z wykorzystaniem:
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

Szczegółowe wymagania:
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

Film demontracyjny:
[Link]

Wymagania:
- .NET SDK 10
- PostgreSQL (connection string set in ztpai/appsettings*.json or user secrets)
- RabbitMQ (host set to localhost in ztpai/appsettings.json)
- MinIO (address/credentials set in API config)

Uruchomienie:
- `docker compose up` w folderze głównym
- `dotnet restore`
- Worker: `dotnet run --project NotificationService`
- API: `dotnet run --project ztpai --launch-profile https`
- WebApp: `dotnet run --project ztpai.WebApp --launch-profile https`

Przydatne porty i credentials:
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

Roadmap & TODO:
- Atraktycja graficznie strona główna z produktami
- Wyświetlanie obrazków produktów
- Dodanie koszyka w Web App za pomocą Redis
- Możliość składania zamówień z Web App
- Implementacja refreshToken w Web App (funkcjonalność w api istnieje)
