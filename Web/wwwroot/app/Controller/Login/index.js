var LoginController = function () {
    this.initialize = function () {
        registerEvent();
    }
    function login (user, pass) {
        $.ajax({
            type: "POST",
            data: {
                UserName: user,
                Password: pass
            },
            dataType: "json",
            url: "/admin/login/signin",
            success: function (res) {
                if (res.Success) {
                    window.location.href = "/Admin/Home/Index";
                } else {
                    system.notify('Đăng nhập thất bại', 'error');
                }
            }
        });
    }

    function registerEvent () {
        $('#frmLogin').validate({
            errorClass: "red",
            ignore: [],
            lang: 'vi',
            rules: {
                userName: {
                    required: true
                },
                password: {
                    required: true
                }
            }
        });
        $('#btnLogin').on('click',
            function (e) {
                if ($('#frmLogin').valid()) {
                    e.preventDefault();
                    let user = $('#txtUserName').val();
                    let pass = $('#txtPassword').val();
                    login(user, pass);
                }
            });
    }
    
}