🎬 ApiPelículas
ApiPelículas es una aplicación web full-stack desarrollada con C# .NET para el backend y Angular para el frontend. Este proyecto fue creado como práctica para aprender Angular, implementando un gestor de películas completo con funcionalidades CRUD, filtrado, paginación y autenticación dinámica.

✨ Características principales
CRUD completo de películas (Crear, Leer, Actualizar, Eliminar)
Interfaz de inicio (Home) con listado de películas
Sistema de filtrado para búsqueda avanzada
Paginación para mejor rendimiento y experiencia de usuario
Autenticación dinámica con login y registro de usuarios
Gestión de roles para control de acceso
Rutas públicas y protegidas según estado de sesión
API RESTful robusta y escalable
Arquitectura separada frontend/backend
Despliegue en la nube con diferentes proveedores

🛠️ Tecnologías utilizadas
Backend
C# .NET Core - Framework principal
Entity Framework - ORM para base de datos
SQL Server - Base de datos
Docker - Contenedorización del servidor SQL local
Frontend
Angular - Framework de frontend
TypeScript - Lenguaje principal
HTML/CSS - Estructura y estilos
Bootstrap/Material - Estilos y componentes visuales
Infraestructura
Azure SQL Database - Base de datos en la nube
Render - Despliegue de API
Railway - Despliegue alternativo de API
Docker - Contenedores locales

⚙️ Requisitos
Para desarrollo local
.NET 6.0 o superior
Node.js  16+ y npm
Angular CLI
Docker Desktop
SQL Server (local o Azure)

🚀 Instalación y configuración
1. Clonar el repositorio
bash
git clone https://github.com/Edwin-Vargas94/ApiPeliculas.git
cd ApiPeliculas

2. Configurar el Backend (API)
Configurar base de datos
bash
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Evargas12@" -p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server
Configurar cadena de conexión
Edita appsettings.json o appsettings.Development.json:
json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=tcp:apipeliculas.database.windows.net,1433;Initial Catalog=peliculas;Persist Security Info=False;User ID=egvg94;Password=Evargas12;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
  }
}

####Ejecutar migraciones
bash:
dotnet ef database update

####Ejecutar la API
bash:
dotnet run

3. Configurar el Frontend (Angular)

####Instalar dependencias
bash:
cd ClientApp
npm install

####Configurar URL de la API
Edita src/environments/environment.ts
typescript:
export const environment = {
  production: false,
  apiUrl: 'https://apipeliculas-rtsd.onrender.com'
};

####Ejecutar la aplicación
bash:
ng serve

📁 Estructura del proyecto
Código
ApiPeliculas/
├── Controllers/           # Controladores de la API (incluye AuthController)
├── Data/                 # Contexto de Entity Framework
├── Models/               # Modelos de datos (Película, Usuario, LoginDTO, etc.)
├── Repositorio/          # Patrón Repository
├── Services/             # Servicios para autenticación y roles
├── peliculas-app/
│   ├── src/
│   │   ├── app/
│   │   │   ├── components/
│   │   │   │   ├── login/
│   │   │   │   ├── register-user/
│   │   │   │   ├── peliculas/
│   │   │   ├── services/
│   │   │   │   ├── auth.service.ts
│   │   │   └── guards/
│   │   │       └── auth.guard.ts
├── Dockerfile
├── ApiPeliculas.csproj
└── README.md

🎯 Funcionalidades principales

🏠 Página de Inicio
Listado completo de películas
Filtrado por título, género, año, etc.
Paginación para mejor rendimiento
Diseño responsive

📝 Gestión de Películas
Crear: Agregar nuevas películas
Leer: Visualizar detalles
Actualizar: Editar información
Eliminar: Remover del sistema
Contenedor visual con botones de acción CRUD

🔐 Autenticación y Registro
Login dinámico con validación desde API
Registro de usuarios desde componente público
Gestión de roles para control de acceso
Rutas protegidas con AuthGuard
Ruta pública /register-user sin necesidad de login

🔍 Filtros y Búsqueda
Búsqueda por título
Filtrado por categorías
Ordenamiento por criterios
Filtros combinados

🌐 Despliegue
API en Render
https://apipeliculas-rtsd.onrender.com
API en Railway
https://tu-api-railway.com
Base de datos
Desarrollo: SQL Server en Docker local
Producción: Azure SQL Database

📊 Base de datos
Modelo principal: Película
ID
Título
Descripción
Año
Género
Director
Duración
Calificación
Fecha de creación/actualización

Modelo de Usuario:
ID
Nombre de usuario
Email
Contraseña (hash)
Rol

🧪 Testing
Pruebas con Postman
Incluye colección para:
Endpoints CRUD
Filtros y paginación
Autenticación y registro
Validación de errores
bash:
# Archivo: ApiPeliculas.postman_collection.json

Endpoints principales
Código
GET    /api/peliculas
GET    /api/peliculas/{id}
POST   /api/peliculas
PUT    /api/peliculas/{id}
DELETE /api/peliculas/{id}
GET    /api/peliculas/filtrar

POST   /api/auth/login
POST   /api/auth/register
GET    /api/auth/roles

📚 Aprendizajes del proyecto
Primer contacto con Angular
APIs RESTful con .NET
Separación frontend/backend
Despliegue en la nube
Containerización con Docker
Autenticación dinámica y gestión de roles
Modularización de componentes y servicios

🤝 Contribución
Fork del proyecto
Crea una rama feature
Commit de tus cambios
Push a la rama
Abre un Pull Request

📄 Licencia
Este proyecto es de uso público. Puedes usarlo, modificarlo y compartirlo libremente.

🧑‍💻 Autor
Edwin Gibran Vargas González
📌 GitHub: @Edwin-Vargas94
💼 LinkedIn: Edwin Vargas
📧 Email: Edwin_glz94@hotmail.com
⭐ Si te gusta este proyecto, ¡dale una estrella en GitHub!
🔗 Enlaces útiles
Documentación de Angular
Documentación de .NET
Entity Framework Core
Docker Documentation
