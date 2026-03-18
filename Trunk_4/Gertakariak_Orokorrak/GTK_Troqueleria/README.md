# GTK Troquelería
## 1. Objetivo:
Gestionar las no conformidades relativas al negocio de troquelería

## 2. Descripción
Gestiona las no conformidades (NC) relativas al negocio de troquelería. Creación, edición, borrado, documentación adjunta, costes... Habilita unos servicios (ServiciosWeb.asmx) que interactuan con diferentes bases de datos, todas ellas contenidas en el esquema EF "BatzBBDD", y que se encargan también del envío de las diferentes notificaciones programadas.

### Tecnología:
-   .NET framework 4
-   BBDD: 
    -   Oracle (GTK, SAB, XBAT, LeccionesAprendidas, BonosSis) 

## 3. Pre-requisitos
### Software de terceros necesario
- Visual Studio 2017
- .NET 4
- ODT with ODAC 183
- Entity Framework 6.2.0
- jQuery 2.2.3 & 3.3.1, jQuery-ui 1.11.4

### Dependencias con otros proyectos internos
BatzBBDD (al igual que los demás proyectos basados en el paquete GTK)
CapturaGTK

## 4. Instalación / Configuración
### Instrucciones paso a paso para que la app funcione en local
  - Descarga/actualización de código:
    - https://aizbelaga.batz.es/svn/batz/Trunk_4/SBatz/Gertakariak_Orokorrak/GTK_Troqueleria
    - https://aizbelaga.batz.es/svn/batz/Trunk_4/SBatz/Gertakariak_Orokorrak/Gertakariak2.0
    - https://aizbelaga.batz.es/svn/batz/Trunk_4/SBatz/AccesoAutomaticoBD?
    - https://aizbelaga.batz.es/svn/batz/Trunk_4/SBatz/BatzBBDD
    - https://aizbelaga.batz.es/svn/batz/Trunk_4/SBatz/KTROL/KtrolLib?
    - https://aizbelaga.batz.es/svn/batz/Trunk_4/SBatz/Memcached
    - https://aizbelaga.batz.es/svn/batz/Trunk_4/SBatz/PrismaLib?
    - https://aizbelaga.batz.es/svn/batz/Trunk_4/SBatz/SABLib
    - https://aizbelaga.batz.es/svn/batz/Trunk_4/SBatz/SABLib2
    - https://aizbelaga.batz.es/svn/batz/Trunk_4/SBatz/Zaharrak/XBAT?
    - https://aizbelaga.batz.es/svn/batz/Trunk_4/SBatz/KaPlan/KaplanLib 4?
    - https://aizbelaga.batz.es/svn/batz/Trunk_4/SBatz/Localization
    - https://aizbelaga.batz.es/svn/batz/Codigo_Compartido/SBatz/Memcached
    - https://aizbelaga.batz.es/svn/batz/Codigo_Compartido/SBatz/localization
    - https://aizbelaga.batz.es/svn/batz/Codigo_Compartido/SBatz/SabLib
    - https://aizbelaga.batz.es/svn/batz/Codigo_Compartido/SBatz/AccesoAutomaticoBD?
      

### Conexiones a las bases de Datos
  - Conexiones Oracle
    - Usuario sab
    - Usuario incidencias
    - Usuario leccionesaprendidas

### Particularidades (web.config , parametros,...)

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
