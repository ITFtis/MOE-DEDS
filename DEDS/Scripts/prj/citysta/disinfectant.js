var chart, $_dou, $_tablecount, $_utypeselect;
var equipmentTypes;
var allDisinfectantState = [];
var currentCityDatas = []
var usetype = {
    '_1': '環境消毒',
    '_2': ' 登革熱',
    '_3': '紅火蟻',
    '_4': '荔枝椿象',
    '_5': '其他'
}
var citystaInit = function () {
    datahelper.getEquipmentType(function (ts) {
        equipmentTypes = ts;
    });
    //var $_county = $(".sta-county .county-data");
    //vehicleTypes = ts;

    //清單options
    var douoptions = {
        rootParentContainer: '.table-container',
        addable: false, editable: false, deleteable: false,
        fields: [
            { "field": "Town", "title": "鄉鎮" },
            { "field": "ContactUnit", "title": "通聯單位" },
            { "field": "DrugName", "title": "名稱" },
            { "field": "DrugType", "title": "類別" },
            { "field": "DrugState", "title": "狀態" },
            { "field": "Amount", "title": "數量" },
            {
                "field": "UseType", "title": "用途", formatter(v) {
                    return v == undefined ? usetype['_5'] : usetype['_' + v];
                }
            }
        ],
        datas: [],
        tableOptions: {
            onSearch: function (sdad, sdfsd, fd, sf) {
                paintCityColumnChart();
            }
        }
    }
    $_dou = $('.table-container > table').douTable(douoptions);
    var $_tool = $_dou.closest('.dou-bootstrap-table').find('.fixed-table-toolbar');
    $_tool.find('.search').removeClass('float-right');
    $_utypeselect = $('<select class="form-select">').prependTo($_tool);
    $('<option value="" selected>全部</option>').appendTo($_utypeselect);
    for (var k in usetype) {
        $('<option value="' + k + '">' + usetype[k] + '</option>').appendTo($_utypeselect);
    }

    ////var $back = $('<button id="btnExport" class="btn btn-secondary">匯出Excel</button>');
    ////$back.appendTo($_tool);

    ////$('#btnExport').click(function () {
    ////    //alert('開發中');
    ////    var $_t = $("<table>").css('position', 'fixed').css('top', '-300px').css('opacity', 0).appendTo($('body'));
    ////    current._paintTable($_t, false);
    ////    helper.misc.tableToExcel($_t, '積淹水災情統計', '積淹水災情統計');
    ////    setTimeout(function () {
    ////        $_t.remove();
    ////    }, 1000);
    ////});

    $_tablecount = $('<div class="table-count"><label>共 </label><label class="t-count">-</label><label> 筆</label>').appendTo($_tool).find('.t-count');

    //使用狀況選單event
    $_utypeselect.on('change', function () {
        paintCityTable(currentCityDatas);
    })


    helper.misc.showBusyIndicator();
    //依用途
    datahelper.getDisinfectantCount(function (ds) {
        helper.misc.hideBusyIndicator();
        $.each(ds, function () {
            var _this = this;
            var $_c = getCountyJObject(this.City);
            if ($_c.length > 0) {
                $_c.find('.t').text(_this.ALL);
                $_c.find('.use-1').text(_this.disinfection);
                $_c.find('.use-2').text(_this.dengue);
                $_c.find('.use-3').text(_this.Fir_ants);
                $_c.find('.use-4').text(_this.Tessaratoma_papillosa);
                $_c.find('.use-5').text(_this.other);
            }
            else {
                $('.sta-count .t-div .data').text(_this.ALL);
                $('.sta-count .use-1 .data').text(_this.disinfection);
                $('.sta-count .use-2 .data').text(_this.dengue);
                $('.sta-count .use-3 .data').text(_this.Fir_ants);
                $('.sta-count .use-4 .data').text(_this.Tessaratoma_papillosa);
                $('.sta-count .use-5 .data').text(_this.other);
            }
        });
        loadCitySta();
    });

    //依狀態
    datahelper.getDisinfectantStateTable(function (ds) {
        allDisinfectantState = ds;
        paintAllStateChart();
    });
    //});
    var timerflag;
    $(window).on('resize', function () {
        clearTimeout(timerflag);
        timerflagsetTimeout(function () {
            paintAllStateChart();
        }, 200);
    })
}

