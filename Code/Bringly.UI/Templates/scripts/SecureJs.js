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
});
window.onload = function () {    
    var url = window.location.toString();
    if (url.indexOf("Sent") > -1 || url.indexOf("Inbox") > -1) {
        $('.mail').addClass('show');
        $('.mail ul.dropdown-menu').addClass('show');
    }
    if (url.indexOf("?") > 0) {
        //alert();
        var clean_uri = url.substring(0, url.indexOf("?"));
        window.history.replaceState({}, document.title, clean_uri);
    }
    if ($('.UnReadEmailCount').val() != '' && $('.UnReadEmailCount').val()!= undefined) {
        $('.message-count').text('(' + $('#UnReadEmailCount').val() + ')');
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
    }
    else {
        PostDataWithSuccessParam("/user/RemoveFavourite/", { restaurantGuid: restaurantGuid, IsFavourite: IsFavourite }, addToFavouriteSuccess)
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
        }
    }
}

function OpenReviewPopUp(ReviewGuid) {
    console.log(ReviewGuid);
    $('#btnAddReviewPopUp').modelPopUp({
        windowId: "AddReviewPopUp",
        width: 900,
        height: 395,
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
    //swal({
    //    title: "Are you sure?",
    //    text: "You want to skip the review.",
    //    type: "warning",
    //    showCancelButton: true,
    //    confirmButtonClass: "btn-danger",
    //    confirmButtonText: "Yes, skip it!",
    //    closeOnConfirm: false
    //},
    //    function () {
    //        PostData("/User/SkipReview", { reviewguid: guid }, SkipReviewResponse)
    //    });
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
        enableemailbutton();
       
    }
    else {
        $('#partialEmaiList input[name="checkboxemail"]').prop('checked', false); // Unchecks it
        disableemailbutton();
    }
})

function disableemailbutton() {
    $('.refresh-email').addClass("btn-disabled").attr("disabled", "disabled");
    $('.delete-data').addClass("btn-disabled").attr("disabled", "disabled");
    $('.mark-as-read').addClass("btn-disabled").attr("disabled", "disabled");
    $('.mark-as-unread').addClass("btn-disabled").attr("disabled", "disabled");
}

function enableemailbutton() {
    $('.refresh-email').removeClass("btn-disabled").removeAttr("disabled", "disabled");
    $('.delete-data').removeClass("btn-disabled").removeAttr("disabled", "disabled");
    $('.mark-as-read').removeClass("btn-disabled").removeAttr("disabled", "disabled");
    $('.mark-as-unread').removeClass("btn-disabled").removeAttr("disabled", "disabled");
}

$('.singlechecheckbox').on('click', function () {
    if ($('.singlechecheckbox:checked').length == $('.singlechecheckbox').length) {
        $('.checkboxselectall').prop('checked', true); // Checks it
    }
    else {
        $('.checkboxselectall').prop('checked', false); // Unchecks it
    }
    if ($('.singlechecheckbox:checked').length > 0) {
        $('.refresh-email').removeClass("btn-disabled").removeAttr("disabled", "disabled");
        $('.delete-data').removeClass("btn-disabled").removeAttr("disabled", "disabled");
        $('.mark-as-read').removeClass("btn-disabled").removeAttr("disabled", "disabled");
        $('.mark-as-unread').removeClass("btn-disabled").removeAttr("disabled", "disabled");
    }
    else {
        $('.refresh-email').addClass("btn-disabled").attr("disabled", "disabled");
        $('.delete-data').addClass("btn-disabled").attr("disabled", "disabled");
        $('.mark-as-read').addClass("btn-disabled").attr("disabled", "disabled");
        $('.mark-as-unread').addClass("btn-disabled").attr("disabled", "disabled");
    }
})

function DeleteSentEmail()
{
    var EmailGuid = new Array();
    $('.singlechecheckbox:checked').each(function () {
            EmailGuid.push($(this).attr("EmailGuid"));            
    });  
    if (EmailGuid.length > 0) {
        Delete("You want to delete the email.", "Yes, delete it!", "/Email/DeleteEmail", { EmailGuid: EmailGuid }, DeleteSentEmailResponse);        
    }
    else {
        ErrorBlock("Please select at least one checkbox.");
    }
}
function DeleteSentEmailResponse(response, data) {
    if (response) {
        $('.checkboxselectall').prop('checked', false);

        $.each(data.EmailGuid, function (key, value) {
            $('div[divemailguid=' + value + ']').remove();
        });
        $('div.sweet-overlay').css('display', 'none');
        $('.sweet-alert.showSweetAlert.visible').removeAttr('class');
        //alert($('.emails-count:hidden').length);
        if (!$('div#partialEmaiList div').hasClass('emails-count')) {
            PostDataWithSuccessParam("/Email/Sent", {str:""}, EmailListPartialView);
            disableemailbutton();
        }
    }
    else {
        ErrorBlock("Error while deleting email.");
    }
}

