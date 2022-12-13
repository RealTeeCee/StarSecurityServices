$(function () {

    if ($("a.confirmDeletion").length) {
        $("a.confirmDeletion").click(() => {
            if(!confirm("Are you sure you want to permanently remove this item?")) return false;
        });
    }

    if ($("div.alert.notification").length) {
        setTimeout(() => {
            $("div.alert.notification").fadeOut();
        },2000);
    }
});
