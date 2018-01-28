setTimeout(function () {

    $("#txtPassword").val("WoodOliver_321");
    

    setTimeout(function () {
    
        $('#LoginBtn').trigger("click");
        setTimeout(function () {
            //alert("about to login second attempt");
            $('#LoginBtn').trigger("click");
        }, 3000);
    }, 3000);
}, 2000);




   