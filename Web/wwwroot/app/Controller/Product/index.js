﻿var productController = function () {
    this.initialize = function () {
        loadData(false);
        registerEvents();
    }

    function registerEvents() {
        $("#ddl-show-page").on("change",
            function () {
                system.config.pageSize = $(this).val();
                system.config.pageIndex = 1;
                loadData(true);
            });
    }

    function loadData(isPageChanged) {
        let template = $("#table-template").html();
        let render = "";
        $.ajax({
            type: "GET",
            data: {
                categoryId: null,
                keyword: $("#txt-search-keyword").val(),
                page: system.config.pageIndex,
                pageSize: system.config.pageSize
            },
            url: "/Admin/Product/GetAllPaging",
            dataType: "json",
            success: function (res) {
                console.log(res);
                $.each(res.Results,
                    function (i, item) {
                        render += Mustache.render(template,
                            {
                                Id: item.Id,
                                Name: item.Name,
                                Image: item.Image === null ? '<img src="/admin-site/images/notfound.png" width="25">' : '<img src="/admin-site/images/test.jpg" width="25">',
                                Price: system.formatNumber(item.Price, 0),
                                CategoryName: item.ProductCategory.Name,
                                CreatedDate: system.dateTimeFormatJson(item.DateCreated),
                                Status: system.getStatus(item.Status)
                            });
                        $("#lbl-total-records").text(res.RowCount);
                        if (render !== "") {
                            $("#tbl-content").html(render);
                        }
                        pagingWrapper(res.RowCount,
                            function () {
                                loadData();
                            },
                            isPageChanged);
                    });
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
}