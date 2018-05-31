
$(document).ready(function () {

    $('.appointmentdate').click(function () {
        $(this).datepicker({ dateFormat: 'dd-mm-yy', minDate: 1 }).val()
        $(this).datepicker('show');  
    })



    /*$('#ss123').datetimepicker();*/
     
    $('#OpenImgUpload').click(function () { $('#fileUserProfileImage').trigger('click'); });
    $('#itemImageUpload').click(function () { $('#fileItemImage').trigger('click'); });
    $('#stars li').on('mouseover', function () {
        var onStar = parseInt($(this).data('value'), 10); // The star currently mouse on

        // Now highlight all the stars that's not after the current hovered star
        $(this).parent().children('li.star.onhover').each(function (e) {
            if (e < onStar) {
                $(this).addClass('hover');
            }
            else {
                $(this).removeClass('hover');
            }
        });

    }).on('mouseout', function () {
        $(this).parent().children('li.star').each(function (e) {
            $(this).removeClass('hover');
        });
    });


    /* 2. Action to perform on click */
    $('#stars li.onhover').on('click', function () {
        var onStar = parseInt($(this).data('value'), 10); // The star currently selected
        var stars = $(this).parent().children('li.star');

        for (i = 0; i < stars.length; i++) {
            $(stars[i]).removeClass('selected');
        }

        for (i = 0; i < onStar; i++) {
            $(stars[i]).addClass('selected');
        }

        // JUST RESPONSE (Not needed)
        var ratingValue = parseInt($('#stars li.selected').last().data('value'), 10);
        var msg = "";
        if (ratingValue > 1) {
            msg = "Thankyou for the rating.";
        }
        else {
            msg = "Thankyou for the rating. We will improve ourselves.";
        }
        $('#Rating').val(ratingValue);
    });


    var url = window.location.href;
    if (url.indexOf("RestaurantReview") > -1 && url.indexOf("?") > -1) {
        if (window.location.search != "") {
            if (getParameterByName('MessageType') == "Success") {
                Success(getParameterByName('Message'));
            }
            else {
                ErrorBlock(getParameterByName('Message'));
            }
        }
    }       
});
function checkComposeEmailTo(){
        if ($(".compose-form .chosen-select").val() == '') {
            $('.compose-form span[data-valmsg-for="EmailMessage.EmailToGuid"]').removeClass('field-validation-valid').addClass('field-validation-error');
            $('.compose-form span[data-valmsg-for="EmailMessage.EmailToGuid"]').html('<span for="EmailMessage_EmailToGuid" generated="true" class="">Please select user.</span>');
            return false;
        }
        else {
            $('.compose-form span[data-valmsg-for="EmailMessage.EmailToGuid"]').removeClass('field-validation-error').addClass('field-validation-valid');
            $('.compose-form span[data-valmsg-for="EmailMessage.EmailToGuid"]').html('');
            return true;
        }
}
function checkselectedcity() {
    if ($(".formuserprofile .chosen-select.billing").val() == '' || $(".formuserprofile .chosen-select.shipping").val() == '') {
        if ($('.billing').val() == '') {
            $('.formuserprofile span[data-valmsg-for="BillingAddress_CityGuid"]').removeClass('field-validation-valid').addClass('field-validation-error');
            $('.formuserprofile span[data-valmsg-for="BillingAddress_CityGuid"]').html('<span for="BillingAddress_CityGuid" generated="true" class="">Please select city.</span>');
        }
        else if ($('.shipping').val() == '') {

            $('.formuserprofile span[data-valmsg-for="ShippingAddress_CityGuid"]').removeClass('field-validation-valid').addClass('field-validation-error');
            $('.formuserprofile span[data-valmsg-for="ShippingAddress_CityGuid"]').html('<span for="ShippingAddress_CityGuid" generated="true" class="">Please select city.</span>');
        }
        return false;
    }
    else {
        $('.formuserprofile span[data-valmsg-for="ShippingAddress_CityGuid"]').removeClass('field-validation-error').addClass('field-validation-valid');
        $('.formuserprofile span[data-valmsg-for="ShippingAddress_CityGuid"]').html('');
        $('.formuserprofile span[data-valmsg-for="BillingAddress_CityGuid"]').removeClass('field-validation-error').addClass('field-validation-valid');
        $('.formuserprofile span[data-valmsg-for="BillingAddress_CityGuid"]').html('');
        return true;
    }
}
$(function () {
    $('.dashboard-menu ul.list-unstyled.user-menu li a').click(function () {
        localStorage.setItem('thisLink', $(this).parent().attr("class"));       
        $(this).parent().removeClass('class');
    });
    var thisLink = localStorage.getItem('thisLink');
    
    if (thisLink == 'undefined') {    
        $('.lidashboard').addClass('active');
    }
    else {
        $('.' + thisLink).addClass('active');
    }
    var url = window.location.toString();
    if (url.indexOf("Sent") > -1 || url.indexOf("Inbox") > -1) {
        $('li.mail').addClass('show');
        $('li.mail ul.dropdown-menu').addClass('show');
}
});
window.onload = function () {
   
    var url = window.location.toString();
   
    if (url.indexOf("?") > 0) {
        var emailguiidarray = new Array();
        var email= "";
        var urlparts = url.split('?');  
        if (urlparts[1].indexOf('EmailGuid') > -1) {
            emailguiidarray = urlparts[1].split('=');
            if (url.indexOf("Inbox") > -1 && urlparts[1].indexOf('0000') < 0) {
                $('li.notification-' + urlparts[1].split('=')[1] + '').remove();
                $('div[data-target="#' + urlparts[1].split('=')[1] + '"]').click();
            }
            emailguiidarray.forEach(function (querystring) {
                email += querystring + "=";
            });
            //window.history.replaceState({}, document.title, url.replace(email.replace(/=\s*$/, ""), ""));            
            window.history.replaceState({}, document.title, url.split('?')[0]);        
        }
        else if (urlparts[1].indexOf('BusinessTypeGuid') > -1 && urlparts[1].indexOf('id') > -1) {
            $('#selectBusinessHeader').val("/business/LocationListUser?id=" + urlparts[1].split('&')[0].split('=')[1] + "&BusinessTypeGuid=" + urlparts[1].split('&')[1].split('=')[1]);
            emailguiidarray = urlparts[1].split('?');
            emailguiidarray.forEach(function (querystring) {
                email += querystring + "?";
            });
            window.history.replaceState({}, document.title, url.split('?')[0]);         
        }
        //email = "";
        //if (urlparts[1].indexOf('businessGuid') > -1 || urlparts[1].indexOf('guid') > -1) {
        //    businessGuidarray = urlparts[1].split('=');
          
        //    businessGuidarray.forEach(function (querystring) {
        //        email += querystring + "=";
        //    });
        //    window.history.replaceState({}, document.title, url.replace(email.replace(/=\s*$/, ""), ""));
        //}
    }
    
    if ($('.UnReadEmailCount').val() != '' && $('.UnReadEmailCount').val() != undefined) {
        $('.message-count').text('(' + $('#UnReadEmailCount').val() + ')');
    }
    if ($('.CartCount').val() != '' && $('.CartCount').val() != undefined) {
        $('.cart-notice sub').text('(' + $('.CartCount').val() + ')');       
    }  
     
  
    if ($('#EmailMessage_EmailTo').val() != "") {
        $(".chosen-select").val($('#EmailMessage_EmailTo').val()).trigger("liszt:updated");//chosen:updated.chosen
    }
    if ($('#EmailMessage_CreatedByGuid').val() != '' && $('#EmailMessage_CreatedByGuid').val() != undefined) {
        var selectedUserRole = document.getElementById('EmailMessage_CreatedByGuid').value;
        var str_array = selectedUserRole.split(',');
        for (var i = 0; i < str_array.length; i++) {
            str_array[i] = str_array[i].replace(/^\s*/, "").replace(/\s*$/, "");
        }
        $('.compose-form .chosen-select option[value = "' + str_array+'"]').attr("selected","selected");
    } 
    if ($('#CategoryGuid').val() != '' && $('#CategoryGuid').val() != undefined) {
        var selectedUserRole = document.getElementById('CategoryGuid').value;
        var str_array = selectedUserRole.split(',');
        for (var i = 0; i < str_array.length; i++) {
            str_array[i] = str_array[i].replace(/^\s*/, "").replace(/\s*$/, "");
        }
        $('.add-item-form .chosen-select option[value = "' + str_array + '"]').attr("selected", "selected");
    }
    $(".add-item-form .chosen-select#CategoryGuid").chosen().change(function () {
        $('#CategoryGuid').val($(this).val());
    });
}

function getParameterByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}

function responseMessage(msg) {
    $('.success-box').fadeIn(200);
    $('.success-box div.text-message').html("<span>" + msg + "</span>");
}

function addRemoveToFavourite(restaurantGuid, IsFavourite) {

    if ($("div#restaurant-" + restaurantGuid + " [name=favourite]").hasClass("fa-heart-o")) {
        PostDataWithSuccessParam("/user/addtofavourite/", { restaurantGuid: restaurantGuid, IsFavourite: IsFavourite }, addToFavouriteSuccess)
        $('#divfavourite_' + restaurantGuid).attr('title', 'Remove from favourite');
    }
    else {
        PostDataWithSuccessParam("/user/RemoveFavourite/", { restaurantGuid: restaurantGuid, IsFavourite: IsFavourite }, addToFavouriteSuccess)
        $('#divfavourite_' + restaurantGuid).attr('title', 'Add to favourite');
    }
}

function addToFavouriteSuccess(response, data) {
    if (response) {
        if ($("div#restaurant-" + data.restaurantGuid + " [name=favourite]").hasClass("fa-heart-o")) {
            $("div#restaurant-" + data.restaurantGuid + " [name=favourite]").removeClass("fa-heart-o");
            $("div#restaurant-" + data.restaurantGuid + " [name=favourite]").addClass("fa-heart");
        }
        else {
            $("div#restaurant-" + data.restaurantGuid + " [name=favourite]").removeClass("fa-heart");
            $("div#restaurant-" + data.restaurantGuid + " [name=favourite]").addClass("fa-heart-o");
        }

        if (data.IsFavourite == 1) {
            $('#restaurant-' + data.restaurantGuid).remove();
            if ($.trim($('#divfavouritelist').html())=='') {
                GetData("/business/NoRecordFoundPartial", {}, NoRecordFoundPartialViewFavourite)
            }
        }
    }
}
function NoRecordFoundPartialViewFavourite(response) {
    $('#divfavouritelist').html(response);
}
function OpenReviewPopUp(ReviewGuid) {
    console.log(ReviewGuid);
    $('#btnAddReviewPopUp').modelPopUp({
        windowId: "AddReviewPopUp",
        width: 900,
        height: 450,
        url: "/User/AddReview?ReviewGuid=" + ReviewGuid,        
        closeOnOutSideClick: false,
    });
}

$('.approve').on('click', function () {
    var guid = $(this).attr('reviewguid');
    PostDataWithSuccessParam("/User/ApproveReview", { reviewguid: guid, Isapprove: true }, ReviewApprovalResponse)
})
$('.reject').on('click', function () {
    var guid = $(this).attr('reviewguid');
    PostDataWithSuccessParam("/User/ApproveReview", { reviewguid: guid, Isapprove: false }, ReviewApprovalResponse)
})

