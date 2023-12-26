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
        $('#typh').typh({ map: app.map, initEvent: $.menuctrl.eventKeys.popu_init_before }).on($.typh.eventKeys.warn_time_change, function (evt, warn, isrealtime) {
            //if (that.$cloud.instance.isInitCompleted)
            //    that.$cloud.instance.setDatetime(JsonDateStr2Datetime(warn.DateTime));
        });
    });
})