$(document).ready(function () {

    var _urlLink;

    douoptions.addServerData = douoptions.deleteServerData = function (row, callback) {
    }

    douoptions.updateServerData = function (row, callback) {
        transactionDouClientDataToServer(row, _urlLink,
            function (result) {
                jspAlertMsg($("body"), { content: result.Desc, classes: 'modal-sm' });
            })
    }

    $("#_table").DouEditableTable(douoptions); //初始dou table

    //取消
    $('.modal-footer .btn-outline-secondary').hide();
    //確定
    $btn = $('.modal-footer .btn.btn-primary');
    $btn.hide();

    //寄發測試信件
    var $btnSend = $('<button type="button" class="btn btn-primary ">寄發測試信件</button>');
    $btnSend.appendTo($btn.parent());
    $btnSend.on("click", function () {
        _urlLink = app.siteRoot + 'TestMail/ToSendMail';
        $btn.trigger("click");
    });

    //更新Mail設定值(json)
    var $btnSaveJson = $('<button type="button" class="btn btn-secondary ">更新Mail設定值(json)</button>');
    $btnSaveJson.appendTo($btn.parent());
    $btnSaveJson.on("click", function () {
        _urlLink = app.siteRoot + 'TestMail/ToSaveMailJson';
        $btn.trigger("click");
    });
})