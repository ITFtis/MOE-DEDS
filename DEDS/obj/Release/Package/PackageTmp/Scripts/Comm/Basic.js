//變更登打格子位置
//變數前面是要改的格子，最後一個是最後再加的文字
function changetypeposition(...parts) {

    let elements = {};  // 使用一個物件來存儲動態生成的變數
    let colWidth = 12 /( parts.length-1); // 根據 parts 的數量動態計算列寬度，確保欄位不會太長或太短


    parts.forEach((part, index) => {

        if (index == 0) {
            elements['$part' + index] = $('[data-field="' + part + '"]');//將該格子放入變數物件(當成變數)

            elements['$part' + index].addClass('row');//設定成同一行


            // 創建一個新的標籤並插入到第一個元素前
            $('<div class="form-group field-container" style="padding-right: 0.5em;"><label class="col-sm-12 control-label">' + elements['$part' + index].find('label').text() + '</label></div>').insertBefore(elements['$part' + index]);



            //改變元素class讓他同一行
            elements['$part' + index].removeClass('col-md-12').addClass('col-md-3');
            elements['$part' + index].find('label').remove()//第一個元素不要有標籤，因為塞到最上面了
            elements['$part' + index].find('div').removeClass('col-sm-12').addClass('col-sm-12');

        }
        else if (index < parts.length - 2) {

            elements['$part' + index] = $('[data-field="' + part + '"]');//將該格子放入變數物件(當成變數)

            elements['$part' + index].addClass('row');//設定成同一行

            //改變元素class讓他同一行
            elements['$part' + index].removeClass('col-md-12').addClass('col-md');
            elements['$part' + index].find('label').removeClass('col-sm-12').addClass('col-sm-2').addClass('semicolon');
            elements['$part' + index].find('div').removeClass('col-sm-12').addClass('col-sm-11');

        }
        else if (index == parts.length - 2) {

            elements['$part' + index] = $('[data-field="' + part + '"]');//將該格子放入變數物件(當成變數)

            elements['$part' + index].addClass('row');//設定成同一行

            //改變元素class讓他同一行(為了後面多塞文字，所以設定跟前面不同)
            elements['$part' + index].removeClass('col-md-12').addClass('col-md-4');
            elements['$part' + index].find('label').removeClass('col-sm-12').addClass('col-sm-1').addClass('hashtag');
            elements['$part' + index].find('div').removeClass('col-sm-12').addClass('col-sm-11');

        }
        else if (index == parts.length - 1) {
            //最後一個變數塞進後面當文字
            elements['$part' + (index - 1)].append('<label class="col-sm-2 control-label">' + part + '</label>')

        }

    });


    var $row = $('<div class="row col-12 content"></div>').insertBefore(elements['$part1']);//將全部包再row裡面，變成一行


    parts.forEach((part, index) => {
        // 將所有元素移到 row div 內
        $row.append(elements['$part' + index]);
    });

}