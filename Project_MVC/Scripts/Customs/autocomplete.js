$(document).ready(function () {
    var $input = $('.typeahead');
    var link = $('#AutoComplete').data('request-url');
    $input.typeahead({
        autoSelect: true,
        items: 3,
        source: function (query, process) {
            $.ajax({
                url: link,
                type: 'GET',
                success: function (response) {
                    $.each(response, function () {
                        this.name = this.Code + ' - ' + this.Name;
                    });
                    return process(response);
                }
            })
        }
    });

    $input.change(function () {
        var current = $input.typeahead("getActive");
        if (current) {
            if (current.name == $input.val()) {
                $('#hidCode').val(current.Code);
                //console.log(current.Code)
            }
        }
    })
})