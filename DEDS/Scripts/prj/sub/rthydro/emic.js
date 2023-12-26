(function ($) {
    'use strict';
    var pluginName = 'emic';
    var pluginclass = function (element, e) {
        if (e) {
            e.stopPropagation();
            e.preventDefault();
        }
        this.$element = $(element);
        this.settings = { map: undefined, autoShow: true };
        this.currentevent = undefined;
        this.$pinctrl = undefined;
        this.$areactrl = undefined;
        this._floodAreaImageGroundOverlay = [];
        this.currentAllFloodAreaData = [];
        this.currentDisplayFloodAreaData = [];
        this.isShowArea = false;
        this.$tooltip = $('<i  data-toggle="tooltip" data-html="true" data-placement="right" data-animation="false" data-trigger="manual" style="position:absolute !important;" />').appendTo('body');
    };
    pluginclass.prototype = {
        constructor: pluginclass,
        init: function (options) {

            $.extend(this.settings, options);
            var that = this;
            if (this.settings.autoShow)
                that.initUi();
            else
                this.$element.on($.menuctrl.eventKeys.popu_init_before, function () {
                    that.initUi();
                });
        },
        initUi: function () {
            var that = this;

            var hasFirstRepaintPinCompleted = false;
            var $_eventname = $('<div><lable>目前事件:</label><label></label></div>').appendTo(this.$element).find('label');
            this.$pinctrl = $('<div>').appendTo(this.$element).on($.BasePinCtrl.eventKeys.initUICompleted, function () {
                that.$pinctrl.find('.pinswitch').prop('checked', true).trigger('change');
                that.$pinctrl.find('.pinlist').prop('checked', true).trigger('change');
                that.$pinctrl.find('.ctrl').hide();
                $filterContainer.insertBefore(that.$pinctrl.find('.legend'));

                $('.search', that.$element).appendTo($('<div class="col-3">').appendTo($filterContainer));

                $_eventname.parent().insertAfter($filterContainer);
            }).on($.BasePinCtrl.eventKeys.afterSetdata, function (e, ds, dds) {
                that.$element.trigger('get-data-complete', [ds]);
            }).on($.BasePinCtrl.eventKeys.repaintPinCompleted, function (e, ds) {
                //重劃淹水推估
                //that.currentDisplayFloodAreaData = [];
                //$.each(that.currentAllFloodAreaData, function () {
                //    var _a = this;
                //    if ($.grep(ds, function (_d) { return _a.PK_ID == _d.PK_ID; }).length > 0)
                //        that.currentDisplayFloodAreaData.push(_a);
                //});
                //that.$areactrl.PolygonCtrl('reload');
            });

            var $filterContainer = $('<div class="row filter-container">').appendTo(this.$element);//.insertBefore(that.$pinctrl.find('.legend'));

            //縣市、鄉鎮篩選
            var $countySelect = $('<div class="col-2">').appendTo($filterContainer).countySelect({ containerTW: true, autoFitBounds: false, showLabel: false }).on($.countySelect.eventKeys.change, function () {

                var dtype = $typeSelect.val();

                var _c = $countySelect.countySelect('getCountyName');
                var _cityCode = _c == datahelper.AllTW ? undefined : $.grep(window.fhyCity, function (c) {
                    return c.Name.zh_TW == _c || c.Name.zh_TW == _c.replace('台', '臺');
                })[0];
                //var _c1 = _c.replace("台", "臺");
                //var _c2 = _c.replace("臺", "台");
                //that.$pinctrl.PinCtrl('setFilter', function (d) {
                //    return (_c == datahelper.AllTW || _c == d.COUNTY_N || _c1 == d.COUNTY_N || _c2 == d.COUNTY_N) && (dtype == 0 || d.DISASTER_MAIN_TYPE == dtype);
                //});
                setFilter();

                //鄉鎮區市
                $townSelect.empty();
                $('<option value="0">--</option>').appendTo($townSelect);
                $townSelect.off('change');

                if (_cityCode != undefined) {
                    window.datahelper.getFHYTown(_cityCode.Code, function (ts) {
                        $.each(ts, function () {
                            var t = this;
                            $('<option value="' + t.Name.zh_TW + '">' + t.Name.zh_TW + '</option>').appendTo($townSelect);
                        });
                        $townSelect.on('change', function () {
                            setFilter();
                            //dtype = $typeSelect.val();
                            //var tc = $townSelect.val();
                            //if (tc == 0)
                            //    $countySelect.trigger($.countySelect.eventKeys.change);
                            //else {
                            //    that.$pinctrl.PinCtrl('setFilter', function (d) {
                            //        return (_c == 0 || _c == d.COUNTY_N || _c1 == d.COUNTY_N || _c2 == d.COUNTY_N) && (tc == 0 || tc == d.TOWN_N) && (dtype == 0 || d.DISASTER_MAIN_TYPE==dtype);
                            //    });
                            //}
                        });
                    });
                }
            });
            var $townSelect = $('<div class="col-3"><select class="form-control"></select></div>').appendTo($filterContainer).find('select');
            $('<option value="0">--</option>').appendTo($townSelect);

            //var types = ['路樹災情', '廣告招牌災情', '道路、隧道災情', '橋梁災情', '鐵路、高鐵捷運災情', '積淹水災情',
            //    '土石災情', '建物毀損', '水利設施災情', '民生、基礎設施災情', '車輛、交通事故', '環境汙染', '火災', '其他災情'];
            var types = ['路樹災情', '廣告招牌災情', '道路、隧道災情','積淹水災情',
                '土石災情', '民生、基礎設施災情', '環境汙染', '火災', '其他災情'];
            var $typeSelect = $('<div class="col-4"><select class="form-control"></select></div>').appendTo($filterContainer).find('select');
            $('<option value="0">所有災情類別</option>').appendTo($typeSelect);
            for (var i=0; i < types.length; i++) {
                $('<option value="' + types[i] + '">' + types[i] + '</option>').appendTo($typeSelect);
            }
            $typeSelect.on('change', function () {
                setFilter();
            });

            var setFilter = function () {
                helper.misc.showBusyIndicator(that.$element);
                setTimeout(function () {
                    var _c = $countySelect.countySelect('getCountyName');
                    var _c1 = _c.replace("台", "臺");
                    var _c2 = _c.replace("臺", "台");
                    
                    var tc = $townSelect.val();
                    var dtype = $typeSelect.val();

                    that.$pinctrl.PinCtrl('setFilter', function (d) {
                        return (_c == datahelper.AllTW || _c == d.COUNTY_N || _c1 == d.COUNTY_N || _c2 == d.COUNTY_N) && (tc == 0 || tc == d.TOWN_N) && (dtype == 0 || d.DISASTER_MAIN_TYPE == dtype);
                    });
                    helper.misc.hideBusyIndicator(that.$element);
                }, 10);
            }
            

            InitEmic(this.$pinctrl, {
                pinClick: function (d) { that.$element.trigger('pin-click', [d]); },
                loadInfo: function (dt, callback) {
                    datahelper.getEmic(function (d) {
                        if (d.EVENT_NAME && (d.EVENT_NAME.indexOf('演練') >= 0 || d.EVENT_NAME.indexOf('測試') >= 0 || d.EVENT_NAME.indexOf('平時作業') >= 0)) {
                            d.EVENT_NAME = '無事件';
                            d.DISASTER_LIST = undefined;
                        }
                        if (d.EVENT_NAME)
                            $_eventname.text(d.EVENT_NAME);
                        if (d.DISASTER_LIST && d.DISASTER_LIST.DISASTER_DATA) {  //僅show屬於types的災情類型
                            d.DISASTER_LIST.DISASTER_DATA = $.grep(d.DISASTER_LIST.DISASTER_DATA, function (e) {
                                return e.DISASTER_MAIN_TYPE != undefined && types.indexOf(e.DISASTER_MAIN_TYPE) >= 0;
                            });
                        }
                        callback(d);
                    });
                }
            });

        },
        setEvent: function (ev) {
            this.currentevent = ev;
            this.$pinctrl.PinCtrl('reload');
        },
        _showArea: function (_s) { //因有設minZoom，需處理是否呈現
            $.each(this._floodAreaImageGroundOverlay, function () {
                if (_s)
                    this.addTo(app.map);
                else
                    this.remove();
            });
        }
    }

    $.fn[pluginName] = function (arg) {

        var args, instance;

        if (!(this.data(pluginName) instanceof pluginclass)) {

            this.data(pluginName, new pluginclass(this[0]));
        }

        instance = this.data(pluginName);

        if (typeof arg === 'undefined' || typeof arg === 'object') {

            if (typeof instance.init === 'function') {
                instance.init(arg);
            }
            this.instance = instance;
            return this;

        } else if (typeof arg === 'string' && typeof instance[arg] === 'function') {

            args = Array.prototype.slice.call(arguments, 1);

            return instance[arg].apply(instance, args);

        } else {

            $.error('Method ' + arg + ' does not exist on jQuery.' + pluginName);

        }
    };

})(jQuery);