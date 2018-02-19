var currentLeftMenuClick;

$(document).ready(function () {
    bindLeftMenuEvents();
    InitCustomDropdown();
});/******** Document Ready function Ends ********/


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

/********************************* Left menu event starts *********************************/
function bindLeftMenuEvents() {
    /*******  Responsive Layout Script For Left Menu Starts*******/
    $("#mws-nav-collapse").on('click', function (e) {
        $('#mws-navigation > ul').slideToggle('normal', function () {
            $(this).css('display', '').parent().toggleClass('toggled');
        });
        e.preventDefault();
    });
    /*******Responsive Layout Script For Left Menu Ends*******/

    /******** Side Up Down Left menu starts********/
    $("div#mws-navigation ul li a, div#mws-navigation ul li span")
        .on('click', function (event) {
            var currentSelectedItem = $("div#mws-navigation li.active").parents();
            if (currentLeftMenuClick != $(this).attr('title')) {
                currentLeftMenuClick = $(this).attr('title');
                $("div#mws-navigation ul li ul").not(currentSelectedItem).hide('fast');
            }
            if (!!$(this).next('ul').length) {
                $(this).next('ul').not(currentSelectedItem).slideToggle('fast', function () {
                    $(this).toggleClass('closed');
                });
                event.preventDefault();
            }
        });
    /******** Side Up Down Left menu ends********/

    /******** Select Left Menu ********/
    var loc = location.pathname.toString().toLowerCase();
    var $nav = $('div#mws-navigation');
    var current = $nav.find('a[href="' + loc + '"]');
    if (current.length != 0) {
        current.parents().parents().removeClass();
        current.parent().addClass('active');
        current.parent().parent().parent().addClass('activeParent');
        return;
    }
    //if (current.length <= 0) {
    //    var replaceString = GetURLParameter();
    //    loc = loc.replace("/" + replaceString, "");
    //    loc = checkUrlForInnerpages(loc).toLowerCase();
    //    current = $nav.find('a[href="' + loc + '"]');
    //    if (current.length != 0) {
    //        current.parents().parents().removeClass();
    //        current.parent().addClass('active');
    //        current.parent().parent().parent().addClass('activeParent');
    //        return;
    //    }
    //}
    /******** -----> Select Left Menu Ends ----->  ********/
}
function GetURLParameter() {
    var sPageUrl = location.pathname.toLowerCase(); //window.location.href;
    var indexOfLastSlash = sPageUrl.lastIndexOf("/");
    if (indexOfLastSlash > 0 && sPageUrl.length - 1 != indexOfLastSlash)
        return sPageUrl.substring(indexOfLastSlash + 1);
    else
        return 0;
}
function CheckDuplicateCity(evt) {
    if ($.trim($('#CityName').val()) != '') {
        PostData("/Admin/IsDuplicateCity", { cityName: $('#CityName').val(), cityGuid: $('#CityGuid').val() }, DuplicateCityHandler)
    }
    else {
        errorBlock("Please enter city name.");
    }   
}
function DuplicateCityHandler(response) {
    if (response) {
        errorBlock("City already exists");
    }
    else {
        $('#UpdateCityClick').click();
    }
}

function DeleteCity(evt) {
    swal({
        title: "Are you sure?",
        text: "Your will not be able to recover this imaginary file!",
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-danger",
        confirmButtonText: "Yes, delete it!",
        closeOnConfirm: false
    },
        function () {
            PostData("/Admin/DeleteCity", { cityGuid: $('#CityGuid').val() }, DeleteCityHandler)
        });
}
function DeleteCityHandler(response) {
    if (response) {
        swal("Poof! Your imaginary file has been deleted!", {
            icon: "success",
        });
        window.location.reload();
    }
}

