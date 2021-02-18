var productCategoryController = function () {
    this.initialize = function () {
        loadData();
        registerEvent();
    }

    function registerEvent() {
        $("#frmEdit").validate({
            errorClass: "red",
            ignore: [],
            lang: "vi",
            rules: {
                txtName: { required: true },
                txtOrder: { number: true },
                txtHomeOrder: { number: true }
            }
        });
        $("#btnCreateCategory").off("click").on("click",
            function () {
                initDropDownTree();
                $("#modalAddEdit").modal("show");
            });
        $("body").on("click",
            "#btnEdit",
            function (e) {
                e.preventDefault();
                let contextMenuId = $("#hidIdM").val();

                $.ajax({
                    type: "GET",
                    url: "/Admin/ProductCategory/GetById",
                    data: { id: contextMenuId },
                    dataType: "json",
                    beforeSend: function () {
                        system.startLoading();
                    },
                    success: function (res) {
                        let data = res;
                        $("#hidIdM").val(data.Id);
                        $("#txtName").val(data.Name);
                        initDropDownTree(data.CategoryId);
                        $("#txtDescription").val(data.Description);
                        $("#txtThumbnailImg").val(data.ThumbnailImage);
                        $("#txtSeoKeyword").val(data.SeoKeyword);
                        $("#txtSeoDescription").val(data.SeoDescription);
                        $("#txtSeoPageTitle").val(data.SeoPageTitle);
                        $("#txtSeoAlias").val(data.SeoAlias);
                        $("#ckStatus").prop("checked", data.Status === 1);
                        $("#ckShowHome").prop("checked", data.HomeFlag);
                        $("#txtOrder").val(data.SortOrder);
                        $("#txtHomeOrder").val(data.HomeOrder);

                        $("#modalAddEdit").modal("show");
                        system.stopLoading();
                    },
                    error: function () {
                        system.notify("Lỗi", "error");
                        system.stopLoading();
                    }
                });
            });
        $("body").on("click",
            "#btnDelete",
            function (e) {
                e.preventDefault();
                let contextMenuId = $("#hidIdM").val();
                system.confirm("Bạn có muốn xóa ?",
                    function () {
                        $.ajax({
                            type: "POST",
                            url: "/Admin/ProductCategory/Delete",
                            data: { id: contextMenuId },
                            dataType: "json",
                            beforeSend: function () {
                                system.startLoading();
                            },
                            success: function () {
                                system.notify("Xóa thành công", "success");
                                system.stopLoading();
                                loadData();
                            },
                            error: function () {
                                system.notify("Xóa không thành công", "error");
                                system.stopLoading();
                            }
                        });
                    });
            });
        $("#btnSave").on("click",
            function (e) {
                if ($("#frmEdit").valid()) {
                    e.preventDefault();
                    var id = $("#hidIdM").val();
                    var name = $("#txtName").val();
                    var parentId = $("#ddlCategoryId").combotree("getValue");
                    var description = $("#txtDescription").val();
                    var image = $("#txtImage").val();
                    var order = $("#txtOrder").val();
                    var homeOrder = $("#txtHomeOrder").val();
                    var seoKeyword = $("#txtSeoKeyword").val();
                    var seoDescription = $("#txtSeoDescription").val();
                    var seoTitle = $("#txtSeoTitle").val();
                    var seoAlias = $("#txtSeoAlias").val();
                    var status = $("#ckStatus").prop("checked") === true ? 1 : 0;
                    var showHome = $("#ckShowHome").prop("checked");
                    $.ajax({
                        url: "/Admin/ProductCategory/SaveEntity",
                        type: "POST",
                        dataType: "json",
                        data: {
                            Id: id,
                            Name: name,
                            ParentId: parentId,
                            Description: description,
                            HomeOrder: homeOrder,
                            SortOrder: order,
                            Image: image,
                            seoKeyword: seoKeyword,
                            Status: status,
                            HomeFlag: showHome,
                            SeoDescription: seoDescription,
                            SeoTitle: seoTitle,
                            SeoAlias: seoAlias
                        },
                        beforeSend: function () {
                            system.startLoading();
                        },
                        success: function () {
                            system.notify("Cập nhật thành công.", "success");
                            $("#modalAddEdit").modal("hide");

                            resetForm();
                            system.stopLoading();
                            loadData();
                        },
                        error: function () {
                            system.notify("Có lỗi xảy ra !", "error");
                            system.stopLoading();
                        }
                    });
                }
                return false;
            });
    }

    function resetForm() {
        $("#hidIdM").val(0);
        $("#txtName").val("");
        initDropDownTree("");

        $("#txtDescription").val("");
        $("#txtImage").val("");
        $("#txtOrder").val("");
        $("#txtHomeOrder").val("");
        $("#txtSeoKeyword").val("");
        $("#txtSeoDescription").val("");
        $("#txtSeoTitle").val("");
        $("#txtSeoAlias").val("");
        $("#ckStatus").prop("checked", true);
        $("#ckShowHome").prop("checked", false);
    }
    function initDropDownTree(selectedId) {
        $.ajax({
            url: "/Admin/ProductCategory/GetAll",
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
                if (selectedId != undefined) {
                    $("#ddlCategoryId").combotree("setValue", selectedId);
                }
            }
        });
    }
    function loadData() {
        $.ajax({
            url: "/Admin/ProductCategory/GetAll",
            dataType: "json",
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
                let treeArr = system.unflattern(data);
                treeArr.sort(function (a, b) {
                    return a.sortOrder - b.sortOrder;
                });
                $("#treeProductCategory").tree({
                    data: treeArr,
                    dnd: true,
                    onContextMenu: function (e, node) {
                        e.preventDefault();
                        $("#hidIdM").val(node.id);
                        // show context menu
                        $("#contextMenu").menu("show",
                            {
                                left: e.pageX,
                                top: e.pageY
                            });
                    },
                    onDrop: function (target, source, point) {
                        console.log(target);
                        console.log(source);
                        console.log(point);
                        let targetNode = $(this).tree("getNode", target);
                        if (point === "append") {
                            let child = [];
                            $.each(targetNode.children,
                                function (i, item) {
                                    child.push({
                                        key: item.id,
                                        value: i
                                    });
                                });
                            // update database
                            $.ajax({
                                url: "/Admin/ProductCategory/UpdateParentId",
                                type: "POST",
                                dataType: "json",
                                data: {
                                    sourceId: source.id,
                                    targetId: targetNode.id,
                                    items: child
                                },
                                success: function () {
                                    loadData();
                                }
                            });
                        }
                        else if (point === "top" || point === "bottom") {
                            $.ajax({
                                url: "/Admin/ProductCategory/ReOrder",
                                type: "POST",
                                dataType: "json",
                                data: {
                                    sourceId: source.id,
                                    targetId: targetNode.id
                                },
                                success: function () {
                                    loadData();
                                }
                            });
                        }
                    }
                });
            }
        });
    }
}