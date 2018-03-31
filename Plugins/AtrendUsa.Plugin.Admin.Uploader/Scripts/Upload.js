(function ($, ko, pm, undefined) {
    //const
    var historytoloadpercall = 14;
    var model;
    //view models
    function fileViewModel(fileName, uploadDate) {
        var self = this;
        self.fileName = fileName;
        self.uploadDate = uploadDate;
        self.download = function () {
            window.location = 'Upload/DownloadUploadedFile?fileName=' + self.fileName;
        };
    }

    $(document).ajaxError(function (event, jqxhr, settings, thrownError) {
        if (jqxhr.status === 403) {
            window.location.reload();
        } else if (jqxhr.status === 500) {
            alert(jqxhr.responseJSON.Message);
            console.log(jqxhr.responseJSON.Message);
        } else {
            alert(thrownError);
        }
    });

    function viewModel() {
        var self = this;
        self.intervalId = 0;
        self.traceToken = null;
        self.frame = window.frames[0];
        self.currentUploadStage = 0;

        self.uploadHistory = ko.observableArray([]);
        self.uploadTrace = ko.observableArray([]);
        self.showProgress = ko.observable(false);
        self.progressPercent = ko.observable('0.00%');

        self.inProgress = ko.computed(function () {
            return parseFloat(self.progressPercent()) < 100.00 ? 'active' : '';
        });
        self.hasUploadTrace = ko.computed(function () {
            return self.uploadTrace().length > 0;
        });
        self.hasHistory = ko.computed(function () {
            return self.uploadHistory().length > 0;
        });

        self.moveProgress = function (uploadStage) {
            if (uploadStage) {
                var progressPercentValue = uploadStage * 9.1;
                //same stage as last time move by 0.1% -> fake progress
                if (self.currentUploadStage === uploadStage) {
                    progressPercentValue = parseFloat(self.progressPercent()) + 0.08;
                }
                self.progressPercent(progressPercentValue + '%');
                self.currentUploadStage = uploadStage;
            }
        };
        self.init = function () {
            //subscribe to upload initiated event that is triggered as soon as file posted to the server
            pm.bind("uploadInitiated", function (data) {
                if (data) {
                    self.traceToken = data.traceToken;
                    self.startTracing();
                }
            });
        };
        self.startTracing = function () {
            self.showProgress(true);
            self.trace();
            self.intervalId = window.setInterval(function () {
                self.trace();
            }, 3000);
        };
        self.stopTracing = function () {
            self.traceToken = null;
            window.clearInterval(self.intervalId);
            pm({
                target: self.frame,
                type: "uploadComplete",
                data: { complete: true }
            });
        };

        self.trace = function () {
            if (self.traceToken) {
                $.ajax({
                    url: 'Upload/GetUploadStatus',
                    data: JSON.stringify({ traceToken: self.traceToken }),
                    cache: 'false',
                    type: 'POST',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        if (data && data.CurrentUploadStage) {
                            self.moveProgress(data.CurrentUploadStage.Stage);
                            self.uploadTrace($.map(data.UploadStageTrace, function (v) {
                                return { message: v.Message };
                            }));
                            if (data.CurrentUploadStage.Stage === 11) {
                                self.stopTracing();
                                if (data.CurrentUploadStage.StageOutput !== null) {
                                    self.uploadTrace.push(new fileViewModel(data.CurrentUploadStage.StageOutput.FileName));
                                }
                            }
                        }
                    },
                    error: function (jqXhr, textStatus, errorThrown) {
                        self.stopTracing();
                    }
                });
            }
        };
        self.loadUploadHistory = function () {
            $.ajax({
                url: 'Upload/GetUploadHistory',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ take: historytoloadpercall, skip: 0 }),
                type: 'POST',
                cache: 'false',
                success: function (data) {
                    if (data) {
                        self.uploadHistory($.map(data, function (v) {
                            return new fileViewModel(v.FileName, v.UploadDate);
                        }));
                    }
                }
            });
        };
        self.loadMoreUploadHistory = function (d, event) {
            var skip = self.uploadHistory().length + 1;
            $.ajax({
                url: 'Upload/GetUploadHistory',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ take: historytoloadpercall, skip: skip }),
                type: 'POST',
                cache: 'false',
                success: function (data) {
                    if (data) {
                        $.each(data, function (i, v) {
                            self.uploadHistory.push(new fileViewModel(v.FileName, v.UploadDate));
                        });
                        window.scrollTo(0, $(event.target).position().top);
                    }
                }
            });
        };
        self.backToTop = function () {
            window.scrollTo(0, 0);
        };
        self.downloadTemplate = function () {
            window.location = 'Upload/DownloadTemplateFile';
        };
    }



    //Init
    $(document).ready(function () {
        $('#tabs a:first').tab('show');
        model = new viewModel();
        ko.applyBindings(model);
        model.init();
    });
})(jQuery, ko, pm);