function ReviewApprovalResponse(response, data) {
    if (response.MessageType == 0) {
        $('a[reviewguid="' + data.reviewguid + '"]').css("display", "none");
        if (data.Isapprove) {          
            $('p[id=review-' + data.reviewguid + '] label.review-status').css("display", "block").addClass('green-text').text("Approved");
        }
        else {
            $('p[id=review-' + data.reviewguid + '] label.review-status').css("display", "block").addClass('red-text').text("Rejected");
        }
    }
    else {
        ErrorBlock(response.MessageText);
    }
}

$('.skip-review').on('click', function () {
    var guid = $(this).attr('reviewguid');
    Delete("You want to skip the review.", "Yes, skip it!", "/User/SkipReview", { reviewguid: guid }, SkipReviewResponse)
})

function SkipReviewResponse(response) {
    if (response) {
        window.location.href = "/User/RestaurantReview?MessageType=Success&Message=Review skipped successfully.";
    }
    else {
        window.location.href = "/User/RestaurantReview?MessageType=Error&Message=Error.";
    }
}

function getreviewcharcount(evt,maxlength) {
    $('#lblreviewcharactercount').text(maxlength-$(evt).val().length);
}

$('.checkboxselectall').on('click', function () {    
    if ($(this).is(':checked')) {
        $('#partialEmaiList input[name="checkboxemail"]').prop('checked', true); // Checks it
        enableEmailHeaderButton();
       
    }
    else {
        $('#partialEmaiList input[name="checkboxemail"]').prop('checked', false); // Unchecks it
        disableEmailHeaderButton();
    }
})

function disableEmailHeaderButton() {
   
    $('.delete-data').addClass("btn-disabled").attr("disabled", "disabled");
    $('.mark-as-read').addClass("btn-disabled").attr("disabled", "disabled");
    $('.mark-as-unread').addClass("btn-disabled").attr("disabled", "disabled");
}

function enableEmailHeaderButton() {
  
    $('.delete-data').removeClass("btn-disabled").removeAttr("disabled", "disabled");
    $('.mark-as-read').removeClass("btn-disabled").removeAttr("disabled", "disabled");
    $('.mark-as-unread').removeClass("btn-disabled").removeAttr("disabled", "disabled");
}

function SelectSingleEmailCheckbox() {
    if ($('.singlechecheckbox:checked').length == $('.singlechecheckbox').length) {
        $('.checkboxselectall').prop('checked', true); // Checks it
    }
    else {
        $('.checkboxselectall').prop('checked', false); // Unchecks it
    }
    if ($('.singlechecheckbox:checked').length > 0) {
        enableEmailHeaderButton();
    }
    else {
        disableEmailHeaderButton();
    }
}




function DeleteSingleMessage(GuidEmail) {
    var EmailGuid = new Array();
    EmailGuid.push(GuidEmail);
    if (EmailGuid.length > 0) {
        Delete("You want to delete the message.", "Yes, delete it!", "/Email/DeleteEmail", { EmailGuid: EmailGuid }, DeleteInboxEmailResponse);
    }
}
function DeleteInboxEmail() {
    var EmailGuid = new Array();
    $('.singlechecheckbox:checked').each(function () {
        EmailGuid.push($(this).attr("EmailGuid"));
    });
    if (EmailGuid.length > 0) {
        Delete("You want to delete the message.", "Yes, delete it!", "/Email/DeleteEmail", { EmailGuid: EmailGuid }, DeleteInboxEmailResponse);

    }
}
function DeleteInboxEmailResponse(response, data) {
    if (response) {
        $('.checkboxselectall').prop('checked', false);
        $.each(data.EmailGuid, function (key, value) {
            $('div[divemailguid=' + value + ']').remove();
        });
        if (!$('div#partialEmaiList div').hasClass('emails-count')) {
            PostDataWithSuccessParam("/Email/Inbox", {}, EmailListPartialView);
        }
        PostData("/Email/GetUnReadEmailCount", {}, UnreadEmailResponse);
        disableEmailHeaderButton();
        //$('.sa-button-container .cancel').click();
        swal({
            title: "Success",
            text: "Email deleted successfully.",
            type: "success",
            buttons: false,
            timer: 2000,
            showConfirmButton: false
        });
        disableEmailHeaderButton();
       // Success("Email deleted successfully.");
    }
    else {
        ErrorBlock("Error while deleting email.");
    }
    
}

//Mark as Read start

function UnreadEmailResponse(response, data) {
    console.log(response);
    console.log(data);
    $('.message-count').text('(' + response + ')');
    UncheckEmailCheckbox();
}

function markEmailReadOnClickHeader() {
    var EmailGuid = new Array();
    $('.singlechecheckbox:checked').each(function () {
        EmailGuid.push($(this).attr("EmailGuid"));
    });
    if (EmailGuid.length > 0) {
        PostDataWithSuccessParam("/Email/MarkAsRead", { EmailGuid: EmailGuid }, MarkasReadResponse)
    }
}
function MarkEmailasReadOnInboxClick(evt) {
    var id = $(evt).attr('data-target');
    if (!$('#' + id.split('#')[1]).hasClass('show')) {
        $('.emaillist div.collapse').removeClass('show');
    }
    var EmailGuid = new Array();
    if ($('div[divemailguid=' + id.split('#')[1] + '] div').hasClass('unreaded')) {
        EmailGuid.push(id.split('#')[1]);
        PostDataWithSuccessParam("/Email/MarkAsRead", { EmailGuid: EmailGuid }, MarkasReadResponse)     
    }
}
function MarkasReadResponse(response, data) {
 
    var EmailGuid = new Array();
    PostDataWithSuccessParam("/Email/MarkNotificationRead", { EmailGuid: EmailGuid }, NotifPartialViewOnInboxClick)
    if (response == 0) {
        $('.all-notification').html("<div class='arrow-top'><span class='icon-up'></span></div>  <div class='notice-heading'><div class='float-left'><a>No unread notifications</a></div>");
    }
    $('.message-count').text('(' + response + ')');
    $.each(data.EmailGuid, function (key, value) {
        $('div[divemailguid=' + value + '] div').removeClass('unreaded');        
    });
    disableEmailHeaderButton();
    UncheckEmailCheckbox();
}
function NotifPartialViewOnInboxClick(response, data) {
    $('.notification-dropdown .notification-data').html(response);    
}

