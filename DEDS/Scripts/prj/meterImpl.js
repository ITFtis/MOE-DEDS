var InitWraRain = function ($_container, options) {
    $_container.RainCtrl($.extend({
        map: app.map,
        enabledStatusFilter: true,
        canFullInfowindow: true,
        listContainer: 'inner',
        listTheme: 'none',
        autoReload: true,
        legendIcons: [
            { 'name': '正常', 'url': app.siteRoot +'images/pin/雨量_正常.png', 'classes': 'rain_normal', disabled: true },
            { 'name': '大雨', 'url': app.siteRoot +'images/pin/雨量_大雨.png', 'classes': 'rain_heavy' },
            { 'name': '豪雨', 'url': app.siteRoot +'images/pin/雨量_豪雨.png', 'classes': 'rain_extremely' },
            { 'name': '大豪雨', 'url': app.siteRoot +'images/pin/雨量_大豪雨.png', 'classes': 'rain_torrential' },
            { 'name': '超大豪雨', 'url': app.siteRoot +'images/pin/雨量_超大豪雨.png', 'classes': 'rain_exttorrential' },
            { 'name': '無資料', 'url': app.siteRoot +'images/pin/雨量_無資料.png', 'classes': 'rain_nodata', disabled: true }
        ],
        //infoFields: [
        //    {
        //        field: 'CName', title: '站 名', formatter: function (v, data) {
        //            return v;//+ '(' + data.StationID + ')';
        //        }
        //    },
        //    {
        //        field: 'Datetime', title: '時 間', formatter: function (value, row, source) {
        //            return value ? value.DateFormat("dd HH:mm") : '---';
        //        }
        //    },
        //    { field: 'R1H', title: '時雨量', formatter: $.rainFormatter.float, unit: "毫米", sortable: true, align: 'right' },
        //    { field: 'R3H', title: '3小時', formatter: $.rainFormatter.float, unit: "毫米", sortable: true, align: 'right' },
        //    { field: 'R24H', title: '24小時', formatter: $.rainFormatter.float, unit: "毫米", sortable: true, align: 'right' }
        //]
    }, options));
}

var InitWraWater = function ($_container, options) {
    $_container.WaterCtrl($.extend({
        name: "水位站",
        map: app.map,
        enabledStatusFilter: true,
        canFullInfowindow:true,
        autoReload:true,
        listContainer: 'inner',
        listTheme: 'none',
        legendIcons: [
            { name: '正常', url: app.siteRoot +'images/pin/水位站-g.png', classes: 'green_status', disabled:true },
            { name: '一級', url: app.siteRoot +'images/pin/水位站-r.png', classes: 'red_status' },
            { name: '二級', url: app.siteRoot +'images/pin/水位站-o.png', classes: 'orange_status' },
            { name: '三級', url: app.siteRoot +'images/pin/水位站-y.png', classes: 'yellow_status' },
            { name: '無資料', url: app.siteRoot +'images/pin/水位站-gr.png', classes: 'gray_status', disabled: true }],
    }, options));
}
var InitFloodSensor = function ($_container, options) {
    FloodSensorOptions.map = FloodSensorOptions.map || app.map;
    $_container.PinCtrl($.extend(FloodSensorOptions, options));
}

var InitEmic = function ($_container, options) {
    $_container.PinCtrl($.extend({
        name: "EMIC災情",
        map: app.map,
        stTitle: function (data) { return data.CASE_LOC },
        enabledStatusFilter: true,
        canFullInfowindow: true,
        autoReload: true,
        listContainer: 'inner',
        listTheme: 'none',
        loadBase: function (callback) { callback([]); },
        loadInfo: function (dt, callback) { datahelper.getEmic(callback); },
        infoFields: [
            { field: 'CASE_DT', title: '災情時間' },
            { field: 'COUNTY_N', title: '行政區', formatter: function (v, data) { return v + data.TOWN_N } },
            { field: 'CASE_LOC', title: '地點' },
            { field: 'DISASTER_MAIN_TYPE', title: '類別', formatter: function (v, data) { return v + '-' + data.DISASTER_SUB_TYPE } },
            { field: 'CASE_DESCRIPTION', title: '描述', showInList: false },
            { field: 'INJURED_NO', title: '傷亡統計', showInList: false, formatter: function (v, d) { return '傷:' + v + ' 亡:' + d.DEATH_NO + ' 受困:' + d.TRAPPED_NO + ' 失蹤:' + d.MISSING_NO } },
            { field: 'CASE_STATUS', title: '狀態' }
            ],
        legendIcons: [
            { name: '未處理', url: app.siteRoot + 'images/pin/災情_未處理.png', classes: 'red_status' },
            { name: '處理中', url: app.siteRoot + 'images/pin/災情_處理中.png', classes: 'yellow_status' },
            { name: '已處理', url: app.siteRoot + 'images/pin/災情_已處理.png', classes: 'red_status', disabled: true }],
        checkDataStatus: function (data, index) {
            var _i = 0;
            if (data.CASE_STATUS == "處理中")
                _i = 1;
            else if ((data.CASE_STATUS == "已處理"))
                _i = 2;
            return this.settings.legendIcons[_i];
        },
        transformData: function (_base, _info) {
            var r = [];
            if (_info.DISASTER_LIST && _info.DISASTER_LIST.DISASTER_DATA) {
                $.each(_info.DISASTER_LIST.DISASTER_DATA, function (idxi, i) {
                    if (!i.COORDINATE)
                        return;
                    var coors = i.COORDINATE.split(',');
                    i.X = parseFloat(coors[0]);
                    i.Y = parseFloat(coors[1]);
                    r.push(i);
                });
            }
            return r;
        },
    }, options));
}
var InitDengue = function ($_container, options) {
    var $_ctrl = $('<div class="row"><div class="col-md-12"></div></div>').appendTo($_container);
    $_ctrl.PinCtrl($.extend({
        name: "登革熱",
        map: app.map,
        stTitle: function (data) { return data.居住縣市 + data.居住鄉鎮+data.居住村里 },
        enabledStatusFilter: true,
        canFullInfowindow: true,
        autoReload: false,
        cluster: true,
        listOptions: { virtualScroll: true },
        //listContainer: 'inner',
        //listTheme: 'none',
        loadBase: function (callback) { callback([]); },
        loadInfo: function (dt, callback) { helper.data.get(app.siteRoot +'api/dengue/all', function (ds) { callback(ds); }) },
        baseInfoMerge: {
            xfield: '最小統計區中心點X', yfield: '最小統計區中心點Y', aftermerge: function (d) {
                if (d.X == 'None') {
                    d.X = 119;
                    d.Y = 23;
                }
            }
        }
    }, options));
    return $_ctrl;
}
var InitReportDisaster = function ($_container, options) {
   
    $_container.PinCtrl($.extend(floodPinOptions, { map: app.map, enabledStatusFilter: true, autoReload: true, listContainer: 'inner', listTheme:'none'}, options));
}

