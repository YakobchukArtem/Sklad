

$(document).ready(function () {


    $(".delete-btn2").click(function (e) {
        e.preventDefault();
        var productId = $(this).data("id");
        var rowToDelete = $('div[data-id="' + productId + '"]'); // Отримати батьківський рядок кнопки "delete"


        $.ajax({
            url: "/Products/Delete",
            type: "POST",
            cache: false,  // Додайте цей параметр
            data: { id: productId },
            success: function (response) {
                if (response.success) {
                    rowToDelete.remove();
                }
            },
            error: function (error) {
                alert("Error deleting product");
            }
        });
    });


    $(".delete-btn").click(function (e) {
        e.preventDefault();
        var rowToDelete = $(this).closest("tr"); // Отримати батьківський рядок кнопки "delete"

        var productId = $(this).data("id");
        $.ajax({
            url: "/Products/Delete",
            type: "POST",
            cache: false,  // Додайте цей параметр
            data: { id: productId },
            success: function (response) {
                if (response.success) {
                    rowToDelete.remove();
                }
            },
            error: function (error) {
                alert("Error deleting product");
            }
        });
    });
});