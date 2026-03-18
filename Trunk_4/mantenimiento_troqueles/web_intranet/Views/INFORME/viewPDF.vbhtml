@Code
    Layout = Nothing
End Code

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>@h.traducir("Impresion de informe")</title>
    <style type="text/css">
        table{width:100%;}
        .title1{background-color:blue; color:#FFF;padding:4px; width:42%;}
        #table2{font-size:12px;margin-top:50px; margin-bottom:20px; border-collapse:collapse;width:95%;}
        #table2 td{background-color:#add7f7;border-bottom:2px solid blue;padding:5px;}
        #table3{font-size:12px;margin-bottom:40px; width:95%;border-collapse:collapse;}
        #table3 th{background-color:blue; color:#FFF;padding:5px;}
        #table3 td{padding:5px;}
        .tsoldadura td{border:1px solid black;}
        .greeblue{background-color:#94ffff;}
    </style>
</head>
@code
    Dim m As INFORMES = Model.informe
    Dim lstA As IEnumerable(Of ABARCA) = Model.imagenes
End Code
<body>
    <table>
        <tr>
            <td width="15%;" style="vertical-align:top;">
                <img src="@(System.Web.HttpContext.Current.Server.MapPath("~/") + "/Content/logo_20px.png")"  />
            </td>
            <td class="title1" >
  @If m.TIPOINFORME.ToLower.Contains("temple") Then
                    @Html.Encode("TRATAMENTU TERMIKOEN KONTROLA")
                    @<br />
                    @Html.Encode("HEAT TREATMENT INSPECTION")
      @<br />
      @Html.Encode("CONTROL DE TRATAMIENTOS TERMICOS")
  ElseIf m.TIPOINFORME.ToLower.Contains("soldadura") Then
      @Html.Encode("SOLDADUREN KONTROLA")
                    @<br />
                    @Html.Encode("WELDING INSPECTION")
      @<br />
      @Html.Encode("CONTROL DE SOLDADURA")
  End If
            </td>

            <td width="30%;">
                <h4    style="margin-bottom:0;">OF: @m.VALOROF OP:@m.VALOROP</h4>
                <h4 style="margin-bottom:5px; margin-top:2px;">Marcas:</h4>
                <div> @m.MARCA.Replace("|", " ")</div>
            </td>
        </tr>
    </table>
    <table id="table2">
        <tr>
            <td>Cliente/Customer:</td>
            <td><strong>@m.CLIENTE</strong></td>
            <td>Proyecto/Project:</td>
            <td><strong>@m.PROYECTO</strong></td>
        </tr>
        <tr>
            <td>NºPieza/Part:</td>
            <td><strong>@m.NPIEZA</strong></td>
            <td>Descripcion Pieza / Part Description:</td>
            <td><strong>@m.DESCPIEZA</strong></td>
        </tr>
        <tr>
            <td>NºTroquel/Die Number:</td>
            <td><strong>@m.NTROQUEL</strong></td>
            <td>Dureza/Hardness:</td>
            <td><strong>@m.DUREZA</strong></td>
        </tr>   
        <tr>
            <td>Material/Material:</td>
            <td><strong>@m.MATERIAL</strong></td>
            <td>Tratamiento/Secondary Treatment:</td>
            <td><strong>@m.TRATAMSEC</strong></td>
        </tr>   

      
    </table>

    @If m.TIPOINFORME.ToLower.Contains("temple") Then
        @<table id="table3" >
        <tr>
            <th colspan="7" align="center">@m.TIPOINFORME.ToUpper</th>
        </tr>
        <tr>
            <td rowspan="3" Class="greeblue">Dureza/Hardness HRC</td>
            <td Class="greeblue">Req</td>
            <td colspan="3" >@m.DUREZAREQUERIDATEMPLE</td>
            <td Class="greeblue">Temperatura/Temperature ºC</td>
            <td>@m.TEMPERATURATEMPLE</td>
        </tr>
        <tr>
            <td Class="greeblue">Real Max.</td>
            <td>@m.DUREZAREALTEMPLEMAX</td>
            <td Class="greeblue">Real Min.</td>
            <td>@m.DUREZAREALTEMPLEMIN</td>
            <td Class="greeblue"></td>
            <td></td>
        </tr>
        <tr>
                        <td Class="greeblue">Nº Med.</td>
            <td colspan="3" >@m.NUMEROMEDIDASTEMPLE</td>
            <td Class="greeblue"></td>
            <td></td>
        </tr>

    </table>
    ElseIf m.TIPOINFORME.ToLower.Contains("soldadura") Then
        @<table id="table3" class="tsoldadura">
            <tr>
                <th colspan="5" align="center">Soldadura / Soldering</th>
            </tr>
            <tr>
                <td  Class="greeblue">Material</td>
                <td Class="greeblue">Referencia /Reference</td>
                <td Class="greeblue">Varilla / Sold. Rod</td>
                <td Class="greeblue">Int / Power (A)</td>
                <td Class="greeblue">Notas / Notes</td>
            </tr>
            <tr>
                <td>Duro</td>
                <td>@m.MATERIALAPORTACIONSOLDDURO </td>
                <td>@m.VARILLASOLDADURADURO </td>
                <td>@m.INTENSIDADSOLDADURADURO </td>
                <td rowspan="2">@m.NOTAS</td>
            </tr>
            <tr>
                <td>Blando</td>
                <td>@m.MATERIALAPORTACIONSOLDBLANDO </td>
                <td>@m.VARILLASOLDADURABLANDO  </td>
                <td>@m.INTENSIDADSOLDADURABLANDO </td>
            </tr>

        </table>
    End If
            </body>
</html>