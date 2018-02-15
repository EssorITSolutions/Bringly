$(document).ready(function () {

});

function addToFavourite(restaurantGuid) {
    alert('coming'); 
    PostData("/user/addtofavourite/", { restaurantGuid: restaurantGuid }, addToFavouriteSuccess)
}
function addToFavouriteSuccess(response) {
    alert(response);
    $("div#restaurant-" + restaurantGuid + "i[name=favourite]").removeClass("fa-heart-o");
    $("div#restaurant-" + restaurantGuid + "i[name=favourite]").addClass("fa-heart");
}