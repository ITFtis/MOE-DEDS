var citystaInit = function () {

}

var ctrlZoomChange = function (e) {

}
var changeCity = function (e) {

}
var getCountyJObject = function (city) {
    if(city)
        return $('.sta-county .county-data[data-county="' + city + '"]');
    else
        return $('.sta-county .county-data');
}
var currentCity = "臺北市";
$(document).ready(function () {
    var $_county = $(".sta-county .county-data");
    //填統計縣市名稱
    $.each($_county, function () {
        var $_this = $(this);
        var cn = $_this.attr("data-county");
        $_this.find('.title').text(cn);
    });

    $('.zoom-ctrl').on('click', function (e) {
        $(this).parent().toggleClass('zoom-max');
        ctrlZoomChange(e);
        $(this).toggleClass('glyphicon-resize-small');
    });
    $(".sta-county .county-data .btn").on('click', function (e) {
        currentCity = $(this).parent().attr("data-county");
        changeCity(e);
    });
    citystaInit();
});