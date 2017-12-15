/**
 * Created by Administrator on 2017/9/22.
 */
// 根据adcd获取下一级的全部城市的名称

$.fn.extend({   
    alert:function () {
        alert("test");
    },
    getAllCity:function (adcd) {
        var sb=StringBuilder();
        $.ajax({
            type:"post",
            datatype:"json",
            url:"",
            data:{},
            success:function (data) {
                var haha=eval(data);
                $.each(haha,function (i,item) {
                    
                })
            }
        });
    },
    returncall:function (adcd) {
        var sb=StringBuilder();
        $.ajax({
            type:"post",
            datatype:"json",
            url:"",
            data:{},
            success:function (data) {
                var haha=eval(data);
                $.each(haha,function (i,item) {

                })
            }
        });
    }
    
});