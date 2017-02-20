

$(document).ready(function () {

    $("form.dishes").on("submit", function (e) {
        var postData = $(this).serializeArray();
        $.ajax(
        {
            url: "Home/AddToCart",
            type: "POST",
            data: postData,
            success: function (data, textStatus, jqXHR) {
                renderCart(data);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert(errorThrown);
            }
        });
        e.preventDefault();
    });

    $("form.cart").on("submit", function (e) {

        e.preventDefault();

        var proceed = true;
        if ($("#name").val() == "")
        {
            $("#name").parent().addClass("has-error");
            proceed = false;
        }
        else
        {
            $("#name").parent().removeClass("has-error");
        }

        if ($("#adress").val() == "") 
        {
            $("#adress").parent().addClass("has-error");
            proceed = false;
        }
        else {
            $("#adress").parent().removeClass("has-error");
        }

        if ($("#phone").val() == "") 
        {
            $("#phone").parent().addClass("has-error");
            proceed = false;
        }
        else {
            $("#phone").parent().removeClass("has-error");
        }
        
        if( $("#total").html() <= 0)
        {
            alert("Morate da izaberete bar jedno jelo!");
            proceed = false;
        }

        if (proceed) {
            var postData = $(this).serializeArray();
            postData.push({ name: 'price', value: $("#total").html() });

            $.ajax(
            {
                url: "Home/ProcessOrder",
                type: "POST",
                data: postData,
                success: function (data, textStatus, jqXHR) {
                    resetPage();
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert(errorThrown);
                }
            });
        }
        else
            return;
        
    });

    $.ajax(
        {
            url: "Home/GetCart",
            type: "POST",
            dataType: "json",
            success: function (data, textStatus, jqXHR) {
                renderCart(data);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert("Error");
            }
        });
});

function renderCart(data) {
    var parsedData = eval(data);
    var html = '';
    var totalPrice = 0;
    if (parsedData != null)
    {
        for (i = 0; i < parsedData.length; i++) {
            var orderedDish = parsedData[i];
            html += '<li class="list-group-item clearfix" style="padding:0px; margin-top:10px;">';
            html += '<div class="col-lg-12 clearfix" style="padding:0px;">';
            html += '<div class="col-lg-12" style="padding:0px;background-color:azure;">';
            html += ' <div class="col-lg-12 clearfix h3" data-toggle="collapse" data-target="#dishname' + i + '" style="text-decoration:none;text;border-bottom:solid 1px #e7e7e7;margin:0px;padding-bottom:4px;padding-top:4px;vertical-align:central;">';
            html += orderedDish.dish.name;
            html += '<span class="pull-right"><input type="button" id="dishbtn' + i + '" value="X" class="btn btn-default dishbtn"></span></div>';
            html += '<div id="dishname' + i + '" class="col-lg-12 sublinks collapse" style="padding:0px;">';
            html += '<a class="list-group-item h4" data-toggle="collapse" data-target="#supplements' + i + '" style="text-decoration:none; border:none;border-bottom:solid 1px #e7e7e7; margin:0px;"> Supplements </a>';

            if (orderedDish.supplements != null) {
                html += '<div id="supplements' + i + '" class="col-lg-12 sublinks collapse" style="background-color:#fafafa;padding:0px;"><div class="col-lg-12" style="background-color:#fafafa;border-bottom:solid 1px #e7e7e7;">';
                html += '<ul class="list-group">';
                for (j = 0; j < orderedDish.supplements.length; j++) {
                    var supplement = orderedDish.supplements[j];
                    html += '<li><div class="col-lg-12" style="background-color:#fafafa;border-bottom:solid 1px #e7e7e7;"><p>' + supplement.name + ' <span class="pull-right">Price:' + supplement.price + '</span></p></div></li>'
                }
                html += '</ul></div>';
            }
            html += '</div><div class="col-lg-12"><h4>Price: <span style="margin-left:20px;">' + orderedDish.price + '</span><span style="margin-left:80px;"> x ' + orderedDish.quantity + '</span></h4></div></div></div></li>';
            totalPrice += orderedDish.price;
        }
    }
    
    $("#orderedDishes").html(html);
    $("#total").html(totalPrice);
    if (totalPrice > 0)
        updateButtons();
}

function updateButtons() {
    $(".dishbtn").on("click", function (event) {
        var number = this.id.substring(this.id.length - 1);
        $.ajax(
        {
            url: "Home/RemoveFromCart",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ no: number }),
            success: function (data, textStatus, jqXHR) {
                renderCart(data);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert("Error");
            }
        });
    })
}

function resetPage() {
    $("#name").val("");
    $("#adress").val("");
    $("#phone").val("");
    renderCart();
}