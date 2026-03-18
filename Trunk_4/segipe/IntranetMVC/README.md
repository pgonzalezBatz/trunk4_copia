# SEGIPE Intranet

## 1. Objetivo:
Ofrecer al departamento de compras una aplicación para ejercer el seguimiento de los pedidos.


## 2. Descripción
La aplicación ofrece a traves de estados, el seguimiento a los pedidos de proveedor.

Desde esta aplicación se envian los pedidos al proveedor.

A continuación, la herramienta proporciona el seguimiento del pedido a la espera de que el proveedor acepte o solicite cambios.

En caso de que el proveedor acepte el pedido, este pasa a estado aceptado y listo.

En caso de que el proveedor proponga un cambio, el pedido pasa al estado de "propuesto por el proveedor", y a traves de esta aplicación 
se ofrece la posibilidad de aceptar los cambios para que el pedido pase al estado de aceptado.

### Tecnología:
VB.NET, MVC

## 3. Pre-requisitos
### Usuario
Para poder acceder al programa, es necesario tener asignado el recurso correspondiente.

### Programador

Log4net, OracleManagedDataAccess

## 4. Instalación / Configuración
### Instrucciones paso a paso para que la app funcione en local
Hacer check out del projecto en cuestion (Trunk_4/SBatz/sas3) y bajar tambien la carpeta Codigo_Copartido.

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
Esquema SEGIPE de la base de datos de Oracle

## Notas

A pesar de tener su propio esquema de Oracle, la aplicación accede extensivamente el esquema XBAT.

## Autores

Asier Azkuenaga Batiz
