$(document).ready(function () {
    $('.btn-delete-productCategory').click(function () {
        var id = $(this).attr("id").replace("delete-", "");
        if (confirm("Are you sure want to delete this Product Category?")) {
            var token = $('input[name="__RequestVerificationToken"]').val();
            $.ajax({
                url: "/ProductCategories/Delete/" + id,
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