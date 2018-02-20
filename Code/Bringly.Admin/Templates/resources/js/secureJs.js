var currentLeftMenuClick;

$(document).ready(function () {
    bindLeftMenuEvents();
    InitCustomDropdown();

    var url = window.location.href; 
    if (url.indexOf("ManageCities")>-1) {
        if (window.location.search != "") {
            if (getParameterByName('MessageType') == "Success") {
                success(getParameterByName('Message'));
            }
            else {
                errorBlock(getParameterByName('Message'));
            }
        }
    }
});/******** Document Ready function Ends ********/

function getParameterByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}

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

}
function GetURLParameter() {
    var sPageUrl = location.pathname.toLowerCase(); //window.location.href;
    var indexOfLastSlash = sPageUrl.lastIndexOf("/");
    if (indexOfLastSlash > 0 && sPageUrl.length - 1 != indexOfLastSlash)
        return sPageUrl.substring(indexOfLastSlash + 1);
    else
        return 0;
} 

function DeleteCity(cityguid) {
    swal({
        title: "Are you sure?",
        text: "You want to delete the city.",
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-danger",
        confirmButtonText: "Yes, delete it!",
        closeOnConfirm: false
    },
        function () {
            PostData("/Admin/DeleteCity", { cityGuid: cityguid }, DeleteCityHandler)
        });
}
function DeleteCityHandler(response) {
    if (response) {
        swal("City has been deleted!", {
            icon: "success",
        });
        window.location.href = "/Admin/ManageCities";
    }
}
function EditCity(cityguid,cityname) {
    $('#CityName').val(cityname);
    $('#CityGuid').val(cityguid);
    $('#cityHeader').text("Edit City");
    $('#btnupdatecity').val('Update');
}
$('#btnupdatecity').on('click', function () {
    var formcity = $('#formCity').serialize();
    if ($('#btnupdatecity').val() == 'Update') {
        PostData("/Admin/EditCity", formcity, EditCityHandler)
    }
    else {
        PostData("/Admin/AddCity", formcity, SaveCityHandler)
    }
});


function EditCityHandler(response) {
    if (response.MessageType == "0") {//0 for success
        window.location.href = "/Admin/ManageCities?MessageType=Success&Message=" + response.MessageText;  
    }
    else {
        errorBlock(response.MessageText);
    }
}
function SaveCityHandler(response) {
    if (response.MessageType == "0") {//0 for success
        window.location.href = "/Admin/ManageCities?MessageType=Success&Message=" + response.MessageText;      
    }
    else {
        errorBlock(response.MessageText);
    }
}
function CreateGuid() {
    function _p8(s) {
        var p = (Math.random().toString(16) + "000000000").substr(2, 8);
        return s ? "-" + p.substr(0, 4) + "-" + p.substr(4, 4) : p;
    }
    return _p8() + _p8(true) + _p8(true) + _p8();
} 