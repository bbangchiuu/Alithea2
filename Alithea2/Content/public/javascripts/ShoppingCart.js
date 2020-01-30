$(document).ready(function () {
    $('#addItem').click(function (e) {
      
        var colorPro = $("input[name='color_pro']:checked").val();
        if (typeof colorPro === 'undefined') {
            colorPro = 0;
        }

        var inputNameColor = "#nameColor-" + colorPro;
        var nameColor = $(inputNameColor).val();

//        console.log("color: " + colorPro);
//        console.log("color name: " + nameColor);

        var size = $("#pro_size option:selected").val();
        var nameSize = $("#pro_size option:selected").text();
        if (typeof size === 'undefined') {
            size = 0;
            nameSize = "Mặc định";
        }
        
//        console.log("size: " + size);
//        console.log("name size: " + nameSize);

        $.ajax({
            url: "/BuyItem/AddItem", 
            type: "post", // chọn phương thức gửi là post
            dataType: "html", // dữ liệu trả về dạng text
            data: { // Danh sách các thuộc tính sẽ gửi đi
                pro_id: $('#pro_id').val(),
                quantity: $('#qty_input').val(),
                color: colorPro,
                nameColor: nameColor,
                size: size,
                nameSize: nameSize
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


