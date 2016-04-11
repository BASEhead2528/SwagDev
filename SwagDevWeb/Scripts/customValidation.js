// This works for now but is not a DRY approach
// Had hard time working within the unobtrusive framework
// That would require a custom data annotation and client side adapter, however, the image itself
// is not part of the model, only the image location. Thought about using editorTemplates to create
// a form that contains the model and the viewModel necessary but had problems with that
// Below the image is checked for a one to one ratio, could be done better but spent too much time on this already

(function () {

    $.validator.addMethod("imageAspect", function (value, element, params) {
        var image = $("div[id*='swagPreview'] img");
        console.log(Math.round((image.height() / image.width()) * 100));
        return this.optional(element) || (Math.round((image.height() / image.width()) * 100) == 100);
    }, "The image you upload must have an aspect ratio of 1 to 1");

    $('#createForm').validate({
        rules: {
            swagImage: { imageAspect: true, required: true }
        },
        errorPlacement: function (error, element) {
            if (element.attr("name") == "swagImage") {
                $("#imgValMsg").text(error[0].textContent);
            }
            else {
                error.append($('.errorTxt span'));
            }
        },
        success: function (error) {
            error.remove();
        }
    });

    $('#editForm').validate({
        rules: {
            swagImage: { imageAspect: true }
        },
        errorPlacement: function (error, element) {
            if (element.attr("name") == "swagImage") {
                $("#imgValMsg").text(error[0].textContent);
            }
            else {
                error.append($('.errorTxt span'));
            }
        },
        success: function (error) {
            error.remove();
        }
    });

})();

