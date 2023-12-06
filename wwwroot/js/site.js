$(document).ready(function () {

    var currentSortCategory = $("#sortSelect").data("current-sort-category");
    $("#sortSelect").val(currentSortCategory);

    $("#sortSelect").change(function () {


        var data = null;
        var sel = $("#sortSelect").val();
        var selectedParameter = null;
        var anotherParameter = null;

        if (sel === "ID") {
            selectedParameter = sel;
        } else if (sel === "Name") {
            selectedParameter = sel;
        } else if (sel === "Price1") {
            selectedParameter = "Price";
        } else if (sel === "Price2") {
            selectedParameter = "Price";
            anotherParameter = 'DESC';
        } else if (sel === "Count1") {
            selectedParameter = "Count";
        } else if (sel === "Count2") {
            selectedParameter = "Count";
            anotherParameter = 'DESC';
        }

        $.ajax({
            url: "/Products/Products",
            type: "GET",
            data: {
                parameter: selectedParameter,
                desc: anotherParameter
            },
            success: function (data) {
                $('body').html(data);
             
            },
            error: function (error) {
            
            }
        });

    });



$("#sortSelect2").change(function () {

        var data = null;
        var sel = $("#sortSelect2").val();
        var selectedParameter = null;
        var anotherParameter = null;

        if (sel === "ID") {
            selectedParameter = sel;
        } else if (sel === "Name") {
            selectedParameter = sel;
        } else if (sel === "Price1") {
            selectedParameter = "Price";
        } else if (sel === "Price2") {
            selectedParameter = "Price";
            anotherParameter = 'DESC';
        } else if (sel === "Count1") {
            selectedParameter = "Count";
        } else if (sel === "Count2") {
            selectedParameter = "Count";
            anotherParameter = 'DESC';
        }

        $.ajax({
            url: "/Products/Grid_Products",
            type: "GET",
            data: {
                parameter: selectedParameter,
                desc: anotherParameter
            },
            success: function (data) {
                $('body').html(data);
                console.log("Success");
            },
            error: function (error) {
                console.log("Error");
            }
        });
    });

    // Initialize the sort select element based on the current sort category
    ////const currentSortCategory = ViewBag.Data_Tables.Current_sort_category;
    ////const selectedOption = sortSelect.options.find(option => option.value === currentSortCategory);
    ////if (selectedOption) {
    ////    selectedOption.selected = true;
    ////}

    //ViewBag.Data_Tables.Current_sort_category - тут лежить стрінг назва категорії яка була вибрана







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


    function checkImg2() {
        var img2Value = $('#img2').val();
        if (img2Value !== '') {
            $('#deleteimg').css('display', 'block');
        } else {
            $('#deleteimg').css('display', 'none');
        }
    }

    // Викликати функцію при завантаженні сторінки
    checkImg2();

    // Викликати функцію при зміні поля #img2
    $('#img2').on('input', function () {
        checkImg2();
    });

    // Функція для ховання кнопки deleteimg
    function hideDeleteButton() {
        $('#deleteimg').css('display', 'none');
    }








    var activeBlock = null;
    var activeBlock2 = null;
    $(".product_block").click(function (e) {


        var currentWidth = $(this).width();
        
        

        if (currentWidth === 510) {
            closeblock($(this))
            
            
        } else if (activeBlock === null) {
            openblock($(this))
            
        }
        else {
            activeBlock.find('.right_hide').animate({ opacity: 0 }, 200);  
            activeBlock.animate({ width: '290px' }, 300);
            activeBlock2 = activeBlock;
            setTimeout(function () {
                activeBlock2.find('.right_hide').css('display', 'none');
        }, 200);
            

            
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
        
        setTimeout(function () {
            a.find('.right_hide').css('display', 'none');
        }, 300);
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