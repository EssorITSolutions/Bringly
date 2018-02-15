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