
chrome.runtime.onMessage.addListener(
      function (request, sender, sendResponse) {
          if (request.message === "start") {
              start(request.data);
          }
          if (request.message === "loadRAReporting") {
              loadRAReporting();
          }
      }
    );

setTimeout(function () {
    var childFrameCheckOk = document.getElementsByName("Modal1")[0];
    
    

    if (childFrameCheckOk != null) {
        var cFrameContentCheckOk = childFrameCheckOk.contentWindow.document;
        if (cFrameContentCheckOk != null) {
            var ok = cFrameContentCheckOk.getElementById("btnOK");
            if (ok != null) {
                cFrameContentCheckOk.getElementById("btnOK").click();
            }
        }
    }
    

    setTimeout(function () {
        $('#Content1').attr('src', "RAReporting.aspx");

        setTimeout(function () {
            $('#Content1').contents().find('#btnLoad').trigger("click");

            setTimeout(function () {


                var childFrame = document.getElementsByName("Modal1")[0];
                var cFrameContent = childFrame.contentWindow.document;



                cFrameContent.getElementById("dgChoices_ctl00_ctl04_gbcbtnSelect").click();

                //Date out - possibly the last 30 to 60 days
                //dd/mm/yyyy
                //dbDateOutFrom
                //dbDateOutTo



                //Date in - last 72hours
                //dbDateInFrom
                //dbDateInTo


                //TODO


                setTimeout(function () {
                    var now = new Date();
                    //var yesterdayMs = now.getTime() - 1000 * 60 * 60 * 24 * 60; // Offset by one day;
                    //now.setTime(yesterdayMs);

                    //var dd = now.getDate();
                    //var mm = now.getMonth() + 1; //January is 0!

                    //var yyyy = now.getFullYear();
                    //if (dd < 10) {
                    //    dd = '0' + dd;
                    //}
                    //if (mm < 10) {
                    //    mm = '0' + mm;
                    //}
                    //var dateOutFrom = dd + '/' + mm + '/' + yyyy;

                    var dateOutFrom = pastDate(now, 60);
                    var dateOutTo = pastDate(now, 1);

                    $('#Content1').contents().find('#dbDateOutFrom').val(dateOutFrom);
                    $('#Content1').contents().find('#dbDateOutTo').val(dateOutTo);


                    var dateInFrom = pastDate(now, 3);
                    var dateInTo = pastDate(now, 0);

                    $('#Content1').contents().find('#dbDateInFrom').val(dateInFrom);
                    $('#Content1').contents().find('#dbDateInTo').val(dateInTo);


                    setTimeout(function () {
                        $('#Content1').contents().find('#btnExport1').trigger("click");
                    }, 3000);

                }, 3000);

            }, 3000);
        }, 4000);

    }, 4000);


}, 4000);

//btnOK
//

function pastDate(date, subtract) {
    var now = new Date();
    var yesterdayMs = now.getTime() - 1000 * 60 * 60 * 24 * parseInt(subtract); // Offset by x day;
    now.setTime(yesterdayMs);

    var dd = now.getDate();
    var mm = now.getMonth() + 1; //January is 0!

    var yyyy = now.getFullYear();
    if (dd < 10) {
        dd = '0' + dd;
    }
    if (mm < 10) {
        mm = '0' + mm;
    }
    var finalDate = dd + '/' + mm + '/' + yyyy;

    return finalDate;
}

function loadRAReporting() {

    setTimeout(function () {
        $('#Content1').attr('src', "RAReporting.aspx");

        setTimeout(function () {
            $('#Content1').contents().find('#btnLoad').trigger("click");

            setTimeout(function () {


                var childFrame = document.getElementsByName("Modal1")[0];
                var cFrameContent = childFrame.contentWindow.document;



                cFrameContent.getElementById("dgChoices_ctl00_ctl06_gbcbtnSelect").click();


                setTimeout(function () {
                    $('#Content1').contents().find('#btnExport1').trigger("click");
                }, 4000);

            }, 2000);
        }, 4000);

    }, 4000);












}

function getIframeWindow(iframe_object) {
    var doc;

    if (iframe_object.contentWindow) {
        return iframe_object.contentWindow;
    }

    if (iframe_object.window) {
        return iframe_object.window;
    }

    if (!doc && iframe_object.contentDocument) {
        doc = iframe_object.contentDocument;
    }

    if (!doc && iframe_object.document) {
        doc = iframe_object.document;
    }

    if (doc && doc.defaultView) {
        return doc.defaultView;
    }

    if (doc && doc.parentWindow) {
        return doc.parentWindow;
    }

    return undefined;
}

//ASP.NET_SessionId




//setTimeout(function () {

//    //Works in chrome
//    jQuery('#Content1').contents().find('#btnLoad').trigger("click");
//jQuery('#Content1').contents().find('#btnLoad')

//    setTimeout(function () {

//        //Now to select from the modal
//        //Modal1
//        //dgChoices_ctl00_ctl06_gbcbtnSelect


//        //__LASTFOCUS
//        //$('#Modal1').contents().find('#__LASTFOCUS').value = "dgChoices_ctl00_ctl06_gbcbtnSelect";
//        //$('#Modal1').contents().find('#dgChoices_ctl00_ctl06_gbcbtnSelect').focus();
//        // $('#Modal1').contents().find('#dgChoices_ctl00_ctl06_gbcbtnSelect').trigger("click");
//        // alert("click cancom option: " + $('#Modal1').contents().find('#__LASTFOCUS').val());

//        setTimeout(function () {

//            var frame = document.getElementById("Modal1");
//            alert(frame);
//            // var y = frame.contentWindow.document;
//            //var y = (frame.contentWindow || frame.contentDocument);
//            //if (y.document) y = y.document;

//            var element = jQuery('#Modal1').contents().find('#dgChoices_ctl00_ctl06_gbcbtnSelect');

//            alert(element);
//            element.trigger("focus");

//            //var element = y.getElementById("dgChoices_ctl00_ctl06_gbcbtnSelect");

//            //alert(element);

//            //if ("createEvent" in document) {
//            //    var evt = document.createEvent("HTMLEvents");
//            //    evt.initEvent("click", false, true);
//            //    element.dispatchEvent(evt);
//            //}
//            //else
//            //    element.fireEvent("onchange");

//            //var MyIFrame = document.getElementById("Modal1");
//            //var MyIFrameDoc = (MyIFrame.contentWindow.document || MyIFrame.contentDocument);
//            //if (MyIFrameDoc.document) MyIFrameDoc = MyIFrameDoc.document;
//            //MyIFrameDoc.getElementsByTagName("form")[0].submit();



//            //$('#Modal1').contents().find("#Form1").trigger("submit");
//            //$('#Modal1').contents().WebForm_OnSubmit
//            //WebForm_OnSubmit();
//            //document.getElementById("Model1").contentWindow.document.getElementsByTagName("form")[0].submit();
//            //document.getElementsByTagName("form")[0].submit();
//            //alert("submit selection");
//        }, 2000);
//        //javascript:return WebForm_OnSubmit();
//        //alert('foo');


//    }, 2000);

//}, 2000);