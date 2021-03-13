var CartController = function () {
    this.initialize = function () {
        loadData();
        registerEvent();
    }

    function registerEvent() {
        $("body").on("click",
            ".btn-delete",
            function (e) {
                e.preventDefault();
                let productId = $(this).data("id");
                $.ajax({
                    url: "/Cart/RemoveFromCart",
                    type: "POST",

                    data: {
                        productId: productId,
                    },
                    success: function () {
                        system.notify("Xóa sản phẩm khỏi giỏ hàng thành công", "success");
                        loadHeaderCart();
                        loadData();
                    },
                    error: function () {
                        system.notify("Có lỗi xảy ra!", "error");
                    }
                });
            });
        $("body").on("keyup",
            ".txtQuantity",
            function (e) {
                e.preventDefault();
                let productId = $(this).data("id");
                let quantity = $(this).val();
                if (quantity > 0) {
                    $.ajax({
                        url: "/Cart/UpdateCart",
                        type: "POST",

                        data: {
                            productId: productId,
                            quantity: quantity
                        },
                        success: function () {
                            system.notify("Cập nhật số lượng sản phẩm thành công", "success");
                            loadHeaderCart();
                            loadData();
                        }
                    });
                } else {
                    system.notify("Có lỗi xảy ra!", "error");
                }
            });
        $("#btnClearAll").on("click",
            function (e) {
                e.preventDefault();
                $.ajax({
                    url: "/Cart/ClearCart",
                    type: "POST",

                    success: function () {
                        system.notify("Xóa giỏ hàng thành công", "success");
                        loadHeaderCart();
                        loadData();
                    },
                    error: function () {
                        system.notify("Có lỗi xảy ra!", "error");
                    }
                });
            });
    }
    function loadHeaderCart() {
        $("#headerCart").load("/AjaxContent/HeaderCart");
    }

    function loadData() {
        $.ajax({
            url: "/Cart/GetCart",
            type: "GET",
            dataType: "json",

            success: function (res) {
                let template = $("#template-cart").html();
                let render = "";
                let totalAmount = 0;
                $.each(res,
                    function (i, item) {
                        render += Mustache.render(template,
                            {
                                ProductId: item.Product.Id,
                                ProductName: item.Product.Name,
                                Image: item.Product.Image,
                                Price: system.formatNumber(item.Price, 0),
                                Quantity: item.Quantity,
                                Amount: system.formatNumber(item.Price * item.Quantity, 0),
                                Url: `/Product/Detail/${item.Product.Id}`
                            });
                        totalAmount += item.Price * item.Quantity;
                    });
                $("#lbTotalAmount").text(system.formatNumber(totalAmount, 0));
                if (render !== "") {
                    $("#table-cart-content").html(render);
                } else {
                    $("#table-cart-content").html("Không có sản phẩm nào trong giỏ hàng");
                }
            }
        });
    }
}