function DeleteInboxEmail() {
    var EmailGuid = new Array();
    $('.singlechecheckbox:checked').each(function () {
        EmailGuid.push($(this).attr("EmailGuid"));
    });
    if (EmailGuid.length > 0) {
        Delete("You want to delete the email.", "Yes, delete it!", "/Email/DeleteEmail", { EmailGuid: EmailGuid }, DeleteInboxEmailResponse);

    }
    else {
        ErrorBlock("Please select at least one checkbox.");
    }
}
function DeleteInboxEmailResponse(response, data) {
    if (response) {
        $('.checkboxselectall').prop('checked', false);
        $.each(data.EmailGuid, function (key, value) {
            $('div[divemailguid=' + value + ']').remove();
        });
        $('div.sweet-overlay').css('display', 'none');
        $('.sweet-alert.showSweetAlert.visible').removeAttr('class');
        if (!$('div#partialEmaiList div').hasClass('emails-count')) {
            PostDataWithSuccessParam("/Email/Inbox", {str:""}, EmailListPartialView);
        }
        PostData("/Email/GetUnReadEmailCount", {}, UnreadEmailResponse);
        disableemailbutton();
    }
    else {
        ErrorBlock("Error while deleting email.");
    }
}

function UnreadEmailResponse(response, data) {
    $('.message-count').text('(' + response + ')');
    $('.checkboxselectall').prop('checked', false);
    $('.singlechecheckbox').prop('checked', false);
}

function guid() {
    function s4() {
        return Math.floor((1 + Math.random()) * 0x10000)
            .toString(16)
            .substring(1);
    }
    return s4() + s4() + '-' + s4() + '-' + s4() + '-' + s4() + '-' + s4() + s4() + s4();
}

$('.emaillist div[data-toggle="collapse"]').on("click", function () {  
    var id = $(this).attr('data-target');
    if (!$('#' + id.split('#')[1]).hasClass('show')) {
        $('.emaillist div.collapse').removeClass('show');
    }
    var EmailGuid = new Array();
    if ($('div[divemailguid=' + id.split('#')[1] + '] a').hasClass('unreaded')) {        
        EmailGuid.push(id.split('#')[1]);
        PostDataWithSuccessParam("/Email/MarkAsRead", { EmailGuid: EmailGuid },MarkasRead)
    }
})

function MarkasRead(response, data) {
    $('.message-count').text('(' + response + ')');  
    $.each(data.EmailGuid, function (key, value) {
        $('div[divemailguid=' + value + '] a').removeClass('unreaded');
    });
    $('.checkboxselectall').prop('checked', false);
    $('.singlechecheckbox').prop('checked', false);
}

$('.mark-as-read').on("click", function () {
    var EmailGuid = new Array();
    $('.singlechecheckbox:checked').each(function () {
        EmailGuid.push($(this).attr("EmailGuid"));
    });
    if (EmailGuid.length > 0) {
    PostDataWithSuccessParam("/Email/MarkAsRead", { EmailGuid: EmailGuid }, MarkasRead)
    }
    else {
        ErrorBlock("Please select at least one checkbox.");
    }
})
$('.mark-as-unread').on("click", function () {
    var EmailGuid = new Array();
    $('.singlechecheckbox:checked').each(function () {
        EmailGuid.push($(this).attr("EmailGuid"));
    });
    if (EmailGuid.length > 0) {
    PostDataWithSuccessParam("/Email/MarkAsUnRead", { EmailGuid: EmailGuid }, MarkasUnRead)
    }
    else {
        ErrorBlock("Please select at least one checkbox.");
    }
})
function MarkasUnRead(response, data) {
    $('.message-count').text('(' + response + ')');
    $.each(data.EmailGuid, function (key, value) {
        $('div[divemailguid=' + value + '] a').addClass('unreaded');
    });
    $('.checkboxselectall').prop('checked', false);
    $('.singlechecheckbox').prop('checked', false);
}

$('.refresh-email').on('click', function () {
    var url = window.location.href; 
    var SentorInbox = "";
    if (url.indexOf('Sent')>-1) { SentorInbox = "Sent"; } else { SentorInbox = "Inbox";}
    PostDataWithSuccessParam("/Email/Get" + SentorInbox+"EmailPartial", {}, EmailListPartialView)
})
function EmailListPartialView(response, data) {
    if (response) {        
        $('#partialEmaiList').html(response);
    }
    else {
        ErrorBlock("Error while deleting email.");
    }
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

function NotificationPartialView(response,data) {
    $('li.notification-' + data.EmailGuid).fadeOut('slow');;
    var count =  parseInt($('.message-count').text().split('(')[1].split(')')[0])-1;     
    $('.message-count').text('(' + count + ')');
}