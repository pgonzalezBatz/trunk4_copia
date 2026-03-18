# Informes de Troquelería (Extranet)
## 1. Objetivo:
Que los proveedores rellenen o adjunten los informes de calidad y posiblemente otros tipos, con objeto de que sean facilmente accesibles cuando se soliciten.
Esta aplicación se basa en la aplicación de escritorio que desarrollo Pope, para que ambas aplicaciones puedan interoperar. Sin embargo, con el tiempo se han ido añadiendo nuevas tipologias
de informes que la aplicación de Pope no es capaz de "leer".

## 2. Descripción
Cuando a un proveedor se le asigna un pedido de subcontratación y el articulo sea de tipo tratamiento, el sistema le mostrara las marcas correspondientes al proveedor para que 
genere los informes. Estos informes tendran que ser validados por el personal de Batz, aplicación Informes de Troquelería (Intranet), para que el pedido pueda ser facturado.

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
El nombre de la solución es mantenimiento de troqueles por que así se definió en un inicio. El cambio de nombre a Informes de troquelería se solicito a posteriori.
