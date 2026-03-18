# Informes de Troquelería (Intranet)
## 1. Objetivo:
Que los usuarios a los que les atañe, visualicen y hagan seguimiento de los informes de calidad y posiblemente otros tipos, con objeto de que sean facilmente accesibles cuando se soliciten. Se pueden visualizar tanto informes creados por los proveedores, los cuales habra que validar para que puedan facturar, como los creados internamente.

Esta aplicación se basa en la aplicación de escritorio que desarrollo Pope, para que ambas aplicaciones puedan interoperar. Sin embargo, con el tiempo se han ido añadiendo nuevas tipologias de informes que la aplicación de Pope no es capaz de "leer".

## 2. Descripción
Los usuarios tienen la opción de buscar informes de OF-Operaciónes o de seleccionar un proveedor y ver sus informes. Los informes generados por los proveedores, tendran que ser validados en esta aplicación para que puedan continuar su curso.

### Tecnología:
Web, VB.NET, MVC, Oracle
## 3. Pre-requisitos
### Dependencias con otros proyectos internos
Informes de troquelería (Extranet) e Informes de troquelería (Intranet) son dos proyectos interdependientes. 
Tienen El archivo de acceso a base de datos compartido (Es un link en el proyecto de intranet). 

## 4. Instalación / Configuración
### Instrucciones paso a paso para que la app funcione en local
En una máquina Windows con .net 4.5 instalado debería de funcionar simplemente bajando la solucion "mantenimiento_troqueles" que contiene los dos projectos (Extranet e Intranet).
### Conexiones a las bases de Datos
Base de datos Oracle schema: soldaduda

## 6. Autores
Asier Azkuenaga Batiz
## 7. Otras Notas
