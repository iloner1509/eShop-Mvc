var productController = function () {
    this.initialize = function () {
        loadCategories();
        loadData(false);
        registerEvents();
        registerControls();
    }

    function registerEvents() {
        $("#frmEdit").validate({
            errorClass: "red",
            ignore: [],
            lang: "vi",
            rules: {
                txtName: { required: true },
                ddlCategoryId: { required: true },
                txtPrice: {
                    required: true,
                    number: true
                },
                txtOriginalPrice: { number: true },
                txtPromotionPrice: { number: true }
            }
        });
        $("#ddl-show-page").on("change",
            function () {
                system.config.pageSize = $(this).val();
                system.config.pageIndex = 1;
                loadData(true);
            });
        $("#btnSearch").on("click",
            function () {
                loadData();
            });
        $("#txt-search-keyword").on("keypress",
            function (e) {
                if (e.which === 13) {
                    loadData();
                }
            });
        $("#btnCreateProduct").on("click",
            function () {
                resetForm();

                initDropDownTree();
                $("#modalAddEdit").modal("show");
            });
        $("body").on("click",
            ".btn-edit",
            function (e) {
                e.preventDefault();
                let productId = $(this).data("id");
                loadDetails(productId);
            });
        $("body").on("click",
            ".btn-delete",
            function (e) {
                e.preventDefault();
                let productId = $(this).data("id");
                deleteProduct(productId);
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
        $("#btnSave").on("click",
            function (e) {
                if ($("#frmEdit").valid()) {
                    e.preventDefault();
                    var id = $("#hidIdM").val();
                    var categoryId = $("#ddlCategoryId").combotree("getValue");
                    var name = $("#txtName").val();
                    var description = $("#txtDescription").val();
                    var unit = $("#txtUnit").val();
                    var price = $("#txtPrice").val();
                    var originalPrice = $("#txtOriginalPrice").val();
                    var promotionPrice = $("#txtPromotionPrice").val();
                    var image = $("#txtImage").val();
                    var tags = $("#txtTag").val();
                    var seoKeyword = $("#txtSeoKeyword").val();
                    var seoDescription = $("#txtSeoDescription").val();
                    var seoTitle = $("#txtSeoTitle").val();
                    var seoAlias = $("#txtSeoAlias").val();
                    var content = CKEDITOR.instances.txtContent.getData();
                    var status = $("#ckStatus").prop("checked") === true ? 1 : 0;
                    var hot = $("#ckHot").prop("checked");
                    var showHome = $("#ckShowHome").prop("checked");

                    $.ajax({
                        type: "POST",
                        url: "/Admin/Product/SaveEntity",
                        data: {
                            Id: id,
                            Name: name,
                            CategoryId: categoryId,
                            Description: description,
                            Unit: unit,
                            Price: price,
                            OriginalPrice: originalPrice,
                            PromotionPrice: promotionPrice,
                            Image: image,
                            Tags: tags,
                            SeoKeywords: seoKeyword,
                            SeoDescription: seoDescription,
                            SeoTitle: seoTitle,
                            SeoAlias: seoAlias,
                            Status: status,
                            HomeFlag: showHome,
                            HotFlag: hot,
                            Content: content
                        },
                        dataType: "json",
                        success: function () {
                            system.notify("Cập nhật thành công.", "success");
                            $("#modalAddEdit").modal("hide");
                            resetForm();
                            loadData(true);
                        },
                        error: function () {
                            system.notify("Xảy ra lỗi khi cập nhật sản phẩm !", "error");
                        }
                    });
                }
            });
        $("#btn-import").on("click",
            function () {
                initDropDownTree();
                $("#importExcelModal").modal("show");
            });
        $("#btnImportExcel").on("click",
            function () {
                var uploadedFiles = $("#choseFile").get(0).files;
                // Create form data object
                var fileData = new FormData();
                for (var i = 0; i < uploadedFiles.length; i++) {
                    fileData.append("files", uploadedFiles[i]);
                }
                fileData.append("categoryId", $("#ddlImportCategoryId").combotree("getValue"));
                $.ajax({
                    url: "/Admin/Product/ImportExcel",
                    type: "POST",
                    data: fileData,
                    processData: false,
                    contentType: false,

                    success: function () {
                        system.notify("Cập nhật sản phẩm từ file thành công", "success");
                        $("#importExcelModal").modal("hide");
                        loadData();
                    }
                });
            });
        $("#btn-export").on("click",
            function () {
                $.ajax({
                    url: "/Admin/Product/ExportExcel",
                    type: "POST",

                    success: function (res) {
                        system.notify("Xuất file thành công", "success");
                        window.location.href = res;
                    },
                    error: function () {
                        system.notify("Có lỗi khi xuất file", "error");
                    }
                });
            });
    }

    function registerControls() {
        CKEDITOR.replace("txtContent",
            {
                filebrowserUploadUrl: "/Admin/Upload/UploadImageForCkEditor",
                filebrowserUploadMethod: "form"
            });
        //Fix: cannot click on element ck in modal
        $.fn.modal.Constructor.prototype.enforceFocus = function () {
            $(document)
                .off('focusin.bs.modal') // guard against infinite focus loop
                .on('focusin.bs.modal', $.proxy(function (e) {
                    if (
                        this.$element[0] !== e.target && !this.$element.has(e.target).length
                        // CKEditor compatibility fix start.
                        && !$(e.target).closest('.cke_dialog, .cke').length
                        // CKEditor compatibility fix end.
                    ) {
                        this.$element.trigger('focus');
                    }
                }, this));
        };
    }
    function deleteProduct(productId) {
        system.confirm("Bạn có muốn xóa ?",
            function () {
                $.ajax({
                    url: "/Admin/Product/Delete",
                    type: "POST",
                    data: {
                        id: productId
                    },
                    dataType: "json",
                    success: function () {
                        loadData(true);
                        system.notify("Xóa thành công", "success");
                    },
                    error: function () {
                        system.notify("Có lỗi khi xóa sản phẩm !", "error");
                    }
                });
            });
    }
    function loadDetails(productId) {
        $.ajax({
            url: "/Admin/Product/GetById",
            type: "GET",
            data: {
                id: productId
            },
            dataType: "json",
            success: function (res) {
                let data = res;
                $("#hidIdM").val(data.Id);
                $("#txtName").val(data.Name);
                initDropDownTree(data.CategoryId);
                $("#txtDescription").val(data.Description);
                $("#txtUnit").val(data.Unit);
                $("#txtPrice").val(data.Price);
                $("#txtOriginalPrice").val(data.OriginalPrice);
                $("#txtPromotionPrice").val(data.PromotionPrice);
                $("#txtImage").val(data.Image);
                $("#txtTag").val(data.Tags);
                $("#txtSeoKeyword").val(data.SeoKeywords);
                $("#txtSeoDescription").val(data.SeoDescription);
                $("#txtSeoAlias").val(data.SeoAlias);
                $("#txtSeoTitle").val(data.SeoTitle);
                CKEDITOR.instances.txtContent.setData(data.Content);

                $("#ckStatus").prop("checked", data.Status === 1);
                $("#ckHot").prop("checked", data.HotFlag);
                $("#ckShowHome").prop("checked", data.HomeFlag);

                $("#modalAddEdit").modal("show");
            },
            error: function () {
                system.notify("Có lỗi khi truy cập dữ liệu sản phẩm", "error");
            }
        });
    }
    function resetForm() {
        $("#hidIdM").val(0);
        $("#txtName").val("");
        initDropDownTree("");
        CKEDITOR.instances.txtContent.setData("");
        $("#txtDescription").val("");
        $("#txtUnit").val("");
        $("#txtImage").val("");
        $("#txtPrice").val("");
        $("#txtOrginalPrice").val("");
        $("#txtPromotionPrice").val("");
        $("#txtTag").val("");
        $("#txtSeoKeyword").val("");
        $("#txtSeoDescription").val("");
        $("#txtSeoTitle").val("");
        $("#txtSeoAlias").val("");
        $("#ckStatus").prop("checked", true);
        $("#ckHot").prop("checked", false);
        $("#ckShowHome").prop("checked", false);
    }
    function initDropDownTree(selectedId) {
        $.ajax({
            url: "/Admin/Product/GetAllCategories",
            type: "GET",
            dataType: "json",
            async: false,
            success: function (res) {
                let data = [];
                $.each(res,
                    function (i, item) {
                        data.push({
                            id: item.Id,
                            text: item.Name,
                            parentId: item.ParentId,
                            sortOrder: item.SortOrder
                        });
                    });
                let arr = system.unflattern(data);
                $("#ddlCategoryId").combotree({
                    data: arr
                });
                $("#ddlImportCategoryId").combotree({
                    data: arr
                });
                if (selectedId != undefined) {
                    $("#ddlCategoryId").combotree("setValue", selectedId);
                }
            }
        });
    }
    function loadCategories() {
        $.ajax({
            type: "GET",
            url: "/Admin/Product/GetAllCategories",
            dataType: "json",
            success: function (res) {
                let render = "<option value=''>Chọn danh mục</option>";
                $.each(res,
                    function (i, item) {
                        render += "<option value='" + item.Id + "'>" + item.Name + "</option>";
                    });
                $("#dllCategory").html(render);
            },
            error: function (status) {
                console.log(status);
                system.notify("Không load được dữ liệu danh mục !", "error");
            }
        });
    }
    function loadData(isPageChanged) {
        let template = $("#table-template").html();
        let render = "";
        $.ajax({
            type: "GET",
            data: {
                categoryId: $("#dllCategory").val(),
                keyword: $("#txt-search-keyword").val(),
                page: system.config.pageIndex,
                pageSize: system.config.pageSize
            },
            url: "/Admin/Product/GetAllPaging",
            dataType: "json",
            success: function (res) {
                //console.log(res);
                $.each(res.Results,
                    function (i, item) {
                        render += Mustache.render(template,
                            {
                                Id: item.Id,
                                Name: item.Name,
                                Image: item.Image === null ? '<img src="/admin-site/images/test.jpg" width="25">' : `<img src="${item.Image}" width="25">`,
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