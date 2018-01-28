function cleanDate(dateInputId) {
    var dateInput = $('#' + dateInputId);
    var dateValue = dateInput.val();    
    var dtComponents = dateValue.split(' ');
    if (dtComponents.length > 1) {
        dateValue = dtComponents[0];
    }
    dateInput.val(dateValue);
}