var FloodSensorOptions = {
    stTitle: function (d) { return d.SensorName },
    useTimeSeriesData: true, enabledStatusFilter: true, autoReload: true,
    listContainer: 'inner',
    listTheme: 'none',
    pinInfoLabelMinWidth: '68px;vertical-align: top',
    name: '淹水感測設備', infoFields: [
        //{ field: 'SensorUUID', title: 'ID' },
        { field: 'SensorName', title: '站名' },
        { field: 'SourceTime', title: '時間', formatter: function (v, r) { return v ? JsonDateStr2Datetime(v).DateFormat('MM/dd HH:mm') : '-' } },
        { field: 'Depth', title: '淹水深度', formatter: function (v, r) { return (v != undefined ? v.toFixed(0) : '-') + '公分'; } },
        {
            field: 'Operator', title: '單位', formatter: function (v, r) {
                var r = '--';
                if (v != undefined) {
                    var _us = $.grep(AllUnitCodes, function (u) {
                        return u.id == v;
                    });
                    if (_us.length > 0) r = _us[0].name;
                }
                return r;
            }
        },
        {
            field: 'Address', title: '地點', formatter: function (v, r) {
                var td = $.grep(window.alltown, function (t) {
                    return t.TownCode == r.TownCode;
                });
                if (td.length > 0)
                    return td[0].CityName + td[0].Town + v;
                else
                    return v;
            }
        }
    ],
    legendIcons: [{ name: '正常', url: app.siteRoot + 'images/pin/Flood_b_7.png', classes: 'blue_status', disabled: true },
        { name: '淹水', url: app.siteRoot + 'images/pin/Flood_r_7.png', classes: 'red_status', disabled: true },
        { name: '淹水10↑', url: app.siteRoot + 'images/pin/fsensor_10.png', classes: 'red_status', disabled: true },
        { name: '淹水30↑', url: app.siteRoot + 'images/pin/fsensor_30.png', classes: 'red_status' },
        { name: '淹水50↑', url: app.siteRoot + 'images/pin/fsensor_50.png', classes: 'red_status' },
        { name: '待檢核', url: app.siteRoot + 'images/pin/Flood_y_7.png', classes: 'yellow_status', disabled: true  },
        { name: '無資料', url: app.siteRoot + 'images/pin/Flood_g_7.png', classes: 'gray_status', disabled: true  }],
    checkDataStatus: function (data, index) {
        var _i = 0;
        if (!data.SourceTime)// || (Date.now() - JsonDateStr2Datetime(data.SourceTime).getTime()) >= 24 * 60 * 60 * 1000)
            _i = 6;
        else if (data.ToBeConfirm == true)
            _i = 5;
        else if (data.Depth && data.Depth >= 50)
            _i = 4;
        else if (data.Depth && data.Depth >= 30)
            _i = 3;
        else if (data.Depth && data.Depth > 10)
            _i = 2;
        else if (data.Depth && data.Depth >= 0)
            _i = 1;
        return this.settings.legendIcons[_i];
    },
    loadBase: window.datahelper.getFHYFloodSensorStation,
    loadInfo: window.datahelper.getFHYFloodSensorInfoRt,
    transformData: function (_base, _info) {
        var that = this;
        var datas = [];
        $.each(_base, function () {
            var _i = JSON.parse(JSON.stringify(this));
            if (this.Point && this.Point.Latitude != undefined) {
                _i.X = this.Point.Longitude;
                _i.Y = this.Point.Latitude;
            }
            var _if = $.grep(_info, function (_in) { return _in.SensorUUID == _i.SensorUUID });
            if (_if.length > 0)
                _i = $.extend(_i, _if[0]);
            datas.push(_i);
        });
        datas.sort(function (a, b) {
            var av = a.Depth, bv = b.Depth;
            if (!a.SourceTime || (Date.now() - JsonDateStr2Datetime(a.SourceTime).getTime()) >= 24 * 60 * 60 * 1000)
                av = -999;
            if (!b.SourceTime || (Date.now() - JsonDateStr2Datetime(b.SourceTime).getTime()) >= 24 * 60 * 60 * 1000)
                bv = -999;
            return bv - av; //清單淹水的在前面
        });
        window.pinLeafletMaxZIndex += datas.length;
        $.each(datas, function (idx, d) { //淹水的pin pinZIndex越大，才會在上面
            d.pinZIndex = window.pinLeafletMaxZIndex - idx;
        });
        window.ALLDATAS = datas;//選鄉鎮select filter
        return datas;
    },
    pinInfoContent: function (data) {
        //data.Depth = 30;
        var that = this;
        var $_c = $($.BasePinCtrl.defaultSettings.pinInfoContent.call(this, data));
        var eurl = encodeURI(app.CSgdsRoot+ 'FDashboard.html?sensoruuid=' + data.SensorUUID);
        //console.log("eurl:" + eurl);
        var $_info = $('<div style="font-size:1.4rem;padding:0px 8px 0 6px;text-align:end;"></div>').appendTo($_c);
        /*$('<a href="' + eurl + '" target="_FDashboard"><sapn class="glyphicon glyphicon-info-sign" title="淹水感測整合資訊"></a>').appendTo($_info);*/
        if (data.Depth && data.Depth >= 10 && !data.floodarea) {
            var _gid = 'cal-' + helper.misc.geguid();
            $('<sapn id="' + _gid + '" style="font-size:1.2rem;margin-right:1rem;cursor:pointer" class="glyphicon glyphicon-alert" title="淹水範圍推估"></span>').prependTo($_info);
            setTimeout(function () {
                if (_gid) {
                    $('#' + _gid).on('click', function () {
                        var $_this = $(this);
                        $_this.off('click').css('opacity', .3)[0].style.cursor = 'not-allowed';//.addClass('btn disabled');

                        helper.misc.showBusyIndicator($_this.closest('.meterInfoTemplateContent'), { content: '演算中...' });
                        datahelper.estimateFloodingComputeForLightweightDatas([{
                            PK_ID: data.SensorUUID + Date.now(),
                            X: data.Point.Longitude,
                            Y: data.Point.Latitude,
                            DATE: helper.format.JsonDateStr2Datetime(data.SourceTime),
                            CREATE_DATE: helper.format.JsonDateStr2Datetime(data.SourceTime),
                            Sources: "淹水感測器即時推估_SGDS",
                            EMISTYPE: '淹水感測器',
                            DEPTH: data.Depth,
                            SourceCode: 7,
                            TOWN_NAME: data.TownCode,
                            LOCATION_DESCRIPTION: data.Address,
                            Described:'-'
                        }], function (fds) {
                            helper.misc.hideBusyIndicator($_this.closest('.meterInfoTemplateContent'));
                            if (fds && fds.floodarea.length > 0) {
                                //console.log(fds.floodarea[0]);
                                if (fds.floodarea == undefined || fds.floodarea.length == 0 || fds.floodarea[0].Image_Data == undefined)
                                    return;
                                data.floodarea = fds.floodarea[0];
                                that.$element.trigger('add-floodarea', [data]);
                                return;
                                var fa = fds.floodarea[0];

                                data._floodImageOverlay = L.imageOverlay(app.CSgdsRoot + fa.Image_Data.Url, [[fa.Image_Data.MaxY, fa.Image_Data.MaxX], [fa.Image_Data.MinY, fa.Image_Data.MinX]], { interactive: true, zIndex: 201 }).addTo(app.map);
                                data._floodImageOverlay.on('click', function (e) {
                                    var popup = data._floodImageOverlay.unbindPopup().bindPopup('<div class="leaflet-infowindow-title  status-blue">' + data.SensorName + '</div>' + "<div>" + floodPinOptions.pinInfoContent.call({ settings: floodPinOptions }, fa) + "</div>",
                                        $.extend({ closeOnClick: true, autoClose: true, className: 'leaflet-infowindow', minWidth: 250 },
                                            {}))
                                        .openPopup(e.latlng).getPopup();

                                    var _color = '#0000FF';
                                    var $_pop = $(popup.getElement());
                                    var _statusstyle = window.getComputedStyle($_pop.find('.leaflet-infowindow-title')[0]);
                                    var $_title = $_pop.find('.leaflet-infowindow-title');
                                    var _cbtn = $_pop.find('.leaflet-popup-close-button')[0];
                                    var _content = $_pop.find('.leaflet-popup-content')[0];
                                    $_title.css('background-color', _color).css('color', '#fff').css('box-shadow', $_title.css('box-shadow').replace('#999', _color).replace('rgb(153, 153, 153)', _color));
                                    _content.style.borderColor = _cbtn.style.borderColor = _cbtn.style.color = _color;
                                    $_pop.find('.leaflet-popup-tip')[0].style.boxShadow = " 3px 3px 15px " + _color;
                                });
                            }
                        });
                    })
                }
            });
        }
        //$('<form style="font-size:1.75em;padding:0px 8px 0 6px;text-align:end;" action="FDashboard.html" target="_FDashboard"  method="post"><a href="javascript:;" onclick="parentNode.submit();"><sapn class="glyphicon glyphicon-info-sign" title="淹水感測整合資訊"></span></a><input type="hidden" name="sensoruuid" value="' + data.SensorUUID + '"/></form>').appendTo($_c);
        return $_c[0].outerHTML;
    }
};


