$(document).ready(function () {






    $('#img1').on('change', function () {
        var fileInput = $(this)[0];

        if (fileInput.files.length > 0) {
            var file = fileInput.files[0];
            var reader = new FileReader();

            reader.onloadend = function () {
               
                var base64Data = reader.result.split(',')[1];

                $('#deleteimg').css('display', 'block');
                $('#img2').val(base64Data);
            };

            reader.readAsDataURL(file);
        }
    });



    $('#deleteimg').on('click', function () {
        // Очищаємо поля #img1 та #img2
        $('#img1').val('');
        $('#img2').val('');

        // Ховаємо кнопку deleteimg
        hideDeleteButton();
    });

    // Функція для показу кнопки deleteimg
    function showDeleteButton() {
        $('#deleteimg').css('display', 'block');
    }

    // Функція для ховання кнопки deleteimg
    function hideDeleteButton() {
        $('#deleteimg').css('display', 'none');
    }








    var activeBlock = null;

    $(".product_block").click(function (e) {


        var currentWidth = $(this).width();
        console.log(currentWidth)
        console.log(activeBlock)

        if (currentWidth === 510) {
            closeblock($(this))
            
        } else if (activeBlock === null) {
            openblock($(this))
        }
        else {
            activeBlock.find('.right_hide').animate({ opacity: 0 }, 200);
            activeBlock.find('.right_hide').css('display', 'none');
            activeBlock.animate({ width: '290px' }, 300);
            

            
            $(this).animate({ width: '550px' }, 300);
            $(this).find('.right_hide').css('display', 'block');
            $(this).find('.right_hide').animate({ opacity: 1 }, 400);
            
            activeBlock = $(this);
            
        }
    });



    function closeblock(a) {

        
        a.find('.right_hide').animate({ opacity: 0 }, 200);
        a.animate({ width: '290px' }, 300);
        activeBlock = null;
        a.find('.right_hide').css('display', 'none');
    }


    function openblock(b) {
        activeBlock = b;
        b.animate({ width: '550px' }, 300);
        b.find('.right_hide').css('display', 'block');
        b.find('.right_hide').animate({ opacity: 1 }, 400);
    }











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