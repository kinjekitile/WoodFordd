function cleanDate(dateInputId) {
    var dateInput = $('#' + dateInputId);
    var dateValue = dateInput.val();
    var dtComponents = dateValue.split(' ');
    if (dtComponents.length > 1) {
        dateValue = dtComponents[0];
    }
    dateInput.val(dateValue);
}

function setUpDatePicker(inputSelector) {
    inputSelector.datepicker({
        dateFormat: "yy-mm-dd"
    });

    //inputSelector.DatePicker({
    //    flat: false,
    //    date: inputSelector.val(),
    //    current: '@DateTime.Today.ToString("yyyy-MM-dd")',
    //    format: 'Y-m-d',
    //    calendars: 1,
    //    mode: 'single',
    //    onBeforeShow: function () {
    //        inputSelector.DatePickerSetDate(inputSelector.val(), true);
    //    },
    //    onChange: function (formated, dates) {
    //        inputSelector.val(formated);
    //        $('.datefield').DatePickerHide();
    //    }
    //});
}

function setupAutoCompleteLocation(inputSelector, inputTarget) {
    inputSelector.autocomplete({
        source: [{ label: "Pinetown", value: "7" }],
        minLength: 0,
        select: function (event, ui) {
            inputSelector.val(ui.item.label);
            inputTarget.val(ui.item.value);
            return false;
        },
        focus: function (event, ui) {
            inputSelector.val(ui.item.label);
            inputTarget.val(ui.item.value);
            return false;
        }
    }).focus(function () {
        inputSelector.autocomplete('search', "");
    });


    inputSelector.on("click", function () {
        inputSelector.autocomplete('search', "");
    });

    if (window.location.href.indexOf("search") > -1) {
        inputSelector.val(inputTarget.children("option:selected").text());
    }
}

function setupTimePicker(inputSelector) {
    inputSelector.timepicki({
        start_time: ["07", "00", "AM"],
        disable_keyboard_mobile: true
    });
}

subscribeBegin = function () {
    $("#subscribe-form .submit-button").hide();
    $("#subscribe-form .spinner").show();
}

subscribeComplete = function () {
    $("#subscribe-form .spinner").hide();
    $("#subscribe-form .submit-button").show();
}

$(document).ready(function () {
    var searchPanel = $("#search-panel");
    cleanDate("PickupDate");
    cleanDate("DropOffDate");
    setUpDatePicker($("#PickupDate"));
    setUpDatePicker($("#DropOffDate"));
    setupAutoCompleteLocation($("#txtPickupLocation"), $("#Criteria_PickUpLocationId"));
    setupAutoCompleteLocation($("#txtDropOffLocation"), $("#Criteria_DropOffLocationId"));
    setupTimePicker($("#Criteria_PickupTimeFull"));
    setupTimePicker($("#Criteria_DropOffTimeFull"));

    //txtDropOffLocation
    var searchFunc = new searchFunctions();
    searchFunc.init("Criteria_PickUpLocationId", "Criteria_DropOffLocationId", "IsReturnDifferentLocation", "AirportLocationIds", "Criteria_PickupTime", "Criteria_DropOffTime", "different-return-location", "txtPickupLocation");

    function formatSearchToggle() {
        if (searchPanel.hasClass("panel-minimised")) {
            $("a.toggle-search-panel").addClass("expand");
            $("a.toggle-search-panel").attr("style", "color: white;");
        } else {
            $("a.toggle-search-panel").removeClass("expand");
        }
    }

    $("a.toggle-search-panel").on("click", function (e) {
        searchPanel.toggleClass("panel-minimised");
        formatSearchToggle();
        e.preventDefault();
        return false;
    });

    $("#search-panel-container").on("click", function (e) {
        searchPanel.toggleClass("panel-minimised");
        formatSearchToggle();
        e.preventDefault();
        return false;
    });

    formatSearchToggle();

    $("div.faqs div.answer").each(function () {
        var a = $(this);
        a.attr("data-height", a.height() + 20);
        //a.css("height", "0px");
    });

    $("div.faqs a.question").on("click", function (e) {
        var q = $(this);
        var a = $("div.answer", q.parent());

        if (q.hasClass("active")) {
            //close item if its already active
            q.removeClass("active");
            a.removeClass("open"); //.slideUp();
            openCloseAnswer(a, false);

        } else {
            //if the clicked item isn't already active, then close all other items
            var activeQuestions = $("a.question.active");
            var openAnswers = $("div.answer.open");
            activeQuestions.removeClass("active");
            openAnswers.removeClass("open");
            openCloseAnswer(openAnswers, false);

            //activate clicked item
            q.addClass("active");
            a.addClass("open");
            openCloseAnswer(a, true);
        }
        highlightQuestion();
        e.preventDefault();
    });

    function highlightQuestion() {
        activeQuestions = $("a.question.active");
        activeQuestions.parent().removeClass("dimmed");
        if (activeQuestions.length == 0) {
            $("a.question").parent().removeClass("dimmed");
        } else {
            $("a.question").not(activeQuestions).parent().addClass("dimmed");
        }
    }

    function openCloseAnswer(answer, open) {
        if (answer.length > 0) {
            var height = 0;
            if (open) {
                height = answer.attr("data-height");
            }

            answer.animate({
                "height": height + "px"
            }, 500);
        }
    }
    
    function setPriceForVehicle(selectedRate) {
        var vehicleId = selectedRate.attr("data-vehicle-id");
        var total = selectedRate.attr("data-total");
        var cost = selectedRate.attr("data-cost");
        var days = selectedRate.attr("data-days");
        var tax = selectedRate.attr("data-tax");
        var dropOffFee = selectedRate.attr("data-dropOffFee");


        var tooltipText = "Rental: R " + $.format.number(cost, '#,##0.00') + " x " + days + " days\n";
        tooltipText += " + Tax: R " + $.format.number(tax, '#,##0.00') + "\n";
        if (dropOffFee > 0) {
            tooltipText += " + Drop Off fee: R " + $.format.number(dropOffFee, '#,##0.00') + "\n";
        }
        tooltipText += "=================\n"
        tooltipText += "R " + $.format.number(total, '#,##0.00');

        //$("#rate-price-" + vehicleId).html("R " + $.format.number(total, '#,##0.00')).attr("title", tooltipText);;
        $("#rate-price-" + vehicleId).html("R " + total).attr("title", tooltipText);;
    }

    $("input[type='radio'].rate-selector").on("change", function () {
        var vehicleId = $(this).attr("data-vehicle-id");
        setPriceForVehicle($(this));
        //var cost = $(this).attr("data-cost");
        //$("#rate-price-" + vehicleId).html("R " + $.format.number(cost, '#,##0.00'));
    });

    $("div.rates-selection").each(function () {
        var vehicleId = $(this).attr("data-vehicle-id");
        var selectedRate = $("input[name='radRates_" + vehicleId + "']:checked");
        setPriceForVehicle(selectedRate);


        //var cost = selectedRate.attr("data-cost");
        //$("#rate-price-" + vehicleId).html("R " + $.format.number(cost, '#,##0.00'));
    });

});