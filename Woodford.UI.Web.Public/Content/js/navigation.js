function mechMenu() {

    var mouseOverInterval;
    var mouseOutInterval;
    var mouseOutNavInterval;
    var menu;
    var currentItem;
    var delay = 150;
    var slideSpeed = 100;

    this.init = function () {

        menu = this;

        $("#menu>li").mouseover(function () {

            clearInterval(mouseOutInterval);
            clearInterval(mouseOverInterval);

            var item = this;
            $(item).addClass("hover");
            mouseOverInterval = setInterval(function () { menu.showSubMenu(item); }, delay);

        }).mouseout(function () {

            clearInterval(mouseOutInterval);
            clearInterval(mouseOverInterval);

            var item = this;
            $(item).removeClass("hover");
            mouseOutInterval = setInterval(function () { menu.hideSubMenu(item); }, delay);

        });

    }

    this.showSubMenu = function (item) {
        clearInterval(mouseOutInterval);
        clearInterval(mouseOverInterval);

        if (currentItem != item) {
            this.hideAllSubMenus();
        }
        currentItem = item;
        
        $(item).children(".rollover-menu").fadeIn(slideSpeed); //.css("display", "block");
    }

    this.hideSubMenu = function (item) {
        clearInterval(mouseOutInterval);
        clearInterval(mouseOverInterval);

        this.hideAllSubMenus();
    }

    this.hideAllSubMenus = function () {
        currentItem = null;
        clearInterval(mouseOutInterval);
        clearInterval(mouseOverInterval);
        clearInterval(mouseOutNavInterval);
        $("#menu li .rollover-menu").fadeOut(slideSpeed); //.css("display", "none");
    }

}

$(document).ready(function () {
    var menu = new mechMenu();
    menu.init();
});