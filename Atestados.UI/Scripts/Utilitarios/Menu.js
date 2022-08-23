 //   function CreaMenuEstructuraInterna(data) {
 //       var itemsMenu = data.ObjetoRespuesta;

 //       var CreaArray = function(itemsMenu, IdPadre) {
 //           var items;
 //           for (var i = 0; i < itemsMenu.length; i++) {
 //               if (itemsMenu[i].Padre == IdPadre) {
 //                   items = $("<li>").append(
 //                       $("<a>",
 //                           {
 //                               href: '#' + itemsMenu[i].UrlPagina,
 //                               html: itemsMenu[i].Nombre
 //                           }));
                   
 //                   if (itemsMenu[i].TieneHijos) {
 //                       var subList = $("<ul id='menu_" + itemsMenu[i].IdPermiso + "'>");
 //                       $("#menu_" + IdPadre).append(items.append(subList));
 //                       CreaArray(itemsMenu, itemsMenu[i].IdPermiso);
 //                   }
 //                   $("#menu_" + IdPadre).append(items);
 //               }
 //           }
 //       }

 //       CreaArray(itemsMenu, 0);

 //}



    //function CreaMenuEstructuraInterna(data) {
    //    var itemsMenu = data.ObjetoRespuesta;

    //    var CreaArray = function (itemsMenu, IdPadre) {
    //        var items;
    //        for (var i = 0; i < itemsMenu.length; i++) {
    //            if (itemsMenu[i].Padre == IdPadre) {
    //                items = $("<li>").append(
    //                    $("<a>",
    //                        {
    //                            href: '#' + itemsMenu[i].UrlPagina,
    //                            html: itemsMenu[i].Nombre
    //                        }));

    //                if (itemsMenu[i].TieneHijos) {
    //                    items.append(
    //                        "<span class='drop-icon'>▾</span> <label title='Toggle Drop-down' class='drop-icon' for='sm" +
    //                        IdPadre +
    //                        "'>▾</label><input type='checkbox' id='sm" +
    //                        IdPadre +
    //                        "'>");
    //                    var subList = $("<ul id='menu_" + itemsMenu[i].IdPermiso + "' class='sub-menu'>");
    //                    $("#menu_" + IdPadre).append(items.append(subList));
    //                    CreaArray(itemsMenu, itemsMenu[i].IdPermiso);
    //                }
    //                $("#menu_" + IdPadre).append(items);
    //            }
    //        }
    //    }

    //    CreaArray(itemsMenu, 0);

    //}




    function CreaMenuEstructuraInterna(data) {
        var itemsMenu = data.ObjetoRespuesta;

        var CreaArray = function (itemsMenu, IdPadre) {
            var items;
            for (var i = 0; i < itemsMenu.length; i++) {
                if (itemsMenu[i].Padre == IdPadre) {
                    items = $("<li class='searchterm'>").append(
                        $("<a>",
                            {
                                href: itemsMenu[i].UrlPagina,
                                html: itemsMenu[i].Nombre
                            }));
                 

                    if (itemsMenu[i].TieneHijos) {
                        var CssULclass = "dropdown2";
                        //if ($("#menu_" + IdPadre).hasClass("dropdown")) {
                        //    CssULclass = "dropdown2";
                        //}
                        var subList = $("<span class='glyphicon glyphicon-triangle-right color-arrow' aria-hidden='true'></span> <ul id='menu_" + itemsMenu[i].IdPermiso + "' class='" + CssULclass+"' >");
                        $("#menu_" + IdPadre).append(items.append(subList));
                        CreaArray(itemsMenu, itemsMenu[i].IdPermiso);
                    }
                    $("#menu_" + IdPadre).append(items);
                }
            }
        }

        CreaArray(itemsMenu, 0);



        $(".core-menu li").hover(
            function () {
                //i used the parent ul to show submenu
                $(this).children('ul').slideDown('fast');
            },
            //when the cursor away 
            function () {
                $('ul', this).slideUp('fast');
            });
        //this feature only show on 600px device width
        //$(".hamburger-menu").click(function () {
        //    $(".burger-1, .burger-2, .burger-3").toggleClass("open");
        //    $(".core-menu").slideToggle("fast");
        //});


        $("#Btn_MenuPrin").click(function () {
            $(".ContenedorMenu").slideToggle("fast");
            //$(".icon-1, .icon-2, .icon-3").toggleClass("open");
        }).mouseover(function () {
            $(this).css("cursor", "pointer");
        }).mouseout(function () {
            $(this).css("cursor", "default");
        });

        $(".ContenedorMenu").hide();


        $(".searchterm").click(function (event) {
            var url = $(this).find('a').attr('href');
            window.location.href = url;
        });

    }

   
