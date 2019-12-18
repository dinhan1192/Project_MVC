$(document).ready(function () {
    $('.btn-delete-product').click(function () {
        var id = $(this).attr("id").replace("delete-", "");
        if (confirm("Are you sure want to delete this Product ?")) {
            var token = $('input[name="__RequestVerificationToken"]').val();
            $.ajax({
                url: "/Products/Delete/" + id,
                type: "POST",
                data: {
                    __RequestVerificationToken: token
                },
                success: function (data) {
                    alert("Delete success!");
                    window.location.reload();
                },
                error: function (error) {
                    alert("Action fails! Please try again!");
                }
            });
        }
        return false;
    });
})