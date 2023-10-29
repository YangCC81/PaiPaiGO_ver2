$(document).ready(function () {
    $(".wrap-label input[type='text'], .wrap-label textarea").focusin(function () {
        $(this).siblings('label').animate({ left: '80px' }, 500);
        $(this).siblings('.iconicfill-pen-alt2').animate({ left: '15px', opacity: 1 }, 500, function () {
            $(this).addClass('move-pen');
        });
    });

    $(".wrap-label input[type='text'], .wrap-label textarea").focusout(function () {
        $(this).siblings('label').animate({ left: '10px' }, 500);
        $(this).siblings('.iconicfill-pen-alt2').animate({ left: '-30px', opacity: 0 }, 500, function () {
            $(this).removeClass('move-pen');
        });
    });
});
