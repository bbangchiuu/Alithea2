$(document).ajaxStop(function () {
    $("#register").html('Đăng ký');
    console.log("ajax finish");
});

$("#register").click(function() {
    $("#register").html('<i class="fa fa-spinner fa-spin" style="margin-right: 2%; margin-left: -2%"></i>Loading');
});

function checkRegister() {
    var checkRegisterForm = new Map();

    var FullName = $('input[name="FullName"]').val();
    var Username = $('input[name="Username"]').val();
    var Email = $('input[name="Email"]').val();
    var Birthday = $('input[name="BirthDay"]').val();
    var Phone = $('input[name="Phone"]').val();
    var Address = $('input[name="Address"]').val();
    var Password = $('input[name="Password"]').val();
    var ConfirmPassword = $('input[name="confirmpassord"]').val();

//    if (/^[a-zA-Z0-9\+]*$/.test("banggmailcom")) {
//        console.log("co ky tu dac biet");
//    } else {
//        console.log("ko co ky tu dac biet");
//    }

    if (FullName === "") {
        checkRegisterForm.set('FullName', 'Bạn chưa nhập họ tên');
    }

    if (Birthday === "" || Birthday === null) {
        checkRegisterForm.set('Birthday', 'Bạn chưa nhập Birthday');
    }

    if (Phone === "") {
        checkRegisterForm.set('Phone', 'Bạn chưa nhập số điện thoại');
    } else if (/\D/.test(Phone)) {
        checkRegisterForm.set('Phone', 'Số điện thoại không hợp lệ');
    } else if (Phone.length < 10 || Phone.length > 13) {
        checkRegisterForm.set('Phone', 'Số điện thoại không hợp lệ');
    } else if (/^0/.test(Phone) === false) {
        checkRegisterForm.set('Phone', 'Số điện thoại không hợp lệ');
    }

    if (Address === "") {
        checkRegisterForm.set('Address', 'Bạn chưa nhập địa chỉ');
    }

    if (Password === "") {
        checkRegisterForm.set('Password', 'Bạn chưa nhập mật khẩu');
    } else if (Password.length <= 6) {
        checkRegisterForm.set('Password', 'Mật khẩu phải nhiều hơn 6 ký tự');
    } else if (/[A-Z]+/.test(Password) === false) {
        checkRegisterForm.set('Password', 'Mật khẩu phải có ít nhất 1 chứ in hoa');
    } else if (/^[a-zA-Z0-9\+]*$/.test(Password)) {
        checkRegisterForm.set('Password', 'Mật khẩu phải có ít nhất 1 ký tự đặc biệt');
    }
    
    if (ConfirmPassword === "") {
        checkRegisterForm.set('ConfirmPassword', 'Bạn chưa nhập lại mật khẩu');
    } else if (ConfirmPassword !== Password) {
        checkRegisterForm.set('ConfirmPassword', 'Mật khẩu nhập lại không đúng');
    }

    if (Username === "") {
        checkRegisterForm.set('Username', 'Bạn chưa nhập tài khoản');
    } else {
        var URL_Username = "/api/UserAccounts?username=" + Username;
        console.log("url: " + URL_Username);
        $.ajax({
            async: false,
            type: 'GET',
            url: URL_Username,
            success: function (data) {
                console.log("data: " + data.Password);
                checkRegisterForm.set('Username', 'Tài khoản này đã tồn tại');
            },
            error: function (data) {
                //get the status code
                console.log("data: " + data.Password);
            }
        });
    }

    if (Email === "") {
        checkRegisterForm.set('Email', 'Bạn chưa nhập email');
    } else if (/^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/.test(Email) === false) {
        checkRegisterForm.set('Email', 'Email không hợp lệ');
    } else {
        var URL_Email = "/api/UserAccounts?email=" + Email;
        $.ajax({
            async: false,
            type: 'GET',
            url: URL_Email,
            success: function (data) {
                checkRegisterForm.set('Email', 'Email này đã tồn tại');
            },
            error: function (data) {
                //get the status code
                console.log("data: " + data);
            }
        });
    }

    console.log("count: " + checkRegisterForm.size);

  
    if (checkRegisterForm.size === 0) {
        return true;
    }

    if (checkRegisterForm.has('FullName')) {
        $("#validateFullName").html(checkRegisterForm.get('FullName'));
    } else {
        $("#validateFullName").html("");
    }

    if (checkRegisterForm.has('Username')) {
        $("#validateUsername").html(checkRegisterForm.get('Username'));
    } else {
        $("#validateUsername").html("");
    }

    if (checkRegisterForm.has('Email')) {
        $("#validateEmail").html(checkRegisterForm.get('Email'));
    } else {
        $("#validateEmail").html("");
    }

    if (checkRegisterForm.has('Birthday')) {
        $("#validateBirthday").html(checkRegisterForm.get('Birthday'));
    } else {
        $("#validateBirthday").html("");
    }

    if (checkRegisterForm.has('Phone')) {
        $("#validatePhone").html(checkRegisterForm.get('Phone'));
    } else {
        $("#validatePhone").html("");
    }

    if (checkRegisterForm.has('Address')) {
        $("#validateAddress").html(checkRegisterForm.get('Address'));
    } else {
        $("#validateAddress").html("");
    }

    if (checkRegisterForm.has('Password')) {
        $("#validatePassword").html(checkRegisterForm.get('Password'));
    } else {
        $("#validatePassword").html("");
    }

    if (checkRegisterForm.has('ConfirmPassword')) {
        $("#validateConfirmPassword").html(checkRegisterForm.get('ConfirmPassword'));
    } else {
        $("#validateConfirmPassword").html("");
    }

    return false;
}