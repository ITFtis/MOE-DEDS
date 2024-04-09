window.app = window.app || {};
//if (!app.siteRoot)
//    app.siteRoot = helper.misc.getScriptPath("Scripts/gis/Main").indexOf('localhost') >= 0 ? "http://140.116.66.35/SGDS/" : helper.misc.getScriptPath("Scripts/gis/Main");
app.CSgdsRoot = "https://www.dprcflood.org.tw/SGDS/";
//app.epaapi = 'https://pj.ftis.org.tw/EPAAPI/';
app.epaapi = EPAAPI == '' ? 'https://pj.ftis.org.tw/EPAAPI/' : EPAAPI;

//app.siteRoot = 'https://pj.ftis.org.tw/DEDS/'


(function (window) {
    //var app.siteRoot = app.siteRoot;// helper.misc.getScriptPath("Scripts/gis/Main");
    window.wraLake = undefined;
    window.forestLake = undefined;

    window.forestEventManager = undefined;
    window.countyXY = undefined;

    var getData = function (url, paras, callback, option) {
        var _ajaxoptions = $.extend({
            url: url,
            type: "GET",
            dataType: "json",
            contentType: "application/json; charset=utf-8", //加contentType IE有問題，但放在server同一domain是OK的
            //async: _async,
            data: paras
        }, option);

        console.log(url + '參數:' + JSON.stringify(paras));
        $.ajax(_ajaxoptions)
            .done(function (dat, status) {
                var d = dat.d ? dat.d : (dat.Data ? dat.Data : dat);
                d = d.Data ? d.Data: d;
                callback(d); //dat.Data是fly v2
            }).fail(function (dat, status) {
                console.log('error:' + dat.responseText);
            });
    };

    //縣市資料
    var loadCountyXY = function (async) {
        if (!window.countyXY) {
            //getFHYCity(function (cs) {
            //    window.countyXY = cs;
            //})
            $.BasePinCtrl.helper.ajaxGeneralRequest({
                url: app.CSgdsRoot + "WS/FloodComputeWS.asmx/Countys ",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST",
                async: async
            }, function (d) {
                window.countyXY = d.d;
            });
        }
        return window.countyXY;
    }

    var countyXYContainTaiwan = undefined;
    //縣市資料(含全臺)
    var getCountyXYContains0 = function () {
        if (!countyXYContainTaiwan) {
            var cs = JSON.parse(JSON.stringify(datahelper.loadCountyXY(false)));
            //var cs = JSON.parse(JSON.stringify(window.countyXY));
            cs.splice(0, 0, { PK_ID: 0, CountyName: datahelper.AllTW });
        }
        return cs;
    }

    //土地利用類型
    var getLandUseType = function () {
        if (!window.landUseType) {
            $.BasePinCtrl.helper.ajaxGeneralRequest({
                url: app.CSgdsRoot + "WS/FloodComputeWS.asmx/LandUseType ",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST",
                async: false
            }, function (d) {
                window.landUseType = d.d;
            });
        }
        return window.landUseType;
    };

    var getEMISWATER_TYPE = function () {
        if (!window.EMISWATER_TYPE) {
            $.BasePinCtrl.helper.ajaxGeneralRequest({
                url: app.CSgdsRoot + "WS/FloodComputeWS.asmx/wsEMISWATER_TYPE ",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"
            }, function (d) {
                window.EMISWATER_TYPE = d.d;
            });
        }
        return window.EMISWATER_TYPE;
    };

    //取得事件清單資料，村里淹水災情事件
    var getFloodEvents = function (async) {
        //if (async==undefined) async = true;
        if (!window.floodEvents) {
            $.BasePinCtrl.helper.ajaxGeneralRequest({
                url: app.CSgdsRoot + "WS/FloodComputeWS.asmx/FloodEvents ",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST",
                //async: async
            }, function (d) {
                window.floodEvents = d.d;
            });
        }
        return window.floodEvents;
    }
    //取水利署事件清單 >>災情資訊查詢用
    var loadWraEvents = function (callback) {
        if (!window.waEvents) {
            $.BasePinCtrl.helper.ajaxGeneralRequest({
                url: app.CSgdsRoot + "WS/FloodComputeWS.asmx/WraEvents  ",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"
            }, function (d) {
                window.waEvents = d.d;
                callback(window.waEvents);
            });
        }
        else
            callback(window.waEvents);
    }

    
    //取單一演算結果DTM
    var loadDEMCalculateData = function (_FloodingClass, callback) {
        var _cd = $.extend({}, _FloodingClass);
        _cd.DATE = JsonDateStr2Datetime(_cd.DATE);
        if (_cd.Recession_DATE) _cd.Recession_DATE = JsonDateStr2Datetime(_cd.Recession_DATE);
        if (_cd.CREATE_DATE) _cd.CREATE_DATE = JsonDateStr2Datetime(_cd.CREATE_DATE);
        if (_cd.MODIFY_DATE) _cd.MODIFY_DATE = JsonDateStr2Datetime(_cd.MODIFY_DATE);
        $.BasePinCtrl.helper.ajaxGeneralRequest({
            url: app.CSgdsRoot + "WS/FloodComputeWS.asmx/GetDEMCalculateData",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: "POST",
            data: JSON.stringify({ computeDistance: 500, ds: _cd })
        }, function (d) {
            callback(d.d);
        });
    };
    //雨量站頻率分析資料
    var rainFrequencyData = function (stno, callback) {
        $.BasePinCtrl.helper.ajaxGeneralRequest({
            url: app.CSgdsRoot + "WS/FloodComputeWS.asmx/RainFrequencyData",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: "POST",
            data: JSON.stringify({ stno: stno })
        }, function (d) {
            callback(d.d);
        });
    };

    var convertFloodToUiObject = function (d) {
        console.log('淹水點:' + d.d.length);
        //flood:來至水利署(同一網格只用淹水+高程最大資料取代影響戶數及土地利用); floodarea:同一網格只用淹水+高程最大資料; handdrawflood:人工圈繪; statistics:僅用來至水利署且同一網格只用淹水+高程最大資料
        var _r = { flood: [], floodarea: [], handdrawflood: [], statistics: [] };
        var _floodkey = [];
        var _floodGroup = [];
        var _handdrawfloodkey = [];
        var _handdrawfloodGroup = [];

        var _handdrawfloodGridId = [];
        $.each(d.d, function () {
            if (!this.NOTIFICATION_Data) {
                console.log(this.EffectAddress);
                return;
            }
            $.extend(this, this.NOTIFICATION_Data);
            this.Described = this.NOTIFICATION_Data.Described;

            var _key = (this.GridId == 0 ? this.PK_ID : this.GridId) + this.COUNTY_NAME + this.TOWN_NAME;
            //if (Math.random() % 5 == 0)
            //    this.IsFromWraEMIS = false;
            if (this.IsFromWraEMIS) {
                //組group
                if (_floodkey.indexOf(_key) < 0) {
                    _floodkey.push(_key);
                    _floodGroup.push({ key: _key, g: [this] });
                }
                else {
                    var _g = $.grep(_floodGroup, function (_gg) {
                        return _gg.key == _key;
                    })[0];
                    _g.g.push(this);

                }
                _r.flood.push(this);
            }
            else {
                //組group
                if (_handdrawfloodkey.indexOf(_key) < 0) {
                    _handdrawfloodkey.push(_key);
                    _handdrawfloodGroup.push({ key: _key, g: [this] });
                }
                else {
                    var _g = $.grep(_handdrawfloodGroup, function (_gg) {
                        return _gg.key == _key;
                    })[0];
                    _g.g.push(this);

                }
                _r.handdrawflood.push(this);
            }


        });
        //flood找出最大淹水深度+高程,並改同一group計算值
        $.each(_floodGroup, function () {
            var maxdata = this.g[0];
            var maxdataidx = 0;
            if (this.g.length != 1) {
                ////找出最大淹水深度+高程

                $.each(this.g, function (_idx) {
                    if ((this.DEPTH / 100 + this.Z_D) > (maxdata.DEPTH / 100 + maxdata.Z_D)) {
                        maxdata = this;
                        maxdataidx = _idx;
                    }
                });

            }
            $.each(this.g, function (_idx) {
                if (_idx != maxdataidx) {
                    this._Land = this.Land;
                    this.Land = maxdata.Land;
                    this._AffectStat = this.AffectStat;
                    this.AffectStat = maxdata.AffectStat;
                }

                //infoField用
                this.AffectHouse = maxdata.AffectStat ? maxdata.AffectStat.TotalHouse : 0;
                this.AffectArea = maxdata.AffectStat ? maxdata.AffectStat.TotalArea : 0;
                this.AffectHouse30cmUp = maxdata.AffectStat ? maxdata.AffectStat.TotalHouse30cmUp : 0;
                this.AffectArea30cmUp = maxdata.AffectStat ? maxdata.AffectStat.TotalArea30cmUp : 0;
                this.AffectHouse50cmUp = maxdata.AffectStat ? maxdata.AffectStat.TotalHouse50cmUp : 0;
                this.AffectArea50cmUp = maxdata.AffectStat ? maxdata.AffectStat.TotalArea50cmUp : 0;
            });
            _r.statistics.push(maxdata); //加入statistics
            _r.floodarea.push(maxdata);     //加入floodarea
        });
        //handdrawflood找出最大淹水深度+高程,並改同一group計算值
        $.each(_handdrawfloodGroup, function () {
            var maxdata = this.g[0];
            if (this.g.length != 1) {
                ////找出最大淹水深度+高程

                $.each(this.g, function () {
                    if ((this.DEPTH / 100 + this.Z_D) > (maxdata.DEPTH / 100 + maxdata.Z_D))
                        maxdata = this;
                });
            }
            $.each(this.g, function () {
                this._Land = this.Land;
                this.Land = maxdata.Land;
                this._AffectStat = this.AffectStat;
                this.AffectStat = maxdata.AffectStat;

                //infoField用
                this.AffectHouse = maxdata.AffectStat ? maxdata.AffectStat.TotalHouse : 0;
                this.AffectArea = maxdata.AffectStat ? maxdata.AffectStat.TotalArea : 0;
                this.AffectHouse30cmUp = maxdata.AffectStat ? maxdata.AffectStat.TotalHouse30cmUp : 0;
                this.AffectArea30cmUp = maxdata.AffectStat ? maxdata.AffectStat.TotalArea30cmUp : 0;
                this.AffectHouse50cmUp = maxdata.AffectStat ? maxdata.AffectStat.TotalHouse50cmUp : 0;
                this.AffectArea50cmUp = maxdata.AffectStat ? maxdata.AffectStat.TotalArea50cmUp : 0;
            });
            _r.floodarea.push(maxdata);     //加入floodarea
        });

        return _r;
    }

    //
    var estimateFloodingComputeForLightweightDatas = function (floodings, callback) {
        $.BasePinCtrl.helper.ajaxGeneralRequest({
            url: app.CSgdsRoot + "WS/FloodComputeWS.asmx/EstimateFlooding",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: "POST",
            data: JSON.stringify({ floodings: floodings})
        }, function (d) {


            callback(convertFloodToUiObject(d));
            // callback(_r);
            //$.each(_floodkey, function () {
            //    var _g = $.grep(_r.flood, function () {
            //    })
            //});

            //_r.flood = $.grep(d.d, function (_d) {
            //    return _d.IsFromWraEMIS;
            //});
            //_r.handdrawflood = $.grep(d.d, function (_d) {
            //    return !_d.IsFromWraEMIS;
            //});
        });
    }

    //取淹水演算結果
    var loadFloodComputeForLightweightDatas = function (st, et, countyID, datatype, callback) {
        $.BasePinCtrl.helper.ajaxGeneralRequest({
            url: app.CSgdsRoot + "WS/FloodComputeWS.asmx/GetFloodComputeForLightweightData",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: "POST",
            data: JSON.stringify({ beginDT: st, endDT: et, computeDistance: 500, CountyID: countyID, dataType:datatype})
        }, function (d) {
           

            callback(convertFloodToUiObject(d));
            // callback(_r);
            //$.each(_floodkey, function () {
            //    var _g = $.grep(_r.flood, function () {
            //    })
            //});

            //_r.flood = $.grep(d.d, function (_d) {
            //    return _d.IsFromWraEMIS;
            //});
            //_r.handdrawflood = $.grep(d.d, function (_d) {
            //    return !_d.IsFromWraEMIS;
            //});
        });
    }

    //取淹水設施結果
    var loadEMISFacilitys = function (st, et, countyID, datatype, callback) {
        $.BasePinCtrl.helper.ajaxGeneralRequest({
            url: app.CSgdsRoot + "WS/FloodComputeWS.asmx/wsEMISWaterByTime",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: "POST",
            data: JSON.stringify({ startDate: st, endDate: et})
        }, function (d) {
            var _r = null;
            if (d && d.d)
                _r = $.grep(d.d, function (_f) {
                    var _append = false;
                    if (datatype === undefined || datatype == 2)
                        _append = true;
                    else if (datatype == 0 && !_f.IsTest)
                        _append = true;
                    else if (datatype == 1 && _f.IsTest)
                        _append = true;
                    return _append;
                });
            callback( _r);
        });
    }

    var getFHYFloodSensorInfoLast24Hours_Address = function (address, callback) {
        $.BasePinCtrl.helper.ajaxGeneralRequest({
            url: app.CSgdsRoot + "WS/FHYBrokerWS.asmx/GetFHYFloodSensorInfoLast24Hours_Address",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ 'address': address }),
            type: "POST"
        }, function (d) {
            callback(d.d.Data);
        });
    }
    var getFHYFloodSensorStation = function (callback) {
        getData(app.CSgdsRoot + 'WS/FHYBrokerWS.asmx/GetFHYFloodSensorStation', undefined, callback, { type: 'POST' });
    }
    var getFHYFloodSensorInfoRt = function (dt, callback) {
        getData(app.CSgdsRoot + 'WS/FHYBrokerWS.asmx/GetFHYFloodSensorInfoRt', undefined, callback, { type: 'POST' });
    }
    var getFHYFloodSensorInfoLast24Hours = function (id, callback) {
        getData(app.CSgdsRoot + 'WS/FHYBrokerWS.asmx/GetFHYFloodSensorInfoLast24Hours', JSON.stringify({ sensorUUID: id }), callback, { type: 'POST' });
    }


    var getAllTown = function (callback) {
        if (window.alltown) {
            callback(window.alltown);
        }
        else
            //$.get(app.siteRoot+'Data/Town.json', function (ts) {
            //$.ajax({ url: app.siteRoot + 'Data/Town.json', async: false }, function (ts) {
            //    window.alltown = ts;
            //    callback(ts);
            //});
        helper.data.get(app.siteRoot + 'Data/Town.json', function (ts) {
            window.alltown = ts;
            callback(ts);
        }, { async: false })
    }
    var getFHYCity = function (callback) {
        //getAllTown(function (ts) {
        //    var t = {};
        //    var r = [];
        //    $.each(ts, function () {
        //        if (!t[this.CityCode]) {
        //            t[this.CityCode] = this.CityCode;
        //            r.push({ PK_ID: this.CityCode, CountyName: this.CityName, Code: this.CityCode, Name: { zh_TW: this.CityName } });
        //        }
        //    });
        //    window.fhyCity = r;
        //    callback(r);
        //})

        if (window.fhyCity) {
            callback(window.fhyCity);
        }
        else
            getData(app.CSgdsRoot + "WS/FHYBrokerWS.asmx/GetFHYCity", undefined, function (d) {
                window.fhyCity = d;
                callback(window.fhyCity)
            }, { type: 'POST', async:false });
    }
    var getFHYTown = function (cityCode, callback) {
        var k = 'fhyTown' + cityCode;
        //var r = $.grep(window.alltown, function (_v) {
        //    return _v.CityCode == cityCode;
        //})
        //callback(r);
        if (window[k]) {
            callback(window[k]);
        }
        else
            getData(app.CSgdsRoot + "WS/FHYBrokerWS.asmx/GetFHYTown", JSON.stringify({ cityCode: cityCode }), function (d) {
                window[k] = d;
                callback(window[k])
            }, { type: 'POST' });
    }

    var getAllCctv = function (callback) {
        helper.data.get(app.siteRoot + 'api/fmg/get/allbase', callback);
    }
    var getCctvCameras = function (id, sid) {
        var r = null;
        helper.data.get(app.siteRoot + 'api/fmg/get/cctv/' + id + '/' + sid, function (ds) {
            r= ds;
        }, { async: false })
        return r;
    }
    var getEmic = function (callback) {
        helper.data.get(app.siteRoot + 'api/emic/rt', callback);
    }

    var getEpaSource = function (dt,callback) {
        helper.data.get(app.epaapi + 'GetCount', function (ds) {
            var r = $.grep(ds, function (d) { return d.Town == '環保局'; });
            callback(r);
        }, undefined, true);
    }
    var getCleaningTeam = function (dt,callback) {
        helper.data.get(app.epaapi + 'GetCount', function (ds) {
            var r = $.grep(ds, function (d) { return d.Town != '環保局'; });
            callback(r);
        }, undefined, true);
    }
    var getOpenContract = function (dt,callback) {
        helper.data.get(app.epaapi + 'OpenContract', callback, undefined,true);
    }
    var getLandfill = function (dt,callback) {
        helper.data.get(app.siteRoot + 'Data/landfill.json', callback, undefined, true);
    }

    var getIncineration = function (dt, callback) {
        helper.data.get(app.siteRoot + 'Data/incineration.json', callback, undefined, true);
    }

    var getVehicleCount = function (callback) {
        helper.data.get(app.epaapi + 'vehicleCount', callback, undefined, false);
    }
    var getVehicletable = function (city,callback) {
        helper.data.get(app.epaapi + 'vehicletable?City='+city, callback, undefined, false);
    }
    var getVehicleType = function (callback) {
        helper.data.get(app.epaapi + 'vehicleType', callback, undefined, false);
    }
    var getDisinfectorCount = function (callback) {
        helper.data.get(app.epaapi + 'DisinfectorCount', callback, undefined, false);
    }
    var getDisinfectortable = function (city, callback) {
        helper.data.get(app.epaapi + 'Disinfectortable?City=' + city, callback, undefined, false);
    }
    var getDisinfectantCount = function (callback) {
        helper.data.get(app.epaapi + 'DisinfectantCount', callback, undefined, false);
    }
    var getDisinfectanttable = function (city, callback) {
        helper.data.get(app.epaapi + 'Disinfectanttable?City=' + city, callback, undefined, false);
    }
    var getDisinfectantStateTable = function (callback) {
        helper.data.get(app.epaapi + 'DisinfectantStateTable', callback, undefined, false);
    }
    var getEquipmentType = function (callback) {
        helper.data.get(app.epaapi + 'Equipment', callback, undefined, false);
    }
    
    //預抓資料
    var preInitData = function () {
        //if ($("#forestLake").length>0) //服務連不上
        //    datahelper.loadForestEventManager(true);
        datahelper.getAllTown(function () { });
        getFHYCity(function () { });
        var countys = datahelper.loadCountyXY(false);
        datahelper.getFloodEvents(false);

        boundary.helper.GetBoundaryData(boundary.data.County, function (b) { });
        //boundary.helper.GetBoundaryData(boundary.data.Town, function (b) { });

        //datahelper.loadAllVillagePolygons(undefined, app.county ? [app.county] : undefined);

        //datahelper.getLandUseType();
        //datahelper.getEMISWATER_TYPE();
        
        
       
        $('body').trigger('preInitData-completed');
    }
    window.datahelper = {
        preInitData: preInitData,                                                   //初始基本資料
        loadCountyXY: loadCountyXY,                                                 //縣市資料
        getCountyXYContains0: getCountyXYContains0,                                 //縣市資料(含全臺)
        getAllTown: getAllTown,                                                     //鄉鎮資料*/ >>淹水感測用

        getFloodEvents: getFloodEvents,                                             //取淹水事件資料
        getLandUseType: getLandUseType,                                             //土地利用種類
        getEMISWATER_TYPE: getEMISWATER_TYPE,                                       //水利設施種類
        loadWraEvents: loadWraEvents,                                               //取水利署事件清單
        //loadWraLake: loadWraLake,                                                   //水利署堰塞湖
        //loadForestLake: loadForestLake,                                             //林務局堰塞湖
        //loadForestEventManager: loadForestEventManager,


        loadFloodComputeForLightweightDatas: loadFloodComputeForLightweightDatas,   //取積淹水演算資料(不含DEM,Adress)
        loadEMISFacilitys: loadEMISFacilitys,                                       //水利設施資料
        loadDEMCalculateData: loadDEMCalculateData,                                 //取淹水網格DEM資料

        estimateFloodingComputeForLightweightDatas: estimateFloodingComputeForLightweightDatas, //災情預估


        rainFrequencyData: rainFrequencyData,                                        //雨量站頻率分析資料

        getEmic: getEmic,
        //fmg cctv
        getAllCctv : getAllCctv,
        getCctvCameras : getCctvCameras,
        //防災資源
        getEpaSource: getEpaSource,                                                 //環保局
        getCleaningTeam: getCleaningTeam,                                           //清潔隊
        getOpenContract: getOpenContract,                                           //開口合約
        getLandfill: getLandfill,                                                   //垃圾掩埋場
        getIncineration: getIncineration,                                           //焚化廠

        getVehicleCount: getVehicleCount,                                           //清潔車輛統計
        getVehicletable: getVehicletable,                                           //清潔車輛清單-單一縣市
        getVehicleType: getVehicleType,                                             //清潔車輛類別
        getDisinfectorCount: getDisinfectorCount,                                   //消毒設備統計
        getDisinfectortable: getDisinfectortable,                                   //消毒設備清單-單一縣市
        getDisinfectantCount: getDisinfectantCount,                                 //消毒藥劑統計
        getDisinfectanttable: getDisinfectanttable,                                 //消毒藥劑清單-單一縣市
        getDisinfectantStateTable: getDisinfectantStateTable,                       //消毒藥劑統計-依狀態

        getEquipmentType: getEquipmentType,                                         //設備種類                  

        //淹水感測用
        getFHYFloodSensorInfoLast24Hours_Address: getFHYFloodSensorInfoLast24Hours_Address, //取淹水感測最後24小時資料
        getFHYFloodSensorStation: getFHYFloodSensorStation,
        getFHYFloodSensorInfoRt: getFHYFloodSensorInfoRt,
        getFHYFloodSensorInfoLast24Hours: getFHYFloodSensorInfoLast24Hours,
        getFHYTown: getFHYTown,
        AllTW:'全臺'
    };

})(window);