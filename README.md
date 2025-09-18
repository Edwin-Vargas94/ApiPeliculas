# 🎬 ApiPelículas

**ApiPelículas** es una aplicación web full-stack desarrollada con **C# .NET** para el backend y **Angular** para el frontend. Este proyecto fue creado como práctica para aprender Angular, implementando un gestor de películas completo con funcionalidades CRUD, filtrado y paginación.

## ✨ Características principales

- **CRUD completo** de películas (Crear, Leer, Actualizar, Eliminar)
- **Interfaz de inicio (Home)** con listado de películas
- **Sistema de filtrado** para búsqueda avanzada
- **Paginación** para mejor rendimiento y experiencia de usuario
- **API RESTful** robusta y escalable
- **Arquitectura separada** frontend/backend
- **Despliegue en la nube** con diferentes proveedores

## 🛠️ Tecnologías utilizadas

### Backend
- **C# .NET Core** - Framework principal
- **Entity Framework** - ORM para base de datos
- **SQL Server** - Base de datos
- **Docker** - Contenedorización del servidor SQL local

### Frontend
- **Angular** - Framework de frontend
- **TypeScript** - Lenguaje principal
- **HTML/CSS** - Estructura y estilos
- **Bootstrap/Material** (según implementación)

### Infraestructura
- **Azure SQL Database** - Base de datos en la nube
- **Render** - Despliegue de API
- **Railway** - Despliegue alternativo de API
- **Docker** - Contenedores locales

## ⚙️ Requisitos

### Para desarrollo local
- **.NET 6.0 o superior**
- **Node.js 16+ y npm**
- **Angular CLI**
- **Docker Desktop**
- **SQL Server** (local o Azure)

## 🚀 Instalación y configuración

### 1. Clonar el repositorio
```bash
git clone https://github.com/Edwin-Vargas94/ApiPeliculas.git
cd ApiPeliculas
```

### 2. Configurar el Backend (API)

#### Configurar base de datos
```bash
# Si usas Docker para SQL Server local
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Evargas12@" -p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server
```

#### Configurar cadena de conexión
Edita `appsettings.json` o `appsettings.Development.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=tcp:apipeliculas.database.windows.net,1433;Initial Catalog=peliculas;Persist Security Info=False;User ID=egvg94;Password=Evargas12;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
  }
}
```

#### Ejecutar migraciones
```bash
dotnet ef database update
```

#### Ejecutar la API
```bash
dotnet run
```

### 3. Configurar el Frontend (Angular)

#### Instalar dependencias
```bash
cd ClientApp  # o el nombre de tu carpeta frontend
npm install
```

#### Configurar URL de la API
Edita `src/environments/environment.ts`:
```typescript
export const environment = {
  production: false,
  apiUrl: 'https://apipeliculas-rtsd.onrender.com' 
};
```

#### Ejecutar la aplicación
```bash
ng serve
```

## 📁 Estructura del proyecto

```
ApiPeliculas/
├── Controllers/           # Controladores de la API
├── Data/                 # Contexto de Entity Framework
├── Models/               # Modelos de datos
├── Migrations/           # Migraciones de base de datos
├── Properties/           # Configuraciones del proyecto
├── Repositorio/          # Patrón Repository (si aplica)
├── PeliculasMappers/     # Mappers/DTOs
├── peliculas-app/        # Aplicación Angular
│   ├── src/
│   │   ├── app/
│   │   │   ├── components/
│   │   │   ├── services/
│   │   │   └── models/
│   └── ...
├── publish/              # Archivos de publicación
├── Dockerfile           # Configuración Docker
├── ApiPeliculas.csproj  # Configuración del proyecto
└── README.md
```

## 🎯 Funcionalidades principales

### 🏠 Página de Inicio
- Listado completo de películas
- Filtrado por título, género, año, etc.
- Paginación para mejor rendimiento
- Diseño responsive

### 📝 Gestión de Películas
- **Crear**: Agregar nuevas películas con información completa
- **Leer**: Visualizar detalles de películas
- **Actualizar**: Editar información existente
- **Eliminar**: Remover películas del sistema

### 🔍 Filtros y Búsqueda
- Búsqueda por título
- Filtrado por categorías
- Ordenamiento por diferentes criterios
- Filtros combinados

## 🌐 Despliegue

### API en Render
La API está desplegada en: `https://apipeliculas-rtsd.onrender.com`

### API en Railway  
Despliegue alternativo en: `https://tu-api-railway.com`

### Base de datos
- **Desarrollo**: SQL Server en Docker local
- **Producción**: Azure SQL Database

## 📊 Base de datos

### Modelo principal: Película
- ID (Primary Key)
- Título
- Descripción
- Año de lanzamiento
- Género
- Director
- Duración
- Calificación
- Fecha de creación/actualización

## 🧪 Testing

### Pruebas con Postman
El proyecto incluye una colección de Postman para testing de la API:

- **Endpoints CRUD** de películas
- **Pruebas de filtrado** y paginación
- **Validación de respuestas** HTTP
- **Testing de casos de error**

```bash
# Importar colección en Postman
# Archivo: ApiPeliculas.postman_collection.json (si está disponible)
```

### Endpoints principales para testing:
```
GET    /api/peliculas           # Obtener todas las películas
GET    /api/peliculas/{id}      # Obtener película por ID
POST   /api/peliculas           # Crear nueva película
PUT    /api/peliculas/{id}      # Actualizar película
DELETE /api/peliculas/{id}      # Eliminar película
GET    /api/peliculas/filtrar   # Filtrar películas
```

## 📚 Aprendizajes del proyecto

Este proyecto fue desarrollado como práctica para:
- **Primer contacto con Angular** y su ecosistema
- Implementación de **APIs RESTful** con .NET
- **Separación de responsabilidades** frontend/backend
- **Despliegue en la nube** con diferentes proveedores
- **Containerización** con Docker
- **Gestión de bases de datos** locales y en la nube

## 🤝 Contribución

Las contribuciones son bienvenidas. Para contribuir:

1. Fork el proyecto
2. Crea una rama feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abre un Pull Request

## 📄 Licencia

Este proyecto es de uso público. Puedes usarlo, modificarlo y compartirlo libremente.

## 🧑‍💻 Autor

**Edwin Gibran Vargas González**
- 📌 GitHub: [@Edwin-Vargas94](https://github.com/Edwin-Vargas94)
- 💼 LinkedIn: [Edwin Vargas](https://www.linkedin.com/in/edwin-vargas-993691129/)
- 📧 Email: Edwin_glz94@hotmail.com

---

⭐ Si te gusta este proyecto, ¡dale una estrella en GitHub!

## 🔗 Enlaces útiles

- [Documentación de Angular](https://angular.io/docs)
- [Documentación de .NET](https://docs.microsoft.com/dotnet/)
- [Entity Framework Core](https://docs.microsoft.com/ef/core/)
- [Docker Documentation](https://docs.docker.com/)