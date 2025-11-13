Ficticia S.A. — ABM de Personas (.NET 8 Web API + MVC Frontend)

Autor: Celeste Ghirardotti

Fecha: 11/2025

Rol simulado: Desarrollador/a .NET (Semi Senior)

Objetivo: Implementar una solución completa basada en .NET 8, que incluye:
	Una API REST para gestionar personas (alta, baja, modificación, listado y filtrado).
	Un frontend MVC (Ficticia.Web) con autenticación simulada, manejo de roles y sesiones, e interfaz adaptable al usuario logueado.

Arquitectura del Proyecto
Estructura modular:

/src

 ├── Ficticia.API             → Capa de presentación (Web API, controllers, auth, swagger)
 
 ├── Ficticia.Application     → Capa de aplicación (servicios, DTOs, validaciones)
 
 ├── Ficticia.Domain          → Capa de dominio (entidades, lógica base)
 
 ├── Ficticia.Infrastructure  → Capa de persistencia (EF Core, DbContext)
 
 └── Ficticia.Web             → Capa MVC (frontend web, vistas, autenticación, consumo de API)

/tests

 └── Ficticia.Tests           → Tests unitarios (xUnit)

Tecnologías principales
	.NET 8 (C#)
	Entity Framework Core (SQL Server / InMemory)
	ASP.NET Core MVC (frontend)
	Swagger / OpenAPI (documentación interactiva)
	JWT simulado (autenticación basada en roles)
	xUnit (testing)
	Session y Cookies (gestión del login)
	Bootstrap 5 (interfaz responsiva)
	Logging con ILogger<T>

Decisiones Técnicas
1️ Persistencia
	ORM: Entity Framework Core 8
	Base de datos: SQL Server LocalDB
	Entidades:
		Person
		AttributeType
		PersonAttribute
Se utiliza una tabla pivote (PersonAttributes) para permitir atributos dinámicos por persona.

2️ Autenticación y Autorización
Autenticación simulada por rol (sin base de datos de usuarios).
El acceso se define por el valor del token enviado en el header:

	Token enviado	Rol asignado	Permisos
	
	Bearer admin	Admin	CRUD completo
	
	Bearer consultor	Consultor	Solo lectura

Lógica aplicada tanto en la API como en el frontend MVC:
	Si el usuario inicia sesión con contraseña "admin", obtiene token "Bearer admin".
	Si inicia con "consultor", obtiene token "Bearer consultor".
Los controladores validan el token y aplican [Authorize(Policy = "AdminOnly")] o [Authorize(Policy = "ReadOnly")].

3️ Autenticación en el Frontend (MVC)
Se implementó un controlador de autenticación (AuthController) con las siguientes acciones:

Acción	Ruta	Descripción

GET /Auth/Login	Muestra formulario de login	

POST /Auth/Login	Verifica contraseña (admin o consultor) y guarda el token en Session	

GET /Auth/Logout	Limpia la sesión y redirige al inicio	

Filtrado de Personas (en MVC)
Se agregó un formulario de filtrado en el frontend MVC para listar personas según:
	Nombre
	Estado (Activo / Inactivo)
	Edad mínima / máxima
Los filtros se envían mediante query strings al endpoint /api/persons, aprovechando la lógica ya disponible en el backend.
El botón “Limpiar” reinicia los filtros.

Lógica:
Si la contraseña ingresada es "admin", se guarda en sesión:

	RoleToken = "admin"

Si es "consultor", se guarda:

	RoleToken = "consultor"

Estos valores se utilizan automáticamente por PersonService al agregar el header de autorización en cada request al API.

4️ Control de Acceso en la Web
El frontend MVC (Ficticia.Web) ahora valida el rol en cada vista:
En Home/Index:
	Muestra la lista de personas desde la API.
	Si el rol es admin, muestra botones para crear, editar y eliminar.
	Si es consultor o no logueado, solo puede ver la tabla.
En PersonsController:
	Antes de ejecutar acciones de escritura (Create, Edit, Delete), verifica el rol en sesión.
	Si el usuario no es admin, se redirige a Home con un mensaje:
	¡Solo los usuarios con rol Admin pueden realizar esta acción!

5️ UI Dinámica y Sesión
	Se agregó un Layout dinámico (_Layout.cshtml) que:
	Muestra el rol actual del usuario en la barra superior.
	Cambia entre los botones “Iniciar sesión” y “Cerrar sesión” según la sesión activa.
	Permite navegar fácilmente entre Home, Personas y Login.
	Se agregó un sistema de mensajes globales con TempData["Message"].

Ejecución del Proyecto
A Clonar el repo

	git clone https://github.com/tuusuario/ficticia-app.git
	cd ficticia-app

B Crear la base de datos

	dotnet ef database update --project src/Ficticia.Infrastructure --startup-project src/Ficticia.API

C Correr la API

	dotnet run --project src/Ficticia.API


API disponible en:

	https://localhost:5022

D Correr el Frontend MVC

	dotnet run --project src/Ficticia.Web

Aplicación Web disponible en:

	https://localhost:7090

Credenciales Demo

	Usuario (contraseña)	Rol asignado	Permisos
	
	admin	Admin	Crear, editar, eliminar y listar
	
	consultor	Consultor	Solo lectura (listar)
	
Endpoints Principales (API)

	Método	Endpoint	Rol	Descripción
	
	GET	/api/persons	Admin / Consultor	Listar personas
	
	GET	/api/persons/{id}	Admin / Consultor	Obtener persona
	
	POST	/api/persons	Admin	Crear persona
	
	PUT	/api/persons/{id}	Admin	Editar persona
	
	DELETE	/api/persons/{id}	Admin	Eliminar persona
	
Testing

Ejecutar los tests unitarios:

	dotnet test

Cobertura:
	Creación y actualización de personas
	Validación de identificación única
	Eliminación
	Filtrado
	Validación de acceso por rol

Notas finales:
	Arquitectura escalable hacia Clean Architecture.
	Autenticación simple pero efectiva para demos o entrevistas técnicas.
	Código 100% compilable y funcional.
	Frontend y API integrados vía HttpClient con headers dinámicos.
	UI amigable y clara, con feedback visual y roles simulados.