//Mark as Read end


//Mark as UnRead start

function MarkEmailasUnReadHeader() {
    var EmailGuid = new Array();
    $('.singlechecheckbox:checked').each(function () {
        EmailGuid.push($(this).attr("EmailGuid"));
    });
    if (EmailGuid.length > 0) {
        PostDataWithSuccessParam("/Email/MarkAsUnRead", { EmailGuid: EmailGuid }, MarkasUnReadResponse)
    }
}

function MarkasUnReadResponse(response, data) {    
    var EmailGuid = new Array();  
    PostDataWithSuccessParam("/Email/MarkNotificationRead", { EmailGuid: EmailGuid }, NotifiPartViewUnreadEmail)
    $.each(data.EmailGuid, function (key, value) {
        $('div[divemailguid=' + value + '] div').addClass('unreaded');
    });
    console.log(response);
    $('.message-count').text('(' + response + ')');
    UncheckEmailCheckbox();
    disableEmailHeaderButton();
}
function NotifiPartViewUnreadEmail(response, data) {
    $('.notification-dropdown .notification-data').html(response);    
}
//Mark as UnRead end

$('.refresh-email').on('click', function () {    
    PostDataWithSuccessParam("/Email/Inbox", { }, EmailListPartialView)
})
function UncheckEmailCheckbox() {
    $('.checkboxselectall').prop('checked', false);
    $('.singlechecheckbox').prop('checked', false);
}

function EmailListPartialView(response, data) {
    console.log(response);
    disableEmailHeaderButton();
    UncheckEmailCheckbox();
    if (response) {        
        if ($.trim(response) == "") {
            $('.emaillist .shadow-box.top-bar-mail').remove();
            GetData("/Email/NoRecordFoundPartial", {}, NoRecordFoundPartialView)
        }
        else {
            $('#partialEmaiList').html(response);
        }
    }
}
function NoRecordFoundPartialView(response) {
    $('#partialEmaiList').html(response);
}
function NotificationPartialView(response, data) {
    $('li.notification-' + data.EmailGuid).fadeOut('slow');;
    var count = parseInt($('.message-count').text().split('(')[1].split(')')[0]) - 1;

    $('.message-count').text('(' + count + ')');

}




function EmailComposePopup() {
    $('.EmailComposePopup').modelPopUp({
        windowId: "ComposeEmail",
        width: 900,
        height: 620,
        url: "/Email/ComposeEmail",
        closeOnOutSideClick: false,
    });
}

$('.notification-message').on('click', function () {
    var EmailGuid = new Array();
    EmailGuid.push($(this).attr('emailguid'))
    PostDataWithSuccessParam("/Email/MarkNotificationRead", { EmailGuid: EmailGuid}, NotificationPartialView)
})

$('form[name=compose-form]').submit(function (e) {
    if ($('#EmailMessage_Body').val().length > 1000 || $('#EmailMessage_Subject').val().length > 50) { return false; }
    else {
        if ($('#EmailMessage_EmailTo').val() != '' && $('#EmailMessage_Body').val() != '' && $('#EmailMessage_Subject').val() != '') {
            swal({
                title: "Please wait...",
                buttons: false,
                showConfirmButton: false
            });
        }
        return true;
    }
})

function fillEmailTo() {
    $('#EmailMessage_EmailTo').val($(".chosen-select").val());
    var mailContents = CKEDITOR.instances['EmailMessage_Body'].getData();
    $('#EmailMessage_Body').val(mailContents);
}
function replyandForwardMessage(EmailGuid, replyorforward) {
    
    var boolval = (replyorforward == 'forward') ? "false" : "true";
    $('.EmailComposePopup').modelPopUp({
        windowId: "ComposeEmail",
        width: 900,
        height: 620,
        url: "/Email/ComposeEmail?EmailGuid=" + EmailGuid + "&Isreply=" + boolval+"",
        closeOnOutSideClick: false,
    });
}

