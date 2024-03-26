$(document).ready(function () {

    //key放入Name
    var fn = douHelper.getField(douoptions.fields, "Name");
    if (fn != null) {
        fn.formatter = function (v, d) {
            var skey = '<span id="skey" style="display:none">' + d.Id + '</span>';
            return v + skey;
        }
    }

    var a = {};
    a.item = '<span class="btn btn-secondary glyphicon glyphicon-download-alt"> 確認資料</span>';
    a.event = 'click .glyphicon-download-alt';
    a.callback = function importQdate(a, b, c, d) {

        var aryIds = [];

        $('.bootstrap-table #_table tbody tr').find('#skey').each(function (index) {
            var Id = $(this).html();
            aryIds.push(Id);
        })

        helper.misc.showBusyIndicator();
        $.ajax({
            url: app.siteRoot + 'ConUnitPerson/UpdateConfirm',
            datatype: "json",
            type: "Post",
            data: { "Ids": aryIds },
            success: function (data) {
                if (data.result) {
                    alert("確認資料成功");
                    $('.filter-toolbar-plus .btn-confirm').trigger('click');
                } else {
                    alert("確認資料失敗：\n" + data.errorMessage);
                }
            },
            complete: function () {
                helper.misc.hideBusyIndicator();
            },
            error: function (xhr, status, error) {
                var err = eval("(" + xhr.responseText + ")");
                alert(err.Message);
                helper.misc.hideBusyIndicator();
            }
        });
    };

    douoptions.appendCustomToolbars = [a];

    var $_masterTable = $("#_table").DouEditableTable(douoptions); //初始dou table
})