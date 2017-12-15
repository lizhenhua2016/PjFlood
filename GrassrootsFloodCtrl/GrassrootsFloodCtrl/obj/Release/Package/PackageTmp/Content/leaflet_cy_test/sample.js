//sample:扩展jquery对象的方法，bold()用于加粗字体。
(function ($) {
    $.fn.extend({
        "bold": function () {
            ///<summary>
            /// 加粗字体
            ///</summary>
            return this.css({ fontWeight: "bold" });
        }
    });
})(jQuery);