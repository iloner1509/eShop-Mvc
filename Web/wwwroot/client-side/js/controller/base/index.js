var BaseController = function () {
    this.initialize = function () {
        registerEvent();
    }

    function registerEvent() {
        $("body").on("click",
            ".add-to-cart,.cart-button",
            function (e) {
                e.preventDefault();
                let productId = $(this).data("id");
                $.ajax({
                    url: "/Cart/AddToCart",
                    type: "POST",

                    data: {
                        productId: productId,
                        quantity: 1
                    },
                    success: function () {
                        system.notify("Thêm sản phẩm vào giỏ hàng thành công", "success");
                        loadHeaderCart();
                    },
                    error: function () {
                        system.notify("Có lỗi xảy ra!", "error");
                    }
                });
            });
        $("body").on("click",
            ".remove-cart",
            function (e) {
                e.preventDefault();
                let productId = $(this).data("id");
                $.ajax({
                    url: "/Cart/RemoveFromCart",
                    type: "POST",

                    data: {
                        productId: productId
                    },
                    success: function () {
                        system.notify("Xóa sản phẩm khỏi giỏ hàng thành công", "success");
                        loadHeaderCart();
                    }
                });
            });
    }

    function loadHeaderCart() {
        $("#headerCart").load("/AjaxContent/HeaderCart");
    }
}