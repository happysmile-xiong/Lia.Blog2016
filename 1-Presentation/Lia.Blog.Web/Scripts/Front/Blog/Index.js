var Index = {
    _domain:"",
    _pageSize: 10,
    _pageCount: 1,
    Init: function () {
        Index.LoadList(1);
    },
    LoadList: function (pageIndex) {
        $.ajax({
            cache: false,
            url: "/Blog/GetList",
            data: { p: pageIndex, s: Index._pageSize },
            type: "GET",  
            async: true,
            success: function (msg) {
                $("#loading").hide();
                $("#post_list").empty();
                if (msg != null && msg.Items != null && msg.Items.length > 0) {
                    $("#BlogTemplate").tmpl(msg.Items).appendTo("#post_list");

                    $("#pagination").show();
                    Index._pageCount = msg.TotalCount / Index._pageSize;
                    if (msg.TotalCount % Index._pageSize > 0) {
                        Index._pageCount += 1;
                    }
                    Index.InitPager(msg.TotalCount, pageIndex);
                }
                else {
                    if ($("#nodata").hasClass("hide"))
                        $("#nodata").removeClass("hide");

                    $("#nodata").appendTo("#post_list");
                }
            }
        });
    },
    //初始化分页
    InitPager: function (RecordCount, pageIndex) {
        $("#pagination").setPager({ RecordCount: RecordCount, PageIndex: pageIndex, PageSize: Index._pageSize, buttonClick: Index.pageClick });
    },
    //分页控件点击 --全部
    pageClick: function (RecordCount, pageIndex) {
        Index.LoadList(pageIndex);
    },
};