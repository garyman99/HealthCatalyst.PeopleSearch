
// todo: this is tightly coupled to jQuery--might be good to inject in an abstraction
var seedModuleConstructor = (function (moduleContainerId) {
    // create a container to hold each of the UI elements and the logic around them
    var moduleElements = function (containerId) {
        var seederContainer = $(document.getElementById(containerId));
        if (seederContainer == null || seederContainer.length === 0) {
            throw "Seeder Module Exception: Seeder container was not specified";
        }

        // "base" element type
        function SeederElement() {
            this.instances = [];
        }
        SeederElement.prototype.GetInstances = function () { return this.instances; };
        SeederElement.prototype.Initialize = function (required, selector) {
            var instances = seederContainer.find(selector);
            if (required && instances.length === 0) {
                throw "Seeder Module Exception: Required element with selector " +
                    selector +
                    " returned no elements";
            }
            this.instances = instances;
        };
        SeederElement.prototype.Disable = function () {
            this.GetInstances().attr("disabled", "disabled");
        }
        SeederElement.prototype.Enable = function () {
            this.GetInstances().removeAttr("disabled");
        }
        SeederElement.prototype.HandleProcessing = function () {
            this.GetInstances().addClass("sm-started");
            this.GetInstances().removeClass("sm-stopped");
            this.GetInstances().removeClass("sm-errored");
        }
        SeederElement.prototype.HandleProcessingStopped = function () {
            this.GetInstances().addClass("sm-stopped");
            this.GetInstances().removeClass("sm-started");
            this.GetInstances().removeClass("sm-errored");
        }
        SeederElement.prototype.HandleError = function () {
            this.GetInstances().addClass("sm-errored");
            this.GetInstances().removeClass("sm-started");
            this.GetInstances().removeClass("sm-stopped");
        }
        SeederElement.prototype.BindOnClick = function (event) {
            var instances = this.GetInstances();
            instances.on("click", event);
        }

        // start button(s)
        function StartButton() {}
        StartButton.prototype = Object.create(SeederElement.prototype); // copy prototype on base
        StartButton.prototype.HandleProcessing = function () {
            SeederElement.prototype.HandleProcessing.call(this);
            SeederElement.prototype.Disable.call(this);
        }
        StartButton.prototype.HandleProcessingStopped = function () {
            SeederElement.prototype.HandleProcessingStopped.call(this);
            SeederElement.prototype.Enable.call(this);
        }

        // stop button(s)
        function StopButton() { }
        StopButton.prototype = Object.create(SeederElement.prototype); // copy prototype on base
        StopButton.prototype.HandleProcessing = function () {
            SeederElement.prototype.HandleProcessing.call(this);
            SeederElement.prototype.Enable.call(this);
        }
        StopButton.prototype.HandleProcessingStopped = function () {
            SeederElement.prototype.HandleProcessingStopped.call(this);
            SeederElement.prototype.Disable.call(this);
        }

        // status indicators
        function StatusIndicator() { }
        StatusIndicator.prototype = Object.create(SeederElement.prototype); // copy prototype on base
        StatusIndicator.prototype.HandleProcessing = function () {
            SeederElement.prototype.HandleProcessing.call(this);
            this.GetInstances().text("Started");
        }
        StatusIndicator.prototype.HandleProcessingStopped = function () {
            SeederElement.prototype.HandleProcessingStopped.call(this);
            this.GetInstances().text("Stopped");
        }
        StatusIndicator.prototype.HandleError = function () {
            SeederElement.prototype.HandleError.call(this);
            this.GetInstances().text("Errored");
        }

        // logs
        function SeedLogElement() { }
        SeedLogElement.prototype = Object.create(SeederElement.prototype); // copy prototype on base
        SeedLogElement.prototype.HandleError = function (errorMessage) {
            SeederElement.prototype.HandleError.call(this);
            this.GetInstances().append("Errored\r\n");
            this.GetInstances().append(errorMessage + "\r\n");
        }
        SeedLogElement.prototype.HandleStateUpdate = function (stateData) {
            var instances = this.GetInstances();
            for (var i = 0; i < stateData.LogMessages.length; i++) {
                instances.append(stateData.LogMessages[i] + "\r\n");
            }

            for (var i = 0; i < instances.length; i++) {
                var instance = instances[i];
                instance.scrollTop = instance.scrollHeight;
            }
        }

        // create the instances of the types above
        var startButton = new StartButton();
        startButton.Initialize(true, ".sm-startButton");
        var stopButton = new StopButton();
        stopButton.Initialize(false, ".sm-stopButton");
        var statusIndicator = new StatusIndicator();
        statusIndicator.Initialize(false,".sm-statusIndicator");
        var log = new SeedLogElement();
        log.Initialize(false, ".sm-log");
        var allElements = [startButton, stopButton, statusIndicator, log];

        // todo: could prototype these methods--no need for them to be created with 
        //       each instance of the module
        var handleProcessing = function () {
            for (var i = 0; i < allElements.length; i++) {
                allElements[i].HandleProcessing();
            }
        }
        var handleProcessingStopped = function () {
            for (var i = 0; i < allElements.length; i++) {
                allElements[i].HandleProcessingStopped();
            }
        }
        var handleError = function (errorMessage) {
            for (var i = 0; i < allElements.length; i++) {
                allElements[i].HandleError(errorMessage);
            }
        }
        var handleStateUpdate = function (stateData) {
            if (stateData.Started === true) {
                handleProcessing();
            } else if (stateData.Stopped === true) {
                handleProcessingStopped();
            }
            log.HandleStateUpdate(stateData);
        }
        var bindStartAndStopEvents = function (startEvent, stopEvent) {
            startButton.BindOnClick(startEvent);
            stopButton.BindOnClick(stopEvent);
        }

        return {
            SeederContainer: seederContainer,
            HandleProcessing: handleProcessing,
            HandleProcessingStopped: handleProcessingStopped,
            HandleError: handleError,
            HandleStateUpdate: handleStateUpdate,
            BindStartAndStopEvents: bindStartAndStopEvents
        }
    }(moduleContainerId); // instantiate using the provided module container id

    // define the functions for interacting with the controller
    var controllerService = function(handleSeedingStartDelegate, handleSeedingStopDelegate, handleErrorDelegate) {
        var startUrl = "/PersonSeeder/StartSeeding";
        var stopUrl = "/PersonSeeder/StopSeeding";

        var start = function () {
            $.ajax({
                url: startUrl,
                datatype: "text",
                type: "POST",
                success: function(data) {
                    if (data.Started === true) {
                        handleSeedingStartDelegate();
                    } else if (data.Errored === true) {
                        handleErrorDelegate(data.ErrorMessage);
                    }
                },
                error: function(data) {
                    handleErrorDelegate(data);
                }
            });
        }

        var stop = function() {
            $.ajax({
                url: stopUrl,
                datatype: "text",
                type: "POST",
                success: function(data) {
                    if (data.Started === false) {
                        handleSeedingStopDelegate();
                    } else if (data.Errored === true) {
                        handleErrorDelegate(data.ErrorMessage);
                    }
                },
                error: function(data) {
                    handleErrorDelegate(data);
                }
            });
        }

        return {
            StartSeeding: start,
            StopSeeding: stop
        }
    }(moduleElements.HandleProcessing, moduleElements.HandleProcessingStopped, moduleElements.HandleError);

    // create a recursive function for pulling
    var stateMonitor = (function (stateUpdateDelegate) {
        var stateUrl = "/PersonSeeder/GetState";
        var delay = 5000;

        var getState = function() {
            $.ajax({
                url: stateUrl,
                datatype: "text",
                type: "POST",
                success: function(data) {
                    stateUpdateDelegate(data);
                },
                error: function() {
                    console.log("Error getting state");
                },
                complete: function () {
                    // todo: do not like having ui related code in here....
                    if ($(".sm-loading").is(":visible")) {
                        $(".sm-loading").hide();
                    }
                    if ($(".sm-content").is(":hidden")) {
                        $(".sm-content").show();
                    }
                }
            });
            setTimeout(getState, delay);
        }

        var startMonitoring = function () {
            // todo: these elements should exist inside the ui elements container
            $(".sm-loading").show();
            $(".sm-content").hide();
            getState();
        }
        return {
            Start: startMonitoring
        }
    })(moduleElements.HandleStateUpdate);

    // wire up the ui elements 
    moduleElements.BindStartAndStopEvents(controllerService.StartSeeding, controllerService.StopSeeding);

    stateMonitor.Start();

    return {
        start: controllerService.StartSeeding,
        stop: controllerService.StopSeeding
    }
});