$(function () {
    initTable();
    //获取参数
    function GetParams(params) {
        var temp = {
            //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
            PageSize: params.limit, //页面大小
            PageIndex: params.offset / params.limit, //页码
            name: $("#key").val(),
            position: $("#txtposition").val(),
            post: $("#post").val(),
            year: globalYear,
            order: params.order,
            field: params.sort,
            adcd: "@ViewData["id"]"
    };
    return temp;
}
    //初始化表格
function initTable() {
    $('#table').bootstrapTable({
        url: "/api/CountryPerson/GetCountryPersonList",
        queryParams: GetParams,
        striped: true,
        height: $(window).height() - $("nav").height() - 120,
        columns: [
            {
                title: '序号',
                align: 'center',
                width: '50',
                formatter: function (value, row, index) {
                    var option = $("#table").bootstrapTable("getOptions");
                    return ((option.pageNumber - 1) * option.pageSize) + index + 1;
                }
            }, {
                title: '地区',
                field: 'position',
                align: 'center',
                valign: 'middle',
                width: '100'
            }, {
                title: '创建人',
                field: 'userName',
                align: 'center',
                valign: 'middle',
                width: '90'
            }, {
                title: '创建时间',
                field: 'post',
                align: 'center',
                valign: 'middle',
                width: '100'
            }, {
                title: '应急时间名称',
                field: 'phone',
                align: 'center',
                valign: 'middle',
                width: '100'
            }, {
                title: '操作',
                align: 'center',
                valign: 'middle',
                width: '100',
                formatter: function (value, row, index) {
                    var html = '<a href="/Supervise/AppWarnInfoList/1" class="tdEdit" data-id="' + row.id + '" style="margin:0 3px">查看</a>';
                    return html;
                }
            }
        ],
        onCheck: function () {
            doTool();
        },
        onUncheck: function () {
            doTool();
        },
        onCheckAll: function () {
            doTool();
        },
        onUncheckAll: function () {
            doTool();
        }
    });
}
})