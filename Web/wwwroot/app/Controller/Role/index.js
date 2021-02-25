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
    }

    function resetForm() {
        $("#hidIdM").val("");
        $("#txtName").val("");
        $("#txtDescription").val("");
    }
}