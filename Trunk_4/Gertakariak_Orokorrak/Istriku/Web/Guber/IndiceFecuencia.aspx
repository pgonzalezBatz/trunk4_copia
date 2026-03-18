<%@ page title="" language="vb" autoeventwireup="false" masterpagefile="~/MPWeb.Master" codebehind="IndiceFecuencia.aspx.vb" inherits="IstrikuWebRaiz.IndiceFecuencia" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .Invisible {
            visibility: hidden;
            display: none;
        }

        .CruzVerde {
            border-collapse: collapse;
        }

            .CruzVerde span {
                text-transform: capitalize;
            }

        .CeldaCruzVerde {
            border-color: black;
            border-style: solid;
            font-family: Verdana, Geneva, Tahoma, sans-serif;
            font-size: xx-large;
        }

        .CeldaCruzVerde_SinAccidentes {
            background-color: green;
        }

        .CeldaCruzVerde_Incidente {
            background-color: blue;
        }

        .CeldaCruzVerde_Accidente {
            background-color: red;
        }

        .Hoy {
            color: white;
            font-style: italic;
            text-decoration: underline overline;
            text-shadow: 2px 2px 4px #000000;
        }
    </style>

    <script type="text/javascript">
        var cal;
        function onCalendarShown(sender, args) {
            //var cal = $find("calendar1");
            cal = sender;
            //Setting the default mode to month
            cal._switchMode("months", true);

            //Iterate every month Item and attach click event to it
            if (cal._monthsBody) {
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        Sys.UI.DomEvent.addHandler(row.cells[j].firstChild, "click", call);
                    }
                }
            }

            event = event || window.event
            sender._popupDiv.parentElement.style.top = event.pageY + "px";
            sender._popupDiv.parentElement.style.left = event.pageX + "px";
        }
        function onCalendarHidden(sender, args) {
            //var cal = $find("calendar1");
            cal = sender;
            //Iterate every month Item and remove click event from it
            if (cal._monthsBody) {
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        Sys.UI.DomEvent.removeHandler(row.cells[j].firstChild, "click", call);
                    }
                }
            }

        }
        function call(eventElement) {
            var target = eventElement.target;
            switch (target.mode) {
                case "month":
                    //var cal = $find("calendar1");
                    cal._visibleDate = target.date;
                    cal.set_selectedDate(target.date);
                    //cal._switchMonth(target.date);
                    cal._blur.post(true);
                    cal.raiseDateSelectionChanged();

                    __doPostBack("<%= imgFechaRevision.UniqueID %>", "");

                    break;
            }
        }
    </script>
