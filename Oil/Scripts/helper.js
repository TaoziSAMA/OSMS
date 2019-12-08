//AJax 同步  异步
function AjaxPost(url, param, callback, async) {
    $.ajax({
        type: 'POST',
        url: url,
        async: async==null?true:false,
        dataType: "json",
        data: param,
        success: function (data) {
            if (data.Result != false) {
                callback(data.info)
            } else {
                layer.alert(data.ErrorMsg, { icon: 2 })
            }
        }
    });
}
//AJax 返回HTML
function AjaxHTML(url, param, callback, async) {
    $.ajax({
        type: 'POST',
        url: url,
        async: async == null ? true : false,
        data: param,
        success: function (data) {
            callback(data)
        }
    });
}
//AJax 同步  异步 不带报错
function AjaxPostData(url, param, callback, async) {
    $.ajax({
        type: 'POST',
        url: url,
        async: async == null ? true : false,
        dataType: "json",
        data: param,
        success: function (data) {
                callback(data)
            } 
    });
}
//弹出HTML 高度自适应
function LayerHtml(title, html,width,type) {
    window.Layindex = layer.open({
        title: title,
        type: type == null ? 1 : type,
        anim: 4,
        shade: 0,
        area:width==null? 'auto':width+'px',
        content: html
    });
}
//弹出HTML 高度自适应
function LayerHtmlSet(title, html, width, heigth, type) {
     window.Layindex= layer.open({
         title: title,
         type: type == null ? 1 : type,
         anim: 4,
         area: [width + 'px', heigth + 'px'], //宽高
         content: html
     });
}
//一般弹框 alert
function Layeralert(text, title)
{
     layer.open({
        title: title == null ? '提示' : title,
        type: 0,
        shadeClose: true, //点击遮罩关闭
        // content: '<div style="text-align:center;"><span>' + text + '</span></div>'
        content: text
    });
}
//时间格式转换
function GetTime(val,type,y,mo,d,h,m,s) {
    var time = new Date(parseInt(val.replace("/Date(", "").replace(")/", ""), 10));
    var year = time.getFullYear();
    var mon = time.getMonth() + 1;
    mon = mon > 9 ? mon : "0" + mon;
    var day = time.getDate() > 9 ? time.getDate() : "0" + time.getDate();
    var hour = time.getHours() > 9 ? time.getHours() : "0" + time.getHours();
    var mi = time.getMinutes() > 9 ? time.getMinutes() : "0" + time.getMinutes();
    var sec = time.getSeconds() > 9 ? time.getSeconds() : "0" + time.getSeconds();
    var str;
   if (type == "y") {
        str = year + "" + (y == null ? "-" : y) 
    }
    else if (type == "ym") {
        str = year + "" + (y == null ? "-" : y) + "" +mon + (mo == null ? "" : mo)
    }
    else if (type == "yd") {
        str = year + "" + (y == null ? "-" : y) + "" + mon + (mo == null ? "-" : mo) + day + (d == null ? "" : d)
    }
    else if (type == "yh") {
        str = year + "" + (y == null ? "-" : y) + "" +mon + (mo == null ? "-" : mo) + day + (d == null ? "" : d) + " " + hour + (h == null ? "" : h)
    }
    else if (type == "ymi") {
        str = year + "" + (y == null ? "-" : y) + "" +mon + (mo == null ? "-" : mo) + day + (d == null ? "" : d) + " " + hour + (h == null ? ":" : h) + mi + (m == null ? "" : m)
    }
    else if (type == "ys") {
        str = year + "" + (y == null ? "-" : y) + "" +mon + (mo == null ? "-" : mo) + day + (d == null ? "" : d) + " " + hour + (h == null ? ":" : h) + mi + (m == null ? ':' : m) + s + (s == null ? "" : s)
    }
    return str;
}
//序列化字符串(layui.table) 废弃
function GetParameters(id) {
    var obj = $(""+id+"").serializeArray()
    var count=0;
    for (let index in obj) {
            count++;
    };
    var str="";
    for (let index in obj) {
        if (obj[index].value!='') {
            str +=obj[index].name+":" +'"'+ obj[index].value+'"';
            if (index != count - 1) {
                str += ","
            }
        }
       
    };
    //str+="}"
    return str;
}
//计算年龄
function GetAge(strBirthday) {
    var returnAge;
    var strBirthdayArr = GetTime(strBirthday, 'yd').split("-");
    var birthYear = strBirthdayArr[0];
    var birthMonth = strBirthdayArr[1];
    var birthDay = strBirthdayArr[2];
    d = new Date();
    var nowYear = d.getFullYear();
    var nowMonth = d.getMonth() + 1;
    var nowDay = d.getDate();
    if (nowYear == birthYear) {
        returnAge = 0;//同年 则为0岁
    }
    else {
        var ageDiff = nowYear - birthYear; //年之差
        if (ageDiff > 0) {
            if (nowMonth == birthMonth) {
                var dayDiff = nowDay - birthDay;//日之差
                if (dayDiff < 0) {
                    returnAge = ageDiff - 1;
                }
                else {
                    returnAge = ageDiff;
                }
            }
            else {
                var monthDiff = nowMonth - birthMonth;//月之差
                if (monthDiff < 0) {
                    returnAge = ageDiff - 1;
                }
                else {
                    returnAge = ageDiff;
                }
            }
        }
        else {
            returnAge = -1;//返回-1 表示出生日期输入错误 晚于今天
        }
    }
    return returnAge;//返回周岁年
}
function GetLeve(leve)
{
    var lenename;
    if (leve==0) {
        lenename = "总部";
    } else if (leve == 1) {
        lenename = "大区";
    } else if (leve == 2) {
        lenename = "区域";
    } else if (leve == 3) {
        lenename = "油站";
    }
    return lenename;
}