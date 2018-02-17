/*************************************** Model Window Starts ***************************************/
/*
How to use : 
 $(this).modelPopUp({
            windowId: "addVacancyLocationDetails",
            width: 900,
            url: url
            closeOnOutSideClick: false,
        });
*/
function closeModelPopUpForm(refereshPreviousPage) {
    if (refereshPreviousPage == true) {
        window.parent.location.reload();
    }
    $("#" + model_windowId).remove();
    $("#" + model_DivModalOverlayId).remove();

}
var model_windowId, model_DivModalOverlayId;
(function ($) {
    $.fn.modelPopUp = function (params) {
        var defaults = {
            parent: "body",
            windowId: "_windowId",
            url: null,
            width: 750,
            height: 700,
            scroll: "no",
            addCloseButton: false,
            IFrameId: "_IFrameId",
            DivModalOverlayId: "divmodaloverlay",
            closeOnOutSideClick: false,
            close: function () {
                $("#" + params.windowId).remove();
                $("#" + params.DivModalOverlayId).remove();
            },
        };
        //Overwrite default options 
        // with user provided ones 
        // and merge them into "options". 
        var params = $.extend({}, defaults, params);

        var modal = "";
        modal += "<div id=\"" + params.DivModalOverlayId + "\" class=\"modal-overlay\"></div>";
        modal += "<div id=\"" + params.windowId + "\" class=\"modal-window\" style=\"width:" + params.width + "px; height:" + params.height + "px; margin-top:-" + (params.height / 2) + "px; margin-left:-" + (params.width / 2) + "px;\">";
        if (params.addCloseButton) {
            modal += "<button style=\"float:right\" class=\"btn btn-dark-Green\" onclick=\"closeModelPopUpForm()\">Close</button>";
        }
        modal += "<iframe width='" + params.width + "'  id='" + params.IFrameId + "' height='" + params.height + "' frameborder='0' scrolling='" + scroll + "' allowtransparency='true' src='" + params.url + "'></iframe>";
        modal += "</div>";
        $(params.parent).append(modal);

        if (params.closeOnOutSideClick) {
            $("#" + params.DivModalOverlayId).click(function () {
                params.close();
            });
        }
        model_windowId = params.windowId;
        model_DivModalOverlayId = params.DivModalOverlayId;
        //Close on PouUp Close
        $(document).keyup(function (e) {
            if (e.keyCode == 27) {
                params.close();
                $(document).off("keyup");
            }
        });
    }
})(jQuery);
/*************************************** Model Window Ends *****************************************/

function GetQueryStringValueFromString(url, name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        //results = regex.exec(location.search);
        results = regex.exec(url);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}
