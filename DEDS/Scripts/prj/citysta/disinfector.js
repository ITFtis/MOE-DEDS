var chart, $_dou, $_tablecount, $_utypeselect;
var equipmentTypes;
var currentCityDatas = []
var citystaInit = function () {
    datahelper.getEquipmentType(function (ts) {
        equipmentTypes = ts;
    });
    //var $_county = $(".sta-county .county-data");
    //vehicleTypes = ts;
    var usetype = {
        '_1': '環境消毒',
        '_2': ' 登革熱',
        '_3': '公用',
        '_4': '其他'
    }
    //清單options
    var douoptions = {
        rootParentContainer: '.table-container',
        addable: false, editable: false, deleteable: false,
        fields: [
            { "field": "Town", "title": "鄉鎮" },
            { "field": "ContactUnit", "title": "通聯單位" },
            { "field": "DisinfectInstrument", "title": "名稱" },
            { "field": "Amount", "title": "數量" },
            {
                "field": "UseType", "title": "用途", formatter(v) {
                    return v == undefined ? usetype['_4'] : usetype['_' + v];
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
    $_tablecount = $('<div class="table-count"><label>共 </label><label class="t-count">-</label><label> 筆</label>').appendTo($_tool).find('.t-count');

    //使用狀況選單event
    $_utypeselect.on('change', function () {
        paintCityTable(currentCityDatas);
    })


    helper.misc.showBusyIndicator();
    datahelper.getDisinfectorCount(function (ds) {
        helper.misc.hideBusyIndicator();
        $.each(ds, function () {
            var _this = this;
            var $_c = getCountyJObject(this.City);
            if ($_c.length > 0) {
                $_c.find('.t').text(_this.ALL);
                $_c.find('.use-1').text(_this.disinfection);
                $_c.find('.use-2').text(_this.dengue);
                $_c.find('.use-3').text(_this.share);
                $_c.find('.use-4').text(_this.other);
            }
            else {
                $('.sta-count .t-div .data').text(_this.ALL);
                $('.sta-count .use-1 .data').text(_this.disinfection);
                $('.sta-count .use-2 .data').text(_this.dengue);
                $('.sta-count .use-3 .data').text(_this.share);
                $('.sta-count .use-4 .data').text(_this.other);
            }
        });
        loadCitySta();
    });

    //});
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
    datahelper.getDisinfectortable(currentCity, function (ds) {
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
    $.each(equipmentTypes, function () {
        temp[this] = 0;
    });
   
    $.each(currentCityDatas, function () {
        if (!this)
            return;
        var v = this.DisinfectInstrument;
        if (!v || temp[v] == undefined)
            temp['其他'] += this.Amount;
        else
            temp[v] += this.Amount;
    })

    var datas = Object.keys(temp).map(function (k) {
        return [k, temp[k]];
    });
    var colors = ["#FE2371", "#544FC5", "#2CAFFE", "#FE6A35", "#000000", "#6B8ABC", "#1C74BD", "#00A6A6", "#D568FB"];

    var cidx = 0;
    const getData = data => data.map(point => ({
        name: point[0],
        y: point[1],
        color: colors[cidx++ % colors.length]
    }));

    chart = Highcharts.chart('chart-count', {
        chart: {
            type: 'column',
            spacing: [0, 0, 0, 0],
        },
        credits: {
            enabled: false
        },
        title: {
            text: '設備類別統計表',
            align: 'center'
        },
        plotOptions: {
            series: {
                grouping: false,
                borderWidth: 0
            }
        },
        legend: {
            enabled: false
        },
        tooltip: {
            shared: true,
            formatter: function (evt) {
                var _r = [this.points[0].key];
                var t = 0;
                $.each(this.points, function () {
                    var thisp = this;
                    _r.push('<div style="width:60px;"><span >數量</span></div>:<span style="color:{point.color}">\u25CF</span>' + this.y + ' ');
                    t += this.y;
                });
                //_r.push('總計:' + t + '輛');
                return _r.join('<br>');
            }
        },
        xAxis: {
            type: 'category',
        },
        yAxis: [{
            title: {
                text: '數量'
            },
            showFirstLabel: false
        }],
        series: [
            {
                name: '類別',
                id: 'main',
                //dataLabels: [{
                //    align: 'center',
                //    verticalAlign: 'top',
                //    enabled: true,
                //    inside: false,
                //    colors: 'balck',
                //    shadow: true,
                //    crop: false, //datalabel至頂
                //    overflow: 'none',//datalabel至頂
                //    y: -36,//datalabel至頂+36
                //    y: -20,/*僅show使用中*/
                //    format: '<span style="text-align:center;display:block;">{y}<br><span style="opacity:.5;">{point.notuse}</span></span>',
                //    format: '<span style="text-align:center;display:block;">{y}</span>' /*僅show使用中*/
                //}],
                data: getData(datas).slice()//getData(data[2020]).slice()
            }
        ],
        exporting: {
            allowHTML: true
        }
    });



}

//$(document).ready(function () {

//});