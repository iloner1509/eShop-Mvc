var ProductDetailController = function () {
    this.initialize = function () {
        registerEvent();
    }

    function registerEvent() {
        $("#btnAddToCart").on("click",
            function (e) {
                e.preventDefault();
                var id = parseInt($(this).data("id"));
                $.ajax({
                    url: "/Cart/AddToCart",
                    type: "POST",
                    dataType: "json",
                    data: {
                        productId: id,
                        quantity: parseInt($("#txtQuantity").val())
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
    }

    function loadHeaderCart() {
        $("#headerCart").load("/AjaxContent/HeaderCart");
    }
}