$(document).ready(function () {
    var $input = $('.typeahead');
    $input.typeahead({
        autoSelect: true,
        items: 3,
        source: function (query, process) {
            $.ajax({
                url: '/Products/GetListProductCategories',
                type: 'GET',
                success: function (response) {
                    $.each(response, function () {
                        this.name = this.Id + ' - ' + this.Name;
                        //this.name = this.Name;
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
                $('#hidProductCategoryId').val(current.Id);
                console.log(current.Id)
            }
        }
    })
})