var InitFmgCctv = function ($_container, options) {
    $_container.CctvCtrl($.extend({
        map: app.map,
        stTitle: function (data) { return data.name },
        canFullInfowindow: true,
        loadBase: datahelper.getAllCctv,
        loadInfo: function (dt, callback) { callback([]) },
        legendIcons: [{ name: '正常', url: app.siteRoot + 'images/pin/CCTV_near.png', classes: 'blue_status' }],
        transformData: function (_base, _info) {
            $.each(_base, function () {
                var coor = helper.gis.TWD97ToWGS84(this.x_tm97, this.y_tm97);
                this.X = coor.lon;
                this.Y = coor.lat;
                this.urls = [];
            });
            return _base;
        },
        pinInfoContent: function (d, infofields) {
            if (d.urls.length == 0) {
                var r = datahelper.getCctvCameras(d.id, d.sourceid);
                if (r) {
                    var _cameras = $.grep(r.cameras, function (_c) {
                        return _c.images && _c.images.length > 0;
                    });
                    d.urls = $.map(_cameras, function (_c) {
                        var _u = _c.images[_c.images.length - 1];
                        var _si = _u.lastIndexOf('/');
                        var _ei = _u.lastIndexOf('.');
                        var _ts = _u.substr(_si + 1, _ei - _si - 1);
                        return {id:_c.id,name:_c.cctv_name,url: _u.replace(_ts, 'new') };//_c.images[_c.images.length - 1];
                    });
                }
            }
            return $.CctvCtrl.defaultSettings.pinInfoContent.call(this,d, infofields);
        }
    }, options));
}

