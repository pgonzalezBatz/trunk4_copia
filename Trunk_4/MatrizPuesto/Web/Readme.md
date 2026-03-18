# MATRIZPUESTO
## 1. Objetivo:
Control de la documentación subida por las empresas y trabajadores externos en Batz 
## 2. Descripción
Se trata de evaluar la documentación necesaria que las trabajadores de cada uno de los puestos requieren y como es el estado en el que se encuentra cada trabajador. Si tiene el documento requerido o no.
Por cada uno de los puestos, podremos ver los trabajadores que pertenecen. Por cada trabajador los documentos requeridos y el estado de los mismos. Si son correctos o no correctos, entregados o no, validados o no.
Desde la aplicación se podrán subir estos documentos por trabajador, ver los documentos subidos, ver un histórico de esos documentos subidos y por último validarlos o rechazarlos.
La aplicación utiliza roles para el acceso a las distintas funcionalidades.
Existe un módulo de administración, donde configuraremos qué documentos son los requeridos por trabajador y por puesto.
Por cada Trabajador/Puesto mostraremos los documentos correctos, los icorrectos, sus fechas de caducidad y se permitirá ir subiendo estos documentos para estar al día.
Se permitirá configurar que documentos se requieren, y se podrá ir validando uno por uno esos documentos para marcarlos como correctos o incorrectos. Tambien podremos marcar algunos como "NO CADUCA" (Válidos por vida)




### Tecnología:
Está desarrollado en .NET asp con el framwork 4. JUtiliza tanto el AjaxcontrolToolkit para validación de formularios y demás utilidades y además el log4net para escribir el log de actividades.
La BBDD es Oracle con Oracle.ManagedDataAccess. AdokLib es la librería de funciones de Oracle utilizadas


## 3. Pre-requisitos

### Software de terceros necesario


### Dependencias con otros proyectos internos

Los proyectos que utilizan son LocalizationLib 4, Memcached y SABLib2



## 4. Instalación / Configuración
### Instrucciones paso a paso para que la app funcione en local

Descargar del repositorio la aplicación
https://aizbelaga.batz.es/svn/Batz/Trunk_4/SBatz/MatrizPuesto

Además de LocalizationLib 4, Memcached y SABLib2

Instalar el cliente de oracle 12.1.024


### Conexiones a las bases de Datos

El acceso a las distintas BBDD utilizadas se encuentra en el web.config.
La BBDD está en Oracle (MatrizPuesto), y está sincronizada con la BBDD SAB en el sentido de que la identificación del usuario se realiza accediendo a estas tablas de la misma forma que el acceso a la intranet.

- El acceso a los puestos a mostrar se realiza mediante una llamada a un webservice ("https://intranet-test.batz.es/pks/json/GetTrabajadoresDependientes?DNI=")
- Lo mismo para el acceso a la información de las personas por puesto y planta. De la forma siguiente: ( https://intranet-test.batz.es/pks/json/GetPersonaPuesto?idplanta=)


### Particularidades (web.config , parametros,...)


#### Configurar entornos (TEST, PRE, PRO)






## 5. Puesta en Marcha / Uso

El menú principal se define en APP_DATA/menu.xml y está diferenciado por el rol definido a cada usuario en la tabla roles.
En la pantalla principal se saca un listado con aquellas puestos que recivimos del webservice y se ofrece la posibilidad de obtener los trabajadores por cada puesto o los documentos por cada uno de esos puestos como consulta. (Para la asignación o desasignación existe una opción de menú)
Por lo tanto podremos asignar y desasignar documentos a puestos y tambien podremos asignar o desasignar documentos a trabajadores.

En caso de que haya algún error en algun documento, se muestra en rojo. Para ir al detalle en ambos casos pulsaremos en su correspondiente botón y podremos acceder al estado de los documentos de los trabajadores.
Dentro del menú encontramos las opciones correspondientes a la asignación de qué documentos son obligatorios para cada puesto y cada trabajador. 
Estos serán los que se chequeen posteriormente y decidirán si su estado es correcto o con errores.

La parte fundamental se encuentra bajo la opción de Listados. 
Para facilitar la validación de los documentos se ha facilitado un par de vistas "Validación Todos Doc Trabajadores. 
En estas vistas se ordenan por fecha de recepción descendente todos los documentos aún no validados, así podremos ver de golpe todos los documentos que requieren atención. Se separan los documentos de empresa por un lado, y los de trabajador por otro.

Tambien se ha añadiso una vista donde se muestra una matriz para ver qué documentos de formación tiene cada trabajador y cual es su estado. 
Se mostrarán aquellos documentos que, por su puesto, estén asignados a ese trabajador (Matriz Trabajadores Formación). 
Se mostrarán en blanco cuando no se ha subido la documentación de ese determinado curso, y se mostrará el icono correspondiente al estado del curso cuando sí se haya subido la documentación del curso en cuestión (verdo correcto, rojo incorrecto).
También se podrán ver los documentos subidos pulsando en su icono. Si queremos filtrar la matriz por los trabajadores de una empresa concreta lo filtraremos en el textbox supoerior.



## 6. Autores

- Jon Zarraga

## 7. Otras Notas


## 8. Documentación de usuario

***
***
