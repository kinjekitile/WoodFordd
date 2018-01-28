//http://admin.woodford.co.za/asklngpids
var url = "http://localhost:8083/asklngpids";
//var url = "http://admin.woodford.co.za/asklngpids";

console.log("this is background.js reporting for duty");
document.addEventListener('DOMContentLoaded', function () {


 
    $("#btnFetchReservations").click(function () {
        console.log("btn clicked");
        $("#btnFetchReservations").val("Fetching Reservations...");
        sendReservationsToTab();
        $("#btnFetchReservations").val("Load Reservations");
    });
});


chrome.runtime.onMessage.addListener(function (message, sender, sendResponse) {
      if (message.message === "loaded") {
          console.log("target page loaded event received");
          //sendReservationsToTab();
      }
     // alert("message received: " + message.message);
      console.log(message.message);

      if (message.message === "loadItem") {
          console.log("LoadItem message received");
         
          var resId = message.resId;

          sendResDetailsToTab(resId);
          
      }

      if (message.message === "exportChecked") {
          console.log("target page export check event received");

          var data = { "id": message.resId, "exported": message.exported };

          $.post(url + "/SetAsExported", data)
            .done(function (response, status, jqxhr) {

                console.log('success save exported status');
            })
            .fail(function (jqxhr, status, error) {

                console.log('failed save exported status');
            });
      }
  }
);

function sendReservationsToTab() {
    console.log("send res to tab");
    chrome.tabs.query({ currentWindow: true, active: true }, function (tabs) {
        var activeTab = tabs[0];

        //http://admin.woodford.co.za/asklngpids
        //http://localhost:8083/asklngpids
        $.get(url, {}, function (data) {
           
            chrome.tabs.sendMessage(activeTab.id, { "message": "loadTable", "data": data });
            console.log("sent");
        });
       
    });
}

function sendResDetailsToTab(resId) {
    var itemUrl = url + "/details/" + resId;

    console.log("send res details to tab");
    chrome.tabs.query({ currentWindow: true, active: true }, function (tabs) {
        var activeTab = tabs[0];

        $.get(itemUrl, {}, function (data) {

            chrome.tabs.sendMessage(activeTab.id, { "message": "loadItem", "data": data });
            console.log("sent details");
        });

    });
}

