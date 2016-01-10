function Common() {
    _this = this;

    this.init = function () {
        $("#LoginPopup").click(function () {
            _this.showPopup("/Login/Ajax", initLoginPopup);
        });
    }

    this.showPopup = function (url, callback)
    {
        $.ajax({
            type : "GET",
            url: url,
            success: function (data)
            {
                showModalData(data, callback);
            }
        });
    }

    function initLoginPopup(modal) {
        $("#LoginButton").click(function () {
            $.ajax({
                type: "POST",
                url: "/Login/Ajax",
                data : $("#LoginForm").serialize(),
                success: function (data) {
                    showModalData(data);
                    initLoginPopup(modal);
                }
            });
        });
    }

    function showModalData(data, callback) {
        $(".modal-backdrop").remove();
        var popupWrapper = $("#PopupWrapper");
        popupWrapper.empty();
        popupWrapper.html(data);
        var popup = $(".modal", popupWrapper);
        $(".modal", popupWrapper).modal();
        if (callback != undefined) {
            callback(popup);
        }
    }
}

var common = null;
$().ready(function () {
    common = new Common();
    common.init();
});