// Calculation price cart start
$('a.decrease').on('click', function () {
    var itemguidoriginal = $(this).attr('decreasequantity');
    var quantity = parseInt($('#quantity_' + itemguidoriginal + ' .quanity-input input[type="text"]').val());    
    if (quantity == 1) {
        swal({
            title: "Quantity Error",
            text: "Quantity cannot be less than 1.",
            type: "error",
            buttons: false,
            timer: 3000,
            showConfirmButton: false
        });
    }
    else {
        quantity = $('#quantity_' + itemguidoriginal + ' .quanity-input  input[type="text"]').val(quantity - 1);
        var price = $('.td-price-' + itemguidoriginal + '  input[type="text"]').val(); 
        var subtotal = parseFloat($('#cart #SubTotal').val());
        var total = parseFloat($('#cart #Total').val());
        var discount = parseFloat($('.span-discount-' + itemguidoriginal).text());
        var totaldiscount = 0;
        var itemguid = 0;
        $('table#cart tbody tr').each(function () {
            itemguid = $(this).attr('id');
            if (itemguid != undefined) {
                itemguid = $(this).attr('id').split('_')[1];
                totaldiscount += parseFloat($('#quantity_' + itemguid + ' .quanity-input  input[type="text"]').val()) * parseFloat($('.span-discount-' + itemguid).text());
            }
        });
        $('#cart #Discount').val(totaldiscount);
        $('#cart #SubTotal').val(parseFloat(subtotal) - parseFloat(price));
        $('#cart #Total').val(parseFloat(total) - parseFloat(price) + discount);
    }
})
$('a.increase').on('click', function () {
    var itemguidoriginal = $(this).attr('increasequantity');
    var quantity = parseInt($('#quantity_' + itemguidoriginal + ' .quanity-input  input[type="text"]').val());    
    $('#quantity_' + itemguidoriginal + ' .quanity-input  input[type="text"]').val(quantity+ 1);
    var price = $('.td-price-' + itemguidoriginal +'  input[type="text"]').val();
    var subtotal = $('#cart #SubTotal').val();   
    var total = $('#cart #Total').val(); 
    var totaldiscount = 0;
    var itemguid = "";
    $('table#cart tbody tr').each(function () {
        itemguid = $(this).attr('id');
        if (itemguid != undefined) {
            itemguid = $(this).attr('id').split('_')[1];
            totaldiscount += parseFloat($('#quantity_' + itemguid + ' .quanity-input  input[type="text"]').val()) * parseFloat($('.span-discount-' + itemguid).text());         
        }
    });   
    var discount = parseFloat($('.span-discount-' + itemguidoriginal).text());
    $('#cart #Discount').val(parseFloat(totaldiscount));
    $('#cart #SubTotal').val(parseFloat(subtotal) + parseFloat(price));    
    $('#cart #Total').val(parseFloat(total) + parseFloat(price) - discount );
})
// Calculation price cart end
function guid() {
    function s4() {
        return Math.floor((1 + Math.random()) * 0x10000)
            .toString(16)
            .substring(1);
    }
    return s4() + s4() + '-' + s4() + '-' + s4() + '-' + s4() + '-' + s4() + s4() + s4();
}


function DeleteSentEmail() {
    var EmailGuid = new Array();
    $('.singlechecheckbox:checked').each(function () {
        EmailGuid.push($(this).attr("EmailGuid"));
    });
    if (EmailGuid.length > 0) {
        Delete("You want to delete the message.", "Yes, delete it!", "/Email/DeleteEmail", { EmailGuid: EmailGuid }, DeleteSentEmailResponse);
    }

}
function DeleteSentEmailResponse(response, data) {

    if (response) {
        $('.checkboxselectall').prop('checked', false);
        $('body').removeClass('stop-scrolling');
        $.each(data.EmailGuid, function (key, value) {
            $('div[divemailguid=' + value + ']').remove();
        });
        if (!$('div#partialEmaiList div').hasClass('emails-count')) {
            PostDataWithSuccessParam("/Email/Sent", { str: "" }, EmailListPartialView);
            disableEmailHeaderButton();
            $('.sa-button-container .cancel').click();
        }
    }
    else {
        ErrorBlock("Error while deleting email.");
    }
}


// Item quantity decrease and increase and add to cart start

function decreaseItemQuantity(itemguid) {
    var quantity = parseInt($('.lblQuantity_'+itemguid).text());
    if (quantity == 1) {
        swal({
            title: "Quantity Error",
            text: "Quantity cannot be less than 1.",
            type: "error",
            buttons: false,
            timer: 3000,
            showConfirmButton: false
        });
    }
    else {
        quantity = parseInt($('.lblQuantity_' + itemguid).text());
        $('.lblQuantity_' + itemguid).text(quantity-1);    
    }
}

function increaseItemQuantity(itemguid) {
    var quantity = parseInt($('.lblQuantity_' + itemguid).text());
    $('.lblQuantity_' + itemguid).text(quantity + 1);
}

function Addtocart(itemguid) {
    var item = {
        ItemGuid: itemguid,
        Quantity: parseInt($('.lblQuantity_' + itemguid).text())
    }
    PostDataWithSuccessParam("/Restaurant/addToCart", { item: item }, addToCartResponse)


    
}
function addToCartResponse(response) {
    if (response) {
        //Success("Item added to cart.");
        PostDataWithSuccessParam("/Restaurant/getCartCount", {}, cartCountResponse)
    }
    else {
        Error("Error while adding item to cart");
    }
}
function cartCountResponse(response) {
    $('.cart-notice sub').text('(' + response + ')');
}
function deleteCartItem(ItemGuid) {
   
    PostDataWithSuccessParam("/Restaurant/deleteItemFromCart", { ItemGuid: ItemGuid }, deleteItemFromCartResponse)
}
function deleteItemFromCartResponse(response, data) {   
    if (response) {
        $('#tr_' + data.ItemGuid).remove();
        PostDataWithSuccessParam("/Restaurant/getCartCount", {}, cartCountResponse)
       // Success("Item deleted successfully.");
    }
    else {
        Error("Error while deleting item from cart");
    }
}

// Item quantity decrease and increase and add to cart start end


function OpenMenuItemPopUp(ItemGuid) {
    $('#anchorAddMenuItemPopUp').modelPopUp({
        windowId: "AddMenuItemPopUp",
        width: 900,
        height: 600,
        url: "/Restaurant/AddItem?ItemGuid=" + ItemGuid,
        closeOnOutSideClick: false,
    });
}

