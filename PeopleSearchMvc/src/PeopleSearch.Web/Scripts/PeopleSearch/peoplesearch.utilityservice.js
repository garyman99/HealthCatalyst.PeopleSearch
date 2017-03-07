var UtilityService = (function () {
    return {
        inherit: function(parent, child) {
            child.prototype = Object.create(parent.prototype);
        }
    }
})(); 