Ficticia S.A. — ABM de Personas (.NET 8 Web API)

Autor: [Celeste Ghirardotti]
Fecha: [11/2025]
Rol simulado: Desarrollador/a .NET (Semi Senior)

Objetivo: Implementar una API REST sencilla para gestionar personas en el área de seguros de vida de Ficticia S.A., siguiendo buenas prácticas de arquitectura, validación, y organización del código.

Arquitectura del Proyecto
Proyecto estructurado en capas modulares (monolito limpio):
/src
 ├── Ficticia.API             → Capa de presentación (controllers, auth, swagger)
 ├── Ficticia.Application     → Capa de aplicación (servicios, DTOs, validaciones)
 ├── Ficticia.Domain          → Capa de dominio (entidades y reglas básicas)
 └── Ficticia.Infrastructure  → Capa de persistencia (EF Core, DbContext)
/tests
 └── Ficticia.Tests           → Tests unitarios (xUnit)


Tecnologías principales:
	.NET 8 (C#)
	Entity Framework Core (SQL Server / InMemory)
	Swagger (documentación)
	JWT simulado (roles)
	xUnit (testing)
	Logging con Microsoft.Extensions.Logging

Decisiones Técnicas
1️ Persistencia
	ORM: Entity Framework Core 8
	Base de datos: SQL Server LocalDB
	Se usa FicticiaDbContext con entidades:
	Person
	AttributeType
	PersonAttribute

2️ Atributos dinámicos
	Se implementó la estrategia de tabla pivote:
		Persons
		AttributeTypes
		PersonAttributes
-> Esto permite agregar nuevos atributos sin alterar el esquema de la tabla Persons.
Cada Person puede tener N atributos con nombre y valor configurables.

3️ Autenticación y roles
	JWT simulado (sin login real):
		Se envía el header:
		Authorization: Bearer admin → Rol Admin (CRUD completo)
		Authorization: Bearer consultor → Rol Consultor (solo lectura)

4️ Validaciones
	Data Annotations en las entidades (campos obligatorios, unicidad).
	Reglas de negocio (edad válida, identificación única) implementadas en PersonService.

5️ Logging
	Logging básico a consola mediante ILogger<T> en los controllers.

Ejecución del Proyecto
1️ Clonar el repo
	git clone https://github.com/tuusuario/ficticia-app.git
	cd ficticia-app

2️ Crear la base de datos
	dotnet ef database update --project src/Ficticia.Infrastructure --startup-project src/Ficticia.API

3️ Correr la API
	dotnet run --project src/Ficticia.API


La API se ejecutará en:

-> https://localhost:5022/swagger

Credenciales Demo (JWT simulado)
	Rol	Token en Swagger
		Admin	Bearer admin
		Consultor	Bearer consultor
Usá el botón “Authorize” en Swagger para probar los endpoints según el rol.

Endpoints principales
	Método	Endpoint	Rol	Descripción
	GET	/api/persons	Admin / Consultor	Listar y filtrar personas
	GET	/api/persons/{id}	Admin / Consultor	Obtener una persona
	POST	/api/persons	Admin	Crear persona
	PUT	/api/persons/{id}	Admin	Editar persona
	DELETE	/api/persons/{id}	Admin	Eliminar persona
	GET	/api/attributetypes	Todos	Listar tipos de atributos
	POST	/api/attributetypes	Admin	Crear atributo dinámico

Testing
	Ejecutar los tests unitarios
	dotnet test


Cobertura:
	Creación y actualización de personas
	Validación de identificación única
	Eliminación
	Filtrado por nombre

Migraciones EF incluidas
	Se genera automáticamente la base de datos mediante:
		dotnet ef migrations add InitialCreate
		dotnet ef database update

Notas finales
	Arquitectura preparada para escalar a Clean Architecture si se desea.
	Atributos dinámicos implementados de forma flexible y mantenible.
	Código 100% compilable y listo para demo en entrevista.
	Swagger incluye definición de seguridad con Bearer tokens simulados.