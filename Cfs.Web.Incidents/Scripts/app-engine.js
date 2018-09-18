var dialogWidth = 800;
var dialogHeight = 600;


var patterns = {
    email: /^([\d\w-\.]+@([\d\w-]+\.)+[\w]{2,4})?$/,
    phone: /^\D?(\d{3})\D?\D?(\d{3})\D?(\d{4})$/,
    time: /^([0-1]?\d):([0-5]\d)\s?(?:AM|PM)?$/i
};






var OpenDialog = function (url, onCloseCallback) {

    $.fancybox.open({
        padding: 0,
        width: dialogWidth,
        height: dialogHeight,
        autoSize: false,
        closeClick: false,
        closeBtn: false,
        modal: true,
        href: url,
        type: 'iframe',
        helpers:{
            overlay: {
                locked: false
            }
        },
        afterLoad: function () {

            $('body').addClass('blurred');

        },
        beforeClose: function () {

            $('body').removeClass('blurred');

            if (onCloseCallback) {
                onCloseCallback();
            }
        }
    });


};


var OpenSmallDialog = function (url, width, height, onCloseCallback) {

    $.fancybox.open({
        padding: 0,
        autoSize: false,
        width: width,
        height: height,
        closeClick: false,
        closeBtn: false,
        modal: true,
        href: url,
        type: 'iframe',
        helpers: {
            overlay: {
                locked: false
            }
        },
        afterLoad: function () {

            $('body').addClass('blurred');

        },
        beforeClose: function () {

            $('body').removeClass('blurred');

            if (onCloseCallback) {
                onCloseCallback();
            }
        }
    });


};





var ShowPhotoGallery = function (className) {
    $('a.' + className).fancybox({
        padding: 0,
        helpers: {
            overlay: {
                locked: false
            }
        }
    });
};




var ShowNotification = function (message, close) {

    var notification = new NotificationFx({
        wrapper: document.getElementById('page-container'),
        message: '<span class="icon icon-bubble"></span><p>' + message + '</p>',
        layout: 'bar',
        effect: 'slidetop',
        type: 'notice',
        onClose: function () {
            close;
        }
    }).show();
};


var ShowWarningNotification = function (message, close) {
    var notification = new NotificationFx({
        wrapper: document.getElementById('page-container'),
        message: '<span class="icon icon-warning"></span><p>' + message + '</p>',
        layout: 'bar',
        effect: 'slidetop',
        type: 'warning',
        onClose: function () {
            close;
        }
    }).show();
};


var ShowErrorNotification = function (message, close) {
    var notification = new NotificationFx({
        wrapper: document.getElementById('page-container'),
        message: '<span class="icon icon-sad2"></span><p>ERROR: ' + message + '</p>',
        layout: 'bar',
        effect: 'slidetop',
        type: 'error',
        //ttl: 0,
        onClose: function () {
            close;
        }
    }).show();
};




/*  AJAX METHODS */

function Get(url, data, onComplete, onFail, onFinally) {
    if (data === undefined || data == null) {
        $.ajax({
            url: url,
            type: 'GET',
            dataType: 'JSON',
            xhrFields: { withCredentials: true }
        }).done(function (data) { onComplete(data); })
            .fail(function (xhr, text, error) {
                var err = eval("(" + xhr.responseText + ")");
                var message = err.Message;
                if (err.ExceptionMessage) {
                    message += ' ' + err.ExceptionMessage;
                }
                ShowErrorNotification(message);
                onFail;
            })
            .always(onFinally);

    }
    else {
        $.ajax({
            url: url,
            type: 'GET',
            data: data,
            dataType: 'JSON',
            xhrFields: { withCredentials: true }
        }).done(function (data) { onComplete(data); })
            .fail(function (xhr, text, error) {
                var err = eval("(" + xhr.responseText + ")");
                var message = err.Message;
                if (err.ExceptionMessage) {
                    message += ' ' + err.ExceptionMessage;
                }
                ShowErrorNotification(message);
                onFail;
            })
            .always(onFinally);
    }
}


function Post(url, data, onComplete, onFail, onFinally) {
    $.ajax({
        url: url,
        type: 'POST',
        data: data,
        //dataType: 'JSON',
        xhrFields: { withCredentials: true }
    }).done(function (data) {

        onComplete(data);
    }).fail(function (xhr, text, error) {
        var ct = xhr.getResponseHeader("content-type") || "";
        var message = '';
        if (ct.indexOf('text/plain') > -1) {
            message = xhr.responseText;
        }
        else if (ct.indexOf('json') > -1) {
            err = eval("(" + xhr.responseText + ")");

            var message = err.Message;
            if (err.ExceptionMessage) {
                message += ' ' + err.ExceptionMessage;
            }
        }
        ShowErrorNotification(message);
        onFail;
    }).always(function () { onFinally; });

}


function Delete(url, data, onComplete, onFail, onFinally) {
    $.ajax({
        url: url,
        type: 'DELETE',
        data: data,
        //dataType: 'JSON',
        xhrFields: { withCredentials: true }
    }).done(function (data) { onComplete(data); })
        .fail(function (xhr, text, error) {
            var err = eval("(" + xhr.responseText + ")");
            var message = err.Message;
            if (err.ExceptionMessage) {
                message += ' ' + err.ExceptionMessage;
            }
            ShowErrorNotification(message);
            onFail;
        })
        .always(function () { onFinally; });

}



