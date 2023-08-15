/*版本:1.0.1 
 *更新内容:新增分页 
 * 时间:2022-09-17
 * */

var LP = {
    Init: function () {
        
        LP.AllChecked(); //全选/反选   
        LP.InitLayuiDate(); 
      //  LP.InitDefval();
        LP.LayerTips();
    },
    InitLayuiDate: function () {
        //layui-laydate---   1：【年月日】选择器   2：【年月日时分秒】选择器  3：【时分秒】选择器  4：【年月】选择器
        var getLen = $("[layui-laydate]").length;
        if (getLen > 0) {
            $("[layui-laydate]").each(function (i, dom) {
                var _this = this;
                var type = "date"; var laydate = $(_this).attr('layui-laydate');
                if (laydate == "2") {
                    type = 'datetime';
                } else if (laydate == "3") {
                    type = 'time';
                } else if (laydate == "4") {
                    type = 'month';;
                }
                layui.laydate.render({
                    elem: _this,
                    type: type,
                    done: function (value, date, endDate) {
                        $(_this).val(value);
                        $(_this).change();
                    }
                });
            });
        }
    }, 
    //全选/反选
    AllChecked: function () {
        if ($("[name=chk_all]").length > 0) {
            $("[name=chk_all]").on("click", function () {
                $("[name=chk_list]").prop("checked", $(this).prop("checked"));
            });
        }
    }, 
    //获取用户是否选中数据
    isChecked: function (name) {
        var isTrue = false;
        var cond = $('input[name="' + name + '"]');
        for (var i = 0; i < cond.length; i++) {
            if (cond[i].checked == true) {
                isTrue = true;
                return isTrue;
            }
        }
        return isTrue;
    },
    //腾讯模板引擎
    TmplReader: (obj, tmpid, htmlid) => {
        var html = template(tmpid, obj);
        if (html == '{Template Error}') html = "";
        if (htmlid == undefined || htmlid == "") htmlid = tmpid.replace("tpl_", "");
        if (document.querySelector('#' + htmlid) != null)
            document.querySelector('#' + htmlid).innerHTML = html;
    },
    CommonAjax: function (url, data, title, cb) {
        if (title == undefined || title == null || title == "") {
            title = "确定操作吗？";
        }
        layer.confirm(title, function () {
            http.post(url, data).then(r => {
                if (typeof cb == "function") {
                    cb(r);
                } else {
                    if (r) {
                        LP.LayerMsg(r, function () {
                            if (r.attr != undefined && r.attr != null) {
                                window.location.href = r.attr;
                            } else {
                                window.location.reload();
                            }
                        });
                    }
                }
            }, err => {

            })
        }, function () {
            layer.closeAll();
        });
    },

    ///layer弹窗提示:成功1秒自动消失，失败需要手动点
    LayerMsg: function (data, cb) {
        var t = 2000;
        var ico = 5;
        if (data.success) {
            t = 3000;
            ico = 1;
            parent.layer.msg(data.message, {
                icon: ico,
                time: t //2秒关闭（如果不配置，默认是3秒）
            }, cb);
        } else {
            parent.layer.alert(data.message, {
                icon: ico
            }, function () {
                layer.closeAll();
                cb();
            });
        }
    },
    LayerTips: function (ops) {
        $("[tips]").unbind("mouseover").on("mouseover", function (e) {
            var id = $(this).attr("id");
            if (id == undefined) {
                id = new Date().valueOf();
                $(this).attr("id", id);
            }
            var txt = $(this).attr("tips");
            if (txt.length == 0) txt = $(this).text();
            layer.tips(txt, '#' + id, {
                tips: [3, '#FFB800'] //还可配置颜色
                ,
                time: 0
            });
        });
        $("[tips]").on("mouseout", function (e) {
            layer.closeAll();
        });
    },
    //右侧弹框:type:1id或类名 2：url，3具体的html ,2 offset:弹窗位置居右：rb，居中：auto，
    DialogRb: function (url, w, h, messageinfo, type, offset) {
        // messageInfo = (messageinfo == "" || messageinfo == null) ? "message" : messageinfo;
        if (!w) w = 0;
        if (!h) h = 0;
        //自定页
        var width = document.body.clientWidth;
        var height = document.body.clientHeight;
        if (w == 0) w = width * 0.7;
        if (h == 0) h = window.innerHeight;
        if (type == undefined || type == null || type == "") type = 2; //url弹窗
        if (offset == undefined || offset == null || offset == "") offset = "rb"; //url弹窗

        if (type == 1) url = $(url).html();
        if (type == 3) type = 1; //弹div具体内容
        var titleflag = false;
        if (messageinfo) titleflag = [messageinfo, 'font-size:18px;'];//标题栏标识
        var index = layer.open({
            shift: 2,
            content: url,
            type: type,
            title: titleflag,
            closeBtn: 1,
            area: [w + 'px', h + 'px'],
            //  maxmin: true,
            shadeClose: false,
            shade: 0.4,
            offset: offset,
            end: function () {
                layer.closeAll();
            },
            success: function (layero, index) { }
        });
    },
    DialogHtml: (config) => {
        if (!config.w) config.w = 0;
        if (!config.h) config.h = 0;
        var titleflag = false;
        if (config.title) titleflag = [config.title, 'font-size:18px;'];//标题栏标识
        if (!config.btnTxt) config.btnTxt = "提交"; var width = document.body.clientWidth;
        if (config.w == 0) config.w = width * 0.8;
        if (config.h == 0) config.h = window.innerHeight - 200;
        var index = layer.open({
            content: config.html,
            type: 1, offset: 'auto', title: titleflag,
            area: [config.w + 'px', config.h + 'px'],
            shade: 0.4,
            btn: [config.btnTxt, '取消'],
            yes: function (index, layero) {
                if (config.cb) config.cb();
            }, btn2: function (index, layero) {
                layer.close(index);
                if (config.cancelcb) config.cancelcb();
            }
        });
    },
    GetLogUrl: (logtype, bizno, appcode) => {
        if (!appcode) appcode = "bjcenter";
        var url = "/page/log/log/list.html?logtype=" + logtype;
        url += "&bizno=" + bizno;
        return url;
    },
    IsHaveParent: () => { return window.parent != this.window },//检查是否是有父窗口
  
    GetQueryString: function (name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return decodeURIComponent(unescape(r[2]));
        return "";
    },
    //将null值转换成 空字符 
    StrNullToEmpty: function (val) {
        if (val == "" || val == "null" ||
            val == null || val == "NaN" ||
            val == NaN) {
            return "";
        }
        return val;
    },

    //初始化url上参数
    InitUrlParm: function () {
        var parm = window.location.search.replace("?", "").split("&");
        if (parm.length > 0) {
            for (var i = 0; i < parm.length; i++) {
                var tt = parm[i].split('=');
                if (tt.length > 1 && tt[1].length > 0) {
                    var input = $("[name=" + tt[0] + "]");
                    if (input.length > 0) {
                        var type = $(input).attr("type");
                        if (type == "number" || type == "text") {
                            tt[1] = decodeURIComponent(tt[1])
                            $(input).val(tt[1]);
                        } else if (type == "radio") {
                            tt[1] = decodeURIComponent(tt[1])
                            QC.Checked(tt[0], tt[1]);
                        } else if ($(input).prop("tagName").toLowerCase() == "select") {
                            tt[1] = decodeURIComponent(tt[1])
                            $(input).find("option[value=" + tt[1] + "]").attr("selected", "selected");
                        }
                    }
                }
            }
        }
    },
    ParseParams: function (data) {
        try {
            var tempArr = [];
            for (var i in data) {
                var key = encodeURIComponent(i);
                var value = encodeURIComponent(data[i]);
                tempArr.push(key + '=' + value);
            }
            var urlParamsStr = tempArr.join('&');
            return urlParamsStr;
        } catch (err) {
            return '';
        }
    },
    UrlToJson: function (data) {
        var parm = data.split("&");
        var obj = {};
        if (parm.length > 0) {
            for (var i = 0; i < parm.length; i++) {
                var tt = parm[i].split('=');
                if (tt.length > 1) {
                    obj[tt[0]] = tt[1];
                }
            }
        }
        return obj;
    }, 
    ToastOk: function (title, cb) {
        LP.ToastMsg(title, 1)
    },
    ToastMsg: function (title, icon, cb) {
        return layer.msg(title, {
            icon: icon,
            shade: this.shade,
            scrollbar: false,
            time: 3000,
            shadeClose: true
        }, function () {
            if (cb) cb();
        });
    },
    ToastError: function (title, cb) {
        LP.ToastMsg(title, 2, cb)
    },
    ToastCode: function (r, cb) {
        if (r.code == 100)
            LP.ToastMsg(r.message, 1, cb);
        else LP.ToastMsg(r.message, 2, cb);
    },
    //渲染分页
    ReaderPage: (changepage, totalcount, pagesize, cb) => {
        if (changepage == 0) {
            layui.use('laypage', function () {
                var laypage = layui.laypage;
                laypage.render({
                    elem: 'pagezone', //分页容器的id 
                    count: totalcount, //数据总数
                    limit: pagesize, //每页显示的数据条数
                    skin: '#1E9FFF', //自定义选中色值 
                    layout: ['page', 'count'],
                    jump: function (obj, first) {
                        if (!first && cb) {
                            cb(1, obj.curr);
                        }
                    }
                });
            });
        }
    }, 
    DateFormat: function (fmt, date) {
        var o = {
            "M+": date.getMonth() + 1, //月份 
            "d+": date.getDate(), //日 
            "h+": date.getHours(), //小时 
            "m+": date.getMinutes(), //分 
            "s+": date.getSeconds(), //秒 
            "q+": Math.floor((date.getMonth() + 3) / 3), //季度 
            "S": date.getMilliseconds() //毫秒 
        };
        if (/(y+)/.test(fmt))
            fmt = fmt.replace(RegExp.$1, (date.getFullYear() + "").substr(4 - RegExp.$1.length));
        for (var k in o)
            if (new RegExp("(" + k + ")").test(fmt))
                fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[
                    k]).length)));
        return fmt;
    },
    //深复制对象方法    
    CloneObj: function (obj, setVal) {
        var newObj = {};
        if (obj instanceof Array) {
            newObj = [];
        }
        for (var key in obj) {
            var val = obj[key];
            //不复制值
            if (setVal == 0) { newObj[key] = typeof val === 'object' ? LP.CloneObj(val, setVal) : ""; }
            else {
                newObj[key] = typeof val === 'object' ? LP.CloneObj(val, setVal) : val;
            }
        }
        return newObj;
    },
    InitParamater: function () {//初始化字典 
        $("[drop-paramater]").each(function (i, dom) {
            var typekey = $(dom).attr("drop-paramater");
            var key = "[drop-paramater='" + typekey + "']";
            $(key).append("<option value='' selected='selected'>请选择</option>");
            http.get("/configapi/Parmater/NoAuthGetDataList?page=1&pagesize=100&typekey=" + typekey, "").then(r => {
                var info = r.attr.DataSource;
                for (var i = 0; i < info.length; i++) {
                    var it = info[i];
                    var select = "";
                    var val = it.Key;//标识  
                    $(key).append("<option value='" + val + "' " + select + ">" + it.Name + "</option>");
                }
                LP.InitDefval();
            });
        });
    },
    //初始化从租户配置里获取
    InitWebConfig: (times) => {
        if (!times) times = 0;
        times++;
        var key = "webconfig";
        var objStr = localStorage.getItem(key);
        if (objStr && objStr != "null") {
            var info = JSON.parse(objStr);
            for (var it in info) {
                if (info[it]) {
                    var oo = $("[webconfig='" + it + "']")[0];
                    var tag = "";
                    if (oo && oo.tagName) tag = oo.tagName;
                    if (tag.toLowerCase() == "img") $("[webconfig='" + it + "']").attr("src", info[it]);
                    else
                        $("[webconfig='" + it + "']").text(info[it]);
                }
            }
        } else {
            var domain = window.location.host;
            http.get("/userapi/sys_tenant_ex/NoAuthGetInfo?domain=" + domain, "", { loading: false }).then(r => {
                if (r.code == 100) {
                    localStorage.setItem(key, JSON.stringify(r.attr));
                    if (times <= 3) LP.InitWebConfig(times);
                } else {

                }
            });
        }
    },
    WebInfo: () => {
        var key = "webconfig"; var info = {};
        var objStr = localStorage.getItem(key);
        if (objStr) {
            info = JSON.parse(objStr);
        } return info;
    },
    //* 解码base64 
    Decode64: function (str) {
        return decodeURIComponent(atob(str).split('').map(function (c) {
            return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
        }).join(''));
    },
    //函数防抖
    Debounce: function (fn, wait) {
        var timer = null;
        return function () {
            if (timer !== null) {
                clearTimeout(timer);
            }
            timer = setTimeout(fn, wait);
        }
    },
}
$(() => { LP.Init(); })


layui.define(["layer", "jquery"], function (exports) {
    exports("LP", LP);
});



 