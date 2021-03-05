var BillController = function () {
    var cachedObject = {
        products: [],
        paymentMethods: [],
        billStatuses: []
    }
    this.initialize = function () {
        $.when(loadBillStatus(),
            loadPaymentMethod(),
            loadProduct()).done(function () {
                loadData();
            });
        registerEvent();
    }

    function registerEvent() {
        $("#txtFromDate,#txtToDate").datepicker({
            autoclose: true,
            format: "dd/mm/yyyy"
        });
        $("#frmDetail").validate({
            errorClass: "red",
            ignore: [],
            lang: "vi",
            rules: {
                txtCustomerName: { required: true },
                txtCustomerAddress: { required: true },
                txtCustomerMobile: { required: true },
                txtCustomerMessage: { required: true },
                ddlBillStatus: { required: true },
                ddlPaymentMethod: { required: true }
            }
        });
        $("#txt-search-keyword").on("keypress",
            function (e) {
                if (e.which === 13) {
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
        $("#btnCreateBill").on("click",
            function () {
                resetForm();
                $("#modal-detail").modal("show");
            });
        $("body").on("click",
            ".btn-view",
            function (e) {
                e.preventDefault();
                let billId = $(this).data("id");
                $.ajax({
                    type: "GET",
                    url: "/Admin/Bill/GetById",
                    data: {
                        id: billId
                    },

                    success: function (res) {
                        $("#hidIdM").val(res.Id);
                        $("#txtCustomerName").val(res.CustomerName);
                        $("#txtCustomerAddress").val(res.CustomerAddress);
                        $("#txtCustomerMobile").val(res.CustomerMobile);
                        $("#txtCustomerMessage").val(res.CustomerMessage);
                        $("#ddlPaymentMethod").val(res.PaymentMethod);
                        $("#ddlBillStatus").val(res.BillStatus);

                        var billDetail = res.BillDetails;
                        if (res.BillDetails !== null && res.BillDetails.length > 0) {
                            var render = "";
                            var templateDetails = $("#bill-detail-template").html();
                            $.each(billDetail,
                                function (i, item) {
                                    var products = getProductOptions(item.ProductId);
                                    render += Mustache.render(templateDetails,
                                        {
                                            Id: item.Id,
                                            Products: products,
                                            Quantity: item.Quantity
                                        });
                                });
                            $("#tbl-bill-detail").html(render);
                        }
                        $("#modal-detail").modal("show");
                    },
                    error: function () {
                        system.notify("Có lỗi xảy ra!", "error");
                    }
                });
            });
        $("#btnSave").on("click",
            function (e) {
                if ($("#frmDetail").valid()) {
                    e.preventDefault();
                    var id = $("#hidIdM").val();
                    var customerName = $("#txtCustomerName").val();
                    var customerAddress = $("#txtCustomerAddress").val();
                    var customerMobile = $("#txtCustomerMobile").val();
                    var customerMessage = $("#txtCustomerMessage").val();
                    var paymentMethod = $("#ddlPaymentMethod").val();
                    var billStatus = $("#ddlBillStatus").val();

                    var billDetails = [];
                    $.each($("#tbl-bill-detail tr"),
                        function (i, item) {
                            billDetails.push({
                                Id: $(item).data("id"),
                                ProductId: $(item).find("select.ddlProductId").first().val(),
                                Quantity: $(item).find("input.txtQuantity").first().val(),
                                BillId: id
                            });
                        });
                    $.ajax({
                        type: "POST",
                        url: "/Admin/Bill/SaveEntity",
                        data: {
                            Id: id,
                            CustomerName: customerName,
                            CustomerAddress: customerAddress,
                            CustomerMobile: customerMobile,
                            CustomerMessage: customerMessage,
                            PaymentMethod: paymentMethod,
                            BillStatus: billStatus,
                            BillDetails: billDetails,
                            Status: 1
                        },
                        dataType: "json",
                        success: function (res) {
                            system.notify("Cập nhật đơn hàng thành công", "success");
                            $("#modal-detail").modal("hide");
                            resetForm();
                            loadData(true);
                        },
                        error: function () {
                            system.notify("Có lỗi xảy ra!", "error");
                        }
                    });
                }
            });
        $("#btnAddDetail").on("click",
            function () {
                var template = $("#bill-detail-template").html();
                var products = getProductOptions(null);
                var render = Mustache.render(template,
                    {
                        Id: 0,
                        Products: products,
                        Quantity: 0,
                        Total: 0
                    });
                $("#tbl-bill-detail").append(render);
            });
        $("body").on("click",
            ".btn-delete-detail",
            function () {
                $(this).parent().parent().remove();
            });
        $("#btnExport").on("click",
            function () {
                let billId = $("#hidIdM").val();
                $.ajax({
                    type: "POST",
                    url: "/Admin/Bill/ExportExcel",
                    data: {
                        billId: billId
                    },

                    success: function (res) {
                        window.location.href = res;
                    },
                    error: function () {
                        system.notify("Có lỗi xảy ra!", "error");
                    }
                });
            });
    }

    function resetForm() {
        $("#hidIdM").val(0);
        $("#txtCustomerName").val("");

        $("#txtCustomerAddress").val("");
        $("#txtCustomerMessage").val("");
        $("#txtCustomerMobile").val("");
        $("#ddlPaymentMethod").val("");
        $("#ddlBillStatus").val("");
        $("#tbl-bill-detail").html("");
    }

    function loadProduct() {
        return $.ajax({
            type: "GET",
            url: "/Admin/Product/GetAll",
            dataType: "json",
            success: function (res) {
                cachedObject.products = res;
            },
            error: function () {
                system.notify("Có lỗi khi load sản phẩm", "error");
            }
        });
    }

    function loadBillStatus() {
        return $.ajax({
            type: "GET",
            url: "/Admin/Bill/GetBillStatus",
            dataType: "json",
            success: function (res) {
                cachedObject.billStatuses = res;
                var render = "";
                $.each(res,
                    function (i, item) {
                        render += `<option value="${item.Value}">${item.Name}</option>`;
                    });
                $("#ddlBillStatus").html(render);
            },
            error: function () {
                system.notify("Có lỗi khi load trạng thái đơn hàng", "error");
            }
        });
    }

    function loadPaymentMethod() {
        return $.ajax({
            type: "GET",
            url: "/Admin/Bill/GetPaymentMethod",
            dataType: "json",
            success: function (res) {
                cachedObject.paymentMethods = res;
                var render = "";
                $.each(res,
                    function (i, item) {
                        render += `<option value="${item.Value}">${item.Name}</option>`;
                    });
                $("#ddlPaymentMethod").html(render);
            },
            error: function () {
                system.notify("Có lỗi khi load trạng thái đơn hàng", "error");
            }
        });
    }
    function getProductOptions(selectedId) {
        var products = "<select class='form-control ddlProductId'>";
        $.each(cachedObject.products,
            function (i, product) {
                if (selectedId === product.Id) {
                    products += `<option value="${product.Id}" selected="select">${product.Name}</option>`;
                } else {
                    products += `<option value="${product.Id}" >${product.Name}</option>`;;
                }
            });
        products += "</select>";
        return products;
    }

    function getPaymentMethodName(paymentMethod) {
        var method = $.grep(cachedObject.paymentMethods,
            function (e, i) {
                return e.Value === paymentMethod;
            });
        if (method.length > 0) {
            return method[0].Name;
        } else {
            return "";
        }
    }
    function getBillStatusName(billStatus) {
        var status = $.grep(cachedObject.billStatuses,
            function (e, i) {
                return e.Value === billStatus;
            });
        if (status.length > 0) {
            return status[0].Name;
        } else {
            return "";
        }
    }

    function loadData(isPageChanged) {
        let template = $("#table-template").html();
        let render = "";
        $.ajax({
            type: "GET",
            data: {
                startDate: $("#txtFromDate").val(),
                endDate: $("#txtToDate").val(),
                keyword: $("#txt-search-keyword").val(),
                page: system.config.pageIndex,
                pageSize: system.config.pageSize
            },
            url: "/Admin/Bill/GetAllPaging",
            dataType: "json",
            success: function (res) {
                //console.log(res);
                if (res.RowCount > 0) {
                    $.each(res.Results,
                        function (i, item) {
                            render += Mustache.render(template,
                                {
                                    CustomerName: item.CustomerName,

                                    Id: item.Id,
                                    PaymentMethod: getPaymentMethodName(item.PaymentMethod),
                                    BillStatus: getBillStatusName(item.BillStatus),
                                    CreatedDate: system.dateTimeFormatJson(item.DateCreated),
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
                } else {
                    $("#lbl-total-records").text("0");
                    $("#tbl-content").html("");
                }
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