</asp:Content>
<asp:content id="Content2" contentplaceholderid="cp" runat="server">
        
        <table class="GridViewASP" style="width: 1%; margin: auto; ">
            <tr class="HeaderStyle">
                <th style="white-space: nowrap;">
                    <asp:Label ID="Label4" runat="server" Text="Dias sin accidente con baja"></asp:Label></th>
            </tr>
            <tr class="RowStyle" style="text-align: center;">
                <td class="CeldaCruzVerde">
                    <asp:Label ID="lblNumDias" runat="server"></asp:Label></td>
            </tr>
        </table>
        <hr style="width:100%;" />
        <asp:Panel id="pnFechaBusqueda" runat="server" HorizontalAlign="Center">
            <asp:TextBox ID="txtFechaCruzVerde" runat="server" Width="100px" CssClass="Invisible"></asp:TextBox>
            <asp:Panel ID="pnBotones" runat="server" CssClass="PanelBotones">
                <asp:ImageButton ID="imgFechaRevision" runat="server" AlternateText="Calendario" ImageUrl="~/App_Themes/Batz/IconosAjaxControl/CalendarExtender/Calendario.gif" ImageAlign="AbsMiddle" />
                <asp:Label ID="lblFechaCruzVerde" runat="server"></asp:Label>
            </asp:Panel>
            <act:CalendarExtender ID="ce_imgFechaRevision" runat="server" TargetControlID="txtFechaCruzVerde" PopupButtonID="pnBotones" DefaultView="Months" TodaysDateFormat="MMMM , yyyy"
                OnClientHidden="onCalendarHidden" OnClientShown="onCalendarShown" BehaviorID="calendar1" PopupPosition="BottomRight" />
        </asp:Panel>
                    <br />

        <act:TabContainer ID="tc_CruzVerde" runat="server" ActiveTabIndex="0">
		    <act:TabPanel runat="server" ID="tp_Batz" HeaderText="Batz sKoop">
			    <ContentTemplate>

                    <table runat="server" id="TablaCruzVerde" class="CruzVerde" style="margin: auto;">
                        <tr>
                            <td></td>
                            <td></td>
                            <td id="td01" runat="server" class="CeldaCruzVerde">01</td>
                            <td id="td02" runat="server" class="CeldaCruzVerde">02</td>
                            <td id="td03" runat="server" class="CeldaCruzVerde">03</td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td id="td04" runat="server" class="CeldaCruzVerde">04</td>
                            <td id="td05" runat="server" class="CeldaCruzVerde">05</td>
                            <td id="td06" runat="server" class="CeldaCruzVerde">06</td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td id="td07" runat="server" class="CeldaCruzVerde">07</td>
                            <td id="td08" runat="server" class="CeldaCruzVerde">08</td>
                            <td id="td09" runat="server" class="CeldaCruzVerde">09</td>
                            <td id="td10" runat="server" class="CeldaCruzVerde">10</td>
                            <td id="td11" runat="server" class="CeldaCruzVerde">11</td>
                            <td id="td12" runat="server" class="CeldaCruzVerde">12</td>
                            <td id="td13" runat="server" class="CeldaCruzVerde">13</td>
                        </tr>
                        <tr>
                            <td id="td14" runat="server" class="CeldaCruzVerde">14</td>
                            <td id="td15" runat="server" class="CeldaCruzVerde">15</td>
                            <td id="td16" runat="server" class="CeldaCruzVerde">16</td>
                            <td id="td17" runat="server" class="CeldaCruzVerde">17</td>
                            <td id="td18" runat="server" class="CeldaCruzVerde">18</td>
                            <td id="td19" runat="server" class="CeldaCruzVerde">19</td>
                            <td id="td20" runat="server" class="CeldaCruzVerde">20</td>
                        </tr>
                        <tr>
                            <td id="td21" runat="server" class="CeldaCruzVerde">21</td>
                            <td id="td22" runat="server" class="CeldaCruzVerde">22</td>
                            <td id="td23" runat="server" class="CeldaCruzVerde">23</td>
                            <td id="td24" runat="server" class="CeldaCruzVerde">24</td>
                            <td id="td25" runat="server" class="CeldaCruzVerde">25</td>
                            <td id="td26" runat="server" class="CeldaCruzVerde">26</td>
                            <td id="td27" runat="server" class="CeldaCruzVerde">27</td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td id="td28" runat="server" class="CeldaCruzVerde">28</td>
                            <td id="td29" runat="server" class="CeldaCruzVerde">29</td>
                            <td id="td30" runat="server" class="CeldaCruzVerde">30</td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td class="CeldaCruzVerde"></td>
                            <td id="td31" runat="server" class="CeldaCruzVerde">31</td>
                            <td class="CeldaCruzVerde"></td>
                            <td></td>
                            <td></td>
                        </tr>
                    </table>

                    <table class="GridViewASP" style="width: 1%; margin: auto; ">
                        <tr class="HeaderStyle">
                            <th style="white-space: nowrap;">
                                <asp:Label ID="Label5" runat="server" Text="Maximo dias sin bajas"></asp:Label></th>
                        </tr>
                        <tr class="RowStyle" style="text-align: center;">
                            <td class="CeldaCruzVerde">
                                <asp:Label ID="lblNumDiasMax" runat="server"></asp:Label></td>
                        </tr>
                    </table>

                </ContentTemplate>
            </act:TabPanel>
            <act:TabPanel runat="server" ID="tp_Troqueleria" HeaderText="Batz Troqueleria">
			    <ContentTemplate>                  
                    <table runat="server" id="TablaCruzVerde_T" class="CruzVerde" style="margin: auto;">
                        <thead>
