$(function () {
    $('[data-admin-menu]').hover(function () {
        $('[data-admin-menu]').toggleClass('open');
    });

    $('[data-cartpartial-menu]').hover(function () {
        $('[data-cartpartial-menu]').toggleClass('open');
    });

    $('[data-accountpartial-menu]').hover(function () {
        $('[data-accountpartial-menu]').toggleClass('open');
    });

});