//人口密集圖
InitPopulationDensity = function ($_container, options) {
    var $_ctrl =$('<div class="row"><div class="col-md-12"></div></div>').appendTo($_container);
    var kmloptions = $.extend({
        map: app.map, useSearch: false, useLabel: false, useList: false,
        //autoGenLegendIcons: true, styleSelector: true, legendContainer: '#legendDiv',
        name: "人口密集圖",
        url: app.siteRoot + 'Data/人口密集圖.kmz',
        type: $.BasePinCtrl.type.polygon
    }, options);
    $_ctrl.KmlCtrl(kmloptions)
        .on($.BasePinCtrl.eventKeys.initUICompleted, function () {
            //_ctrl.find('.legend  img').insertBefore(_ctrl.find('.checkbox-name'));
            //_ctrl.find('.legend').addClass('offdisplay');
            //current.__pinctrl.instance.repaintLegendUI(current.settings.legendIcons);
            var sdd = $_ctrl.instance;
        });
    return $_ctrl;
}

var InitResourceKmlCtrl = function ($_container, options, name, clustername) {
    return $_container.KmlCtrl($.extend({
        map: app.map, useSearch: true, cluster: true,
        name: name,
        legendIcons: [{ 'name': name, 'url': app.siteRoot +'Images/pin/' + name + '.png', classes: 'blue_status' }],
        url: 'kml/' + name + '.kmz',
        checkDataStatus: function (data, index) { return this.settings.legendIcons[0] },
        type: $.BasePinCtrl.type.point,
        cluster: {
            //imagePath: 'http://localhost:1928/images/cluster/'+clustername,
            maxZoom: 18,
            styles: [
                {
                    //textColor: 'white',
                    url: 'images/cluster/' + clustername + '.png',
                    //url:'Scripts/gis/google/markerclusterimage/m1.png',
                    height: 40,
                    width: 40,
                },
                {
                    //textColor: 'white',
                    url: 'images/cluster/' + clustername + '.png',
                    height: 46,
                    width: 46
                },
                {
                    //textColor: 'white',
                    url: 'images/cluster/' + clustername + '.png',
                    height: 52,
                    width: 52,
                },
                {
                    //textColor: 'white',
                    url: 'images/cluster/' + clustername + '.png',
                    height: 58,
                    width: 58,
                },
                {
                    //textColor: 'white',
                    url: 'images/cluster/' + clustername + '.png',
                    height: 64,
                    width: 64,
                }
            ],
            gridSize: 50
        }
    }, options))
        .on($.BasePinCtrl.eventKeys.initUICompleted, function () {
            $_container.instance.__pinctrl.instance._mapctrl._defaultIconCreateFunction = function (markerClusterGroup, mg) { //複寫 for leaflet markerCluster
                var divIcon = markerClusterGroup._defaultIconCreateFunction(mg);
                //var _class = $.BasePinCtrl.pinIcons.rain.noData.classes;
                //if ($.grep(mg.getAllChildMarkers(), function (r) { return r.pinstatus.classes == $.BasePinCtrl.pinIcons.rain.extremely_torrential.classes }).length > 0)
                //    _class = $.BasePinCtrl.pinIcons.rain.extremely_torrential.classes;
                //else if ($.grep(mg.getAllChildMarkers(), function (r) { return r.pinstatus.classes == $.BasePinCtrl.pinIcons.rain.torrential.classes }).length > 0)
                //    _class = $.BasePinCtrl.pinIcons.rain.torrential.classes;
                //else if ($.grep(mg.getAllChildMarkers(), function (r) { return r.pinstatus.classes == $.BasePinCtrl.pinIcons.rain.extremely.classes }).length > 0)
                //    _class = $.BasePinCtrl.pinIcons.rain.extremely.classes;
                //else if ($.grep(mg.getAllChildMarkers(), function (r) { return r.pinstatus.classes == $.BasePinCtrl.pinIcons.rain.heavy.classes }).length > 0)
                //    _class = $.BasePinCtrl.pinIcons.rain.heavy.classes;
                //else if ($.grep(mg.getAllChildMarkers(), function (r) { return r.pinstatus.classes == $.BasePinCtrl.pinIcons.rain.normal.classes }).length > 0)
                //    _class = $.BasePinCtrl.pinIcons.rain.normal.classes;
                divIcon.options.html = '<div><span>' + name[0]+ mg.getAllChildMarkers().length+'<span></div>';
                divIcon.options.className += " custer-" + name + " ";
                return divIcon;
            }
        });
}
var Init環保局資源 = function ($_meter) {
    return $('<div class="row"><div class="col-md-12"></div></div>').appendTo($_meter).PinCtrl({
        map: app.map, 
        name: "環保局",
        stTitle: function (data) { return data.City+data.Town },
        loadBase: function (callback) { callback([]) },
        loadInfo: datahelper.getEpaSource,
        listTheme:'info epa_status source-ctrl',
        infoFields: [
            { field: 'City', title: '單位', formatter: function (v, d) { return d.City + d.Town; }, showInfo: false },
            { field: 'Vehicle', title: '清潔車輛', unit: '台',showInList :false },
            { field: 'Disinfector', title: '清潔設備', unit: '台', showInList: false },
            { field: 'SolidDrugStateDisinfectant', title: '消毒藥品(固)', unit: '公斤', showInList: false },
            { field: 'LiquidDrugStateDisinfectant', title: '消毒藥品(液)', unit: '公升', showInList: false },
            { field: 'Toilet', title: '流動廁所', showInList: false }
        ],
        legendIcons: [{ 'name': '環保局', url: app.siteRoot + 'images/pin/環保局.png', classes: 'epa_status' }],
        baseInfoMerge: {
            xfield: 'xx', yfield: 'yy', aftermerge: function (d) {
                var sdd = d;
            }
        }
    });
}
var Init清潔隊資源 = function ($_meter) {
    return $('<div class="row"><div class="col-md-12"></div></div>').appendTo($_meter).PinCtrl({
        map: app.map, 
        name: "清潔隊",
        stTitle: function (data) { return data.Town +'清潔隊' },
        loadBase: function (callback) { callback([]) },
        listTheme: 'info cleaningteam_status source-ctrl',
        loadInfo: datahelper.getCleaningTeam,
        infoFields: [
            { field: 'City', title: '單位', formatter: function (v, d) { return d.Town +'清潔隊'; }, showInfo: false },
            { field: 'Vehicle', title: '清潔車輛', unit: '台', showInList: false },
            { field: 'Disinfector', title: '清潔設備', unit: '台', showInList: false },
            { field: 'SolidDrugStateDisinfectant', title: '消毒藥品(固)', unit: '公斤', showInList: false },
            { field: 'LiquidDrugStateDisinfectant', title: '消毒藥品(液)', unit: '公升', showInList: false },
            { field: 'Toilet', title: '流動廁所', showInList: false }
        ],
        legendIcons: [{ 'name': '清潔隊', url: app.siteRoot + 'images/pin/清潔隊.png', classes: 'cleaningteam_status' }],
        baseInfoMerge: {
            xfield: 'xx', yfield: 'yy', aftermerge: function (d) {
                var sdd = d;
            }
        }
    });
}
var Init垃圾掩埋場 = function ($_meter) {
    return $('<div class="row"><div class="col-md-12"></div></div>').appendTo($_meter).PinCtrl({
        map: app.map, 
        name: "垃圾掩埋場",
        stTitle: function (data) { return data.name  },
        loadBase: function (callback) { callback([]) },
        listTheme: 'info landfill_InOperation_status source-ctrl',
        loadInfo: datahelper.getLandfill,
        infoFields: [
            { field: 'city', title: '縣市', showInList: false },
            { field: 'town', title: '鄉鎮', showInList: false },
            { field: 'name', title: '名稱', showInInfo:false },
            { field: 'manage', title: '管理機關', showInList: false },
            { field: 'state', title: '現況', showInList: false }
        ],
        legendIcons: [{ 'name': '營運中', url: app.siteRoot + 'images/pin/垃圾掩埋場_營運中.png', classes: 'landfill_InOperation_status  infowindow-title-white' },
            { 'name': '有餘裕量未封閉', url: app.siteRoot + 'images/pin/垃圾掩埋場_有餘裕量未封閉.png', classes: 'landfill_margin_status infowindow-title-white' },
            { 'name': '無餘裕量未封閉', url: app.siteRoot + 'images/pin/垃圾掩埋場_無餘裕量未封閉.png', classes: 'landfill_nomargin_status infowindow-title-white' }],
        //baseInfoMerge: {
        //    xfield: 'lon', yfield: 'lat'
        //},
        checkDataStatus: function (data, index) {
            
            return $.BasePinCtrl.helper.getDataStatusLegendIcon(this.settings.legendIcons, data.state);
        }
    });
}

