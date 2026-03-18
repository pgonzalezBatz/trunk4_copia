function cascade_select(s1, s2,f_callback) {
    s1.change(function () {
        var url = f_callback(s1.val());
        s2.empty();
        $.getJSON(url, function (json) {
            $.each(json, function (index,o) {
                s2.append('<option value="' + o.id + '">' + o.name + '</option>');
            });
        });
    });
}