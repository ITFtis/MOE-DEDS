var chart, $_dou, $_tablecount, $_utypeselect;
var vehicleTypes;
var currentCityDatas = []
var citystaInit = function () {
    datahelper.getVehicleType(function (ts) {
        //var currentCity = "臺北市";
        //var currentCityDatas = [];
        var $_county = $(".sta-county .county-data");
        vehicleTypes = ts;
        //填統計縣市名稱
        //$.each($_county, function () {
        //    var $_this = $(this);
        //    var cn = $_this.attr("data-county");
        //    $_this.find('.title').text(cn);
        //});

        //清單options
        var douoptions = {
            rootParentContainer: '.table-container',
            addable: false, editable: false, deleteable: false,
            fields: [
                { "field": "Town", "title": "鄉鎮" },
                { "field": "ContactUnit", "title": "通聯單位" },
                { "field": "VehicleName", "title": "車輛名稱" },
                { "field": "VehicleState", "title": "車輛現況" },
                { "field": "Load", "title": "載重(頓)" },
                {
                    "field": "VehicleType", "title": "車輛類別", formatter(v, d) {
                        var r = v;
                        var vs =$.grep(vehicleTypes, function (veh) {
                            return veh.Type == v;
                        });
                        if (vs.length > 0)
                            r = vs[0].Name + '(' + r + ')';
                        return r;
                    }
                },
                { "field": "PlateNumber", "title": "車牌號碼" }
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
        $('<option value="0" selected>全部</option>').appendTo($_utypeselect);
        $('<option value="1">使用中</option>').appendTo($_utypeselect);
        $('<option value="2">非使用中</option>').appendTo($_utypeselect);
        $_tablecount = $('<div class="table-count"><label>共 </label><label class="t-count">-</label><label> 筆</label>').appendTo($_tool).find('.t-count');

        //使用狀況選單event
        $_utypeselect.on('change', function () {
            paintCityTable(currentCityDatas);
        })


        helper.misc.showBusyIndicator();
        datahelper.getVehicleCount(function (ds) {
            helper.misc.hideBusyIndicator();
            $.each(ds, function () {
                var _this = this;
                var $_c = getCountyJObject(this.City);
                if ($_c.length > 0) {
                    $_c.find('.t').text(_this.ALL);
                    $_c.find('.use').text(_this.USE);
                    $_c.find('.notuse').text(_this.NOTUSE);
                }
                else {
                    $('.sta-count .t').text(_this.ALL);
                    $('.sta-count .use').text(_this.USE);
                    $('.sta-count .notuse').text(_this.NOTUSE);
                }
            });
            loadCitySta();
        });
        
    });
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
    datahelper.getVehicletable(currentCity, function (ds) {
        helper.misc.hideBusyIndicator();
        currentCityDatas = ds;

        paintCityTable(ds);
    });
}
var paintCityTable = function (ds) {
    var v = $_utypeselect.val();
    var fds = $.grep(currentCityDatas, function (d) {
        return v == 0 ? true : (v == 1 ? d.VehicleState && d.VehicleState.trim() == "使用中" : !d.VehicleState || d.VehicleState.trim() != "使用中");
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

    //temp object
    var dataUse = {
        use: {},
        notuse: {}
    };

    var alltypes = []; //補齊欄位用
    var nodeftype = '其他';
    //各類別數量(分使用中、非使用中)
    $.each(currentCityDatas, function () {
        if (!this)
            return;
        var ut = dataUse.use;
        if (!this.VehicleState || this.VehicleState.trim() != "使用中")
            ut = dataUse.notuse;
        var t = this.VehicleType;// || '其他';
        //組中文類別
        if (t) {
            var fts = $.grep(vehicleTypes, function (tobj) { return tobj.Type == t });
            if (fts.length > 0)
                t += fts[0].Name;
            else t = nodeftype;
        }
        else t = nodeftype;
        if (alltypes.indexOf(t) < 0)
            alltypes.push(t);

        if (ut[t] == undefined)
            ut[t] = 0;
        ut[t]++;
    })

    /*僅show使用中-不用補欄位*/
    /**use ,notuse補欄位(欄位數要一樣)**/
    //for (var i = 0; i < alltypes.length; i++) {
    //    var t = alltypes[i];
    //    if (dataUse.use[t] == undefined)
    //        dataUse.use[t] = 0;
    //    if (dataUse.notuse[t] == undefined)
    //        dataUse.notuse[t] = 0;
    //}

    //dataUse.use及dataUse.notuse轉成 series 要用data
    var notusedatas = Object.keys(dataUse.notuse).map(function (k) {
        return [k, dataUse.notuse[k]];
    });
    var usedatas = Object.keys(dataUse.use).map(function (k) {
        return [k, dataUse.use[k], dataUse.notuse[k]]; // dataUse.notuse[k]use加入notuse值，show label用
    });

    //sort
    notusedatas.sort(function (a, b) {
        //if (b[0] == '其他')
        //    return 99999999;
        return a[0].localeCompare(b[0]);
    });
    usedatas.sort(function (a, b) {
        //if (b[0] == '其他')
        //    return 99999999;
        return a[0].localeCompare(b[0]);
    });

    var colors = ["#FE2371", "#544FC5", "#2CAFFE", "#FE6A35", "#000000", "#6B8ABC", "#1C74BD", "#00A6A6", "#D568FB"];

    var cidx = 0;
    const getData = data => data.map(point => ({
        name: point[0],
        y: point[1],
        use: point[1],
        notuse: point[2], //show point label用
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
            text: '車輛種類統計表(使用中)',
            align: 'left'
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
                    _r.push('<div style="width:60px;"><span >' + this.series.name + '</span></div>:<span style="color:{point.color}">\u25CF</span>' + this.y + '輛 ' );
                    t += this.y;
                });
                //_r.push('總計:' + t + '輛');
                return _r.join('<br>');
            }
        },
        xAxis: {
            type: 'category',
            //accessibility: {
            //    description: 'Countries'
            //},
            //max: 4,
            //labels: {
            //    useHTML: true,
            //    animate: true,
            //    format: '{chart.options.countries.(value).ucCode}<br>' +
            //        '<span class="f32"><span class="flag {value}"></span></span>',
            //    style: {
            //        textAlign: 'center'
            //    }
            //}
        },
        yAxis: [{
            title: {
                text: '數量'
            },
            showFirstLabel: false
        }],
        series: [
            //{
            //color: 'rgba(158, 159, 163, 0.5)',

            //pointPlacement: -0.25,
            //linkedTo: 'main',
            //data: notusedatas.slice(),// dataPrev[2020].slice(),
            //name: '非使用中'
            //},
            {
                name: '使用中',
                id: 'main',
                //dataSorting: {
                //    enabled: true,
                //    matchByName: true
                //},
                dataLabels: [{
                    align: 'center',
                    verticalAlign: 'top',
                    enabled: true,
                    inside: false,
                    colors: 'balck',
                    shadow: true,
                    crop: false, //datalabel至頂
                    overflow: 'none',//datalabel至頂
                    y: -36,//datalabel至頂+36
                    y: -20,/*僅show使用中*/
                    format: '<span style="text-align:center;display:block;">{y}<br><span style="opacity:.5;">{point.notuse}</span></span>',
                    format: '<span style="text-align:center;display:block;">{y}</span>' /*僅show使用中*/
                }],
                data: getData(usedatas).slice()//getData(data[2020]).slice()
            }
        ],
        exporting: {
            allowHTML: true
        }
    });



}

//$(document).ready(function () {
   
//});