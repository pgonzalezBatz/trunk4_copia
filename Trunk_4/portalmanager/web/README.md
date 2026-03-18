# Portal de Manager
## 1. Objetivo:
Ofrecer un portal a los mandos para que puedan gestionar a sus colaboradores.
## 2. Descripción
La aplicación tiene 3 partes. 
En la primera, ofrece una serie de enlaces para acceder a listados de Cognos.
A continuación tenemos la opción de hacer solicitudes de contratación.
Por último, la evaluación de personal.
Dependiendo del rol, sobre todo para RRHH, tenemos un 4º apartado (EKI) que ofrece varios tipos de listado y busquedas para hacer seguimiento de plantilla.
### Tecnología:
VB.NET, MVC
## 3. Pre-requisitos
### Dependencias con otros proyectos internos
Los enlaces mencionados, dependen de listados de Cognos.
## 4. Instalación / Configuración
### Instrucciones paso a paso para que la app funcione en local
Hacer check out del projecto en cuestion (Trunk_4/SBatz/portalmanager) y bajar tambien la carpeta Codigo_Copartido.

La primera ejecución en local puede que no funcione debido a como se configura el servidor web local que ejecuta visual estudio. 
Si el error indica que la sección de la Web.config esta bloqueada, tenemos que acceder al archivo aplicationhost.config dentro de la 
carpeta .vs/config/ que se encuentra en la solución. A continuación tenemos que cambiar estas lineas:
```
<section name="anonymousAuthentication" overrideModeDefault="Deny" />
<section name="windowsAuthentication" overrideModeDefault="Deny" />

<add name="AnonymousAuthenticationModule" lockItem="true" />
<add name="WindowsAuthenticationModule" lockItem="true" />
```
Con estas otras:
```
<section name="anonymousAuthentication" overrideModeDefault="Allow" />
<section name="windowsAuthentication" overrideModeDefault="Allow" />

<add name="AnonymousAuthenticationModule" lockItem="false" />
<add name="WindowsAuthenticationModule" lockItem="false" />
```
### Conexiones a las bases de Datos
Esquema EKI de Oracle y Epsilon de Microsoft SQL Server.
### Particularidades (web.config , parametros,...)
Varios roles mas alla de los usuarios básicos se controlan desde Web.config.
El mapeo de los formularios a los parametros de Epsilon tambien se encuentran aquí.
## 5. Puesta en Marcha / Uso
El acceso básico de usuario se controla con los recursos de SAB.
Tenemos accesos a roles especificos que se controlan desde el Web.config
## 6. Autores
Asier Azkuenaga Batiz
