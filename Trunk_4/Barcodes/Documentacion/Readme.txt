Pistola instalada en la línea MQBA0.

Terminal: \\sisdlemqba0
Ruta carpeta principal: \\sisdlemqba0\c$\BarcodeMQBA0
Carpetas:
	Documentación
	NEW *APLICACIÓN EN USO*
	OLD backups

Configuración:
El archivo "barcode labels.pdf" es el configurador de la pistola para la 'two-way communication'.
He tenido problemas configurándola conectada al terminal del taller, pero configurándola allí mismo en mi portátil, ha funcionado.
Una vez configurada la pistola, la nueva versión de la aplicación sustituirá a la de la carpeta \NEW\, siempre guardando la versión anterior en \OLD\. 

Errores:
Los errores de desconexión suelen ser o bien por parada del servicio o bien por la reconexión de la pistola en un usb erróneo.

Backup:
Se guardará la última versión operativa de la carpeta de la aplicación en la carpeta ../backup del repositorio, y en la carpeta "OLD" de la máquina del taller.


¡¡ ATENCIÓN !!

El archivo "../backup/REFS_MQBA0.txt" debe actualizarse (en repositorio y en \\sisdlemqba0\c$\BarcodeMQBA0\NEW) siempre que cambien las referencias, ya que contiene algunas referencias que no se encuentran en la BD.