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
    a.item = '<span class="btn btn-secondary glyphicon glyphicon-download-alt"> 檢核確認</span>';
    a.event = 'click .glyphicon-download-alt';
    a.callback = function importQdate(a, b, c, d) {

        var aryIds = [];

        //$('.bootstrap-table #_table tbody tr').find('#skey').each(function (index) {
        $('.bootstrap-table #_table tbody tr').each(function (index) {
            if ($(this).find('#btnEdit').is(":visible")) {
                var Id = $(this).find('#skey').html();
                aryIds.push(Id);
            }            
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

    douoptions.afterAddServerData = function (row, callback) {
        $_masterTable.douTable('destroy');
        jspAlertMsg($("body"), { autoclose: 2000, content: '新增資料完成!!', classes: 'modal-sm' },
            function () { location.reload(); });
    }

    douoptions.afterCreateEditDataForm = function ($container, row) {
        //加提示字
        var $p1 = $('div[data-field=ConUnit]').find('label');
        var remind = '<span class="text-danger fw-lighter ms-2">*</span>';
        $(remind).appendTo($p1);

        var $p2 = $('div[data-field=ConType]').find('label');
        var remind = '<span class="text-danger fw-lighter ms-2">*</span>';
        $(remind).appendTo($p2);

        var $p3 = $('div[data-field=Name]').find('label');
        var remind = '<span class="text-danger fw-lighter ms-2">*</span>';
        $(remind).appendTo($p3);

        var $p4 = $('div[data-field=Position]').find('label');
        var remind = '<span class="text-danger fw-lighter ms-2">*</span>';
        $(remind).appendTo($p4);

        var $p5 = $('div[data-field=Tel]').find('label');
        var remind = '<span class="text-danger fw-lighter ms-2">*</span>';
        $(remind).appendTo($p5);

        var $p6 = $('div[data-field=Mobile]').find('label');
        var remind = '<span class="text-danger fw-lighter ms-2">*</span>';
        $(remind).appendTo($p6);

        var $p7 = $('div[data-field=EMail]').find('label');
        var remind = '<span class="text-danger fw-lighter ms-2">*</span>';
        $(remind).appendTo($p7);
    }

    douoptions.tableOptions.onLoadSuccess = function (datas) {
        setPermissions();
    }

    douoptions.tableOptions.onToggle = function (ps) {
        setPermissions();
    }

    var a = {};
    a.item = function (v, r) {
        var btn = "";
        if (!r.IsImport) {
            btn = '<span id="btnEdit" class="btn btn-data-manager-ctrl btn-default btn-sm text-white bg-primary" title="編輯">編輯</span>';
        }
        return btn;
    }
    a.event = 'click #btnEdit';
    a.callback = function importQdate(evt, value, row, index) {
        $('#_table tbody tr').eq(index).find('.btn-update-data-manager').trigger('click') 
    };

    var b = {};
    b.item = function (v, r) {
        var btn = "";
        if (!r.IsImport) {
            btn = '<span id="btnDelete" class="btn btn-data-manager-ctrl btn-default btn-sm" title="刪除">刪除</span>';
        }
        return btn;
    }
    b.event = 'click #btnDelete';
    b.callback = function importQdate(evt, value, row, index) {
        $('#_table tbody tr').eq(index).find('.btn-delete-data-manager').trigger('click')
    };

    douoptions.appendCustomFuncs = [a, b];

    douoptions.afterUpdateServerData = douoptions.afterAddServerData = function (row, callback) {
        

        callback();
        setPermissions();
    }

    douoptions.deleteServerData =
        function (row, callback) {
            //wdate日期在view頁面調整成物件，但dou架構流程(row)資料破壞到，故row時間需要做轉換
            transactionDouClientDataToServer(row, $.AppConfigOptions.baseurl + 'ConUnitPerson/Delete',
                function (result) {
                    {
                        alert('資料刪除成功');
                        location.reload();
                    }
                })
        };

    var $_masterTable = $("#_table").DouEditableTable(douoptions).on($.dou.events.delete, function (e, row) {
        setPermissions();
    }); //初始dou table

    //文字
    $('.bootstrap-table .btn-toolbar .btn-add-data-manager').text('人員新增');

    //必填(只有一個選項)
    if ($('.filter-toolbar-plus [data-fn="ConUnit"] option').length == 2) {
        $('.filter-toolbar-plus [data-fn="ConUnit"] option[value=""]').remove();
    }

    //修改權限設定
    function setPermissions() {

        $('.bootstrap-table .fixed-table-container .btn-update-data-manager').hide();
        $('.bootstrap-table .fixed-table-container .btn-delete-data-manager').hide();

        //其它單位只能檢視
        //////datas[0].ConUnit
        $.each($('.bootstrap-table.conunitpersoncontroller table.table tbody tr'), function (index, value) {
            if (ConUnitName != '' && ConUnitName != $(this).find('.dou-field-ConUnit').text()) {
                $(this).find('#btnEdit').hide();
                $(this).find('#btnDelete').hide();
            }
        });        
    }
})