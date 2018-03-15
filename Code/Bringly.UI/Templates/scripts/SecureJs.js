$(document).ready(function () {

    $('#OpenImgUpload').click(function () { $('#fileUserProfileImage').trigger('click'); });
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
    //$(".compose-form .chosen-select").chosen().change(function () {
    //    if ($(this).val() == '') {
    //        $('.compose-form span[data-valmsg-for="EmailMessage.EmailToGuid"]').removeClass('field-validation-valid').addClass('field-validation-error');
    //        $('.compose-form span[data-valmsg-for="EmailMessage.EmailToGuid"]').html('<span for="EmailMessage_EmailToGuid" generated="true" class="">Please select user.</span>');
    //        return false;
    //    }
    //    else {
    //        $('.compose-form span[data-valmsg-for="EmailMessage.EmailToGuid"]').removeClass('field-validation-error').addClass('field-validation-valid');
    //        $('.compose-form span[data-valmsg-for="EmailMessage.EmailToGuid"]').html('');
    //        return true;
    //    }
    //});
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
$(function () {
    $('.dashboard-menu ul.list-unstyled.user-menu li a').click(function () {
        localStorage.setItem('thisLink', $(this).parent().attr("class"));
       
        $(this).parent().removeClass('class');
    });

    var thisLink = localStorage.getItem('thisLink');
    if (thisLink) {
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
            window.history.replaceState({}, document.title, url.replace(email.replace(/=\s*$/, ""), ""));            
        }
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
                GetData("/Email/NoRecordFoundPartial", {}, NoRecordFoundPartialViewFavourite)
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