var Init焚化廠 = function ($_meter) {
    return $('<div class="row"><div class="col-md-12"></div></div>').appendTo($_meter).PinCtrl({
        map: app.map,
        name: "焚化廠",
        pinInfoLabelMinWidth: '5.2rem',
        classes:'source-data',
        stTitle: function (data) { return data.incineratorname },
        loadBase: function (callback) { callback([]) },
        listTheme: 'info incineration_PuPu_status source-ctrl',
        loadInfo: datahelper.getIncineration,
        infoFields: [
            { field: 'address', title: '地址', showInList: true },
            { field: 'area', title: '面積',unit:'公頃', showInList: false },
            { field: 'operationtype', title: '營運型態', showInInfo: false },
            { field: 'dsnprcqt', title: '設計處理量',unit:'公噸/日', showInList: false },
            { field: 'incineratornum', title: '爐數', unit: '組', showInList: false },
            { field: 'management', title: '管理單位', showInList: false },
            { field: 'operationdept', title: '操作單位', showInList: false },
            { field: 'incinmanageragency', title: '監督機構', showInList: false }
        ],
        legendIcons: [{ 'name': '公有公營', url: app.siteRoot + 'images/pin/焚化廠_公有公營.png', classes: 'incineration_PuPu_status infowindow-title-white' },
            { 'name': '公有民營', url: app.siteRoot + 'images/pin/焚化廠_公有民營.png', classes: 'incineration_PuPr_status infowindow-title-white' },
            { 'name': '民有民營', url: app.siteRoot + 'images/pin/焚化廠_民有民營.png', classes: 'incineration_PrPr_status infowindow-title-white' }],
        checkDataStatus: function (data, index) {

            return $.BasePinCtrl.helper.getDataStatusLegendIcon(this.settings.legendIcons, data.operationtype);
        }
    });
}
var Init開口合約 = function ($_meter) {
    return $('<div class="row"><div class="col-md-12"></div></div>').appendTo($_meter).PinCtrl({
        map: app.map, 
        name: "開口合約",
        stTitle: function (data) { return data.OPName },
        loadBase: function (callback) { callback([]) },
        canFullInfowindow:true,
        listTheme: 'info opencontract_status source-ctrl',
        loadInfo: datahelper.getOpenContract,
        infoFields: [
            { field: 'OPName', title: '合約名稱', showInInfo: false },
            { field: 'City', title: '縣市', showInList: false },
            { field: 'TOWN', title: '鄉鎮', showInList: false },
            { field: 'Name', title: '類別', showInList: false },
            { field: 'Fac', title: '廠商', showInList: false },
            { field: 'Owner', title: '負責人', showInList: false },
            { field: 'TEL', title: '連絡電話', showInList: false },
            { field: 'MobileTEL', title: '行動電話', showInList: false },
            { field: 'OContractDateEnd', title: '合約截止日', showInList: false }
        ],
        legendIcons: [{ 'name': '開口合約', url: app.siteRoot + 'images/pin/開口合約.png', classes: 'opencontract_status' }],
        baseInfoMerge: {
            xfield: 'xx', yfield: 'yy'
        },
        pinInfoContent: function (data) {
            //data.Depth = 30;
            var that = this;
            var $_c = $($.BasePinCtrl.defaultSettings.pinInfoContent.call(this, data));

            var $_base = $_c.find('table');
            var $_detail = $('<div><table></div>')

            $_c.empty();

            $_tabpanel = helper.bootstrap.genBootstrapTabpanel($_c, undefined,'contract-info-tab', ['合約', '細項'], [$_base, $_detail],true);
            $_tabpanel.find('.tab-content > .tab-pane:first').addClass('show active');
            $_detail.find('table') .bootstrapTable({
                columns: [
                    { title: '項目', field: 'Items' },
                    { title: '單位', field: 'Unit' },
                    { title: '數量', field: 'Count' },
                    { title: '價格', field: 'Price' },
                    { title: '預算', field: 'Budge' }
                ],
                data: data.contract
            });
            return $_c[0].outerHTML;
        }

    });
}


