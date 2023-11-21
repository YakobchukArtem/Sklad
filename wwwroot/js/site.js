$(document).ready(function () {

    var activeBlock = null;

    $(".product_block").click(function (e) {


        var currentWidth = $(this).width();
        console.log(currentWidth)
        console.log(activeBlock)

        if (currentWidth === 510) {
            $(this).animate({ width: '290px' }, 300);
            activeBlock = null;
        } else if (activeBlock === null) {
            activeBlock = $(this);
            $(this).animate({ width: '550px' }, 300);
        }
        else {
            activeBlock.animate({ width: '290px' }, 300);
            $(this).animate({ width: '550px' }, 300);
            activeBlock = $(this);
            
        }
    });




    $(".delete-btn2").click(function (e) {
        e.preventDefault();
        var productId = $(this).data("id");
        var rowToDelete = $('div[data-id="' + productId + '"]'); 


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