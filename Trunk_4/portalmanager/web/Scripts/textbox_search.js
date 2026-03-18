/*
Ajax-JSON texbox search. Displays a dinamyc list of matches from the textbox
Parameters:
    url= string where the ajax call will be made. JSON response it's expected
    searchBox= JQuery object of the box in which the search is typed
    onleElementToAppend= a function accepting one parameter (the return of the response) only called when one element returned
    manyElementsToAppend= a function accepting 3 parameters. The jquery div container, a value fron the JSON and a function required to call for element binding.
                           This function that is to be called, needs 2 parameters: The element to bind and a function to be executed when binding
    elementContainer= This parameter it's optional. Use it when the default container that's displayed when searching, needs to be changed
*/
function textboxSearch(url, searchBox, oneElementToAppend, manyElementsToAppend, elementContainer) {
    var fDisplay = function (sB) {
        var pos = sB.offset();
        if (typeof (elementContainer) == 'undefined') {
            elementContainer = '<div id="divavoidcolision" style="position:absolute;left:' + pos.left.toString() + 'px;top:' + (pos.top + sB.outerHeight()).toString() + 'px;display:block;width:48%;height:200px;background-color:Gray;overflow:auto;background-color:#EEE;border:1px solid #8FABFF;"></div>';
        };
        if ($('#'+$(elementContainer).attr('id')).length == 0) {
            $("body").append(elementContainer);
            //Avoid propagation when inside box click
            $('#' + $(elementContainer).attr('id')).click(function (event) {
                event.stopPropagation();
            });
        }   
        else {
            $('#' + $(elementContainer).attr('id')).empty();
        };
   };
   var fHide = function () {
       $('#divavoidcolision').remove();
   };
   var bindProveedorBehaviors = function (element, f) {
       element.click(function () {
           f();
           fHide();
           return false;
       });
   };
   //prevent form submit
   searchBox.keydown(function (e) {
       if (e.keyCode == 13) {
           return false;
       };
   });
   
   searchBox.keyup(function (e) {
       clearTimeout($.data(this, 'timer'));
       var me = this;
       $(this).data('timer', setTimeout(function () {
           if (me.value.length > 2) {
               fDisplay(searchBox);
               $.getJSON(url + me.value, function (json) {
                   if (json.length == 0) {
                       $('#divavoidcolision').append("<h4>No elements found</h4>");
                   }
                   else {
                       if (json.length == 1 & e.keyCode == 13) {
                           oneElementToAppend(json[0]);
                           fHide();
                           return false;
                       }
                       else {
                           $.each(json, function (key, val) {
                               manyElementsToAppend($('#divavoidcolision'), val, bindProveedorBehaviors)
                           });
                       };
                   };
               });
           }
           else {
               fHide();
           };
       },500));
   });
   $('html').click(function () {
       if ($("#divavoidcolision").length > 0) {
           fHide();
       }
   });
};