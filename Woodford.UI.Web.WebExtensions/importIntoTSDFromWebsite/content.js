
var extensionId = "chmommfmfikniomgeeenlefngglonaae"; //OLI
//var extensionId = "bifgdlgcpjoiiamkaapgmmedeagonkkc"; //TK

chrome.runtime.onMessage.addListener(

      function (request, sender, sendResponse) {
          if (request.message === "start") {
              start(request.data);
          }
          if (request.message === "loadTable") {
              loadTable(request.data);
          }
          if (request.message === "loadItem") {
              loadItem(request.data);
          }
      }
    );

function start(data) {

    $('iframe[name=Content1]').contents().find('#txtLastName').val(data.name);

}
function loadItem(data) {
    exportResJSON(data);


}
function loadTable(data) {
    console.log("loadTable");

    var checkDiv = document.getElementById("resContainer");

    if (checkDiv != null) {
        checkDiv.parentNode.removeChild(checkDiv);
    }

    var div = document.createElement("div");
    div.setAttribute("id", "resContainer");
    div.setAttribute("style", "margin-top: 50px;");
    div.innerHTML = data;



    var insertAfter = document.getElementById("#Content1");

    document.body.insertBefore(div, insertAfter);
    console.log("table inserted");



    $('.chkExported').change(function () {

        console.log("checkbox changed");

        var exported = $(this).is(':checked');



        var resId = $(this).attr('id').replace('chkExported_', '');

        chrome.runtime.sendMessage(
            extensionId, // PUT YOUR EXTENSION ID HERE
            { "message": "exportChecked", "resId": resId, "exported": exported }
        );

    });

    $('.exportLink').click(function () {

        console.log("export button clicked");

        var resId = $(this).attr('id');

        console.log("Id:" + resId);

        chrome.runtime.sendMessage(
            extensionId, // PUT YOUR EXTENSION ID HERE
            { "message": "loadItem", "resId": resId }
        );
    });
}

function exportResJSON(item) {

    //Res ID in note field
    $('#Content1').contents().find('#txtNote').val(item.Id);

    //User Details
    $('#Content1').contents().find('#txtEmail').val(item.Email);
    $('#Content1').contents().find('#txtFirstName').val(item.FirstName);
    $('#Content1').contents().find('#txtLastName').val(item.LastName);
    $('#Content1').contents().find('#txtCellPhone').val(item.MobileNumber);
    $('#Content1').contents().find('#txtPassport').val(item.IdNumber);

    //only for loyalty members
    if (item.User == null) {

    } else {
        if (item.User.IsLoyaltyMember) {
            $('#Content1').contents().find('#txtPreferredNum').val(item.User.LoyaltyNumberFull);
        }
    }

    ////Vehicle Class
    $('#Content1').contents().find('#ddlClass').val(item.Vehicle.VehicleGroup.Title);

    //Source of business
    //ddlSource - value 10 for woodford.co.za
    $('#Content1').contents().find('#ddlSource').val("10");



    //Pickup
    $('#Content1').contents().find('#ddlPickupLoc').val(mapLocation(item.PickupBranchId));

    var pickupDate = new Date(parseInt(item.PickupDate.substr(6)));


    var pickupHours = pickupDate.getHours();
    if (pickupHours > 12) {
        pickupHours = pickupHours - 12;
    }
    if (pickupHours.toString().length == 1) {
        pickupHours = "0" + pickupHours.toString();
    }

    $('#Content1').contents().find('#txtPickupDate').val(getDateInTSDFormat(pickupDate));
    $('#Content1').contents().find('#txtHourOut').val(pickupHours);
    $('#Content1').contents().find('#txtMinOut').val("00");
    $('#Content1').contents().find('#ddlAmPmOut').val(getAmPm(pickupDate.getHours()));


    ////Dropoff
    $('#Content1').contents().find('#ddlDropLoc').val(mapLocation(item.DropOffBranchId));

    var dropoffDate = new Date(parseInt(item.DropOffDate.substr(6)));



    setTimeout(function () {

        var dropoffHours = dropoffDate.getHours();
        if (dropoffHours > 12) {
            dropoffHours = dropoffHours - 12;
        }
        if (dropoffHours.toString().length == 1) {
            dropoffHours = "0" + dropoffHours.toString();
        }

        $('#Content1').contents().find('#txtDropOffDate').val(getDateInTSDFormat(dropoffDate));
        $('#Content1').contents().find('#txtHourIn').val(dropoffHours);
        $('#Content1').contents().find('#txtMinIn').val("00");
        $('#Content1').contents().find('#ddlAmPmIn').val(getAmPm(dropoffDate.getHours()));
        $('#Content1').contents().find('#txtDays').val(item.NumberOfDays);



        //Now we set the custom rate
        setTimeout(function () {
            //Set custom rate
            $('#Content1').contents().find('#ddlPlanType').val("CUSTOM RATE");
            $('#Content1').contents().find('#btnCustRate').click();

            setTimeout(function () {
                //Wait for modal to open

                var rateFrame = document.getElementsByName("Modal1")[0];
                $(rateFrame).attr('id', 'rateModal');



                $('#rateModal').contents().find('#txtDaily1Amount').val(item.AdjustedPricePerDay);
                $('#rateModal').contents().find('#txtDaily2Amount').val(item.AdjustedPricePerDay);

                var perKm = mapPerKm(item.Vehicle.VehicleGroup.Title);

                $('#rateModal').contents().find('#txtPerMile').val(perKm);

                $('#rateModal').contents().find('#ddlCalHour').val("C");

                $('#rateModal').contents().find("#txtDaily1Days").val(item.NumberOfDays);

                setTimeout(function () {
                    //butSave
                    $('#rateModal').contents().find("#butSave").trigger("click");

                    setTimeout(function () {
                        //Set this again, for some reason it gets reset after closing the custom rate modal.
                        $('#Content1').contents().find('#ddlClass').val(item.Vehicle.VehicleGroup.Title);


                        //Check if booking has any extras.

                        //butExtras

                        $('#Content1').contents().find("#butExtras").trigger("click");
                        setTimeout(function () {
                            var optionsFrame = document.getElementsByName("Modal1")[0];
                            $(optionsFrame).attr('id', 'optionsModal');

                            $('#optionsModal').contents().find("#btnNew").trigger("click");

                            setTimeout(function () {
                                //Modal2
                                var addOptionFrame = document.getElementsByName("Modal2")[0];
                                $(addOptionFrame).attr('id', 'newOptionsModal');

                                //table id tblMain
                                //class name of rows rgRow


                                //dgExtras_ctl00_ctl04_chkAdd
                                //$('#newOptionsModal').contents().find("#dgExtras_ctl00_ctl04_chkAdd").checked = true;
                                $('#newOptionsModal').contents().find("#dgExtras_ctl00_ctl04_chkAdd").prop("checked", "checked");
                                
                                setTimeout(function () {
                                    //$('#newOptionsModal').contents().find("#btnAdd").trigger("click");
                                }, 4000);
                                //Open extras modal
                            }, 2000);
                            //Open extras modal
                        }, 2000);


                    }, 2000);
                    //Close custom rate modal
                }, 2000);


            }, 4000);
            //open custom rate modal
        }, 1000);

    }, 4000);




}