var Init雷達迴波圖 = function ($_meter) {
    var _layer = undefined;
    var $_silder = undefined;
    var _timer = Date.now();
    var $_legend = $('#rader-legend');

    var $_p = $('<div class="row"><div class="col-md-12"></div></div>').appendTo($_meter);
    $_p.PinCtrl({
        map: app.map, name: '雷達迴波圖', useLabel: false, useList: false, autoReload: { auto: false, interval: 10 * 1000 }
    }).on($.BasePinCtrl.eventKeys.initUICompleted, function () {
        var _timerflag = undefined;
        var timerreload = function () {
            clearInterval(_timerflag);
            _timerflag = setInterval(function () {
                $_p.find('.pinswitch').trigger('change');
            }, 5 * 60 * 1000);
        }
        $_p.find('.pinswitch').off('change').on('change', function () {
            var s = $(this).is(':checked');
            if (Date.now() - _timer >= 60 * 1000) {
                if (_layer) {
                    _layer.remove();
                    _layer = null;
                    $_silder.remove();
                }
                _timer = Date.now();
            }
            if (!_layer && s) {
                helper.misc.showBusyIndicator($_p, { timeout :10000});
                var gparas = JSON.parse(JSON.stringify(L.DouLayer.Qqesums.DefaulParas));
                gparas.url = app.siteRoot + 'rad/rt';
                gparas.parser = L.DouLayer.Qqesums.Parser.sqpe;
                $.getJSON(gparas.url, function (r) {
                    helper.misc.hideBusyIndicator($_p);
                    if (!r) {
                        $("#other-layer-info .rader-layer").removeClass('offdisplay');//.text(_linfo);
                        $("#other-layer-info .rader-layer-time").text('-無資料');
                        return;
                    }
                    gparas.parser(gparas, r.Content);

                    _layer = L.DouLayer.gridRectCanvas({ 'opacity': 1, ycellsize: gparas.ycellsize, xcellsize: gparas.xcellsize, noMask: true });//.addTo(app.map);
                    _layer.on('mousemove', function (evt) {
                        if (!L.Browser.mobile)
                            _layer.bindTooltip('dBZ:' + evt.griddata.val, { className: "qpesums_tooltip" }).openTooltip(evt.latlng);
                    }).on('mouseout', function (evt) {
                        if (!L.Browser.mobile && _layer.getTooltip() != null)
                            _layer.closeTooltip();
                    });
                    //用$.map 資料太大會有Maximum call stack size exceeded
                    var gs = L.DouLayer.Qqesums.gridData2cellData(gparas.datas, gparas.colorDef);//$.map(gdata.datas, function (val, i) {return { lng: val[0], lat: val[1], color: getcolor(gdata.colorDef, val[2]) }});

                    $_legend.empty().show();
                    L.DouLayer.Qqesums.genLegend($_legend, gparas.colorDef, 24, 16, 'dBZ', '雷達迴波');
                    _layer.setData(gs);
                    $_silder = bindSilder($_p, _layer);

                    $("#other-layer-info .rader-layer").removeClass('offdisplay');//.text(_linfo);
                    $("#other-layer-info .rader-layer-time").text(helper.format.JsonDateStr2Datetime(r.Datetime).DateFormat('MM/dd HH:mm:ss'));
                    _layer.addTo(app.map);
                });
                timerreload();
            } else {
                if (_layer) {
                    if (s) {
                        _layer.addTo(app.map);
                        $_silder.removeClass('offdisplay');
                        $("#other-layer-info .rader-layer").removeClass('offdisplay');
                    }
                    else {
                        _layer.remove();
                        $_silder.addClass('offdisplay');
                        $("#other-layer-info .rader-layer").addClass('offdisplay');
                    }
                }
                else
                    $("#other-layer-info .rader-layer").addClass('offdisplay');
            }
            if ($("#other-layer-info .rader-layer").hasClass('offdisplay'))
                $_legend.hide();
            else
                $_legend.show();
        });
    });


    return $_p;
}

