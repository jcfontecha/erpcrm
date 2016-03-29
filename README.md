# ERP y CRM para Leguizamo

##Instalar MySQL en Windows
https://dev.mysql.com/downloads/windows/

 - MySQL Installer
 - MySQL Connectors
 - MySQL for Visual Studio

## Montar un servidor de MySQL
Una vez teniendo esto, montar el SQL erpv2.sql en un servidor de MySQL. 

Yo utilicé MAMP. Deberías hacerlo también para evitar problemas.
https://www.mamp.info/en/

Hubo algo que también hice pero no sé si afecte. Edité el archivo C:\MAMP\conf\mysql\my.ini
de la siguiente manera:

<a href="http://es.tinypic.com?ref=dcpldv" target="_blank"><img src="http://i65.tinypic.com/dcpldv.png" border="0" alt="Image and video hosting by TinyPic" width="400"></a>

Agregando:
lower_case_table_names=2

Y ya habiendo hecho esa modificación, importas el SQL a PHPMyAdmin

## En Visual Studio

Ir al Server Explorer:

<a href="http://es.tinypic.com?ref=2yv0uvd" target="_blank"><img src="http://i64.tinypic.com/2yv0uvd.png" border="0" alt="Image and video hosting by TinyPic" width="400"></a>

Y agregar la base de datos de MySQL:

<a href="http://es.tinypic.com?ref=21bq7pc" target="_blank"><img src="http://i67.tinypic.com/21bq7pc.png" border="0" alt="Image and video hosting by TinyPic" width="400"></a>

 - Host: localhost
 - User: root
 - Password: root

Espero que con eso funcione.
