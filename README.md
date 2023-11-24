# Reto Técnico Sophos

Este es un proyecto de reto técnico desarrollado en tecnología .NET Core utilizando Visual Studio. La aplicación implementa un backend diseñado para gestionar datos básicos de una institución educativa.

## Herramientas Utilizadas

- Visual Studio
- Postman
- Docker
- Swagger

## Requisitos para la Ejecución

- .NET (Versión mínima 6.0)

## Pasos de Ejecución

1. En la ruta del proyecto .NET, ejecutar los siguientes comandos:
   ```bash
   dotnet restore
   dotnet build
   dotnet run
   ```

## Configuración de la Base de Datos

La base de datos se encuentra actualmente en Azure SQL Database, por lo que se requiere permisos de IP. Si desea probarla localmente, utilice el SCRIPT de creación y población de la base de datos y actualice las conexiones en la aplicación.

## Peticiones y Autenticación

Las peticiones se escuchan en el puerto 7119. Inicie autenticándose utilizando el endpoint de Login con la siguiente URL:
[https://localhost:7119/api/Login/Autorizar](https://localhost:7119/api/Login/Autorizar)

## Características Implementadas

- Listar los cursos ofrecidos, mostrando su nombre, nombre del curso prerrequisito, número de créditos y cupos disponibles.
- Listar los alumnos matriculados, mostrando nombre, facultad a la que pertenece y más información.
- Listar los profesores, mostrando nombre, máximo título académico, años de experiencia en docencia y nombre del curso o de los cursos que dicta.
- Agregar nuevos cursos, alumnos y/o profesores.
- Actualizar información de cursos, alumnos y/o profesores.
- Eliminar cursos, alumnos y/o profesores.
- Buscar curso, alumno y/o docente por nombre.
- Buscar curso por estado de cupos (si tiene o no cupos disponibles).
- Buscar alumnos por facultad a la que pertenece.
- Seleccionar un curso y mostrar la información de este (nombre, número de estudiantes inscritos, profesor que dicta el curso, cantidad de créditos, entre otros), además de un listado de los alumnos que están cursando dicha asignatura.
- Seleccionar un alumno y mostrar la información de este (nombre, número de créditos inscritos, semestre que cursa, entre otros datos relevantes), además de un listado de los cursos que tiene matriculados y un listado de las asignaturas que ya cursó.