function CheckIsMerchant(evt) {
    if ($(evt).val() == 3) {
        $('.checkboxcompany').prop("checked", "checked");
    }
    else {
        $('.checkboxcompany').prop("checked", false);
    }
}
function Checkagreement() {
    if ($('.useragreement').is(':checked')) {
        if ($(".formusrregistration .chosen-select.businesstype").val() == '' || $(".formusrregistration .chosen-select.selectedcity").val() == '') {
            if ($('.selectedcity').val() == '') {
                $('.formusrregistration span[data-valmsg-for="CityGuid"]').removeClass('field-validation-valid').addClass('field-validation-error');
                $('.formusrregistration span[data-valmsg-for="CityGuid"]').html('<span for="CityGuid" generated="true" class="">Please select city.</span>');
            }
            else if ($('.businesstype').val() == '') {
                $('.formusrregistration span[data-valmsg-for="BusinessTypeGuid"]').removeClass('field-validation-valid').addClass('field-validation-error');
                $('.formusrregistration span[data-valmsg-for="BusinessTypeGuid"]').html('<span for="BusinessTypeGuid" generated="true" class="">Please select Business Type.</span>');
            }
            return false;
        }
        else if ($('#UserRegistrationType').val() == 3 || $('#UserRegistrationType').val() == 2)
        {
            PostData("/User/FindUsertoberegistered", { ItemGuid: ItemGuid, businesstype: $(".formusrregistration .chosen-select.businesstype").val() }, FindUsertoberegisteredResponse)
        }
        else {
            $('.formusrregistration span[data-valmsg-for="BusinessTypeGuid"]').removeClass('field-validation-error').addClass('field-validation-valid');
            $('.formusrregistration span[data-valmsg-for="BusinessTypeGuid"]').html('');
            $('.formusrregistration span[data-valmsg-for="CityGuid"]').removeClass('field-validation-error').addClass('field-validation-valid');
            $('.formusrregistration span[data-valmsg-for="CityGuid"]').html('');
            return true;
        }
    }
    else {
        swal({
            title: "Error",
            text: "Please check user agreement checkbox.",
            type: "error",
            buttons: false,
            timer: 3000,
            showConfirmButton: true
        });
        return false;
    }
}
function FindUsertoberegisteredResponse(response) {
    $('.formusrregistration span[data-valmsg-for="BusinessTypeGuid"]').removeClass('field-validation-error').addClass('field-validation-valid');
    $('.formusrregistration span[data-valmsg-for="BusinessTypeGuid"]').html('');
    $('.formusrregistration span[data-valmsg-for="CityGuid"]').removeClass('field-validation-error').addClass('field-validation-valid');
    $('.formusrregistration span[data-valmsg-for="CityGuid"]').html('');
    return response;
}
function Checkcvrnumber(registrationtype) {
    if (registrationtype == '3') {
        if ($('#CVRNumber').val() == '' || $('#CVRNumber').val().length != 10) {
            swal({
                title: "Error",
                text: "CVR number should be of 10 digits.",
                type: "error",
                buttons: false,
                timer: 3000,
                showConfirmButton: true
            });
            $('#CVRNumber').css("border-color", 'red');
            return false;
        }
        else {
            return true;
        }
    }
    else {
        return true;
    }
}
$(function () {
    $('#CVRNumber').on('keydown', function (e) { -1 !== $.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) || (/65|67|86|88/.test(e.keyCode) && (e.ctrlKey === true || e.metaKey === true)) && (!0 === e.ctrlKey || !0 === e.metaKey) || 35 <= e.keyCode && 40 >= e.keyCode || (e.shiftKey || 48 > e.keyCode || 57 < e.keyCode) && (96 > e.keyCode || 105 < e.keyCode) && e.preventDefault() });
})
/////////////////////  Manage Business Type

function DeleteBusinessType(BusinessTypeguid) {
    swal({
        title: "Are you sure?",
        text: "You want to delete Business Type.",
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-danger",
        confirmButtonText: "Yes, delete it!",
        closeOnConfirm: false
    },
        function () {
            PostData("/Business/DeleteBusinessType", { BusinessTypeGuid: BusinessTypeguid }, DeleteBusinessTypeHandler)
        });
}
function DeleteBusinessTypeHandler(response) {
    if (response.MessageType == "0") {//0 for success
        window.location.href = "/Business/ManageBusinessType?MessageType=Success&Message=" + response.MessageText;
    }
    else {
        errorBlock(response.MessageText);
    }
}
function EditBusinessType(BusinessTypeguid, BusinessTypename) {
    $('#BusinessTypeName').val(BusinessTypename);
    $('#BusinessTypeGuid').val(BusinessTypeguid);
    $('#btnupdateBusinessType').val('Update');
}
$('#btnupdateBusinessType').on('click', function () {
    if ($.trim($('#BusinessTypeName').val()) != '') {
        var formBusinessType = $('#formBusinessType').serialize();
        if ($('#btnupdateBusinessType').val() == 'Update') {
            PostData("/Business/EditBusinessType", formBusinessType, EditBusinessTypeHandler)
        }
        else {
            PostData("/Business/AddBusinessType", formBusinessType, SaveBusinessTypeHandler)
        }
    }
    else {
        errorBlock("Please enter Business Type .");
    }
});
function EditBusinessTypeHandler(response) {
    if (response.MessageType == "0") {//0 for success
        window.location.href = "/Business/ManageBusinessType?MessageType=Success&Message=" + response.MessageText;
    }
    else {
        errorBlock(response.MessageText);
    }
}
function SaveBusinessTypeHandler(response) {
    if (response.MessageType == "0") {//0 for success
        window.location.href = "/Business/ManageBusinessType?MessageType=Success&Message=" + response.MessageText;
    }
    else {
        errorBlock(response.MessageText);
    }
}