var Init累積雨量圖 = function ($_meter) {
    //https://cwbopendata.s3.ap-northeast-1.amazonaws.com/DIV2/O-A0040-003.kmz
    var _layer = undefined;
    var $_silder = undefined;
    var _timer = Date.now();
    var $_legend = $('#dayrainfall-legend');
    var _legendColorDef = [{ min: 300, max: 5000, color: "#FFD6FE" }, { min: 200, max: 300, color: "#FE38FB" },
    { min: 150, max: 200, color: "#DB2DD2" }, { min: 130, max: 150, color: "#AB1FA2" },
    { min: 110, max: 130, color: "#AA1801" }, { min: 90, max: 110, color: "#D92203" },
    { min: 70, max: 90, color: "#FF2A06" }, { min: 50, max: 70, color: "#FFA81D" },
    { min: 40, max: 50, color: "#FFD328" }, { min: 30, max: 40, color: "#FEFD31" },
    { min: 20, max: 30, color: "#00FB30" }, { min: 15, max: 20, color: "#26A31B" },
    { min: 10, max: 15, color: "#0177FC" }, { min: 6, max: 10, color: "#00A5FE" },
    { min: 2, max: 6, color: "#01D2FD" }, { min: 1, max: 2, color: "#9EFDFF" }, { min: 0, max: 1, color: "#EDEDE6" }];
    var $_p = $('<div class="row"><div class="col-md-12"></div></div>').appendTo($_meter);
    var $_p = $('<div class="row"><div class="col-md-12"></div></div>').appendTo($_meter);
    $_p.PinCtrl({
        map: app.map, name: '累積雨量圖', useLabel: false, useList: false
    }).on($.BasePinCtrl.eventKeys.initUICompleted, function () {
        var _timerflag = undefined;
        var timerreload = function () {
            clearInterval(_timerflag);
            _timerflag = setInterval(function () {
                $_p.find('.pinswitch').trigger('change');
            }, 5 * 60 * 1000);
        }
        $_p.find('.pinswitch').off('change').on('change', function () {
            var s = $(this).is(':checked');
            if (Date.now() - _timer >= 60 * 1000) {
                if (_layer) {
                    _layer.remove();
                    _layer = null;
                    $_silder.remove();
                }
                _timer = Date.now();
            }
            if (!_layer && s) {
                helper.misc.showBusyIndicator();
                $.getJSON(app.siteRoot + 'rain/img/dayrainfall', function (r) {
                    helper.misc.hideBusyIndicator();
                    var url = r.Url;
                    //var time = r.cwbopendata.dataset.time.obsTime;
                    var x1 = r.MaxX;
                    var x2 = r.MinX;
                    var y1 = r.MaxY;
                    var y2 = r.MinY;
                    var _linfo = (r.Time ? helper.format.JsonDateStr2Datetime(r.Time).DateFormat('MM/dd HH:mm:ss') : '');

                    var imageBounds = [[y1, x1], [y2, x2]];
                    app.map.createPane('cloudimage1').style.zIndex = 350;
                    //var newOverlay = new google.maps.GroundOverlay(url, imageBounds, { map: this.map, opacity: this.currentOpacity }); //1.東南亞imageBounds對不齊??;2.GroundOverlay zoom in out會卡卡的
                    _layer = L.imageOverlay(url, imageBounds, { opacity: 1, pane: 'cloudimage1' }).addTo(app.map);
                    $_silder = bindSilder($_p, _layer);
                    $("#other-layer-info .sum-rainfall-layer").removeClass('offdisplay');//.text(_linfo);
                    $("#other-layer-info .sum-rainfall-layer-time").text(_linfo);
                    $_legend.empty().show();

                    L.DouLayer.Qqesums.genLegend($_legend, _legendColorDef, 24, 16, '毫米(mm)', '累積雨量');
                });
                timerreload();
            } else {
                if (_layer) {
                    if (s) {
                        _layer.addTo(app.map);
                        $_silder.removeClass('offdisplay');
                        $("#other-layer-info .sum-rainfall-layer").removeClass('offdisplay');
                    }
                    else {
                        _layer.remove();
                        $_silder.addClass('offdisplay');
                        $("#other-layer-info .sum-rainfall-layer").addClass('offdisplay');
                    }
                }
                else
                    $("#other-layer-info .rader-layer").addClass('offdisplay');
            }
            if ($("#other-layer-info .sum-rainfall-layer").hasClass('offdisplay'))
                $_legend.hide();
            else
                $_legend.show();
        });
    });

    return $_p;
}

