$(document).ready(function () {/********Document Ready function Starts********/
    InitCustomDropdown();
    CheckBoxCheckUnCheck();
    
});/********Document Ready function Ends********/
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


$.fn.Iframecenter = function () {

    return this.each(function () {
        var top = ($(window).height() - $(this).outerHeight()) / 2;
        var left = ($(window).width() - $(this).outerWidth()) / 2;

        $(this).css({ position: 'fixed', margin: 0, top: (top > 0 ? top : 0) + 'px', left: (left > 0 ? left : 0) + 'px' });
    });
};


/*************************************** Bring Element To Center Ends *****************************************/

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

/************************************** *Referesh CHkeditor Instance Starts***************************************/
$(function () {
    $("form[data-has-chk-editor='true']").find(':submit').on("click", function () {
        UpdateCKEDITOREditorInstance();
    });
});
function UpdateCKEDITOREditorInstance() {
    for (instance in CKEDITOR.instances) {
        CKEDITOR.instances[instance].updateElement();
    }
}
/************************************** *Referesh CHkeditor Instance Ends***************************************/
/***************************************Ajax Loading Bar Starts***************************************/
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
        css: { width: "140px", height: "140px" }//,
        // message: "<img src='/Templates/resources/images/loading.gif'/>"

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
/***************************************Ajax Loading Bar Ends***************************************/
/***************************************** DropDown Search Feature Starts *****************************************/

function InitCustomDropdown() {
    var config = {
        'select': {
            no_results_text: "No results match",
            placeholder_text_single: "Select Option",
            placeholder_text_multiple: "Select Some Options"
        },
    }
    for (var selector in config) {
        $(selector).not(".notchosen").chosen(config[selector]);
    }
}

/***************************************** DropDown Search Feature Ends *****************************************/

function CheckBoxCheckUnCheck() {
    $('th.checkbox-column :checkbox').on('change', function () {
        $(this).parents('table').eq(0).find('tr:visible').find('td.checkbox-column :checkbox').prop("checked", $(this).prop('checked')).trigger('change');
    });
}

/********************************* Show messages starts *********************************/
function ErrorBlock(msg) {
    sweetAlert("Error !!!", msg, "error");
}
function Success(msg) {
    sweetAlert("Success", msg, "success");
}
/********************************* Show messages starts *********************************/
/************************************** *AJAX Custom POST Starts***************************************/
function PostData(url, _data, _successHandler, ShowBlackImage) {
    if (ShowBlackImage == null || ShowBlackImage == undefined) {
        ShowBlackImage = true;
    }
    $.ajax({
        type: 'POST',
        url: url,
        data: _data,
        success: _successHandler,
        global: ShowBlackImage
    });
}
/************************************** *AJAX Custom POST Ends***************************************/

/************************************** *AJAX Custom POST Starts***************************************/
function PostDataWithSuccessParam(url, _data, _successHandler, ShowBlackImage) {
    if (ShowBlackImage == null || ShowBlackImage == undefined) {
        ShowBlackImage = true;
    }
    $.ajax({
        type: 'POST',
        url: url,
        data: _data,
        success: function successHandler(result) {
            _successHandler(result, _data)
        },
        global: ShowBlackImage
    });
}
/************************************** *AJAX Custom POST Ends***************************************/

$('.carousel').carousel({
    interval: 5000
})

$('#myTab a').click(function (e) {
    e.preventDefault()
    $(this).tab('show')
})

$('input:checkbox').change(function () {
    if ($(this).is(":checked")) {
        $('label.toggle-on').addClass("on");
    } else {
        $('label.toggle-on').removeClass("on");
    }
});

$('input:checkbox').change(function () {
    if ($(this).is(":checked")) {
        $('label.toggle-off').addClass("off");
    } else {
        $('label.toggle-off').removeClass("off");
    }
});
$(document).ready(function () {
    $('.dropdown-menu-form .dropdown-menu').on('click', function (e) {
        if ($(this).hasClass('show')) {
            e.stopPropagation();
        }
    });
    $('.close-icon').on('click', function () {
        $(this).parents('div.dropdown-menu').removeClass('show');
    });

});
$(function () {
    $("#fileUserProfileImage").change(function () {
        var regex = /^([a-zA-Z0-9\s_\\.\-:])+(.jpg|.jpeg|.gif|.png|.bmp)$/;
        if (regex.test($(this).val().toLowerCase())) {
            if (window.FormData !== undefined) {

                var fileData = new FormData();
                fileData.append("FileToUpload", $(this).get(0).files[0]);
                $.ajax({
                    url: '/User/UploadProfileImage',
                    type: "POST",
                    contentType: false, // Not to set any content header  
                    processData: false, // Not to process data  
                    data: fileData,
                    success: function (result) {
                        if (result.IsSuccess) {
                            Success(result.Message);
                            $('img[name=leftProfileImage]').attr("src", result.NewImage);
                        }
                        else {
                            ErrorBlock(result.Message);           
                        }                        
                    },
                    error: function (err) {
                        ErrorBlock(err.statusText);                        
                    }
                })
            } else {
                ErrorBlock("FormData is not supported.");
            }
        } else {
            ErrorBlock("Please upload a valid image file.");
        }
    });
});

function fileImageSuccess(response) {

}