function editbusinessprofile() {
    $('.disabled-textbox').removeAttr('disabled').removeClass('disabled-textbox');
    $('#divNewField').removeAttr('disabled');
    $('.divprofileedit').addClass('display-none');
    $('.divprofileupdate').removeClass('display-none');
    $('.divaddproperty').removeClass('display-none');
    $('#divNewField img').removeClass('display-none');
    $('.divprofilecancel').removeClass('display-none');
    $('.citydropdown').removeClass('display-none');
    $('.cityname').addClass('display-none');
}
function cancelbusinessprofile() {
    addDisableClassProfile();
}
function addDisableClassProfile() {
    $('table tr td.data input').attr('disabled', 'disabled').addClass('disabled-textbox');
    $('#divNewField input').attr('disabled', 'disabled').addClass('disabled-textbox');
    $('#divNewField').attr('disabled', 'disabled');
    $('.divaddproperty').addClass('display-none');
    $('#divNewField img').addClass('display-none');
    $('textarea').attr('disabled', 'disabled').addClass('disabled-textbox');
    $('.divprofileedit').removeClass('display-none');
    $('.divprofileupdate').addClass('display-none');
    $('.divprofilecancel').addClass('display-none');
    $('.cityname').removeClass('display-none');
    $('.citydropdown').addClass('display-none');
}



function updatelocation() {
    var CustomFiledList = {};
    var x = 0;
    if ($('.dynamicdata').length > 0) {
        $('.dynamicdata').each(function () {
            if ($(this).find('.dynamicfieldname').val() != "" && $(this).find('.dynamicfieldvalue').val() != "" && $(this).find('.dynamicfieldguid').val() != "") {
                CustomFiledList[x] = { LocationGuid: $('#BusinessGuid').val(), CustomPropertyGuid: $(this).find('.dynamicfieldguid').val(), Field: $(this).find('.dynamicfieldname').val(), Value: $(this).find('.dynamicfieldvalue').val() };
                
            }
            else {
                CustomFiledList[x] = { LocationGuid: $('#BusinessGuid').val(),Field: $(this).find('.dynamicfieldname').val(), Value: $(this).find('.dynamicfieldvalue').val() };
            }
            x++;
        });
        PostData("/Business/UpdateCustomField", { CustomFiledList: CustomFiledList }, CustomFieldHandler)
    }
    else {
        var formBusinessType = $('#formrestaurent').serialize();
        PostData("/Business/UpdateLocation", formBusinessType, businessProfileHandler)
    }
    
}
function CustomFieldHandler(response) {
    if (response) {
        var formBusinessType = $('#formrestaurent').serialize();
        PostData("/Business/UpdateLocation", formBusinessType, businessProfileHandler)
    }
    else {
        ErrorBlock("Error while updating custom properties.");
    }
}


function updatebusinessprofile() {
    var formBusinessType = $('#formrestaurent').serialize();
    PostData("/Restaurant/UpdateRestaurentProfile", formBusinessType, businessProfileHandler)
}
function businessProfileHandler(response) {
    if (response != '') {
        addDisableClassProfile();
        $('#CityName').html(response);
        window.location.href = "/Business/LocationList";
    }
    else {
        ErrorBlock("Error while updating restaurent profile.");
    }
}

function isNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}
function DeleteLocation(guid) {
    swal({
        title: "Are you sure?",
        text: "You want to delete Location.",
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-danger",
        confirmButtonText: "Yes, delete it!",
        closeOnConfirm: false
    },
        function () {
            PostDataWithSuccessParam("/Business/DeleteLocation", { BusinessGuid: guid }, DeleteLocationHandler)
        });

}
function DeleteLocationHandler(response, data) {
    console.log(data);
    if (response.MessageType == "0") {//0 for success
        Success(response.MessageText);
        $('.tr-' + data.BusinessGuid).remove();
        if ($('#tablelocationlist tr').length < 2) {
            GetData("/Business/NoRecordFoundPartial", {}, NoRecordFoundPartialViewLocation)
        }
    }
    else {
        ErrorBlock(response.MessageText);
    }
}

function NoRecordFoundPartialViewLocation(response) {
    $('#divlocationlist').html(response);
}


function DeleteUser(guid) {
    swal({
        title: "Are you sure?",
        text: "You want to delete User.",
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-danger",
        confirmButtonText: "Yes, delete it!",
        closeOnConfirm: false
    },
        function () {
            PostDataWithSuccessParam("/User/DeleteUser", { UserGuid: guid }, DeleteUserHandler)
        });
}
function DeleteUserHandler(response, data) {
    console.log(data);
    if (response.MessageType == "0") {//0 for success
        Success(response.MessageText);
        $('.tr-' + data.UserGuid).remove();
        if ($('#tableuserlist tr').length < 2) {
            GetData("/User/NoRecordFoundPartial", {}, NoRecordFoundPartialViewLocation)
        }
    }
    else {
        ErrorBlock(response.MessageText);
    }
}
function addNewField() {
    GetData("/Business/AddNewProperty", {}, AddNewPropertyResponse)
}
function AddNewPropertyResponse(response) {
    $('#divNewField').append(response);
}
function Getcustomfields(BusinessGuid) {
    var businessObject = $('#frmaddlocation').serialize();   
    PostData("/Business/AddLocation", businessObject, AddLocationHandler)  
}
function AddLocationHandler(response) {
    console.log(response);
    if (response) {
        var CustomFiledList = {};
        var x = 0;
        if ($('.dynamicdata').length > 0) {
            $('.dynamicdata').each(function () {
                if ($(this).find('.dynamicfieldname').val() != "" && $(this).find('.dynamicfieldvalue').val() != "") {
                    CustomFiledList[x] = { LocationGuid: response, Field: $(this).find('.dynamicfieldname').val(), Value: $(this).find('.dynamicfieldvalue').val() };
                    x++;
                }
            });
            if ($('.dynamicdata').length == CustomFiledList.length - 1) {
                PostData("/Business/AddCustomField", { CustomFiledList: CustomFiledList }, AddCustomFieldHandler)
            }
        }
    }
    else {
        ErrorBlock("Error while adding location.");
    }
}

