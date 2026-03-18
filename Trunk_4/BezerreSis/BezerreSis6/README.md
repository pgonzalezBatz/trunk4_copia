# BEZERRESIS - RECLAMACIONES DE CLIENTE
## 1. Objetivo:
Gestionar las reclamaciones de cliente

## 2. Descripción
Gestiona las reclamaciones de cliente, desde su creación hasta su cierre. A partir de esa información se obtendrán unos indicadores de calidad.

### Tecnología:
-   .NET framework 4.6.1
-   MVC
-   BBDD: 
    -   Oracle (Bezerresis, GTK, SAB) 
    -   IBM-AS400 (BRAIN)

## 3. Pre-requisitos
### Software de terceros necesario
- Visual Studio 2017
- .NET 4.6.1
- ODT with ODAC 183
- Entity Framework 6.2.0
- Cliente para IBM AS400 (IBM i Access)
- Bootstrap 3.3.6
- jQuery 2.2.3, jQuery-ui 1.11.4

### Dependencias con otros proyectos internos
Comparte maestros y está estrechamente ligado a GertakariakSA.

Las nuevas reclamaciones de cliente que no se asignen a una planta filial de Batz se crearán a su vez en GertakariakSA de la planta en curso (las asignadas a plantas filiales deberían estar ya previamente creadas en su 'GertakariakSA' correspondiente).

Importante: proyecto "MigracionVentas_exe" 
* Después de muchas pruebas de rendimiento (ver versiones v2 -> v4) se ha llegado a la conclusión de que tirar de la tabla VENTAS_CSV de Sql Server es muy costoso.
* Para aliviar el proceso, se ha decidido migrar a oracle la información relativa a 2018 en adelante (para calcular indicadores de 2019 en adelante, lo que necesita información de un año atrás).
* Esta información se ha guardado en la tabla VENTASNEW (esquema Bezerresis) --> ahora en VENTAS
* Esta información deberá ser actualizada a mes vencido el día 9 de cada mes, por la noche 
* Actualmente la información de la BD oracle está actualizada para enero de 2020 (siempre es a mes vencido, por lo que hay datos hasta diciembre de 2019)
* __Creado__ proceso mensual a día 9 por la noche, que ejecuta el proyecto MigracionVentas_exe 

## 4. Instalación / Configuración
### Instrucciones paso a paso para que la app funcione en local
  - Descarga/actualización de código:
    - https://aizbelaga.batz.es/svn/batz/Trunk_4/SBatz/BezerreSis
    - https://aizbelaga.batz.es/svn/batz/Trunk_4/SBatz/Memcached
    - https://aizbelaga.batz.es/svn/batz/Codigo_Compartido/SBatz/Memcached

### Conexiones a las bases de Datos
  - Conexiones Oracle
    - Usuario sab
    - Usuario incidencias
    - Usuario bezerresis
  - Conexión AS400
    - Usuario COGNOS

### Particularidades (web.config , parametros,...)
Privilegios de 'BEZERRESIS' en BD:

USER/SCHEMA | TABLE NAME | PRIVILEGES
-|-|-
INCIDENCIAS | CARACTERISTICAS | DELETE,INSERT,SELECT,UPDATE
INCIDENCIAS | ESTRUCTURA | SELECT
INCIDENCIAS | GERTAKARIAK | DELETE,INSERT,SELECT,UPDATE
INCIDENCIAS | G8D | DELETE,INSERT,SELECT,UPDATE
SAB | GRUPOS | INDEX,REFERENCES,SELECT
SAB | GRUPOSPLANTAS | INDEX,REFERENCES,SELECT
SAB | GRUPOSRECURSOS | INDEX,REFERENCES,SELECT
SAB | PLANTAS | INDEX,REFERENCES,SELECT
SAB | RECURSOS | INDEX,REFERENCES,SELECT
SAB | USUARIOS | REFERENCES,SELECT
SAB | USUARIOSGRUPOS | INDEX,REFERENCES,SELECT

A día ~~24/10/2019~~ 27/02/2020 la aplicación está en modo testing para Batz Igorre (y en producción una versión anterior). En caso de querer usarla para otras plantas habrá que definir los maestros correspondientes para dichas plantas

#### Configurar entornos (TEST, PRE, PRO)
Se configuran automáticamente con las transformaciones del Web.config (paquete NuGet SlowCheetah)

## 5. Puesta en Marcha / Uso
...

## 6. Autores
Daniel Iglesias Elejabarrieta

## 7. Otras Notas
...

## 8. Documentación de usuario
...

***

