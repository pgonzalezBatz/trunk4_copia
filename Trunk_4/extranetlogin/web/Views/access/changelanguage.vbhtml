@imports web
    @For Each r In Model
           @<form action = "" method="post">
               @Html.Hidden("idcultura", r.id)
               <input type = "submit" value="@r.nombre" Class="btn btn-default btn-lg btn-block" />
           </form>
    Next
