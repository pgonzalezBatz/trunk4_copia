# Cost carriers
## 1. Objetivo:
Gestionar todo el proceso de vida de un portador de coste permitiendo la creación de nuevos proyectos, plantillas
y todo el flujo de validación hasta su apertura en XPERT.

## 2. Descripción
Permite crear plantillas para la creación de portadores de coste. Basándonos en dichas plantillas podemos
seleccionar un proyecto y se nos generará toda la estrucutura del proyecto partiendo de
las plantas, estados, grupos de coste y finalmente pasos.

Dichos pasos puede obtener datos automaticamente de otros sistemas o ser introducidos manualmente.

### Tecnología:
La tecnología empleada es MVC sobre framework .NET 4.6 contra base de datos Oracle y AS400
## 3. Pre-requisitos
### Software de terceros necesario
- Visual Studio 2017
- .NET framework 4.6
- Cliente de AS400 para el acceso a XPERT
### Dependencias con otros proyectos internos
- Oferta técnica para la obtención de datos (a través de servicios web)
- Bonos de sistemas (a través de servicios web)
## 4. Instalación / Configuración
### Instrucciones paso a paso para que la app funcione en local
- Descargar el código fuente de la aplicación de la ruta https://aizbelaga.batz.es/svn/batz/Trunk_4/SBatz/CostCarriers
- Descargar el código fuente de Memcached de la ruta https://aizbelaga.batz.es/svn/batz/Trunk_4/SBatz/Memcached
- Descargar el código fuente de SABLib2 de la ruta https://aizbelaga.batz.es/svn/batz/Trunk_4/SBatz/SABLib
### Conexiones a las bases de Datos
- Conexión al esquema SAB en Oracle
- Conexión al esquema Costcarriers en Oracle
- Conexión a XPERT en AS400
### Particularidades (web.config , parametros,...)
Hay dos rutas para el acceso a los servicios web
- CostCarriersLib_ServicioBonos_ServicioBonos: da acceso a los métodos del servicio de bonos en la ruta https://intranet2.batz.es/BonosSis/Publico/ServicioBonos.asmx
- CostCarriersLib_ServicioOfertaTecnica_ServiceCostCarriers: da acceso a los métodos del servicio de oferta técnica en la ruta https://intranet2.batz.es/OfertaTecsis/Publico/ServiceCostCarriers.asmx

Hay una entrada en los appsettings "UsuariosCambio" en el que podemos indicar mediante "dominio\usuario" los usuarios a los cuales se les va a dar privilegios
para poder suplantar la identidad de otro en la aplicación mediante la ruta https://intranet2.batz.es/costcarriers/login/cambiarusuario
#### Configurar entornos (TEST, PRE, PRO)

## 5. Puesta en Marcha / Uso

## 6. Autores
Iker Llanos Zubizarreta (EXTernal - Aubay)

***
***

# EJEMPLOS - QUITAR ESTA PARTE DEL FICHERO FINAL

***

## -CABECERAS

# Esto es un tag h1
## Esto es un tag h2
###### Esto es un tag h6

## -ÉNFASIS

*Texto en cursiva*

_Texto también en cursiva_

**Texto en negrita**

__Texto también en negrita__

~~Esta palabra está tachada~~ 

_**Puedes** combinarlos_

## -LISTAS

### Con orden
1. Item 1
1. Item 2
1. Item 3
   1. Item 3a
   1. Item 3b

### Sin orden
* Item 1
* Item 2
  * Item 2a
  * Item 2b
  
## -IMAGENES
![Logo de Batz Group](https://intranet2.batz.es/Baliabideorokorrak/logo_batz_group.png "Tooltip")

## -LINKS
Link con nombre: [Intranet Batz](https://intranet2.batz.es "Aqui se pondria el tooltip que se quiera")

Link directo: https://intranet2.batz.es

Email link: <helpdesk@batz.es>

## -BLOQUES DE CÓDIGO

Bloque de código
```
function fancyAlert(arg) {
  if(arg) {
    $.facebox({div:'#foo'})
  }
}
```

```
<asp:Label runat="server" text="Esto es un label" />
```

## -ESCAPE DE CARACTERES
Poner el caracter \

\* Sin la contrabarra, esta palabra sería un elemento de una lista no ordenada

## -TABLAS
First Header | Second Header
------------ | -------------
Content from cell 1 | Content from cell 2
Content in the first column | Content in the second column

## -CITAS
As Kanye West said:

> We're living the future so
> the present is our past.
>> Second quote