//登革熱資料https://od.cdc.gov.tw/eic/Dengue_Daily_last12m.json
$(document).ready(function () {
    var currentevent = undefined;
    mapHelper.createMap('leaflet', function () {
        
        var $_mainContainer = $('#main-ctrl');
        var isshownearcctv = false; //是否show鄰近cctv
        var shownearcctvcenter = undefined; //鄰近cctv中心點
        $('#_nearcctv').on('click', function () {
            $(this).toggleClass('selected');
            isshownearcctv = $(this).hasClass('selected');
            shownearcctv();
        })
        //組tab
        var $_tab = helper.bootstrap.genBootstrapTabpanel($_mainContainer, undefined, undefined,
            ['綜整資訊', '雨量站', '淹水感測', 'EMIC災情'],
            ['<div class="fsta-c">', '<div class="rain-c meter">', '<div class="fsensor-c meter">', '<div class="emic-c meter">']
        );
        $_tab.appendTo($_mainContainer).find('.nav').addClass('nav-fill');

        //統計
        var $_fsta= $_tab.find('.fsta-c').fsta().on('change-tab-index', function (e, i) {
            $_tab.find('.nav-item>.nav-link:eq('+i+')').tab("show");
        });

        //雨量
        $_tab.find('.rain-c').rain({ map: app.map }).on('get-data-complete', function (e, ds) {
            $_fsta.fsta('setRainData', ds); //get-data-complete取雨量資料完 , 計算雨量統計資料
        }).on('pin-click', function (e, d) {
            shownearcctvcenter = [d.X, d.Y];
            shownearcctv();
        });
       
        //淹水感測
        $_tab.find('.fsensor-c').fsensor({ map: app.map }).on('get-data-complete', function (e, ds) {
            $_fsta.fsta('setFloorData', ds);
        }).on('pin-click', function (e,d) {
            shownearcctvcenter = [d.X, d.Y];
            shownearcctv();
        });
        //通報災情
        $_tab.find('.emic-c').emic({ map: app.map }).on('get-data-complete', function (e, ds) {
            $_fsta.fsta('setEmicData', ds);
        }).on('pin-click', function (e, d) {
            shownearcctvcenter = [d.X, d.Y];
            shownearcctv();
        });

        var $_cctv =$('<div>').appendTo($('body')).cctv({ map: app.map });

        var shownearcctv = function () {
            if (isshownearcctv && shownearcctvcenter) {
                $_cctv.cctv('shownear', shownearcctvcenter[0], shownearcctvcenter[1]);
            }
            else
                $_cctv.cctv('hide');
        }

        var initCurrentEvent = function () {
            if (!currentevent)
                return;
            //$_ceventname.text(currentevent.EventName);
            $_tab.find('.fsta-c').fsta('setEvent', currentevent);
            $_tab.find('.emic-c').rdisaster('setEvent', currentevent);
        }
        ////取水利署事件
        //window.datahelper.loadWraEvents(function (d) {
        //    var _eventid = helper.misc.getRequestParas()['eventid'];
        //    //_eventid = 'T2004';
        //    //_eventid = 'R00443';// 'T2004';
        //    if (_eventid) {
        //        currentevent = $.grep(d, function (d) { return d.EventNo == _eventid; })[0];
        //        currentevent.Enabled = true;
        //    }
        //    else
        //        currentevent = d[0];
        //    if (currentevent.Enabled) {
        //        initCurrentEvent();
        //    }
        //});
        var $_cwbtyph;
        //$('a[href="#typh"]').off("click").on('click', function () {
        //    if (!$_cwbtyph) {
        //        $_cwbtyph = $("<div>").appendTo($('body')).KmlCtrl({
        //            map: app.map, useLabel: true, useList: false, useSilder: false, name: 'cwb-typh', type: $.BasePinCtrl.type.feature,
        //            autoReload: { auto:true, interval:  5*60*1000 },
        //            url: 'https://opendata.cwb.gov.tw/fileapi/v1/opendataapi/W-C0034-002?Authorization=CWB-AB2D9D1E-94B3-4F31-A9EE-2115D507CA49&downloadType=WEB&format=KMZ',
        //            pinsize: { x: 12, y: 12, minx: 8, maxx: 32, miny: 10, maxy: 32, step: 4, anchor: "cneter" },
        //            geometryOtherOptions: {
        //                pointToLayer: function (feature, latlng) {
        //                    return L.marker(latlng, {
        //                        icon: L.icon({
        //                            iconUrl: app.siteRoot + 'images/pin/'+feature.properties.Style.IconStyle.Icon.href
        //                        })
        //                    });
        //                }
        //            },
        //            checkDataStatus: function (data, index) {
        //                if (data.placemarkName == '路徑潛勢預測') {
        //                    data.kmlStatus.fillColor = "#ff0000";
        //                    data.kmlStatus.fillOpacity = 0.125;
        //                }
        //                return data.kmlStatus;
        //            }
        //        }).on($.BasePinCtrl.eventKeys.initUICompleted, function () {
        //            $_cwbtyph.find('.pinswitch').prop("checked", true).trigger('change');
        //        }).hide();
        //    }
        //    else {
        //        $_cwbtyph.find('.pinswitch').prop("checked", !$_cwbtyph.find('.pinswitch').is(':checked')).trigger('change');
        //    }
        //});
        $('#typh').typh({ map: app.map, initEvent: $.menuctrl.eventKeys.popu_init_before }).on($.typh.eventKeys.warn_time_change, function (evt, warn, isrealtime) {
            //if (that.$cloud.instance.isInitCompleted)
            //    that.$cloud.instance.setDatetime(JsonDateStr2Datetime(warn.DateTime));
        });
        var _trafficlayer;
        $('#traffic-ctrl').on('click', function () {
            var _turl = 'https://{s}.google.com/vt?hl=zh-TW&src=app&x={x}&y={y}&z={z}&s=Galil&lyrs=m@248149186,traffic|seconds_into_week:-1&style=15';
            $(this).toggleClass('selected');
            if (!_trafficlayer) {
                app.map.createPane('google-traffic-pane').style.zIndex = 201;
                _trafficlayer = L.tileLayer(_turl, { "subdomains": ["mts0", "mts1", "mts2", "mts3"], pane: 'google-traffic-pane' });//
            }
            if (app.map.hasLayer(_trafficlayer)) {
                _trafficlayer.remove();
            }
            else {
                _trafficlayer.addTo(app.map);
            }
        });

        //資源
        
        var $_source = $('#source-ctrl');
        $('<div class="item-title">氣象圖資</div>').appendTo($_source);
        Init雷達迴波圖($_source).on($.BasePinCtrl.eventKeys.initUICompleted, function () {
            $(this).find('.pinswitch').prop('checked', true).trigger('change');
        });
        Init累積雨量圖($_source).on($.BasePinCtrl.eventKeys.initUICompleted, function () {
            //$(this).find('.pinswitch').prop('checked', true).trigger('change');
        });

        InitPopulationDensity($_source);

        $('<div class="item-title">應變資源</div>').appendTo($_source);
        Init環保局資源($_source);
        Init清潔隊資源($_source);
        Init開口合約($_source);
        $('<div class="item-title">垃圾處裡</div>').appendTo($_source);
        Init垃圾掩埋場($_source);
        Init焚化廠($_source);

        if (helper.misc.getRequestParas()['dengue']=="true")
            InitDengue($_source);
    });
})