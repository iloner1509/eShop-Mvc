var UserController = function () {
    this.initialize = function () {
        loadData();
        registerEvent();
    };

    function registerEvent() {
        $("#frmEdit").validate({
            errorClass: "red",
            ignore: [],
            lang: "vi",
            rules: {
                txtFullName: { required: true },
                txtUserName: { required: true },
                txtPassword: {
                    required: true,
                    minlength: 6
                },
                txtConfirmPassword: {
                    equalTo: "#txtPassword"
                },
                txtEmail: {
                    required: true,
                    email: true
                }
            }
        });
        $("#txt-search-keyword").on("keypress",
            function (e) {
                if (e.which === 13) {
                    e.preventDefault();
                    loadData();
                }
            });
        $("#btnSearch").on("click",
            function () {
                loadData();
            });
        $("#ddl-show-page").on("change",
            function () {
                system.config.pageSize = $(this).val();
                system.config.pageIndex = 1;
                loadData(true);
            });
        $("#btnCreateUser").on("click",
            function () {
                resetForm();
                initRole();
                $("#modalAddEdit").modal("show");
            });
        $("body").on("click",
            ".btn-edit",
            function (e) {
                e.preventDefault();
                let userId = $(this).data("id");
                $.ajax({
                    url: "/Admin/User/GetById",
                    type: "GET",
                    data: {
                        id: userId
                    },
                    dataType: "json",
                    success: function (res) {
                        let data = res;
                        $("#hidIdM").val(data.Id);
                        $("#txtFullName").val(data.FullName);
                        $("#txtUserName").val(data.UserName);
                        $("#txtEmail").val(data.Email);
                        $("#txtPhoneNumber").val(data.PhoneNumber);

                        $("#ckStatus").prop("checked", data.Status === 1);
                        initRole(data.Roles);
                        disableField(true);
                        $("#modalAddEdit").modal("show");
                    },
                    error: function () {
                        system.notify("Có lỗi khi load dữ liệu !", "error");
                    }
                });
            });
        $("#btnSave").on("click",
            function (e) {
                if ($("#frmEdit").valid()) {
                    e.preventDefault();
                    let id = $("#hidIdM").val();
                    let fullName = $("#txtFullName").val();
                    let userName = $("#txtUserName").val();
                    let email = $("#txtEmail").val();
                    let phoneNumber = $("#txtPhoneNumber").val();
                    let password = $("#txtPassword").val();
                    let roles = [];
                    $.each($('input[name="ckRoles"]'),
                        function (i, item) {
                            if ($(item).prop("checked") === true) {
                                roles.push($(item).prop("value"));
                            }
                        });
                    let status = $("#ckStatus").prop("checked") === true ? 1 : 0;
                    $.ajax({
                        url: "/Admin/User/SaveEntity",
                        type: "POST",
                        data: {
                            Id: id,
                            FullName: fullName,
                            UserName: userName,
                            Password: password,
                            Email: email,
                            PhoneNumber: phoneNumber,
                            Status: status,
                            Roles: roles
                        },
                        dataType: "json",
                        success: function () {
                            system.notify("Cập nhật dữ liệu thành công", "success");
                            $("#modalAddEdit").modal("hide");
                            resetForm();
                            loadData(true);
                        },
                        error: function () {
                            system.notify("Có lỗi khi cập nhật dữ liệu !", "error");
                        }
                    });
                }
            });
        $("body").on("click",
            ".btn-delete",
            function (e) {
                e.preventDefault();
                let userId = $(this).data("id");
                system.confirm("Bạn có muốn xóa ?",
                    function () {
                        $.ajax({
                            url: "/Admin/User/Delete",
                            type: "POST",
                            data: {
                                id: userId
                            },

                            success: function () {
                                system.notify("Xóa thành công", "success");
                                loadData(true);
                            },
                            error: function () {
                                system.notify("Có lỗi khi xóa !", "error");
                            }
                        });
                    });
            });
    }

    function disableField(isDisable) {
        $("#txtUserName").prop("disabled", isDisable);
        $("#txtPassword").prop("disabled", isDisable);
        $("#txtConfirmPassword").prop("disabled", isDisable);
    }

    function resetForm() {
        disableField(false);
        $("#hidIdM").val("");
        $("#txtFullName").val("");
        initRole();
        $("#txtUserName").val("");
        $("#txtPassword").val("");
        $("#txtConfirmPassword").val("");
        $("#txtEmail").val("");
        $("#txtPhoneNumber").val("");
        $('input[name="ckRoles"]').removeAttr("checked");
        $("#ckStatus").prop("checked", true);
    }
    function loadData(isPageChanged) {
        $.ajax({
            type: "GET",
            data: {
                categoryId: $("#dllCategory").val(),
                keyword: $("#txt-search-keyword").val(),
                page: system.config.pageIndex,
                pageSize: system.config.pageSize
            },
            url: "/Admin/User/GetAllPaging",
            dataType: "json",
            success: function (res) {
                //console.log(res);
                if (res.RowCount > 0) {
                    let template = $("#table-template").html();
                    let render = "";
                    $.each(res.Results,
                        function (i, item) {
                            render += Mustache.render(template,
                                {
                                    Id: item.Id,
                                    UserName: item.UserName,
                                    FullName: item.FullName,

                                    Avatar: item.Avatar === null ? '<img src="/admin-site/images/notfound.png" width="25">' : '<img src="/admin-site/images/test.jpg" width="25">',

                                    DateCreated: system.dateTimeFormatJson(item.DateCreated),
                                    Status: system.getStatus(item.Status)
                                });
                            if (render !== "") {
                                $("#tbl-content").html(render);
                            }
                            pagingWrapper(res.RowCount,
                                function () {
                                    loadData();
                                },
                                isPageChanged);
                        });
                } else {
                    $("#tbl-content").html("");
                    //pagingWrapper(res.RowCount,
                    //    function () {
                    //        loadData();
                    //    },
                    //    true);
                }
                $("#lbl-total-records").text(res.RowCount);
            },
            error: function (status) {
                console.log(status);
                system.notify("Không nạp được dữ liệu", "error");
            }
        });
    }

    function pagingWrapper(recordCount, callBack, changePageSize) {
        let totalSize = Math.ceil(recordCount / system.config.pageSize);
        // unbind pagination if existed or change page size was clicked
        if ($("#paginationUL a").length === 0 || changePageSize === true) {
            $("#paginationUL").empty();
            $("#paginationUL").removeData("twbs-pagination");
            $("#paginationUL").unbind("page");
        }
        // bind pagination
        $("#paginationUL").twbsPagination({
            totalPages: totalSize,
            visiblePages: 10,
            first: "Đầu",
            prev: "Trước",
            next: "Tiếp",
            last: "Cuối",
            onPageClick: function (event, page) {
                system.config.pageIndex = page;
                setTimeout(callBack(), 200);
            }
        });
    }

    function initRole(selectedRoles) {
        $.ajax({
            url: "/Admin/Role/GetAll",
            type: "GET",
            dataType: "json",

            success: function (res) {
                let template = $("#role-template").html();
                let data = res;
                let render = "";
                $.each(data,
                    function (i, item) {
                        let checked = "";
                        if (selectedRoles !== undefined && selectedRoles.indexOf(item.Name) !== -1) {
                            checked = "checked";
                        }
                        render += Mustache.render(template,
                            {
                                Name: item.Name,
                                Description: item.Description,
                                Checked: checked
                            });
                    });
                $("#list-roles").html(render);
            }
        });
    }
}