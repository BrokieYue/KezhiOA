<script type="text/javascript">
$.extend({
    bonkerTable: function (id) {
        var isHaveBonkerTable = false;
        $(window).scroll(function () {
            try {
                var $table = $("#" + id + " table");
                var $th = $("tr:first", $table);
                var offTop = $th.offset().top + $th.height();
                var scrolltop = $(window).scrollTop();
                if (offTop <= scrolltop) {
                    if (!isHaveBonkerTable) {
                        $th = $("tr:first", $table);
                        var $td = $(">td", $th[0]);
                        var left = $th.eq(0).offset().left;
                        var bonkerTable = "<table id='bonkerTable'    cellspacing='0' cellpadding='0' style='position:fixed;top:0px;left:" + left + "px; font-size:12px;'><tr>";
                        var tdClass = $("td", $table).eq(0).attr("class");
                        for (var i = 0; i < $td.length; i++) {
                            bonkerTable += "<td class='" + tdClass + "' style='border-top-width:0px;border-left-width:0px;width:" + $td.eq(i).width() + "px'>" + $td.eq(i).text() + "</td>";
                        }
                        bonkerTable += "</tr></table>";
                        $(bonkerTable).attr("class", $table.attr("class"));
                        isHaveBonkerTable = true
                        $(bonkerTable).prependTo("body");
                        if ($.browser.msie && ($.browser.version == "6.0") && !$.support.style) {
                            $("#bonkerTable").css("position", "absolute");
                            var obj = $("#bonkerTable")[0];
                            window.onscroll = function () {
                                obj.style.top = (document.body.scrollTop || document.documentElement.scrollTop) + 'px';
                            }
                            window.onresize = function () {
                                obj.style.top = (document.body.scrollTop || document.documentElement.scrollTop) + 'px';
                            }
                        }
                    }
                } else {
                    if (isHaveBonkerTable) {
                        isHaveBonkerTable = false;
                        $("#bonkerTable").remove();
                    }
                }
            } catch (err) { }
        });
    }
});
</script>