(function () {
    var el = $('#username');
    el.text('Test');

    var main = $('#main');
    main.on('mouseenter', function () {
        main.css('background-color', '#888');
    });
    main.on('mouseleave', function () {
        main.css('background-color', '');
    });

    var menuItems = $('ul.menu li a');

}) ();