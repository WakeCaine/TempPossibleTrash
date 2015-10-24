/*! 
* Creates a bootstrap modal window
/**
@Sam Ranjbari
**/

var bs = (function(window, dom, undefined) {
    var ModalDialog = function(options) {
        this.events = {};
        this.submitted = false;

        this.eventsMapper = {
            closed: 'closed',
            submitted: 'submitted'
        }

        this.metadata = $.extend({
            Id: 'myModal',
            content: 'content...',
            headerText: '&nbsp;',
            actionButtonText: 'Yes, Continue',
            actionButtonClass: 'btn-primary',
            closeButtonText: 'Close',
            appendTo: '',
            contentTemplateId: null,
            bindingData: null,
            largeModal: false
        }, options || {});
    }

    ModalDialog.prototype = {
        init: function() {
            var self = this;
            var dfd = $.Deferred();
            if (this.modalId().length !== 0) {
                this.modalId().remove();
            }

            $.get("/templates/modaltmpl.htm")
                .done(function(data) {
                    self.setup(data);

                    dfd.resolve();
                });

            return dfd.promise();
        },

        setup: function(data) {
            var self = this;

            if (this.metadata.contentTemplateId != null) {
                _.templateSettings.variable = "";
                var tmplMarkup = $(this.metadata.contentTemplateId).html();
                this.metadata.content = tmplMarkup;

                var compiledContent = _.template(this.metadata.content);
                this.metadata.content = compiledContent(this.metadata.bindingData);
            }

            _.templateSettings.variable = "rc";

            var compiled = _.template(data);

            this.metadata.appendTo = this.metadata.appendTo || 'body';
            $(this.metadata.appendTo).append(compiled(this.metadata));

            $("#" + this.metadata.Id + " .modalActionButton").click(function(e) {
                e.preventDefault();

                self.submitted = true;
                self.emit(self.eventsMapper.submitted, e);
            });

            $("#" + this.metadata.Id).on('hidden.bs.modal', function(e) {
                if (!self.submitted) {
                    self.emit(self.eventsMapper.closed, e);
                }
            });
        },

        show: function(func) {
            var self = this;
            $.when(this.init()).then(function() {
                self.modalId().modal(self.metadata);

                if (typeof func === 'function') {
                    func();
                }
            });
        },

        hide: function() {
            this.modalId().modal('hide');
        },

        hideCloseButtons: function() {
            this.modalId().find(".modalCloseButton").hide();
        },

        showCloseButtons: function() {
            $(".modalCloseButton").show();
        },

        hidePrimaryButtons: function() {
            this.modalId().find(".btn-primary").hide();
        },

        on: function(event, callback) {
            if (!this.events[event]) {
                this.events[event] = [];
            }
            this.events[event].push(callback);
        },

        emit: function(event, data) {
            var callbacks = this.events[event];
            if (callbacks) {
                callbacks.forEach(function(callback) {
                    callback(data);
                });
            }
        },

        modalId: function() {
            return $("#" + this.metadata.Id);
        }
    }

    return {
        ModalDialog: ModalDialog
    }
})(this, this.document);