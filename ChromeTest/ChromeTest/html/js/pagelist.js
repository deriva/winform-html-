0/*单页面的增删改查*/

var BJPL = {
    Init: function () {
        //初始化url参数到搜索对象里
        //	Bind.Init(ban);
        //	BJPL.BindVue();
        BJPL.InitUrlParmToSearch();
        BJPL.SearchData(0);

    },
    //初始化url参数到搜索对象里
    InitUrlParmToSearch: () => {
        var parm = window.location.search.replace("?", "").split("&");
        if (parm.length > 0) {
            for (var i = 0; i < parm.length; i++) {
                var tt = parm[i].split('=');
                if (tt.length > 1 && tt[1].length > 0) {
                    //    bfUnilt._setVmVal(ban, "Search." + tt[0], decodeURIComponent(tt[1]));
                    //ban["Search." + tt[0]] = decodeURIComponent(tt[1]);
                }
            }
        }
    }, 
    SearchData: (changepage) => { //初始化搜索数据
        var searchData = $("#fm").serializeJson();
        for (var it in searchData) {
            ban.data.Search[it] = searchData[it];
        }
        if (ban.data.SearchUrl == undefined) return;
        if (changepage == 0) {
            ban.data.Search.Page = 1;
        }
        layer.closeAll();
        ban.data.Lst = [];
        if (ban.data.SearchUrl) {
            var layindex = layer.msg('正在努力加载中......', {
                icon: 16,
                shade: 0.21
            });
            var parm = LP.ParseParams(ban.data.Search);
            var rd = winformObj.FeatchGet(ban.data.SearchUrl + "?" + parm);
            layer.close(layindex);
            var r = JSON.parse(rd);
            var lst = [];
            var totalCount = 0;
            layer.closeAll();
            if (r.code == 100) {

                lst = r.Data;
                totalCount = r.TotalCount;

                ban.data.Lst = lst;
                if (ban.data.LstCb) {//回调处理下数据
                    eval(ban.data.LstCb + "()");
                }
                if (ban.data.searchcb) {
                    try {
                        eval(ban.data.searchcb + "()");
                    } catch (e) {
                        console.log(e);
                    }
                }
                BJPL.ReaderData();
                BJPL.ReaderPage(changepage, totalCount);
                if (ban.data.lstreadercb) {//渲染后回调
                    eval(ban.data.lstreadercb + "()");
                }
            } else {
                BJPL.ReaderData();
                BJPL.ReaderPage(changepage, 0);
                if (!r.message) r.message = "暂无数据";
                top.message.Warn(r.message);
                if (ban.data.searchcb) {
                    try {
                        eval(ban.data.searchcb + "('" + JSON.stringify(r) + "')");
                    } catch (e) {
                        console.log(e);
                    }
                }
            }

        }
    },
    //渲染分页
    ReaderPage: (changepage, totalcount) => {
        if (changepage == 0) {
            layui.use('laypage', function () {
                var laypage = layui.laypage;
                laypage.render({
                    elem: 'pagezone', //分页容器的id 
                    count: totalcount, //数据总数
                    limit: ban.data.Search.PageSize, //每页显示的数据条数
                    theme: '#1E9FFF', //自定义选中色值
                    layout: ['page', 'count'],
                    jump: function (obj, first) {
                        if (!first) {
                            ban.data.Search.Page = obj.curr;
                            BJPL.SearchData(1);
                        }
                    }
                });
            });
        }
    },
    //渲染列表数据
    ReaderData: () => {
        LP.TmplReader({  lst:  (ban.data.Lst)     }, 'tpl_lsttable', 'listtable');

    },
    Reset: () => {
        var data = ban.data.Search;
        for (var it in data) ban.data.Search[it] = "";
        ban.data.Search.Page = 1;
        ban.data.Search.PageSize = 20;
        LP.InitDefval();
    },
    //编辑保存数据
    Submit: function () {
        layer.confirm("确定提交吗", function () {
            var data = $("#fmedit").serializeJson();
            var r = winformObj.Featch(ban.data.EditSaveUrl, data);
            LP.ToastCode(r);
            if (r.code == 100) {
                BJPL.SearchData(0);
            } 
        });
    },
    //弹窗:编辑 
    DialogEdit: (i) => {
        var data = ban.data.Edit;
        if (i >= 0) {
            ban.data.Edit = ban.data.Lst[i];
            data = ban.data.Edit;
        }
        else {
            data = {};
        }
        var html = template('tpl_edit', data);
        var w = 0; h = 0;
        if (ban.data.EditW) w = ban.data.EditW;
        LP.DialogRb(html, w, h, "", 3);

    },
    //弹窗:查看
    DialogView: (i) => {
        var html = template('tpl_view', ban.data.Lst[i]);
        console.log(html)
        var w = 700; h = 0;
        if (ban.data.DetailW) w = ban.data.DetailW;
        LP.DialogRb(html, w, h, "查看", 3);
    },
    //取消
    Cancel: (obj) => {
        layer.confirm("确定取消吗", function () {
            var r = winformObj.Featch(ban.data.CancelUrl, obj);
            LP.ToastCode(r);
            if (r.code == 100) {
                BJPL.SearchData(0);
            }
        });
    },
}

$(() => {

    BJPL.Init();
});
