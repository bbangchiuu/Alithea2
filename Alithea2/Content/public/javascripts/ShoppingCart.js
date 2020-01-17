$(document).ready(function () {
    console.log("pro2222: ");
    $('#addItem').click(function (e) {
        var colorPro = $("input[name='color_pro']:checked").val();
        if (typeof colorPro === 'undefined') {
            colorPro = 0;
        }
        console.log("color: " + colorPro);

        var size = $("#pro_size option:selected").val();
        console.log("size: " + size);

        $.ajax({
            url: "/BuyItem/AddItem", // gửi ajax đến file result.php
            type: "post", // chọn phương thức gửi là post
            dataType: "html", // dữ liệu trả về dạng text
            data: { // Danh sách các thuộc tính sẽ gửi đi
                pro_id: $('#pro_id').val(),
                quantity: $('#qty_input').val(),
                color: colorPro,
                size: $("#pro_size option:selected").val()
            },
            success: function (result) {
                console.log(result);
                $('#qty_order').empty();
                $('#qty_order').html(result);
            }
        });
        e.preventDefault();
    });

    $(".qty_input").change(function () {
        console.log(this.value);
        if (this.value <= 0) {
            this.value = 1;
        }
    });
});


