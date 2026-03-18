<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="BusquedaUsuarios.ascx.vb" Inherits="WebRaiz.BusquedaUsuarios" %>         
    <div id="scrollable-dropdown-menu" style="display:inline-flex;">
        <asp:TextBox runat="server" ID="txtInput" columns="80" CssClass="form-control typeahead" ToolTip="Seleccione uno" AutoComplete="Off"></asp:TextBox>                   
        <asp:Button runat="server" ID="btnFire" style="visibility:hidden;" />  
    </div>	
    <asp:HiddenField runat="server" ID="hfValue" />            
    <script type="text/javascript">
        Sys.Application.add_load(init); //Para que funcione tambien con ajax
        function init() {
            $(document).ready(function () {
                var names = new Bloodhound({
                    datumTokenizer: Bloodhound.tokenizers.obj.whitespace('id', 'no', 'nt', 'fa', 'fb'),
                    queryTokenizer: Bloodhound.tokenizers.whitespace,
                    remote: {
                        url: '<%=RutaPaginaBusqueda %>',
                        replace: function (url, uriEncodedQuery) {
                            var newUrl = url + '?q=' + uriEncodedQuery + '&o=<%=Opcion%>&sa=<%=SoloActivos%>';
                            return newUrl;
                        },
                    }
                });

                $('#scrollable-dropdown-menu .typeahead').typeahead(null, {
                    source: names,
                    displayKey: 'no',
                    val:'id',
                    highlight: true,
                    minLength: 3,
                    limit: 50,
                    templates: {
                        empty: ['<div class="empty-message"><%=Traducir("No matches")%></div>',],
                        suggestion: function(data) {
                            var html='';
                            if(data.fb==1)
                                html="<div class='bg-danger'>";
                            else
                                html='<div>';
                            return html + data.no + ' - ' + data.nt + '</div>';
                        }
                    },
                }).on('typeahead:selected typeahead:autocompleted', function (e, datum) {                 
                    $('#<%=hfValue.ClientID%>').val(datum.id);
                    var doPostBack=<%=PostBack.ToString.ToLower%>;
                    if (doPostBack.toString()=='true') {                                            
                        var str ='<%=btnFire.ClientID%>'.replace(/_/gi, '$');
                        __doPostBack(str, '');
                        return false;
                    }                
                });
            });
        }   
    </script>