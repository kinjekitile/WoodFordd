function searchFunctions() {
    var _pickUpLocationSelector;
    var _dropOffLocationSelector;
    var _returnDifferentLocationChk;
    var _airportLocationsSelector;
    var _pickupTimeSelector;
    var _dropOffTimeSelector;
    var _returnLocationContainer;
    var _pickupTextBox;
    var self;

    this.init = function (pickUpLocationSelectorId, dropOffLocationSelectorId, returnDifferentLocationChkId, airportLocationsSelectorId, pickupTimeSelectorId, dropOffTimeSelectorId, returnLocationContainerId, pickupTextBoxId) {
        self = this;

        _pickUpLocationSelector = $("#" + pickUpLocationSelectorId);
        _dropOffLocationSelector = $("#" + dropOffLocationSelectorId);
        _returnDifferentLocationChk = $("#" + returnDifferentLocationChkId);
        _airportLocationsSelector = $("#" + airportLocationsSelectorId);
        _pickupTimeSelector = $("#" + pickupTimeSelectorId);
        _dropOffTimeSelector = $("#" + dropOffTimeSelectorId);
        _returnLocationContainer = $("#" + returnLocationContainerId);
        _pickupTextBox = $("#" + pickupTextBoxId);

        self.setReturnLocationSelectedState();
        self.checkIfLocationsAreDifferent();
        self.showHideReturnLocation(false);
        self.modifyTimes();

        self.addBehaviour();
    }

    this.addBehaviour = function () {
        

        _returnDifferentLocationChk.on("change", function () {
            self.showHideReturnLocation(true);
            self.modifyTimes();
            self.setReturnLocationSelectedState();
        });        

        _pickUpLocationSelector.on("change", function () {
            self.modifyTimes();
            self.setReturnLocationSelectedState();
            self.hidePickupDropDown();
        });

        _dropOffLocationSelector.on("change", function () {
            self.modifyTimes();
        });

        //_pickupTextBox.on("focus", function () {
        //    self.showPickupDropDown();
        //})
    }

    //this.setupLocationAutocomplete = function () {
    //    _pickupTextBox
    //}

    this.showPickupDropDown = function () {
        _pickUpLocationSelector.show();
        _pickupTextBox.hide();
        _pickUpLocationSelector.focus();
    }

    this.hidePickupDropDown = function () {
        _pickupTextBox.val(_pickUpLocationSelector.children("option:selected").text());
        _pickUpLocationSelector.hide();
        _pickupTextBox.show();
    }

    this.setReturnLocationSelectedState = function () {
       
        if (!_returnDifferentLocationChk.is(":checked")) {
            _dropOffLocationSelector.val(_pickUpLocationSelector.val());
        }
    }

    this.addTimesForAirport = function (timeSelector, startTime, endTime, prepend) {
        var optionsString = "";
        var t = 0;
        for (var i = startTime; i <= endTime; i++) {

            var amPm = "AM";
            var text = (startTime + t);
            var value = (startTime + t);
            if (value == 0) {
                text = "12";
            } else {
                if (value > 12) {
                    text = value - 12;
                    amPm = "PM";
                }
            }

            optionsString += "<option class='airport-only-time' value='" + value + "'>" + text + ":00 " + amPm + "</option>";
            t++;
        }
        if (prepend) {
            timeSelector.prepend(optionsString);
        } else {
            timeSelector.append(optionsString);
        }
    }

    this.removeTimesNotAirport = function (timeSelector) {
        $("option.airport-only-time", timeSelector).remove();
    }

    this.modifyTimeDropDown = function (locationSelector, timeSelector) {
        if (self.isAirport(locationSelector.val())) {
            if ($("option", timeSelector).length < 24) {
                self.addTimesForAirport(timeSelector, 0, 6, true);
                self.addTimesForAirport(timeSelector, 23, 23, false);
            }
        } else {
            if ($("option", timeSelector).length > 23) {
                self.removeTimesNotAirport(timeSelector);
            }
        }
    }

    this.isAirport = function (locationId) {
        var airports = _airportLocationsSelector.val();
        var a = airports.split(",");
        var found = false;

        for (var i = 0; i < a.length; i++) {
            if (a[i] == locationId) {
                found = true;
            }
        }
        return found;
    }

    this.showHideReturnLocation = function (userChanged) {
        var show = _returnDifferentLocationChk.is(":checked");
        var returnLocationContainer = _returnLocationContainer;
        if (show) {
            returnLocationContainer.show();
            $("#lblDropoffDiffLocation").css("padding-top", "0px");
        } else {
            returnLocationContainer.hide();
            $("#lblDropoffDiffLocation").css("padding-top", "30px");
            if (userChanged) {
                _dropOffLocationSelector.val(_pickUpLocationSelector.val());
            }
        }
    }

    this.modifyTimes = function () {
        self.modifyTimeDropDown(_pickUpLocationSelector, _pickupTimeSelector);

        if (_returnDifferentLocationChk.is(":checked")) {
            self.modifyTimeDropDown(_dropOffLocationSelector, _dropOffTimeSelector);
        } else {
            self.modifyTimeDropDown(_pickUpLocationSelector, _dropOffTimeSelector);
        }


    }

    this.checkIfLocationsAreDifferent = function () {
        var pickupLocationId = _pickUpLocationSelector.val();
        var dropOffLocationId = _dropOffLocationSelector.val();
        //alert(pickupLocationId);
        //alert(dropOffLocationId);
        _returnDifferentLocationChk.prop('checked', pickupLocationId != dropOffLocationId);
    }

}