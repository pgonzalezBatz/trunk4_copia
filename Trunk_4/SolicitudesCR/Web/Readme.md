# SOLICITUDESCR
## 1. Objetivo:
Posibilitar la entrada segura de solicitudes al Consejo Rector. Similar al programa de código Ético, con algunas peculiaridades. (Está solo en test, no en producción) 
## 2. Descripción.
Se trata de habilitar los formularios precisos para que los usuarios, una vez identificados, puedan agregar comentarios y peticiones de forma segura. En el caso de SolicitudesCR se ha habilitado la posibilidad de anexar ficheros como complemento a la solicitud.

Esos comentarios se categorizan por los distintos ámbitos que se han predefinido en la BBDD.
Tambien se posibilatará el acceso a los distintos documentos donde se define este código ético y se les mostrará según el idioma configurado por el usuario en la intranet.
El repositorio de estos ficheros es una carpeta compartida de la web llamada "Ficheros" a la que accederemos desde el mismo servidor web.
La aplicación utiliza roles para definir quien entra en unas opciones y quien no. 
Se habilitará un administrador por planta (admin cuando rol001 = 2, el campo rol099 es el código de planta, rol000 es el cod usuario) y serán quienes se encargarán de revisar cada entrada y realizar la acción que corresponda y desviar la entrada a la persona adecuada.
Las acciones se comunicarán al remitente por correo para mantenerle informado y serán:
- En Trámite (Aún no se ha realizado la acción solicitada)
- Aceptada (Ya se ha realizado lo sugerido y se le comunica al usuario que la abrió)
- Denegada (La entrada no es procedente)

Tambien se añade un combo para seleccionar el responsable que debe acometer la acción (RRHH o Presidencia)
La visualización de los registros es solo de aquellos que están pendientes o que han sido reenviados a los departamentos correspondientes. No lo rechazados o cerrado. Para verlos debemos pulsar el check de "Todos".

Se debe seleccionar tambien el tipo de solicitud de la que se trata (Excedencias, bajas, Reducción de jornada...)

### Tecnología:
Está desarrollado en .NET asp con el framwork 4. Utiliza tanto el AjaxcontrolToolkit para validación de formularios y demás utilidades y además el log4net para escribir el log de actividades.
La BBDD es Oracle con Oracle.ManagedDataAccess. SolicitudesCR es la librería de funciones de Oracle utilizadas.


## 3. Pre-requisitos

### Software de terceros necesario


### Dependencias con otros proyectos internos

Los proyectos que utilizan son LocalizationLib 4, Memcached y SABLib2


## 4. Instalación / Configuración
### Instrucciones paso a paso para que la app funcione en local

Descargar del repositorio la aplicación
https://aizbelaga.batz.es/svn/Batz/Trunk_4/SBatz/Cetico

Además de LocalizationLib 4, Memcached y SABLib2

Instalar el cliente de oracle 12.1.024



### Conexiones a las bases de Datos

El acceso a las distintas BBDD utilizadas se encuentra en el web.config.
La BBDD está en Oracle (en test, SolicitudesCR-Test), y está sincronizada con la BBDD SAB debido a que la identificación de usuario se realiza con el mismo código que el acceso a laq intranet.
 


### Particularidades (web.config , parametros,...)

Todos los documentos subidos, se almacenan en un sistema de ficheros en el website dentro de la carpeta:
"solicitudesCr/Ficheros""

#### Configurar entornos (TEST, PRE, PRO)

La configuración para la dirección email a los que notificar las entradas se configura en el web.config. Tambien el usuario con el que administrar la aplicación. 
Ejemplo

    <add key="RecursoSolicitudesCR" value="429"/>
    <add key="administrar" value="63690"/>
    <add key="emailpresidencia" value="anderbilbao@batz.es"/>
    <add key="emailcomite" value="jgalan@batz.es"/>




## 5. Puesta en Marcha / Uso


## 6. Autores

- Jon Zarraga

## 7. Otras Notas


## 8. Documentación de usuario

***
***