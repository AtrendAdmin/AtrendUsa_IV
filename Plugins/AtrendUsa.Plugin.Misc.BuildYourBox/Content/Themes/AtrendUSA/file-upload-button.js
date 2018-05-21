$(function () {
    var totalFiles = 0;

    var maxSize = window.MAX_ATTACHMENTS_SIZE || 25 * 1024 * 1024;
    
    var validTypes = window.ATTACHMENT_TYPES || ["jpg", "jpeg", "gif", "png"];

    $('body').on('change', '.uploaded input[type=file]', function (event) {
        var $self = $(this);
        var filename = $self.val().split('\\').pop();

        if (!validateExtension(filename)) {
            var msg = "Allowed attachments types are: " + validTypes.join(", ");
            setAttachmentErrorMessage(msg);
            $self.val("");
            return false;
        }
        if (!validateAttachmentsSize()) {
            var maxSizeInMb = maxSize / 1024 / 1024;
            setAttachmentErrorMessage("Max allowed size of attachments is " + maxSizeInMb + " MBytes");
            $self.val("");
            return false;
        }

        setAttachmentErrorMessage("");

        var $button = $self.closest('.b-button');
        var $container = $self.closest('.b-upload-button');

        var name = $self.attr('name');

        var id = "file" + (++totalFiles);

        $self.attr("id", id);
        $button.hide();

        var newInput = $('<input />');
        newInput.attr('type', 'file');
        newInput.attr('name', name);

        var newButton = $('<div class="button-1 b-button">');
        newButton.append('<span>Add File</span>');
        newButton.append(newInput);

        $container.append(newButton);
        $container.find('.b-files-to-upload').append('<li>' + filename + ' <a href="#" class="b-remove-upload" data-file-id="' + id + '">(Remove)</a></li>');
    });

    $('body').on('click', '.b-files-to-upload .b-remove-upload', function (event) {
        var $self = $(this);
        var fileInput = $('#' + $self.attr('data-file-id'));

        fileInput.closest('.b-button').remove();
        $self.closest('li').remove();

        return false;
    });

    function validateAttachmentsSize() {
        var $inputs = $('.uploaded input[type=file]');
        var totalSize = 0;
        $inputs.each(function (i, input) {
            totalSize += input.files[0].size;
        });

        return totalSize <= maxSize;
    }

    function validateExtension(filename) {
        var validTypes = window.ATTACHMENT_TYPES;
        var ext = filename.split('.').pop().toLowerCase();
        return !!ext && validTypes.indexOf(ext) !== -1;
    }

    function setAttachmentErrorMessage(message) {
        $(".uploaded .field-validation-error").text(message);
    }
});
 