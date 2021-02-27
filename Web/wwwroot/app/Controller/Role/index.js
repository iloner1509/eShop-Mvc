var RoleController = function () {
    this.initialize = function () {
        loadData();
        registerEvent();
    }

    function loadData(isPageChanged) {
        $.ajax({
            type: "GET",
            data: {
                keyword: $("#txt-search-keyword").val(),
                page: system.config.pageIndex,
                pageSize: system.config.pageSize
            },
            url: "/Admin/Role/GetAllPaging",
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
                                    Name: item.Name,
                                    Description: item.Description
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

    function registerEvent() {
        $("#frmEdit").validate({
            errorClass: "red",
            ignore: [],
            lang: "vi",
            rules: {
                txtName: { required: true }
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

                $("#modalAddEdit").modal("show");
            });
        $("body").on("click",
            ".btn-edit",
            function (e) {
                e.preventDefault();
                let userId = $(this).data("id");
                $.ajax({
                    url: "/Admin/Role/GetById",
                    type: "GET",
                    data: {
                        id: userId
                    },
                    dataType: "json",
                    success: function (res) {
                        let data = res;
                        $("#hidIdM").val(data.Id);
                        $("#txtName").val(data.Name);
                        $("#txtDescription").val(data.Description);

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
                    let name = $("#txtName").val();
                    let description = $("#txtDescription").val();

                    $.ajax({
                        url: "/Admin/Role/SaveEntity",
                        type: "POST",
                        data: {
                            Id: id,
                            Name: name,
                            Description: description
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
                            url: "/Admin/Role/Delete",
                            type: "POST",
                            data: {
                                id: userId
                            },

                            success: function () {
                                system.notify("Xóa thành công", "success");
                                loadData();
                            },
                            error: function () {
                                system.notify("Có lỗi khi xóa !", "error");
                            }
                        });
                    });
            });
        $("body").on("click",
            ".btn-grant",
            function () {
                $("#hidRoleIdM").val($(this).data("id"));
                $.when(loadFunctionList()).done(fillPermission($("#hidRoleIdM").val()));
                $("#modal-grantpermission").modal("show");
            });
        $("#btnSavePermission").off("click").on("click",
            function () {
                let listPermission = [];
                $.each($("#tblFunction tbody tr"),
                    function (i, item) {
                        listPermission.push({
                            RoleId: $("#hidRoleIdM").val(),
                            FunctionId: $(item).data("id"),
                            CanRead: $(item).find(".ckView").first().prop("checked"),
                            CanCreate: $(item).find(".ckCreate").first().prop("checked"),
                            CanUpdate: $(item).find(".ckEdit").first().prop("checked"),
                            CanDelete: $(item).find(".ckDelete").first().prop("checked")
                        });
                    });
                $.ajax({
                    type: "POST",
                    url: "/Admin/Role/SavePermission",
                    data: {
                        listPermission: listPermission,
                        roleId: $("#hidRoleIdM").val()
                    },
                    success: function () {
                        system.notify("Lưu quyền thành công", "success");
                        $("#modal-grantpermission").modal("hide");
                    },
                    error: function () {
                        system.notify("Có lỗi xảy ra khi phân quyền !", "error");
                    }
                });
            });

        function fillPermission(roleId) {
            return $.ajax({
                type: "POST",
                url: "/Admin/Role/ListAllFunction",
                dataType: "json",
                data: {
                    roleId: roleId
                },
                success: function (res) {
                    let listPermission = res;
                    $.each($("#tblFunction tbody tr"),
                        function (i, item) {
                            $.each(listPermission,
                                function (j, jitem) {
                                    if (jitem.FunctionId === $(item).data("id")) {
                                        $(item).find(".ckView").first().prop("checked", jitem.CanRead);
                                        $(item).find(".ckCreate").first().prop("checked", jitem.CanCreate);
                                        $(item).find(".ckEdit").first().prop("checked", jitem.CanUpdate);
                                        $(item).find(".ckDelete").first().prop("checked", jitem.CanDelete);
                                    }
                                });
                        });
                    if ($(".ckView:checked").length === $("#tblFunction tbody tr .ckView").length) {
                        $("#ckCheckView").prop("checked", true);
                    } else {
                        $("#ckCheckView").prop("checked", false);
                    }
                    if ($(".ckCreate:checked").length === $("#tblFunction tbody tr .ckCreate").length) {
                        $("#ckCheckCreate").prop("checked", true);
                    } else {
                        $("#ckCheckCreate").prop("checked", false);
                    }
                    if ($(".ckEdit:checked").length === $("#tblFunction tbody tr .ckEdit").length) {
                        $("#ckCheckEdit").prop("checked", true);
                    } else {
                        $("#ckCheckEdit").prop("checked", false);
                    }
                    if ($(".ckDelete:checked").length === $("#tblFunction tbody tr .ckDelete").length) {
                        $("#ckCheckDelete").prop("checked", true);
                    } else {
                        $("#ckCheckDelete").prop("checked", false);
                    }
                },
                error: function (status) {
                    console.log(status);
                }
            });
        }

        function loadFunctionList(callback) {
            return $.ajax({
                type: "GET",
                url: "/Admin/Function/GetAll",
                dataType: "json",
                success: function (res) {
                    let template = $("#result-data-function").html();
                    let render = "";
                    $.each(res,
                        function (i, item) {
                            render += Mustache.render(template,
                                {
                                    Name: item.Name,
                                    treegridparent: item.ParentId != null ? "treegrid-parent-" + item.ParentId : "",
                                    Id: item.Id,
                                    AllowCreate: item.AllowCreate ? "checked" : "",
                                    AllowEdit: item.AllowEdit ? "checked" : "",
                                    AllowView: item.AllowView ? "checked" : "",
                                    AllowDelete: item.AllowDelete ? "checked" : "",
                                    Status: system.getStatus(item.Status)
                                });
                            if (render !== "") {
                                $("#list-data-function").html(render);
                            }
                            $(".tree").treegrid();
                            $("#ckCheckView").on("click",
                                function () {
                                    $(".ckView").prop("checked", $(this).prop("checked"));
                                });
                            $("#ckCheckCreate").on("click",
                                function () {
                                    $(".ckCreate").prop("checked", $(this).prop("checked"));
                                });
                            $("#ckCheckEdit").on("click",
                                function () {
                                    $(".ckEdit").prop("checked", $(this).prop("checked"));
                                });
                            $("#ckCheckDelete").on("click",
                                function () {
                                    $(".ckDelete").prop("checked", $(this).prop("checked"));
                                });
                            $(".ckView").on("click",
                                function () {
                                    if ($(".ckView:checked").length === res.length) {
                                        $("#ckCheckView").prop("checked", true);
                                    } else {
                                        $("#ckCheckView").prop("checked", false);
                                    }
                                });
                            $(".ckEdit").on("click",
                                function () {
                                    if ($(".ckEdit:checked").length === res.length) {
                                        $("#ckCheckEdit").prop("checked", true);
                                    } else {
                                        $("#ckCheckEdit").prop("checked", false);
                                    }
                                });
                            $(".ckCreate").on("click",
                                function () {
                                    if ($(".ckCreate:checked").length === res.length) {
                                        $("#ckCheckCreate").prop("checked", true);
                                    } else {
                                        $("#ckCheckCreate").prop("checked", false);
                                    }
                                });
                            $(".ckDelete").on("click",
                                function () {
                                    if ($(".ckDelete:checked").length === res.length) {
                                        $("#ckCheckDelete").prop("checked", true);
                                    } else {
                                        $("#ckCheckDelete").prop("checked", false);
                                    }
                                });
                            if (callback !== undefined) {
                                callback();
                            }
                        });
                },
                error: function (status) {
                    console.log(status);
                }
            });
        }

        function resetForm() {
            $("#hidIdM").val("");
            $("#txtName").val("");
            $("#txtDescription").val("");
        }
    }
}