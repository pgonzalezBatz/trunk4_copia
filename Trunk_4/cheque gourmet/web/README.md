# Cheque Gourmet

## 1. Objetivo:
Gestionar el raparto de los cheque de comida Gourmet.

## 2. Descripción
Los trabajadores de Batz tienen acceso a tickets de comida canjeables en los restaurantes de Igorre y tambien en las comidas 
del comedor. 

Para ello los trabajadores tienen que pasar por recepción dondre podran solicitar como máximo 2 chequeas de tipos distintos por mes.

Entre los diferentes tipos de chequeras distinguimos la ayuda que regala Batz mensualmente y la que se resta al trabajador 
de la nómina. La aplicación es utilizada por el personal de repeción y ayuda a controlar toda esta distribución.

### Tecnología:
VB.NET, MVC

## 3. Pre-requisitos
### Usuario
Para poder acceder al programa, es necesario tener asignado el recurso correspondiente ademas de tener asignado el rol en el propio programa.

### Programador
itextsharp para generar PDFs.

Log4net, OracleManagedDataAccess

## 4. Instalación / Configuración
### Instrucciones paso a paso para que la app funcione en local
Hacer check out del projecto en cuestion (Trunk_4/SBatz/cheque gourmet) y bajar tambien la carpeta Codigo_Copartido.

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
Esquema gestionhoras de la base de datos de Oracle

## Autores

Asier Azkuenaga Batiz
