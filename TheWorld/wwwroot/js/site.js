(function () {
    var $sidebarAndWrapper = $('#sidebar,#wrapper');
    var $icon = $('#sidebarToggle i.fa');

    $('#sidebarToggle').on("click", function () {
        $sidebarAndWrapper.toggleClass('hide-sidebar');
        $icon.toggleClass('fa-angle-left fa-angle-right');
    });    
}) ();