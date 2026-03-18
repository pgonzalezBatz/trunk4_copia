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
            background-color:#AABBFF;
            border: 1px solid #000;
        }
        .celda
        {
            background-color:#EEEEFF;
            border: 1px solid #000;
            text-align: center;
        }
        

    </style>
</asp:Content>
<asp:Content ContentPlaceHolderID="cphContenido2" runat="server">
    <font size="3" color="#0023FF">¡OPTIMIZA TU SALARIO! </font>
    <br />
    <font size="2" face="Verdana">¿Quieres ahorrarte hasta 2 mensualidades de la guardería
        de tu hijo? Gracias al acuerdo que BATZ S.COOP. ha firmado con la empresa Chèque
        Dejeuner España,que emite los cheques guardería a través de su producto Educainfantil
        Virtual. </font>
    <br />
    <br />
    <font size="3" color="#0023FF">¿QUÉ SON LOS CHEQUES EDUCAINFANTIL VIRTUAL? </font>
    <br />
    <font size="2" face="Verdana">Son un medio de pago, con un tratamiento fiscal muy beneficioso,
        ya que no son considerados retribución en especie y hasta 1000€/año están exentos
        de tributación por IRPF.<br />
        <br />
        El importe del cheque virtual será tramitado por Chèque Déjeuner realizando la transferencia
        bancaria a las guarderías , el último día hábil de cada mes.<br />
        <br />
        Las guarderías cobrarán la diferencia entre la cantidad abonada por Chèque Déjeuner
        y la cuota mensual que le corresponda al usuario, cuya cuantía  mensual  no  podrá  ser  superior  a 160€ 
        (NO SE SOLICITARÁ, el Cheque Educainfantil Virtual, EL PRIMER MES que acuda el niño/niña al centro, 
        dado que el primer pago ya se ha efectuado de manera anticipada, al inscribir al niño en la Haurreskola).
        
        </font>
    <br />
    <br />
    <font size="3" color="#0023FF">¿QUIÉN PUEDE USARLOS? </font>
    <br />
    <font size="2" face="Verdana">Personal de BATZ S.COOP. con hijos entre 0-3 años que
        estén matriculados o que se quieran matricular en Centros de Educación Infantil
        afiliados a la Red Virtual Educainfantil. </font>
    <br />
    <br />
    <font size="3" color="#0023FF">VENTAJAS FISCALES: </font>
    <br />
    <font size="2" face="Verdana">1 - Ahorramos el % de IRPF de la cantidad destinada al
        cheque guardería virtual.<br />
        2 - Adicionalmente el % de IRPF aplicado en nómina podría verse reducido al descontar
        el cheque guardería virtual.
    </font>
    <br />
    <br />
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td>
            </td>
            <td class="cabecera">
                EN NOMINA
            </td>
            <td class="cabecera">
                EN CHEQUES EDUCAINFANTIL
            </td>
        </tr>
        <tr>
            <td class="cabecera">
                IMPORTE ANUAL GUARDERÍA
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
                TRIBUTACIÓN IRPF<br />
                * Calculado sobre un tipo medio del 17% de IRPF
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
                AHORRO ANUAL CON EDUCAINFANTIL PARA EL EMPLEADO**
            </td>
            <td class="celda" width="200">
                170 €
            </td>
        </tr>
    </table>
    <br />
    <font size="2" face="Verdana"><i>El trabajador se hace cargo de los costes de gestión derivados de este servicio (1.5%)</i></font><br />
    <font size="2" face="Verdana"><i>**Adicionalmente el % de IRPF aplicado en nómina podría
        verse reducido al descontar el cheque guardería virtual. </i></font>
        <br />
        <a href="<%=url.action("formularioalta") %>" class="mine-button">
            <%= h.Traducir("Quiero darme de alta")%></a>
</asp:Content>
