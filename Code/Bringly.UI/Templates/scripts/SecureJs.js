$(document).ready(function () {

});

function addToFavourite(restaurantGuid) {

    if ($("div#restaurant-" + restaurantGuid + " [name=favourite]").hasClass("fa-heart-o")) {
        PostData("/user/addtofavourite/", { restaurantGuid: restaurantGuid }, addToFavouriteSuccess)
        $("div#restaurant-" + restaurantGuid + " [name=favourite]").removeClass("fa-heart-o");
        $("div#restaurant-" + restaurantGuid + " [name=favourite]").addClass("fa-heart");
    }
    else {
        
        PostData("/user/RemoveFavourite/", { restaurantGuid: restaurantGuid }, addToFavouriteSuccess)
        $("div#restaurant-" + restaurantGuid + " [name=favourite]").removeClass("fa-heart");
        $("div#restaurant-" + restaurantGuid + " [name=favourite]").addClass("fa-heart-o");
    }
}
function addToFavouriteSuccess(response) {
    //$("div#restaurant-" + restaurantGuid + " [name=favourite]").removeClass("fa-heart-o");
    //$("div#restaurant-" + restaurantGuid + " [name=favourite]").addClass("fa-heart");
}