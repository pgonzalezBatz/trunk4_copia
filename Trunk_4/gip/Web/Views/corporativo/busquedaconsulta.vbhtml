@imports web
@section title
    - @h.traducir("Busqueda")
End section
@section menu1
    @Html.Partial("menu")
End Section

<div class="row">
            <div class="col-sm-12">
                <form action="" method="get">
                    <div class="input-group">
                        @Html.TextBox("q", Nothing, New With {.class = "typeahead tt-input form-control", .autocomplete = "off", .spellcheck = "off", .placeholder = h.traducir("Buscar Empresas en todas las plantas")})
                        <div class="input-group-btn">
                            <button class="btn btn-primary" type="submit"><i class="glyphicon glyphicon-search"></i></button>
                        </div>
                    </div>
                </form>
</div>
    @If Model IsNot Nothing Then
        Dim j = 1
@<div Class="row">
        <div Class="col-xs-12">
                    @For Each p In Model
                    @<div Class="panel panel-default">
                        <div class="panel-heading">
                            <a data-toggle="collapse" href="#collapse@(j)" data-parent="#accordion">@p.key.nombreplanta</a>
                        </div>
                        <div id="collapse@(j)" class="panel-collapse collapse @(If(j = 1, "in", ""))">
                            <div Class="panel-body">
                                <table class="table">
                                    <thead>
                                        <tr>
                                            <th>@h.traducir("Empresa")</th>
                                            <th>@h.traducir("CIF")</th>
                                            <th></th>
                                        </tr>
                                    </thead>
                                    @For Each e In p
                                    @<tr>
                                        <td>@e.nombre</td>
                                        <td>@e.cif</td>

                                    </tr>
                                    Next
                                </table>
                            </div>
                        </div>
                    </div>
                        j = j + 1
                    Next
        </div>
    </div>
    End If

    
    
</div>
