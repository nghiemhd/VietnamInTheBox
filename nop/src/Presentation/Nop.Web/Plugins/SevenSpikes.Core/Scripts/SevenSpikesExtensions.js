(function ($) {

    var type = false;
    var ua = navigator.userAgent.toLowerCase();

    // Device Object
    var devices = {

        "tablet": [
	    	/ipad/i,            // iPads
	    	/playbook/i,        // Blackberry PlayBook
	    	/xoom/i,            // Motorola Xoom
	    	/tablet(?! pc)/i,   // Generic Tablet (matches HP TouchPad),
	    	                    //    excludes tablet pcs
	    	/froyo/i            // Froyo is currently only available to tablets
        ],

        "mobile": [
			/iphone/i,
			/blackberry/i,
			/android/i,
			/mytouch/i,
			/webos/i,
			/(wap|wml)/i
        ],

        "desktop": [
			/.*/
        ]
    };

    // Check Mobile
    $(['tablet', 'mobile', 'desktop']).each(function () {
        var key = this.toString()
        for (var i = 0; len = devices[key].length, i < len; i++) {
            if (devices[key][i].test(ua)) {
                type = key;
                return false;
            }
        }
    });

    // Assign the device object to jQuery
    $.device = {
        "mobile": type == 'mobile' ? true : false,
        "tablet": type == 'tablet' ? true : false,
        "desktop": type == 'desktop' ? true : false,
        "type": type
    };

})(jQuery);

$.extend({
    getAttrValFromDom: function (elementSelector, elementAttribute, defaultValue) {
        var value = $(elementSelector).attr(elementAttribute);

        if (value == undefined || value == "") {
            if (window.console) {
                window.console.log("'" + elementAttribute + "' was not found.");
            }
            if (defaultValue != undefined) {
                value = defaultValue;
            } else {
                value = "";
            }
        }

        return value;
    },
    getHiddenValFromDom: function (elementSelector, defaultValue) {
        var value = $(elementSelector).val();

        if (value == undefined || value == "") {
            if (window.console) {
                window.console.log("The 'value' was not found.");
            }
            if (defaultValue != undefined) {
                value = defaultValue;
            } else {
                value = "";
            }
        }

        return value;
    },
    getUrlVars: function () {
        var vars = [], hash;
        var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < hashes.length; i++) {
            hash = hashes[i].split('=');
            vars.push(hash[0]);
            vars[hash[0]] = hash[1];
        }
        return vars;
    },
    getUrlVar: function (name) {
        return $.getUrlVars()[name];
    },
    getSpikesViewPort: function () {
        var e = window, a = 'inner';
        if (!('innerWidth' in window)) {
            a = 'client';
            e = document.documentElement || document.body;
        }

        var result = { width: e[a + 'Width'], height: e[a + 'Height'] };

        // We need to ajust the width for browsers that are not Firefox/IE
        // In other browsers the $(window).width() returns the width of the window with out the scroll bar
        // thus we need to change the width, so it can work the same way in all browsers
        // Stefan - 16.08.2013 - Looks like Chrome has been updated to calculate correctly the width of the viewport
        // without including the scrollbar, so no need for this hack here.
        //if (navigator.userAgent.indexOf('Firefox') == -1 && !$.browser.msie) { // NOT Firefox or IE
        //    result.width -= 18;
        //}

        return result;
    },
    isMobile: function () {
        return !$.device.desktop;
    },
    addSpikesWindowEvent: function (eventName,functionToCall) {
        if (window.addEventListener) window.addEventListener(eventName, functionToCall, false);
        else if (window.attachEvent) window.attachEvent("on"+eventName, functionToCall);
    }
});