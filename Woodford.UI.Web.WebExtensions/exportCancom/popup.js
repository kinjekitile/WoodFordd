//http://admin.woodford.co.za/asklngpids
//http://localhost:8083/asklngpids
var url = "http://admin.woodford.co.za/asklngpids";

console.log("this is background.js reporting for duty");
document.addEventListener('DOMContentLoaded', function () {



    $("#btnStart").click(function () {
        console.log("btn clicked");
        $("#btnStart").val("Started");
        startProcess();
        $("#btnStart").val("Load Reservations");
    });
});


chrome.runtime.onMessage.addListener(function (message, sender, sendResponse) {
    if (message.message === "loaded") {
        console.log("target page loaded event received");
        //sendReservationsToTab();
    }


}
);

chrome.tabs.onUpdated.addListener(function (tabId, changeInfo, tab) {
    if (changeInfo.status == 'complete') {

        // do your things

    }
})

function startProcess() {
    console.log("send res to tab");
    chrome.tabs.query({ currentWindow: true, active: true }, function (tabs) {
        var activeTab = tabs[0];
        var data = "";
        chrome.tabs.sendMessage(activeTab.id, { "message": "loadRAReporting", "data": data });
        console.log("sent");


    });
}

