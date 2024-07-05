$(document).ready(function () {
    helper.jspanel.jspAlertMsg(undefined, {
        title: '', content: '<font size="4em">請先選擇右側業務類別進行查詢，方可顯示人員</font>'
    })
    douHelper.getField(dou_options.fields, 'Sort').visible = false;
    douHelper.getField(dou_options.fields, 'Act').formatter = function (v, r) {
        if (v == true) {
            return "<input type='checkbox' checked id='" + r.UID + "' disabled>"
        } else
            return "<input type='checkbox' id='" + r.UID + "' disabled>"
    };
    //dou_options.useMutiSelect = true; 
    dou_options.tableOptions.onLoadSuccess = function (datas) {
        $_t.bootstrapTable('checkBy', { field: 'TypeID', values: ["DD"] });
        $('input[type="checkbox"]').prop('disabled', true);
        return false;
    }
    dou_options.fields.unshift({ field: "OrderCtrl", title: '排序', class: 'order-ctrl', visible: false, visibleEdit: false, width: 60, formatter: function (v, r) { return '<span class="btn btn-info glyphicon glyphicon-sort"></span>' } });

    
    //dou_options.tableOptions.sortName = "Sort";
    //dou_options.tableOptions.sortName = "Act";

    //dou_options.editformWindowStyle = $.editformWindowStyle.detail;
    dou_options.appendCustomToolbars = [{
        item: '<span class="btn btn-primary glyphicon glyphicon-sort" title="編輯">編 輯</span>', event: 'click .glyphicon-sort',
        callback: function (e) {
            $(this).parent().find('> *').toggleClass('d-none');            
            createTableDragger();
            $('input[type="checkbox"]').prop('disabled', false);

            // 監聽所有的 checkbox 變化事件
            $('input[type="checkbox"]').change(function () {
                // 計算已選取的 checkbox 數量
                var checkedCount = $('input[type="checkbox"]:checked').length;

                // 設定你的閾值
                var listName = $('select[placeholder=業務類別]').find(":selected").text();
                var threshold = $('select[placeholder=業務類別]').find(":selected").data("max");

                // 檢查是否超過閾值
                if (threshold !== null && threshold !== '-') {
                    if (checkedCount > threshold) {
                        $(this).prop('checked', false);
                        alert(listName + '\r\n允許最大人數為 ' + threshold + '人，請重新勾選名單');
                        // 在這裡你可以執行額外的動作
                    }
                }
                
            });
        }
    },
    {
        item: '<span class="btn btn-success glyphicon glyphicon-ok d-none" title="儲存">儲 存</span>', event: 'click .glyphicon-sort',
        callback: function (e) {
            $(this).parent().find('> *').toggleClass('d-none');
            var cdatas = $_t.bootstrapTable('getData');
            var sort = 1;
            $.each(cdatas, function (_i, _d) {
                if (_d['Act'] == false && $("input[id=" + _d["UID"] + "]").prop('checked') == true) { // 要被選進名單的人員
                    _d['CategoryId'] = $('select[placeholder=業務類別]').find(":selected").val();
                    _d['Sort'] = sort;
                    _d['Act'] = true;
                    sort += 1;
                } else if (_d['Act'] == true && $("input[id=" + _d["UID"] + "]").prop('checked') == false) // 原本在表內要移除的人員(取消勾選)
                {
                    _d['CategoryId'] = $('select[placeholder=業務類別]').find(":selected").val();
                    _d['Act'] = false;
                } else if (_d['Act'] == true && $("input[id=" + _d["UID"] + "]").prop('checked') == true) // 原本就在表內的人
                {
                    _d['CategoryId'] = $('select[placeholder=業務類別]').find(":selected").val();
                    _d['Sort'] = sort;
                    _d['Act'] = true;
                    sort += 1;
                }


            });
            dou_options.updateServerData(cdatas, function (r) {
                if (r.Success) {
                    helper.jspanel.jspAlertMsg(undefined, { title: '編輯結果', autoclose: r.Success ? 3000 : 20000, content: r.Success ? '編輯成功' : '<div style="color:red;">編輯失敗<br>' + r.Desc + '</div>' });                   
                };
                $_t.DouEditableTable('tableReload', $_t.DouEditableTable('getData'), false);
                destroyTableDragger();
                $('input[type="checkbox"]').prop('disabled', true);

            });
        }
    },
    {
        item: '<span class="btn btn-warning glyphicon glyphicon-remove d-none" title="取消">取 消</span>', event: 'click .glyphicon-sort',
        callback: function (e) {
            $(this).parent().find('> *').toggleClass('d-none');           
            //$_t.DouEditableTable('tableReload', $_t.DouEditableTable('getData'), false);
            destroyTableDragger();
            $('input[type="checkbox"]').prop('disabled', true);
        }
        },
        {
            item: '<span class="btn btn-secondary glyphicon glyphicon-download-alt" title="編輯">檢核確認</span>', event: 'click .glyphicon-download-alt',
            callback: function (e) {
                
                helper.misc.showBusyIndicator();
                $.ajax({
                    url: app.siteRoot + 'ContactEdit/UpdateConfirm',
                    datatype: "json",
                    type: "Post",                    
                    success: function (data) {
                        if (data.result) {
                            alert("確認資料成功");
                            $('span.btn-confirm').click();
                            ////$('.filter-toolbar-plus .btn-confirm').trigger('click');
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
                
            }
        }
    ];
    //douHelper.getField(dou_options.fields, 'Order')
    var $_t = $("<table id='_table'>").appendTo('.body-content').DouEditableTable(dou_options);
    var $_currenttable = $("#_table");
    var $_CategoryText = $('<div class="CategoryText">').insertBefore($_currenttable);
    $('.CategoryText').text("請先選擇欲查詢業務類別");

    var dragger;
    var createTableDragger = function () {
        $_t.DouEditableTable('showTableColumn', 'OrderCtrl', true);
        if (dragger) dragger.destroy();
        dragger = tableDragger($_t[0], {
            mode: 'row',
            dragHandler: 'table .order-ctrl',
            onlyBody: true,
            animation: 300
        });
        var cdatas = $_t.bootstrapTable('getData');
        dragger.on('drag', function () {
            cdatas = $_t.bootstrapTable('getData');
        });
        dragger.on('drop', function (from, to) {
            var element = cdatas[from - 1];
            cdatas.splice(from - 1, 1);
            cdatas.splice(to - 1, 0, element);
            //console.log("from, to:" + from + " " + to)
            $('table tbody > tr').removeClass('moved');//被移動的row 設opacity:0 >>諮詢方式
        });
        ////被移動的row加moved class,為了設opacity:0 >>諮詢方式
        $('table .order-ctrl').off('mousedown').on('mousedown', function () {
            $(this).closest('tbody').find('tr').removeClass('moved');
            $(this).parent().addClass('moved');
        });
    }
    var destroyTableDragger = function () {
        if (dragger) dragger.destroy();
        $_t.DouEditableTable('showTableColumn', 'OrderCtrl', false);
    }

    $('span.btn-confirm').css({ display: "none" }); //把查詢按鈕隱藏 不用給他們點了

    // 把下拉式選單新增Onchange事件 換了之後直接變換
    $('select[data-fn="CategoryId"]').on('change', function () {

        $('select[data-fn="CategoryId"] option:selected').each(function () {
            $(".CategoryText").text($('select[data-fn="CategoryId"] option:selected').text());
            $('span.btn-confirm').click();
        });
    });
});