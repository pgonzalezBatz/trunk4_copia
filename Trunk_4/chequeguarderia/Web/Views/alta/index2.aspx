<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="Web" %>
<%@ Import Namespace="System.Linq" %>
<asp:Content ContentPlaceHolderID="cphHead" runat="server">
    <title>
        <%= h.Traducir("Cheques guardería")%>
    </title>
    <style type="text/css">
        .cabecera
        {
            background-color: #AABBFF;
            border: 1px solid #000;
        }
        .celda
        {
            background-color: #EEEEFF;
            border: 1px solid #000;
            text-align: center;
        }
        .mine-button
        {
            display: block;
            width: 16em;
            padding: 0.2em;
            line-height: 1.4;
            background-color: #F4F4F4;
            border: 1px solid black;
            color: #000;
            text-decoration: none;
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ContentPlaceHolderID="cphContenido2" runat="server">
    <font size="3" color="#0023FF">OPTIMIZATU ZURE SOLDATA </font>
    <br />
    <font size="2" face="Verdana">Zure seme-alabaren haurtzaindegiko bi hileko kuota aurreztu
        nahi duzu? BATZ S.KOOPek Educainfantil Virtual produktuaren bidez Haurtzaindegirako
        txekeak sortzen dituen Cheque Dejeuner España enpresarekin sinatutako akordioari
        esker. </font>
    <br />
    <br />
    <font size="3" color="#0023FF">Zer dira Educainfantil Virtual txekeak? </font>
    <br />
    <font size="2" face="Verdana">Ordaintzeko baliabide bat dira. Oso trataera fiscal onuragarria
        dute, zeren ez dira gauzen bidezko ordainketatzat hartzen, eta urteko 1.000 € arte,
        ez da PFEZ ordaintzen.<br />
        Txeke birtualaren zenbatekoa Cheque Dejeunerek kudeatuko du, hilabetearen azkeneko
        lan-egunean haurtzaindegiari tranferentzia eginez.
        <br />
        Haurtzaindegiak Cheque Dejeuner eta erabiltzaileari dagokion hileko kuotaren arteko
        diferentzia kobratuko dute. Hileko kopurua ezin izango da 160€koa baino handiagoa 
        (EZ DA ESKATUKO Cheque Educainfantil Virtual umea zentrora joaten den LEHENENGO HILABETEAN, 
        lehengo ordainketa aldez aurretik egin delako, umeak Haurreskolan izena ematean). 
        
         </font>
    <br />
    <br />
    <font size="3" color="#0023FF">Nork erabil ditzake? </font>
    <br />
    <font size="2" face="Verdana">Red Virtual Edukainfantilean afiliatuta dauden Haur-heziketa
        Zentruetan matrikulatuta dauden, edo matrikulatu nahi duten, 0-3 urte bitarteko
        seme-alabak dituzten Batzeko langileek. </font>
    <br />
    <br />
    <font size="3" color="#0023FF">ONURA FISKALAK: </font>
    <br />
    <font size="2" face="Verdana">1. Haurtzaindegirako txeke birtualaren zenbatekoaren PFEZ
        portzentaia aurrezten dugu.<br />
        2. Nominari aplikatzen zaion PFEZren portzentaia murriztu daiteke haurtzaindegirako
        txeke birtuala kendutakoan.
        </font>
    <br />
    <br />
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td>
            </td>
            <td class="cabecera">
                NOMINAN
            </td>
            <td class="cabecera">
                EDUCAINFANTIL VIRLTUAL TXEKEETAN
            </td>
        </tr>
        <tr>
            <td class="cabecera">
                HAURTZAINDEGIKO URTEKO KOSTUA
            </td>
            <td class="celda">
                1000 €
            </td>
            <td class="celda">
                1000 €
            </td>
        </tr>
        <tr>
            <td class="cabecera">
                PFEZ<br />
                Bataz besteko %17ko PFEZren gaineko kalkulatuta
            </td>
            <td class="celda">
                170 €
            </td>
            <td class="celda">
                EXENTO
            </td>
        </tr>
    </table>
    <br />
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td class="cabecera">
                EDUCAINFANTILEK LANGILEARI URTEAN SUPOSATZEN DION AURREZKIA**
            </td>
            <td class="celda" width="200">
                170 €
            </td>
        </tr>
    </table>
    <br />
    <font size="2" face="Verdana"><i>Zerbitzu honetatik deribatzen diren gastuak langilearen kargu izango dira (%1.5)</i></font><br />
    <font size="2" face="Verdana"><i>**Gainera, nominan aplikatzen den PFEZren ehunekoa
        murriztu egin daiteke Guarderia Virtual txekea kentzean. </i></font>
    <br />
    <a href="<%=url.action("formularioalta") %>" class="mine-button">
        <%= h.Traducir("Quiero darme de alta")%></a>
</asp:Content>