function GetQueryStringValueFromUrl(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    //results = regex.exec(url);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}

/*************************************** Bring Element To Center Starts *****************************************/
/*
How to use : 
 $('Id').center();
*/
$.fn.center = function () {
    this.css({
        "position": "absolute",
        "top": ($(window).height() - this.height()) / 2 + $(window).scrollTop() + "px",
        "left": ($(window).width() - this.width()) / 2 + $(window).scrollLeft() + "px"
    });
    return this;
}
$.fn.Leftcenter = function () {
    this.css({
        "position": "absolute",
        "left": ($(window).width() - this.width()) / 2 + $(window).scrollLeft() + "px"
    });
    return this;
}
/*************************************** Bring Element To Center Ends *****************************************/


/********************************* Show messages starts *********************************/
function errorBlock(msg) {
    //msg = "<strong>" + msg + "</strong>  <button type=\"button\" class=\"close\">×</button>";
    //$("div#message").text('').append(msg).attr("class", "alert alert-danger fade in alert-dismissable").slideDown().Leftcenter().css("top", $(window).scrollTop() + "px");
    //killMessage();
    //msg = msg + " <div style='float: right'>X</div>";
    //$("#message").text('').append(msg).attr("class", "form-message error").slideDown();
    //bindCloseCLick();
    sweetAlert("Error !!!", msg, "error");

}
function success(msg) {
    sweetAlert("Success", msg, "success");
    //msg = "<strong>" + msg + "</strong>"; //<button type=\"button\" class=\"close\">×</button>";
    //$("div#message").text('').append(msg).attr("class", "alert alert-success fade in alert-dismissable").slideDown().fadeOut(5000).Leftcenter().css("top", $(window).scrollTop() + "px");
    //killMessage();
    //$("#message").text(msg).attr("class", "form-message success").slideDown().fadeOut(5000);
}
//function successBlock(msg) {
//    msg = "<strong>" + msg + "</strong>  <button type=\"button\" class=\"close\">×</button>";
//    $("div#message").text('').append(msg).attr("class", "alert alert-success fade in alert-dismissable").slideDown().Leftcenter();
//    bindCloseCLick();
//}
//$(window).scroll(function () {
//    $('div#message').offset({ top: $(window).scrollTop() + 10 });
//});
//function bindCloseCLick() {
//    /*Used to close form messages */
//    $("div#message").bind("click", function () {
//        closeMessage();
//    });

//    /*Used to close form messages */
//}
//function closeMessage() {
//    $("div#message").slideUp("medium", function () {
//        $(this).html('');
//    });
//}
//function killMessage() {
//    $("div#message").slideUp("fast");
//}
/********************************* Show messages starts *********************************/


/************************************** *Trim Function Starts***************************************/
function trim(stringToTrim) {
    if (!isStringValid(stringToTrim)) return "";
    return stringToTrim.replace(/^\s+|\s+$/g, "");
}
function ltrim(stringToTrim) {
    if (!isStringValid(stringToTrim)) return "";
    return stringToTrim.replace(/^\s+/, "");
}
function rtrim(stringToTrim) {
    if (!isStringValid(stringToTrim)) return "";
    return stringToTrim.replace(/\s+$/, "");
}
function isStringValid(str) {
    if (str == "") return false;
    if (str == undefined) return false;
    return true;
}
/************************************** *Trim Function Ends***************************************/

/************************************** *AJAX POST Starts***************************************/
$(function () {
    $("form[data-post-type='ajax'][ method=\"post\"]").find(':submit').bind("click", function () {
        var form = $(this).parents('form:first');
        if (form.data("has-chkeditor") == true) {
            UpdateCKEDITOREditorInstance();
        }
        var isValid = form.validate().form();
        if (isValid) {
            form.css("opacity", "0.2");
            $.ajax({
                type: 'POST',
                url: form.attr('action'),
                data: form.serialize(),
                success: function (data) {
                    form.css("opacity", "1");
                    var functionName = form.attr('id') + "Success";
                    window[functionName](data, form);
                }
            });
            return false;
        }
        return false;
    });
});
function UpdateCKEDITOREditorInstance() {
    for (instance in CKEDITOR.instances) {
        CKEDITOR.instances[instance].updateElement();
    }
}
/************************************** *AJAX POST Ends***************************************/

/************************************** *AJAX Custom POST Starts***************************************/
function PostData(url, data, _successHandler) {
    $.ajax({
        type: 'POST',
        url: url,
        data: data,
        success: _successHandler
    });
}
/************************************** *AJAX Custom POST Ends***************************************/



/************************************** *AJAX GET Starts***************************************/
//Sorting Click
$(function () {
    $("form[data-post-type='ajax'][ method=\"get\"]").find("table thead tr.sortable").find("a").live("click", function (evt) {
        var anchorTag = $(this);
        GetData(anchorTag.parents("form"), GetQueryStringValueFromUrl("page"), GetQueryStringValueFromString(anchorTag.attr("href"), "sortBy"));
        evt.preventDefault();
    });
});

//Paging Click
$(function () {
    $("form[data-post-type='ajax'][ method=\"get\"]").find("#paging").find("a").live("click", function (evt) {
        var anchorTag = $(this);
        GetData(anchorTag.parents("form"), GetQueryStringValueFromString(anchorTag.attr("href"), "page"), GetQueryStringValueFromUrl("sortBy"));
        evt.preventDefault();
    });
});

$(function () {
    $("form[data-post-type='ajax'][ method=\"get\"]").find(':submit,:reset').live("click", function () {
        var form = $(this).parents('form:first');
        if ($(this).attr("type") == "reset") {
            clearForm(form);
        }
        var isValid = form.validate().form();
        if (isValid) {
            GetData(form, 1);
        }
        return false;
    });
});

function GetData(form, pageId, sortBy) {
    //alert(form.find(".formFields").find("select, textarea, input").serialize());
    var serializeForm = form.find("#formFields").find("select, textarea, input").serialize();//form.find(".formFields").serialize();
    var postUrl = serializeForm + "&page=" + pageId;
    if (sortBy != "" && sortBy != undefined) {
        postUrl = postUrl + "&sortBy=" + sortBy;
    }

    window.history.pushState("", "", "?" + postUrl);
    form.css("opacity", "0.2");
    $.ajax({
        type: 'GET',
        url: form.attr('action'),
        data: postUrl,
        success: function (data) {
            form.css("opacity", "1");

            //Replace Data coming with ID provioded Starts
            var commonVariable = form.data("referesh-panel-id");
            if (commonVariable) {
                $("#" + commonVariable).html(data);
            }
            //Replace Data coming with ID provioded Ends

            //Call custom function and pass data to it Starts
            commonVariable = form.data("referesh-function");
            if (commonVariable) {
                window[commonVariable](data, form);
            }
            //Call custom function and pass data to it Ends

            //Used to give alternate color to Grid Starts
            window.tableAlternateRows();
            //Used to give alternate color to Grid Ends
        }
    });
}

/************************************** *AJAX GET Starts***************************************/

function clearForm(form) {
    $(":input", form).each(function () {
        var type = this.type;
        //var tag = this.tagName.toLowerCase();
        if (type == 'text') {
            this.value = "";
        }
    });
};



function ToggleImage(imageTag) {
    var img = $(imageTag);

    if (img.attr("class") != "on") {
        img.attr("src", img.attr("src").replace("_off", "_on"));
    } else {
        img.attr("src", img.attr("src").replace("_on", "_off"));
    }
    img.toggleClass("on");
}

/***************************************Ajax Loading Bar Starts*************************************** 
$(document).ajaxStart(function () {
    AjaxLoading();
});

$(document).ajaxStop(function () {
    CloseAjaxLoading();
});
function AjaxLoading() {

    $.blockUI({
        centerX: true,
        centerY: true,
        css: { width: "140px", height: "140px" },
        message: "<img src='/Templates/resources/images/loading.gif'/>"

    });
    $('.blockUI.blockMsg').center();
    $('.blockUI.blockMsg').css({
        "border": "0",
        "background-color": "transparent"
    });
}
function CloseAjaxLoading() {
    $.unblockUI();
}
***************************************Ajax Loading Bar Ends***************************************/

//grid filters
$(function () {
    $('[data-filter-grid="true"]').bind("keyup", (function () {
        var gridId = $(this).data("filter-grid-id");
        var rex = new RegExp($(this).val(), 'i');
        $('#' + gridId + ' tr').hide();
        $('#' + gridId + ' tr').filter(function () {
            return rex.test($(this).text());
        }).show();
    }))
});

//Select All CheckBoxs
$('th.checkbox-column :checkbox').live('change', function () {
    $(this).parents('table').eq(0).find('tr:visible').find('td.checkbox-column :checkbox').prop("checked", $(this).prop('checked')).trigger('change');
});