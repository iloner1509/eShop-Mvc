var UserProfile = function () {
    this.initialize = function () {
        registerEvents();
    }

    function registerEvents() {
        $("#btnProfile").on("click",
            function () {
            });
        $("#txtBirthDay").datepicker({
            autoclose: true,
            format: "dd/mm/yyyy"
        });
        $("#frmEdit").validate({
            errorClass: "red",
            ignore: [],
            lang: "vi",
            rules: {
                txtFullName: { required: true },

                txtEmail: {
                    required: true,
                    email: true
                }
            }
        });
        $("#btnSelectImg").on("click",
            function () {
                $("#fileInputImage").click();
            });
        $("#fileInputImage").on("change",
            function () {
                let fileUpload = $(this).get(0);
                let files = fileUpload.files;
                let data = new FormData();
                for (let i = 0; i < files.length; i++) {
                    data.append(files[i].name, files[i]);
                }
                $.ajax({
                    type: "POST",
                    url: "/Admin/Upload/UploadImage",
                    contentType: false,
                    processData: false,
                    data: data,
                    success: function (path) {
                        $("#txtImage").val(path);
                        system.notify("Upload ảnh thành công", "success");
                    },
                    error: function () {
                        system.notify("Xảy ra lỗi khi upload ảnh !", "error");
                    }
                });
            });
        //$("body").on("click",
        //    "#btnConfirmChangePassword",
        //    function (e) {
        //        if ($("#frmChangePassword").valid()) {
        //            //$("#frmChangePassword").submit();
        //            e.preventDefault();
        //            let currentPass = $("#txtCurrentPassword").val();
        //            let newPass = $("#txtNewPassword").val();
        //            let confirmPass = $("#txtConfirmNewPassword").val();

        //            $.ajax({
        //                type: "POST",
        //                url: form.attr("action"),
        //                data: {
        //                    CurrentPassword: currentPass,
        //                    NewPassword: newPass,
        //                    ConfirmNewPassword: confirmPass,
        //                },
        //                dataType: "json",

        //                success: function (res) {
        //                    system.notify("Thay đổi mật khẩu thành công", "success");
        //                    //$("#modalChangePassword").modal("hide");
        //                    $("#frmChangePassword").load("/Admin/User/ChangePassword");
        //                },
        //                error: function () {
        //                    //system.notify("Có lỗi xảy ra!", "error");
        //                    $("#frmChangePassword").load("/Admin/User/ChangePassword");
        //                }
        //            });
        //        }
        //    });
        $("#btnSave").on("click",
            function (e) {
                if ($("#frmEdit").valid()) {
                    e.preventDefault();
                    let id = $("#frmEdit").data("id");
                    let fullName = $("#txtFullName").val();
                    let dob = $("#txtBirthDay").val();
                    let email = $("#txtEmail").val();
                    let avatar = $("#txtImage").val();
                    let phoneNumber = $("#txtPhoneNumber").val();

                    $.ajax({
                        url: "/Admin/User/SaveEntity",
                        type: "POST",
                        data: {
                            Id: id,
                            FullName: fullName,

                            Email: email,
                            PhoneNumber: phoneNumber,
                            BirthDay: dob,
                            Avatar: avatar
                        },
                        dataType: "json",
                        success: function () {
                            system.notify("Cập nhật dữ liệu thành công", "success");
                            loadData();
                            window.location.reload();
                        },
                        error: function () {
                            system.notify("Có lỗi khi cập nhật dữ liệu !", "error");
                        }
                    });
                }
            });
        $("#txtBirthDay").datepicker({
            autoclose: true,
            format: "dd/mm/yyyy"
        });
        $("#btnChangePassword").on("click",
            function () {
                $("#modalChangePassword").modal("show");
            });
    }

    function loadData() {
        let userId = $("#frmEdit").data("id");
        $.ajax({
            url: "/Admin/User/GetById",
            type: "GET",
            data: {
                id: userId
            },
            dataType: "json",
            success: function (res) {
                let data = res;

                $("#txtFullName").val(data.FullName);

                $("#txtEmail").val(data.Email);
                $("#txtPhoneNumber").val(data.PhoneNumber);
                $("#txtImage").val(data.Avatar);
                $("#txtBirthDay").val(data.BirthDay);
            },
            error: function () {
                system.notify("Có lỗi khi load dữ liệu !", "error");
            }
        });
    }
}