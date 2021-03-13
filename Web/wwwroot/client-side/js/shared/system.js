var system = {
    config: {
        pageSize: 10,
        pageIndex: 1
    },
    notify: function (message, type) {
        $.notify(message,
            {
                // whether to hide the notification on click
                clickToHide: true,
                // whether to auto-hide the notification
                autoHide: true,
                // if autoHide, hide after milliseconds
                autoHideDelay: 5000,
                // show the arrow pointing at the element
                arrowShow: true,
                // arrow size in pixels
                arrowSize: 5,
                // position defines the notification position though uses the defaults below
                position: 'right top',
                // default positions
                elementPosition: 'right top',
                globalPosition: 'right top',
                // default style
                style: 'bootstrap',
                // default class (string or [string])
                className: type,
                // show animation
                showAnimation: 'slideDown',
                // show animation duration
                showDuration: 400,
                // hide animation
                hideAnimation: 'slideUp',
                // hide animation duration
                hideDuration: 200,
                // padding between element and notification
                gap: 2
            });
    },
    confirm: function (message, okCallback) {
        bootbox.confirm({
            message: message,
            buttons: {
                confirm: {
                    label: 'Đồng ý',
                    className: 'btn-success'
                },
                cancel: {
                    label: 'Hủy bỏ',
                    className: 'btn-danger'
                }
            },
            callback: function (result) {
                if (result) {
                    okCallback();
                }
            }
        });
    },
    dateFormatJson: function (datetime) {
        if (!datetime) {
            return "";
        }
        let newdate = new Date(parseInt(datetime.substr(6)));
        let month = newdate.getMonth() + 1;
        let day = newdate.getDate();
        let year = newdate.getFullYear();
        if (month < 10)
            month = "0" + month;
        if (day < 10)
            day = "0" + day;

        return day + "/" + month + "/" + year;
    },
    dateTimeFormatJson(datetime) {
        if (!datetime)
            return '';
        let newdate = new Date(parseInt(datetime.substr(6)));
        let month = newdate.getMonth() + 1;
        let day = newdate.getDate();
        let year = newdate.getFullYear();
        let hh = newdate.getHours();
        let mm = newdate.getMinutes();
        let ss = newdate.getSeconds();
        if (month < 10)
            month = "0" + month;
        if (day < 10)
            day = "0" + day;
        if (hh < 10)
            hh = "0" + hh;
        if (mm < 10)
            mm = "0" + mm;
        if (ss < 10)
            ss = "0" + ss;
        return day + "/" + month + "/" + year + " " + hh + ":" + mm + ":" + ss;
    },
    startLoading: function () {
        if ($('.dv-loading').length > 0)
            $('.dv-loading').removeClass('hide');
    },
    stopLoading: function () {
        if ($('.dv-loading').length > 0)
            $('.dv-loading').addClass('hide');
    },
    getStatus: function (status) {
        if (status === 1) {
            return '<span class="badge bg-green">Kích hoạt</span>';
        } else {
            return '<span class="badge bg-red">Khóa</span>';
        }
    },
    formatNumber: function (number, precision) {
        if (!isFinite(number)) {
            return number.toString();
        }
        let a = number.toFixed(precision).split('.');
        a[0] = a[0].replace(/\d(?=(\d{3})+$)/g, '$&,');
        return a.join('.');
    },
    unflattern: function (arr) {
        let map = {};
        let roots = [];
        for (let i = 0; i < arr.length; i += 1) {
            let node = arr[i];
            node.children = [];
            map[node.id] = i; // use map to look-up the parents
            if (node.parentId !== null) {
                arr[map[node.parentId]].children.push(node);
            } else {
                roots.push(node);
            }
        }
        return roots;
    }
}
$(document).ajaxSend(function (e, xhr, options) {
    if (options.type.toUpperCase() === "POST" || options.type.toUpperCase() === "PUT") {
        var token = $('form').find("input[name='__RequestVerificationToken']").val();
        xhr.setRequestHeader("RequestVerificationToken", token);
    }
})