function getDateInTSDFormat(date) {
    var day = parseInt(date.getDate());

    if (day.toString().length == 1) {
        day = "0" + day.toString();
    }

    var month = parseInt(date.getMonth()) + 1;
    if (month.toString().length == 1) {
        month = "0" + month.toString();
    }
    //alert(day + "/" + month + "/" + date.getFullYear());
    return day + "/" + month + "/" + date.getFullYear();
}

function getAmPm(hours) {
    if (hours > 12) {
        return "PM";
    } else {
        return "AM";
    }
    //var hours = (hours + 24 - 2) % 24;
    //var mid = 'AM';
    //if (hours == 0) { //At 00 hours we need to show 12 am
    //    hours = 12;
    //}
    //else if (hours > 12) {
    //    hours = hours % 12;
    //    mid = 'PM';
    //}
    //return mid;
}


function openRateModal() {
    var $f = $("#Content1");
    var fd = $f[0].document || $f[0].contentWindow.document; // document of iframe
    fd.ddlPlanType_onchange('Rez', 'True', 'false');  // run function


    //document.getElementById("Content1").contentWindow.ddlPlanType_onchange('Rez', 'True', 'false');

}

function mapLocation(location) {
    //Id	Title
    //1	King Shaka International
    //2	Cape Town International
    //4	OR Tambo International
    //5	Durban Downtown
    //6	Port Elizabeth International
    //7	Pinetown

    var response = "";
    switch (location) {
        case 1:
            response = "KSIA";
            break;
        case 2:
            response = "CAPE";
            break;

        case 4:
            response = "TAMBO";
            break;
        case 5:
            response = "HO";
            break;
        case 6:
            response = "PE";
            break;
        case 7:
            response = "PINE";
            break;
    }
    return response;
}
//ddlPickupLoc
//<option value="CAPE">CAPE - WOODFORD CAR HIRE</option>
//<option value="HO">HO - WOODFORD GROUP</option>
//<option value="KSIA">KSIA - WOODFORD CAR HIRE</option>
//<option value="ORD">ORD - WOODFORD CAR AND BAKKIE HIRE</option>
//<option value="PE">PE - WOODFORD CAR HIRE</option>
//<option value="PINE">PINE - WOODFORD VEHICLE RENTALS</option>
//<option value="TAMBO">TAMBO - WOODFORD CAR HIRE</option>
//<option value="WFS">WFS - WOODFORD FS</option></select>


function mapPerKm(vehicleGroup) {
    var response = "";
    switch (vehicleGroup) {
        case "MDMR":
            response = "1.99";
            break;
        case "EDMR":
            response = "2.99";
            break;
        case "HDMR":
            response = "2.29";
            break;
        case "CDMR":
            response = "3.66";
            break;
        case "CDAR":
            response = "2.49";
            break;
        case "SDAR":
            response = "2.98";
            break;
        case "FDMR":
            response = "4.80";
            break;
        case "IFAR":
            response = "3.47";
            break;
        case "PCAR":
            response = "4.19";
            break;
        case "PDAR":
            response = "5.19";
            break;
        case "UDAR":
            response = "6.19";
            break;
        case "UFAR":
            response = "6.88";
            break;
        case "LCAR":
            response = "9.19";
            break;
        case "WCAR":
            response = "18.40";
            break;
        case "FFMR":
            response = "4.70";
            break;
        case "IVMR":
            response = "3.80";
            break;
        case "PVMR":
            response = "5.90";
            break;
        case "LFAR":
            response = "11.90";
            break;
        case "SPMQ":
            response = "2.39";
            break;
        case "SKMQ":
            response = "2.39";
            break;
    }

    return response;
}
