$(document).ready(function () {
    dou_options.appendCustomToolbars = [
    {
        item: '<span class="btn btn-secondary  glyphicon glyphicon-export" title="匯出">匯 出</span>', event: 'click .glyphicon-sort',
        callback: function (e) {
            $('body').loading();
            $.ajax({
                type: "GET",
                url: "../DEDS/CommEdit/ExportPDF",
                success: function () {
                    const now = new Date();
                    const year = now.getFullYear() - 1911; 
                    var fileName = "環境部" + year + "年度環境污染事故緊急應變通聯手冊.pdf";
                    var fileUrl = "../DEDS/Data/Comm/Contact_Watermark.pdf";
                    var link = document.createElement("a"); // 建立 <a> 標籤
                    link.href = fileUrl;
                    link.download = fileName; // 設定下載的檔案名稱
                    link.click(); // 自動點擊下載
                    $('body').loading('stop');
                }
            });

        }
    }
    ];  
});