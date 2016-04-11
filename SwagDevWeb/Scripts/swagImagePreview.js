(function () {

    $("#swagImage").on('change', function () {
        if (typeof (FileReader) != "undefined") {
            var imagePreview = $("#swagPreview");
            imagePreview.empty();

            var reader = new FileReader();
            reader.onload = function (e) {
                $("<img />", {
                    "src": e.target.result,
                    "class": "thumb-image",
                    "width": "20%"
                }).appendTo(imagePreview);
            }

            imagePreview.show();

            reader.readAsDataURL($(this)[0].files[0]);
        } else {
            alert("This browser doesn't support FileReader");
        }
    });

    //var previewDiv = $("#swagPreview");
    //var currentImage = document.getElementById("Image").value;

    //console.log(document.getElementById("Image"));

    //if (currentImage != null) {
    //    var startAt = currentImage.indexOf("\Content") - 1;
    //    currentImage = window.location.origin + currentImage.substr(startAt);

    //    $("<img />", {
    //        "src": currentImage,
    //        "class": "thumb-image",
    //        "width": "20%"
    //    }).appendTo(previewDiv);
    //}
   
})();