/* KnockoutJS Binding Handlers and Extenders */

//ko.validation.rules.pattern.message = 'Invalid.';

//ko.validation.init({
//    registerExtenders: true,
//    messagesOnModified: true,
//    insertMessages: true,
//    parseInputAttributes: true,
//    messageTemplate: null
//}, true);


ko.bindingHandlers.autoComplete = {
    init: function (element, params) {
        $(element).autocomplete(params());
    },
    update: function (element, params) {
        $(element).autocomplete('option', 'source', params().source);
    }
};


ko.bindingHandlers.datepicker = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        var options = allBindingsAccessor().datepickerOptions || {};
        $(element).datepicker(options);

        //handle the field changing
        ko.utils.registerEventHandler(element, "change", function () {
            var observable = valueAccessor();
            observable($(element).datepicker("getDate"));
        });

        //handle disposal (if KO removes by the template binding)
        ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
            $(element).datepicker("destroy");
        });
    },
    update: function (element, valueAccessor) {
        var value = ko.utils.unwrapObservable(valueAccessor()),
            $el = $(element);

        //handle date data coming via json from Microsoft
        if (String(value).indexOf('/Date(') == 0) {
            value = new Date(parseInt(value.replace(/\/Date\((.*?)\)\//gi, "$1")));
        }

        var current = $el.datepicker("getDate");

        if (value - current !== 0) {
            $el.datepicker("setDate", value);
        }
    }
};



ko.bindingHandlers.file = {
    init: function (element, valueAccessor) {
        $(element).change(function () {
            var file = this.files[0];
            if (ko.isObservable(valueAccessor())) {
                valueAccessor()(file);
            }
        });
    },
    update: function (element, valueAccessor, allBindingsAccessor) {
        var file = ko.utils.unwrapObservable(valueAccessor());
        var bindings = allBindingsAccessor();

        if (bindings.fileBase64 && ko.isObservable(bindings.fileBase64)) {
            if (!file) {
                bindings.fileBase64(null);
                bindings.fileType(null);
            }
            else {
                var reader = new FileReader();
                reader.onload = function (e) {
                    var result = e.target.result || {};
                    var resultParts = result.split(",");
                    if (resultParts.length === 2) {
                        bindings.fileBase64(resultParts[1]);
                        bindings.fileType(resultParts[0]);
                    }

                    var windowURL = window.URL || window.webkitURL;

                    if (bindings.fileObjectURL && ko.isObservable(bindings.fileObjectURL)) {
                        var oldUrl = bindings.fileObjectURL();
                        if (oldUrl) {
                            windowURL.revokeObjectURL(oldUrl);
                        }
                        bindings.fileObjectURL(file && windowURL.createObjectURL(file));
                    }
                };
                reader.readAsDataURL(file);
            }
        }
    }
};









/* User Session */
var SetUserSession = function (callback) {
    if (window.localStorage.getItem('userName') === null) {
        var url = '/api/session';
        Get(url, null, function (data) {
            //window.localStorage.setItem('')
            for (var key in data) {
                if (data.hasOwnProperty(key)) {
                    window.localStorage.setItem(key, data[key]);
                }
            }
            callback();
        });
    }
};



var LockReport = function (reportId, userId, callback) {


    var statusInfo = {
        incidentId: reportId,
        statusId: 1,
        currentUser: userId,
        lastModified: moment().format('M/D/YYYY H:mm'),
        lastModifiedBy: userId
    };
                
    Post('/api/reports/status', statusInfo, function (data) {
        var lock = {
            incidentId: reportId,
            userId: userId,
            lockStamp: moment().format('M/D/YYYY H:mm')
        };

        Post('/api/lock/', lock, function (data) {
            if (callback) { callback(); }
        });
    });
};



var WriteToLog = function (incidentId, userId, station, details) {
    var log = {
        reportLogId: 0,
        incidentId: incidentId,
        userId: userId,
        logDateTime: moment(new Date()).format('M/D/YYYY H:mm'),
        userStation: station,
        logDetails: details
    };
    Post('/api/log', log, function (data) { });
};


$(document).ready(function () {

    dialogWidth = $(window).width() * 0.9;
    dialogHeight = $(window).height() * 0.9;

    $(window).resize(function () {
        dialogWidth = $(window).width() * 0.9;
        dialogHeight = $(window).height() * 0.9;
    });


    $(window).scroll(function () {
        if ($(window).scrollTop() > 160) {
            $('.report-scroll-header').addClass('report-scroll-header-active');
        }
        else {
            $('.report-scroll-header').removeClass('report-scroll-header-active');
        }
    });

    $('#side-container').hoverIntent(
        {
            over: function () {
                $(this).toggleClass('side-container-active');
            },
            out: function () {
                $(this).toggleClass('side-container-active');
            },
            timeout: 1200
        });


    // Check user session
    if (window.localStorage.getItem('lastFire') === null) {
        window.localStorage.setItem('lastFire', new Date());
        SetUserSession();
    }
    else {
        var now = new Date().getTime();
        var lastFire = new Date(window.localStorage.getItem('lastFire')).getTime();



        var diff = Math.abs(now - lastFire) / (1000 * 60); // difference in minutes


        if (diff > 240) {  // if session more than 4 hours, refresh
            SetUserSession();
            window.localStorage.setItem('lastFire', new Date());
        }
    }
});