function AddCustomFieldHandler(response) {
    if (response) {
        var formBusinessType = $('#formrestaurent').serialize();
        PostData("/Business/UpdateLocation", formBusinessType, businessProfileHandler)
       
    }
    else {
        ErrorBlock("Error while adding other properties.");
    }
}

function removeParentDiv(guid,evt) {
    if (guid == '') {
        $(evt).closest(".dynamicdata").remove();
    }
    else {
        PostDataWithSuccessParam("/Business/DeleteCustomField", { CustomFieldGuid: guid }, DeleteCustomFieldHandler)    
    }    
}

function DeleteCustomFieldHandler(response, data) {
    if (response) {
        $('.div-' + data.CustomFieldGuid).closest(".dynamicdata").remove();
    }
    else {
        ErrorBlock("Error while deleting property.");
    }
}

function GetBusinessInfo(guid) {
    $('#hBusinessName').modelPopUp({
        windowId: "BudinessInfo",
        width: 1200,
        height: 850,
        url: "/Business/GetBusinessInfo?guid=" + guid,
        closeOnOutSideClick: false,
    });
}

function Checkandmakeappointment(BusinessGuid) {
    PostDataWithSuccessParam("/Business/IsSaloonBooked", {
        BusinessGuid: BusinessGuid, SaloonTimeGuid: $("#SaloonTimeGuid_" + BusinessGuid).val(), SaloonTime: $("#SaloonTimeGuid_" + BusinessGuid + "  option:selected").text(), AppointmentDate: $('#AppointmentDate_' + BusinessGuid).val()
    },IsSaloonBookedHandler)    
   }
function IsSaloonBookedHandler(response, data) {
    if (response) {
        PostDataWithSuccessParam("/Business/MakeSaloonAppointment", {
            BusinessGuid: data.BusinessGuid, SaloonTimeGuid: data.SaloonTimeGuid, SaloonTime: data.SaloonTime, AppointmentDate: data.AppointmentDate
        }, SaloonAppointmentHandler)
    }
    else {
        ErrorBlock("This time slot is already booked. Please choose another time slot or date.");
    }
}
function SaloonAppointmentHandler(response, data) {
    if (response) {
        Success("Appointment sent for approval from admin.")
        enableDisableAppControls();
    }
    else {
        ErrorBlock("Error while making appointment.");
    }
}
function cancelapppointment() {
    enableDisableAppControls();
}
function enableDisableAppControls() {
    $('.userappointment  input.data').attr('disabled', 'disabled').addClass('disabled-textbox');  
    $('.textsaloontime').removeClass('display-none');
    $('.timeslotdropdown').addClass('display-none');
    $('.divprofileupdate').addClass('display-none');
    $('.divprofileedit').removeClass('display-none');
}
function editappointment() {
    $('.userappointment  input.data').removeAttr('disabled').removeClass('disabled-textbox');
    $('.textsaloontime').addClass('display-none');
    $('.timeslotdropdown').removeClass('display-none');
    $('.divprofileedit').addClass('display-none');
    $('.divprofileupdate').removeClass('display-none');
}
function updateapppointment(SaloonAppointmentGuid, BusinessGuid, SaloonAppointmentGuid) {
    PostDataWithSuccessParam("/Business/IsSaloonBooked", {
        SaloonAppointmentGuid: SaloonAppointmentGuid, BusinessGuid: BusinessGuid, SaloonTimeGuid: $("#SaloonTimeGuid_" + SaloonAppointmentGuid).val(), SaloonTime: $("#SaloonTimeGuid_" + SaloonAppointmentGuid + "  option:selected").text(), AppointmentDate: $('#AppointmentDate_' + SaloonAppointmentGuid).val()
    }, IsAlreadyBookedHandler)   
}
function IsAlreadyBookedHandler(response, data) {
    if (response) {
        PostDataWithSuccessParam("/Business/MakeSaloonAppointment", {
            SaloonAppointmentGuid:data.SaloonAppointmentGuid,BusinessGuid: data.BusinessGuid, SaloonTimeGuid: data.SaloonTimeGuid, SaloonTime: data.SaloonTime, AppointmentDate: data.AppointmentDate
        }, SaloonAppointmentHandler)
        $('#textsaloontime_' + data.SaloonAppointmentGuid).val(data.SaloonTime);
    }
    else {
        ErrorBlock("This time slot is already booked. Please choose another time slot or date.");
    }
}
function DeleteAppointment(SaloonAppointmentGuid){
    swal({
        title: "Are you sure?",
        text: "You want to delete Appointment.",
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-danger",
        confirmButtonText: "Yes, delete it!",
        closeOnConfirm: false
    },
        function () {
            PostDataWithSuccessParam("/Business/DeleteSaloonAppointment", { SaloonAppointmentGuid: SaloonAppointmentGuid }, DeleteAppointmentHandler)
        });

}
function DeleteAppointmentHandler(response,data) {
    if (response.MessageType == "0") {//0 for success
        //Success('Appointment deleted successfully.')
        $('#AppointmentDate_' + data.SaloonAppointmentGuid).closest('.userappointment').remove();
        $('.sweet-alert.showSweetAlert').remove();
        $('.sweet-overlay').remove();
        if ($.trim($('.userappointment').length) == 0) {
            GetData("/business/CommonNoRecordFoundPartial", { message:"No appointment(s) found."}, NoRecordAppointmentFavourite)
        }
    }
    else {
        ErrorBlock(response.MessageText);
    }
}
function NoRecordAppointmentFavourite(response) {
    $('#divappointment').html(response);
}