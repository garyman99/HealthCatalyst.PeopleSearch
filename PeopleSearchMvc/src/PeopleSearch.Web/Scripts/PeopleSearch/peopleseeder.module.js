
// todo: this is tightly coupled to jQuery--might be good to inject in an abstraction
var SeederModuleFactory = (function ($, utilityService) {
    $ = $ || jQuery;

    // define some that can be utilized by the module service
    var SeederEventTypes = {
        Processing: "processing",
        Stopped: "stopped",
        Errored: "errored",
        Logs: "logs"
    };
    function SeederEventListener() { }
    SeederEventListener.prototype.HandleSeederEvent = function (eventType, eventArgs) { }

    // base ui element type
    // note: utilizing prototype "inheritance" to allow sharing of common functionality
    function UiElement() {
        this.instances = [];
    }
    utilityService.inherit(SeederEventListener, UiElement);
    UiElement.prototype.GetInstances = function () { return this.instances; };
    UiElement.prototype.Initialize = function (uiContainer, required, selector) {
        var instances = uiContainer.find(selector);
        if (required && instances.length === 0) {
            throw "Seeder Module Exception: Required element with selector " +
                selector +
                " returned no elements";
        }
        this.instances = instances;
    };
    UiElement.prototype.Disable = function () {
        this.GetInstances().attr("disabled", "disabled");
    }
    UiElement.prototype.Enable = function () {
        this.GetInstances().removeAttr("disabled");
    }
    UiElement.prototype.HandleSeederEvent = function (eventType, eventArgs) {
        var instances = this.GetInstances();
        if (instances != null && instances.length !== 0) {
            switch (eventType) {
                case SeederEventTypes.Processing:
                    instances.addClass("sm-started");
                    instances.removeClass("sm-stopped");
                    instances.removeClass("sm-errored");
                    break;
                case SeederEventTypes.Stopped:
                    instances.addClass("sm-stopped");
                    instances.removeClass("sm-started");
                    instances.removeClass("sm-errored");
                    break;
                case SeederEventTypes.Errored:
                    instances.addClass("sm-errored");
                    instances.removeClass("sm-started");
                    instances.removeClass("sm-stopped");
                    break;
                case SeederEventTypes.Logs:
                    // nothing to see here
                    break;
                default:
                    console.log("Unknown event type was received: " + eventType);
                    break;
            }
        }
    }

    // base button 
    function Button() { }
    utilityService.inherit(UiElement, Button);
    Button.prototype.BindClickEvent = function (event) {
        var instances = this.GetInstances();
        if (instances != null && instances.length !== 0) {
            instances.on("click", event);
        }
    }

    // start button
    function StartButton() { }
    utilityService.inherit(Button, StartButton);
    StartButton.prototype.HandleSeederEvent = function (eventType, eventArgs) {
        // call parent
        UiElement.prototype.HandleSeederEvent.call(this, eventType, eventArgs);
        var instances = this.GetInstances();
        if (instances != null && instances.length !== 0) {
            switch (eventType) {
                case SeederEventTypes.Processing:
                    UiElement.prototype.Disable.call(this);
                    break;
                case SeederEventTypes.Stopped:
                    UiElement.prototype.Enable.call(this);
                    break;
                case SeederEventTypes.Errored:
                    UiElement.prototype.Enable.call(this);
                    break;
            }
        }
    }

    // stop button
    function StopButton() { }
    utilityService.inherit(Button, StopButton);
    StopButton.prototype.HandleSeederEvent = function (eventType, eventArgs) {
        // call parent
        UiElement.prototype.HandleSeederEvent.call(this, eventType, eventArgs);
        var instances = this.GetInstances();
        if (instances != null && instances.length !== 0) {
            switch (eventType) {
                case SeederEventTypes.Processing:
                    UiElement.prototype.Enable.call(this);
                    break;
                case SeederEventTypes.Stopped:
                    UiElement.prototype.Disable.call(this);
                    break;
                case SeederEventTypes.Errored:
                    UiElement.prototype.Disable.call(this);
                    break;
            }
        }
    }

    // status indicators
    function StatusIndicator() { }
    utilityService.inherit(UiElement, StatusIndicator);
    StatusIndicator.prototype.HandleSeederEvent = function (eventType, eventArgs) {
        // call parent
        UiElement.prototype.HandleSeederEvent.call(this, eventType, eventArgs);
        var instances = this.GetInstances();
        if (instances !== null && instances !== undefined && instances.length !== 0) {
            switch (eventType) {
                case SeederEventTypes.Processing:
                    instances.text("Started");
                    break;
                case SeederEventTypes.Stopped:
                    instances.text("Stopped");
                    break;
                case SeederEventTypes.Errored:
                    instances.text("Errored");
                    break;
            }
        }
    }

    // logs
    function SeedLog() { }
    utilityService.inherit(UiElement, SeedLog);
    SeedLog.prototype.HandleSeederEvent = function (eventType, eventArgs) {
        // call parent
        UiElement.prototype.HandleSeederEvent.call(this, eventType, eventArgs);
        var instances = this.GetInstances();
        if (instances != null && instances.length !== 0) {
            switch (eventType) {
                case SeederEventTypes.Errored:
                    instances.append("Errored\r\n");
                    instances.append(eventArgs + "\r\n");
                    break;
                case SeederEventTypes.Logs:
                    for (var i = 0; i < eventArgs.length; i++) {
                        instances.append(eventArgs[i] + "\r\n");
                    }
                    break;
            }

            // scroll to the bottom of the log(s)
            for (var j = 0; j < instances.length; j++) {
                var instance = instances[j];
                instance.scrollTop = instance.scrollHeight;
            }
        }
    }

    // private method for creating an instance of the module
    var createSeederModule = (function(moduleContainerId) {

        // ui elements container
        var uiElements = function(uiContainerId) {
            var uiContainer = $(document.getElementById(uiContainerId));
            if (uiContainer == null || uiContainer.length === 0) {
                throw "Seeder Module Exception: Seeder container was not specified";
            }

            // instances of the form elements
            var start = new StartButton();
            var stop = new StopButton();
            var statusIndicator = new StatusIndicator();
            var log = new SeedLog();
            var allElements = [start, stop, statusIndicator, log];
            start.Initialize(uiContainer, true, ".sm-startButton");
            stop.Initialize(uiContainer, false, ".sm-stopButton");
            statusIndicator.Initialize(uiContainer, false, ".sm-statusIndicator");
            log.Initialize(uiContainer, false, ".sm-log");

            // event handler for the UI container
            var handleEvent = function(seederEventType, eventArgument) {
                eventArgument = eventArgument || null;

                // in some cases we need to propogate additional events
                var allEvents = [];
                allEvents.push({ eventType: seederEventType, eventArg: eventArgument });
                if (seederEventType === SeederEventTypes.StateUpdate && eventArgument != null) {
                    if (eventArgument.Started === true) {
                        allEvents.push({ eventType: SeederEventTypes.Processing, eventArg: null });
                    } else if (eventArgument.Stopped === true) {
                        allEvents.push({ eventType: SeederEventTypes.Stopped, eventArg: null });
                    }
                }

                // propogate the event to all elements
                for (var i = 0; i < allElements.length; i++) {
                    for (var j = 0; j < allEvents.length; j++) {
                        var localEvent = allEvents[j];
                        allElements[i].HandleSeederEvent(localEvent.eventType, localEvent.eventArg);
                    }
                }
            }

            return {
                SeederContainer: uiContainer,
                HandleSeederEvent: handleEvent,
                BindStartAndStopEvents: function(startEvent, stopEvent) {
                    start.BindClickEvent(startEvent);
                    stop.BindClickEvent(stopEvent);
                }
            }
        }(moduleContainerId); // instantiate the ui elements container

        // allows for events to be published from the controller service to the ui container
        var eventPublisher = function() {
            return {
                publish: function(eventType, eventArgs) {
                    uiElements.HandleSeederEvent(eventType, eventArgs);
                }
            }
        }();

        // define the functions for interacting with the controller
        var controllerService = function(controllerEventPublisher) {
            var startUrl = "/PersonSeeder/StartSeeding";
            var stopUrl = "/PersonSeeder/StopSeeding";

            var successHandler = function(data) {
                if (data.Started === true) {
                    controllerEventPublisher.publish(SeederEventTypes.Processing);
                }
                if (data.Stopped === true) {
                    controllerEventPublisher.publish(SeederEventTypes.Stopped);
                } else if (data.Errored === true) {
                    controllerEventPublisher.publish(SeederEventTypes.Errored, data.ErrorMessage);
                } else {
                    console.log("Unknown state from api" + data);
                }

                // push logs if possible
                if (data.LogMessages !== null && data.LogMessages.length !== 0) {
                    controllerEventPublisher.publish(SeederEventTypes.Logs, data.LogMessages);
                }
            }

            var errorHandler = function (data) {
                controllerEventPublisher.publish(SeederEventTypes.Errored, data);
            }

            // create a recursive function for pulling the current state
            var stateMonitor = (function() {
                var stateUrl = "/PersonSeeder/GetState";
                var delay = 5000;

                var getState = function() {
                    $.ajax({
                        url: stateUrl,
                        datatype: "text",
                        type: "POST",
                        success: successHandler,
                        error: errorHandler,
                        complete: function() {
                            // todo: do not like having ui related code in here....
                            if ($(".sm-loading").is(":visible")) {
                                $(".sm-loading").hide();
                            }
                            if ($(".sm-content").is(":hidden")) {
                                $(".sm-content").show();
                            }
                        }
                    });

                    // recursively call self
                    setTimeout(getState, delay);
                }

                return {
                    startMonitoring: function() {
                        // todo: these should exist in the ui module
                        $(".sm-loading").show();
                        $(".sm-content").hide();
                        getState();
                    }
                }
            })();
            stateMonitor.startMonitoring();

            return {
                startSeeding: function() {
                    $.ajax({ url: startUrl, type: "POST", success: successHandler, error: errorHandler });
                },
                stopSeeding: function() {
                    $.ajax({ url: stopUrl, type: "POST", success: successHandler, error: errorHandler });
                }
            }
        }(eventPublisher);

        // wire up the ui elements 
        uiElements.BindStartAndStopEvents(controllerService.startSeeding, controllerService.stopSeeding);

        return {
            start: controllerService.startSeeding,
            stop: controllerService.stopSeeding
        }
    }); // do not instantiate

    return {
        create: createSeederModule
    }
})(jQuery, UtilityService);