var InitQpf060minRt = function ($_meter) {
    //https://cwbopendata.s3.ap-northeast-1.amazonaws.com/DIV2/O-A0040-003.kmz
    var _layer = undefined;
    var $_silder = undefined;
    var _timer = Date.now();
    var $_legend = $('#qpesums-legend');
    var $_p = $('<div class="row non-public"><div class="col-md-12"></div></div>').appendTo($_meter);
    $_p.PinCtrl({
        map: app.map, name: '預報1小時降雨', useLabel: false, useList: false
    }).on($.BasePinCtrl.eventKeys.initUICompleted, function () {
        var _timerflag = undefined;
        var timerreload = function () {
            clearInterval(_timerflag);
            _timerflag = setInterval(function () {
                $_p.find('.pinswitch').trigger('change');
            }, 5 * 60 * 1000);
        }
        $_p.find('.pinswitch').off('change').on('change', function () {
            var s = $(this).is(':checked');
            if (Date.now() - _timer >= 60 * 1000) {
                if (_layer) {
                    _layer.remove();
                    _layer = null;
                    $_silder.remove();
                }
                _timer = Date.now();
            }
            if (!_layer && s) {
                helper.misc.showBusyIndicator();
                var gparas = JSON.parse(JSON.stringify(L.DouLayer.Qqesums.DefaulParas));
                gparas.url = app.siteRoot + 'api/qpesums/qpf060min/rt';
                gparas.parser = L.DouLayer.Qqesums.Parser.sqpe;
                $.getJSON(gparas.url, function (r) {
                    helper.misc.hideBusyIndicator();
                    if (!r) {
                        $("#other-layer-info .qpfqpe60-layer").removeClass('offdisplay');//.text(_linfo);
                        $("#other-layer-info .qpfqpe60-layer-time").text('-無資料');
                        return;
                    }
                    gparas.parser(gparas, r.Content);

                    _layer = L.DouLayer.gridRectCanvas({ 'opacity': 1, ycellsize: gparas.ycellsize, xcellsize: gparas.xcellsize, noMask: true });//.addTo(app.map);
                    _layer.on('mousemove', function (evt) {
                        if (!L.Browser.mobile)
                            _layer.bindTooltip('降雨量:' + evt.griddata.val, { className: "qpesums_tooltip" }).openTooltip(evt.latlng);
                        //_layer.bindTooltip(JSON.stringify(evt.latlng) + "<br>" + JSON.stringify(evt.griddata)).openTooltip(evt.latlng);
                    }).on('mouseout', function (evt) {
                        if (!L.Browser.mobile && _layer.getTooltip() != null)
                            _layer.closeTooltip();
                    });
                    //用$.map 資料太大會有Maximum call stack size exceeded
                    var gs = L.DouLayer.Qqesums.gridData2cellData(gparas.datas, gparas.colorDef);//$.map(gdata.datas, function (val, i) {return { lng: val[0], lat: val[1], color: getcolor(gdata.colorDef, val[2]) }});
                    //_layer._map = app.map;
                    //_layer.addTo(app.map);

                    $_legend.empty().show();
                    L.DouLayer.Qqesums.genLegend($_legend, gparas.colorDef, 24, 16, '毫米(mm)', '預報雨量');
                    _layer.setData(gs);
                    $_silder = bindSilder($_p, _layer);

                    $("#other-layer-info .qpfqpe60-layer").removeClass('offdisplay');//.text(_linfo);
                    $("#other-layer-info .qpfqpe60-layer-time").text(helper.format.JsonDateStr2Datetime(r.Datetime).DateFormat('MM/dd HH:mm:ss'));
                    _layer.addTo(app.map);
                });

                timerreload();
            } else {
                if (_layer) {
                    if (s) {
                        _layer.addTo(app.map);
                        $_silder.removeClass('offdisplay');
                        //$_legend.removeClass('offdisplay');
                        $("#other-layer-info .qpfqpe60-layer").removeClass('offdisplay');
                    }
                    else {
                        _layer.remove();
                        $_silder.addClass('offdisplay');
                        //$_legend.addClass('offdisplay');
                        $("#other-layer-info .qpfqpe60-layer").addClass('offdisplay');
                    }
                }
                else
                    $("#other-layer-info .rader-layer").addClass('offdisplay');
            }
            if ($("#other-layer-info .qpfqpe60-layer").hasClass('offdisplay'))
                $_legend.hide();
            else
                $_legend.show();
        });
    });

    return $_p;
}

var bindSilder = function ($_c, g) {
    $_c.find('.opacity-container').remove();
    $_oslider = $('<div class="col-12 opacity-container"><div class="opacity-slider" title="透明度"></div></div>').appendTo($_c).find('.opacity-slider')
        .gis_layer_opacity_slider({
            map: app.map,
            range: "min",
            max: 100,
            min: 5,
            value: $_c[0].currentOpacity || 90,
            setOpacity: function (_op) {
                $_c[0].currentOpacity = _op * 100;
                g.setOpacity(_op);
            }
        });//.addClass('offdisplay');
    return $_oslider;
}