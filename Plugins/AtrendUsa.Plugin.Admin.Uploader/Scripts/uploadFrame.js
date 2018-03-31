(function ($, ko, pm, undefined) {
    var model;

    function viewModel() {
        var self = this;
        self.file = ko.observable(null);
        self.traceToken = ko.observable(null);
        self.enableFile = ko.observable(true);
        self.enableUpload = ko.computed(function () {
            return self.file() !== null;
        });

        self.traceToken.subscribe(function (newValue) {
            if (newValue) {
                self.enableFile(false); //disable file input
                pm({ //publish event
                    target: window.parent,
                    type: "uploadInitiated",
                    data: { traceToken: newValue }
                });
            }
        });

        self.init = function() {
            pm.bind("uploadComplete", function (data) {
                if (data) {
                    if (data.complete) {
                        self.enableFile(true); //re-enable file input
                        self.traceToken(null); //erase traceToken
                    }
                }
            });
        };
    }
    
    //the trace token is sent back for the server after post is completed in the hidden field
    ko.bindingHandlers.trace = {
        init: function (element, valueAccessor) {
            valueAccessor()($(element).val());
        },
        update: function (element, valueAccessor) {
            $(element).val(ko.unwrap(valueAccessor()));
        }
    };

    //Init
    $(document).ready(function () {
        model = new viewModel();
        ko.applyBindings(model);
        model.init();
    });

})(jQuery, ko, pm);