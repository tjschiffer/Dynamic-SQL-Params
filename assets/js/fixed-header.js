//<![CDATA[ 
function fixedheaders() {
    if (/Chrome|Firefox/.test(navigator.userAgent)) {

        var overscrolldiv = document.getElementById('overscrolldiv');
        if (overscrolldiv == null) {
            var newdiv = document.createElement('div');
            newdiv.id = 'overscrolldiv';
            newdiv.style.height = '50vh';
            document.getElementsByTagName('body')[0].appendChild(newdiv);
        };

        var fixedheader = $('#fixedheader');
        var header = $('#headerrow');

        //if (fixedheader.length == 0) {
        //    var fixedheader = header.clone().attr('id', 'fixedheader').insertBefore(header);
        //};

        if (fixedheader.length == 0) {
            var fixedheader = header.clone().attr('id', 'fixedheader');
            $('body').prepend(fixedheader);
            var tableClass = header.parents('table').attr('class');
            fixedheader.wrap("<table class=" + tableClass + "></table>");
        };

        var headercells = header.children();
        var fixedheadercells = fixedheader.children();

        fixedheader.css({
            "position": "fixed",
            "width":"100%",
            "z-index": "1"
        });


        for (i = 0; i < headercells.length; i++) {
            var style = window.getComputedStyle(headercells[i], null),
            padding = parseFloat(style.paddingLeft) +
                parseFloat(style.paddingRight),
            width = headercells[i].clientWidth - padding;

            $(fixedheadercells[i]).css({
                "width": width,
                "height": style.height,
                "background": "rgba(255,255,255,0.9)"
            });
        }
        header.css({
            "visibility": "hidden"
        });
    };
};

$(window).load(function () {

    $(window).bind('resize click', function () {
        fixedheaders();
    });

    $(window).blur(function () {
        fixedheaders();
    });

    fixedheaders();


    //Scroll animation
    //$(window).scroll(function () {
    //    var scroll = $(window).scrollTop();
    //    if (scroll < 10) {
    //        $('#fixedheader').css({ 'top': '0' });
    //    }
    //    else
    //    {
    //        $('#fixedheader').css({ 'top': '-100px' });
    //        var scroll = $(window).scrollTop();
    //        setTimeout(function () {
    //            if (scroll == $(window).scrollTop()) {
    //                $('#fixedheader').animate({
    //                    'top': '0'
    //                }, 100)
    //            };
    //        }, 300);
    //    };
    //});
});

    //]]>
