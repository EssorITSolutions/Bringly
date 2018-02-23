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
    var Isapprove = $(this).attr('approve');  
    PostDataWithSuccessParam("/User/ApproveReview", { reviewguid: guid, Isapprove: Isapprove }, ReviewApprovalResponse)
})
$('.reject').on('click', function () {
    var guid = $(this).attr('reviewguid');
    var Isapprove = $(this).attr('approve');
    PostDataWithSuccessParam("/User/ApproveReview", { reviewguid: guid, Isapprove: Isapprove }, ReviewApprovalResponse)
})

function ReviewApprovalResponse(response, data) {
    if (response.MessageType ==0) {
        if (data.Isapprove == 'true') {
            $('a[reviewguid="' + data.reviewguid + '"]').css("display", "none");
            $('a[reviewguid="' + data.reviewguid + '"]#reviewapprovalreject').next().css("display", "block").addClass('green-text').text("Approved");
        }
        else {
            $('a[reviewguid="' + data.reviewguid + '"]').css("display", "none");
            $('a[reviewguid="' + data.reviewguid + '"]#reviewapprovalreject').next().css("display", "block").addClass('red-text').text("Rejected");
        }
    }
    else {
        ErrorBlock(response.MessageText);
    }
}






$('.skip-review').on('click', function () {
    var guid = $(this).attr('reviewguid');
    swal({
        title: "Are you sure?",
        text: "You want to skip the review.",
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-danger",
        confirmButtonText: "Yes, skip it!",
        closeOnConfirm: false
    },
        function () {
            PostData("/User/SkipReview", { reviewguid: guid }, SkipReviewResponse)
        });
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