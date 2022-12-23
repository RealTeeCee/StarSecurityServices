$(function () {

    if ($("a.confirmDeletion").length) {
        $("a.confirmDeletion").click(() => {
            if(!confirm("Are you sure you want to permanently remove this item?")) return false;
        });
    }

    if ($("button.confirmDeletion").length) {
        $("button.confirmDeletion").click(() => {
            if (!confirm("Are you sure you want to permanently remove this item?")) return false;
        });
    }

    if ($("input.confirmDeletion").length) {
        $("input.confirmDeletion").click(() => {
            if (!confirm("Are you sure you want to permanently remove this item?")) return false;
        });
    }

    if ($("div.alert.notification").length) {
        setTimeout(() => {
            $("div.alert.notification").fadeOut();
        },10000);
    }
});
function readUrl(input) {
    if (input.files && input.files[0]) {
        let reader = new FileReader();

        reader.onload = function (e) {
            $("img#imgUpload").attr("src", e.target.result).width(200).height(200);
        };

        reader.readAsDataURL(input.files[0]);
    }
}