var ctrlZoomChange = function (e) {
    paintCityColumnChart();
}
var changeCity = function (e) {
    loadCitySta();
}
var loadCitySta = function () {
    getCountyJObject().find('.btn').removeClass("active");
    getCountyJObject(currentCity).find('.btn').addClass("active");
    helper.misc.showBusyIndicator();
    datahelper.getDisinfectanttable(currentCity, function (ds) {
        helper.misc.hideBusyIndicator();
        currentCityDatas = ds;

        paintCityTable(ds);
    });
}
var paintCityTable = function (ds) {
    var v = $_utypeselect.val();
    var fds = $.grep(currentCityDatas, function (d) {
        return v == '' || v == undefined ? true : '_' + d.UseType == v;
    });
    console.log(currentCity + ":" + currentCityDatas.length + ("總筆數"));
    console.log("過濾後彼數(不含搜尋):" + fds.length);
    $_dou.douTable('tableReload', fds);

    paintCityColumnChart();
}
var paintCityColumnChart = function () {
    var tabledisplaydatas = $_dou.douTable('getData');
    $_tablecount.text(tabledisplaydatas.length)
    console.log("table呈現筆數:" + tabledisplaydatas.length);

    $("#chart-count").empty();

    var temp = {};
    $.each(usetype, function () {
        temp[this] = {amount:0, '固體':0, '液體':0, '乳劑':0, '油劑':0};
    });

    $.each(currentCityDatas, function () {
        if (!this)
            return;
        var v = this.UseType;
        if (!v || usetype['_' + v] == undefined) {
            if (temp['其他'] == undefined)
                temp['其他'] == { amount: 0, '固體': 0, '液體': 0, '乳劑': 0, '油劑': 0 };
            temp['其他'].amount += this.Amount;
            if (this.DrugState != undefined)
                temp['其他'][this.DrugState] += this.Amount;
        }
        else {
            temp[usetype['_' + v]].amount += this.Amount;
            if (this.DrugState != undefined)
                temp[usetype['_' + v]][this.DrugState] += this.Amount;
        }
    })

    var datas = Object.keys(temp).map(function (k) {
        return [k, temp[k]];
    });
    var colors = ["#FE2371", "#544FC5", "#2CAFFE", "#FE6A35", "#6B8ABC", "#1C74BD", "#00A6A6", "#000000", "#D568FB"];

    var cidx = 0;
    const getData = data => data.map(point => ({
        name: point[0],
        y: point[1].amount,
        '固體': parseFloat(point[1].固體.toFixed(2)) +'公斤',
        '液體': parseFloat(point[1].液體.toFixed(2)) + '公升',
        '乳劑': parseFloat(point[1].乳劑.toFixed(2)) + '公升',
        '油劑': parseFloat(point[1].油劑.toFixed(2)) + '公升',
        color: colors[cidx++ % colors.length]
    }));

    chart = Highcharts.chart('chart-count', {
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            type: 'pie'
        },
        credits: {
            enabled: false
        },
        title: {
            text: '藥劑用途統計表',
            align: 'center'
        },
        tooltip: {
            headerFormat: '{series.name}-',
            pointFormat: '{point.name}: <b>{point.percentage:.1f}%</b>',
            pointFormat: '<b>{point.name}</b><br><br><b>固體:{point.固體}</b><br><b>液體:{point.液體}</b><br><b>乳劑:{point.乳劑}</b><br><b>油劑:{point.油劑}</b>'
        },
        accessibility: {
            point: {
                valueSuffix: '%'
            }
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    format: '<span style="color:{point.color}"><b>{point.name}</b>: {point.percentage:.1f} %</span>'
                }
            }
        },
        series: [{
            name: '用途種類',
            colorByPoint: true,
            data: getData(datas).slice()
        }]
    });

}

var paintAllStateChart = function () {
    $.each(allDisinfectantState, function () {
        paintStateChart(this);
    });
}
var paintStateChart = function (stateobj) {
    var $_c = $('.sta-county.state .county-data[data-county="' + stateobj.City + '"]');
    if ($_c.length == 0) 
        $_c = $('.display-container .sta-count.state'); //全臺
    $_c.empty();
    var cid = 'id_'+helper.misc.geguid();
    $('<div id="' + cid + '">').appendTo($_c);
    $('<div class="btn btn-sm btn-default zoom-ctrl glyphicon glyphicon-resize-full" title="放大"></div>').appendTo($_c).on('click', function () {
        $(this).parent().toggleClass('zoom-max');
        setTimeout(function () {
            paintStateChart(stateobj);
        });
        $(this).toggleClass('glyphicon-resize-small');
    });

    var datas = [['固體', stateobj.Solid], ['液體', stateobj.Liquid], ['乳劑', stateobj.Emulsion], ['油劑', stateobj.Oil]];
    var colors = ["#FE2371", "#544FC5", "#2CAFFE", "#FE6A35", "#6B8ABC", "#1C74BD", "#00A6A6", "#000000", "#D568FB"];

    var cidx = 0;
    const getData = data => data.map(point => ({
        name: point[0],
        y: point[1],
        dispay: point[1] + (point[0] =='固體'?'公斤':'公升'),
        color: colors[cidx++ % colors.length]
    }));

    var maxzoom = $_c.hasClass('zoom-max');
    var isTw = stateobj.City == "Taiwan";

    var coptions= {
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            spacing: [maxzoom ? 80 : 0, 0, 0, 0],
            type: 'pie'
        },
        credits: {
            enabled: false
        },
        title: {
            text: isTw ?'全國統計表':stateobj.City,
            align: 'center',
            margin: 0,
        },
        tooltip: {
            headerFormat: '{series.name}<br>',
            //pointFormat: '{point.name}: <b>{point.percentage:.1f}%</b>'
            pointFormat: '{point.name}: <b>{point.dispay}</b>'
        },
        accessibility: {
            point: {
                valueSuffix: '%'
            }
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                colors,
                borderRadius: 5,
                size: maxzoom ?'86%':  '100%',
                slicedOffset: 0,
                dataLabels: {
                    enabled: true,
                    format: '<b>{point.name}</b><br>{point.percentage:.1f} %',
                    distance: -25,
                    filter: {
                        property: 'percentage',
                        operator: '>',
                        value: 4
                    }
                }
            }
        },
        series: [{
            name: '狀態種類',
            colorByPoint: true,
            data: getData(datas).slice()
        }]
    }
    if (isTw && !maxzoom) {
        coptions.plotOptions.size =undefined;
        coptions.chart.spacingTop = 80;
    }
    if (maxzoom || isTw) {
        coptions.plotOptions= {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                 dataLabels: {
                    enabled: true,
                    format: '<span style="color:{point.color}"><b>{point.name}</b>: {point.percentage:.1f} %</span>'
                }
            }
        }
        coptions.dataLabels = {
            style: {
                fontSize:'25px'
            }
        }
    }
    Highcharts.chart(cid, coptions);
}
//$(document).ready(function () {

//});