<%--                            <tr>
                                <th colspan="7">
                                    <asp:DropDownList ID="ddlUG" runat="server" AutoPostBack="true">
                                        <asp:ListItem Value="" Text="Todo"></asp:ListItem>
                                        <asp:ListItem Value="01" Text="UG 1"></asp:ListItem>
                                        <asp:ListItem Value="02" Text="UG 2"></asp:ListItem>
                                        <asp:ListItem Value="03" Text="UG 3"></asp:ListItem>
                                        <asp:ListItem Value="04" Text="UG 4"></asp:ListItem>
                                    </asp:DropDownList>
                                    <hr />
                                </th>
                            </tr>--%>
                        </thead>
                        <tr>
                            <td></td>
                            <td></td>
                            <td id="td_T01" runat="server" class="CeldaCruzVerde">01</td>
                            <td id="td_T02" runat="server" class="CeldaCruzVerde">02</td>
                            <td id="td_T03" runat="server" class="CeldaCruzVerde">03</td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td id="td_T04" runat="server" class="CeldaCruzVerde">04</td>
                            <td id="td_T05" runat="server" class="CeldaCruzVerde">05</td>
                            <td id="td_T06" runat="server" class="CeldaCruzVerde">06</td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td id="td_T07" runat="server" class="CeldaCruzVerde">07</td>
                            <td id="td_T08" runat="server" class="CeldaCruzVerde">08</td>
                            <td id="td_T09" runat="server" class="CeldaCruzVerde">09</td>
                            <td id="td_T10" runat="server" class="CeldaCruzVerde">10</td>
                            <td id="td_T11" runat="server" class="CeldaCruzVerde">11</td>
                            <td id="td_T12" runat="server" class="CeldaCruzVerde">12</td>
                            <td id="td_T13" runat="server" class="CeldaCruzVerde">13</td>
                        </tr>
                        <tr>
                            <td id="td_T14" runat="server" class="CeldaCruzVerde">14</td>
                            <td id="td_T15" runat="server" class="CeldaCruzVerde">15</td>
                            <td id="td_T16" runat="server" class="CeldaCruzVerde">16</td>
                            <td id="td_T17" runat="server" class="CeldaCruzVerde">17</td>
                            <td id="td_T18" runat="server" class="CeldaCruzVerde">18</td>
                            <td id="td_T19" runat="server" class="CeldaCruzVerde">19</td>
                            <td id="td_T20" runat="server" class="CeldaCruzVerde">20</td>
                        </tr>
                        <tr>
                            <td id="td_T21" runat="server" class="CeldaCruzVerde">21</td>
                            <td id="td_T22" runat="server" class="CeldaCruzVerde">22</td>
                            <td id="td_T23" runat="server" class="CeldaCruzVerde">23</td>
                            <td id="td_T24" runat="server" class="CeldaCruzVerde">24</td>
                            <td id="td_T25" runat="server" class="CeldaCruzVerde">25</td>
                            <td id="td_T26" runat="server" class="CeldaCruzVerde">26</td>
                            <td id="td_T27" runat="server" class="CeldaCruzVerde">27</td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td id="td_T28" runat="server" class="CeldaCruzVerde">28</td>
                            <td id="td_T29" runat="server" class="CeldaCruzVerde">29</td>
                            <td id="td_T30" runat="server" class="CeldaCruzVerde">30</td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td class="CeldaCruzVerde"></td>
                            <td id="td_T31" runat="server" class="CeldaCruzVerde">31</td>
                            <td class="CeldaCruzVerde"></td>
                            <td></td>
                            <td></td>
                        </tr>
                    </table>

                    <table class="GridViewASP" style="width: 1%; margin: auto; ">
                        <tr class="HeaderStyle">
                            <th style="white-space: nowrap;">
                                <asp:Label ID="Label6" runat="server" Text="Maximo dias sin bajas"></asp:Label></th>
                        </tr>
                        <tr class="RowStyle" style="text-align: center;">
                            <td class="CeldaCruzVerde">
                                <asp:Label ID="lblNumDiasMax_Troqueleria" runat="server"></asp:Label></td>
                        </tr>
                    </table>

                </ContentTemplate>
            </act:TabPanel>
            <act:TabPanel runat="server" ID="tp_Sistemas" HeaderText="Batz Sistemas">
			    <ContentTemplate>
                    <table runat="server" id="TablaCruzVerde_S" class="CruzVerde" style="margin: auto;">
                        <tr>
                            <td></td>
                            <td></td>
                            <td id="td_S01" runat="server" class="CeldaCruzVerde">01</td>
                            <td id="td_S02" runat="server" class="CeldaCruzVerde">02</td>
                            <td id="td_S03" runat="server" class="CeldaCruzVerde">03</td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td id="td_S04" runat="server" class="CeldaCruzVerde">04</td>
                            <td id="td_S05" runat="server" class="CeldaCruzVerde">05</td>
                            <td id="td_S06" runat="server" class="CeldaCruzVerde">06</td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td id="td_S07" runat="server" class="CeldaCruzVerde">07</td>
                            <td id="td_S08" runat="server" class="CeldaCruzVerde">08</td>
                            <td id="td_S09" runat="server" class="CeldaCruzVerde">09</td>
                            <td id="td_S10" runat="server" class="CeldaCruzVerde">10</td>
                            <td id="td_S11" runat="server" class="CeldaCruzVerde">11</td>
                            <td id="td_S12" runat="server" class="CeldaCruzVerde">12</td>
                            <td id="td_S13" runat="server" class="CeldaCruzVerde">13</td>
                        </tr>
                        <tr>
                            <td id="td_S14" runat="server" class="CeldaCruzVerde">14</td>
                            <td id="td_S15" runat="server" class="CeldaCruzVerde">15</td>
                            <td id="td_S16" runat="server" class="CeldaCruzVerde">16</td>
                            <td id="td_S17" runat="server" class="CeldaCruzVerde">17</td>
                            <td id="td_S18" runat="server" class="CeldaCruzVerde">18</td>
                            <td id="td_S19" runat="server" class="CeldaCruzVerde">19</td>
                            <td id="td_S20" runat="server" class="CeldaCruzVerde">20</td>
                        </tr>
                        <tr>
                            <td id="td_S21" runat="server" class="CeldaCruzVerde">21</td>
                            <td id="td_S22" runat="server" class="CeldaCruzVerde">22</td>
                            <td id="td_S23" runat="server" class="CeldaCruzVerde">23</td>
                            <td id="td_S24" runat="server" class="CeldaCruzVerde">24</td>
                            <td id="td_S25" runat="server" class="CeldaCruzVerde">25</td>
                            <td id="td_S26" runat="server" class="CeldaCruzVerde">26</td>
                            <td id="td_S27" runat="server" class="CeldaCruzVerde">27</td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td id="td_S28" runat="server" class="CeldaCruzVerde">28</td>
                            <td id="td_S29" runat="server" class="CeldaCruzVerde">29</td>
                            <td id="td_S30" runat="server" class="CeldaCruzVerde">30</td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td class="CeldaCruzVerde"></td>
                            <td id="td_S31" runat="server" class="CeldaCruzVerde">31</td>
                            <td class="CeldaCruzVerde"></td>
                            <td></td>
                            <td></td>
                        </tr>
                    </table>

                    <table class="GridViewASP" style="width: 1%; margin: auto; ">
                        <tr class="HeaderStyle">
                            <th style="white-space: nowrap;">
                                <asp:Label ID="Label7" runat="server" Text="Maximo dias sin bajas"></asp:Label></th>
                        </tr>
                        <tr class="RowStyle" style="text-align: center;">
                            <td class="CeldaCruzVerde">
                                <asp:Label ID="lblNumDiasMax_Sistemas" runat="server"></asp:Label></td>
                        </tr>
                    </table>

                </ContentTemplate>
             </act:TabPanel>
         </act:TabContainer>

    <table style="display: inline-table;">
        <tr>
            <td style="background-color: red; width: 20px"></td>
            <td>
                <asp:Label ID="Label1" runat="server" Text="Accidente CON baja"></asp:Label></td>
        </tr>
        <tr>
            <td style="background-color: blue; width: 20px"></td>
            <td>
                <asp:Label ID="Label2" runat="server" Text="Accidente SIN baja"></asp:Label></td>
        </tr>
        <tr>
            <td style="background-color: green; width: 20px"></td>
            <td>
                <asp:Label ID="Label3" runat="server" Text="SIN accidente"></asp:Label></td>
        </tr>
        <tr>
            <td></td>
        </tr>
    </table>


    <%--<table class="GridViewASP">
        <tr class="HeaderStyle">
            <th></th>
            <th>
                <asp:Label ID="Label5" runat="server" Text="Enero" /></th>
            <th>
                <asp:Label ID="Label6" runat="server" Text="Febrero" /></th>
            <th>
                <asp:Label ID="Label7" runat="server" Text="Marzo" /></th>
            <th>
                <asp:Label ID="Label8" runat="server" Text="Abril" /></th>
            <th>
                <asp:Label ID="Label9" runat="server" Text="Mayo" /></th>
            <th>
                <asp:Label ID="Label10" runat="server" Text="Junio" /></th>
            <th>
                <asp:Label ID="Label11" runat="server" Text="Julio" /></th>
            <th>
                <asp:Label ID="Label12" runat="server" Text="Agosto" /></th>
            <th>
                <asp:Label ID="Label13" runat="server" Text="Septiembre" /></th>
            <th>
                <asp:Label ID="Label14" runat="server" Text="Octubre" /></th>
            <th>
                <asp:Label ID="Label15" runat="server" Text="Noviembre" /></th>
            <th>
                <asp:Label ID="Label16" runat="server" Text="Diciembre" /></th>
        </tr>
        <tr class="HeaderStyle RowStyle">
            <th>
                <asp:Label ID="Label17" runat="server" Text="Nº Accidentes" /></th>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
        </tr>
        <tr class="HeaderStyle AlternatingRowStyle">
            <th>
                <asp:Label ID="Label18" runat="server" Text="Indice Incidencia" /></th>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
        </tr>
        <tr class="HeaderStyle RowStyle">
            <th>
                <asp:Label ID="Label19" runat="server" Text="Objetivo" /></th>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
        </tr>
    </table>--%>
        
</asp:content>
<%--<asp:Content ID="Content3" ContentPlaceHolderID="cphPie" runat="server">
</asp:Content>--%>
