var productCategoryController = function () {
    this.initialize = function () {
        loadData();
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