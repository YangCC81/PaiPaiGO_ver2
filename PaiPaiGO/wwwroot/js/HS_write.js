$(document).ready(function () {
    $('input, textarea').focusin(function () {
        $(this).prev('label').css('left', '100px');
        $(this).next('.iconicfill-pen-alt2').removeClass('hide').addClass('show move');
    });

    $('input, textarea').focusout(function () {
        $(this).prev('label').css('left', '10px');
        var $icon = $(this).next('.iconicfill-pen-alt2');
        $icon.removeClass('show move').addClass('hide');

        // 添加這裡的延遲
        setTimeout(function () {
            $icon.removeClass('hide').css('display', 'none');
        }, 1000);  // 這裡的1000毫秒（1秒）應與CSS動畫的最大持續時間相匹配
    });
});
