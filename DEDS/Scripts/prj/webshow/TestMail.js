$(document).ready(function () {

    douoptions.addServerData = douoptions.deleteServerData = function (row, callback) {
    }

    douoptions.updateServerData = function (row, callback) {
        transactionDouClientDataToServer(row, app.siteRoot + 'TestMail/SaveMailJson',
            function (result) {
                jspAlertMsg($("body"), { content: result.Desc, classes: 'modal-sm' });
            })
    }

    $("#_table").DouEditableTable(douoptions); //初始dou table

    $('.modal-footer .btn.btn-primary').text('更新Mail設定值(json)');
    $('.modal-footer .btn-outline-secondary').hide();  

})