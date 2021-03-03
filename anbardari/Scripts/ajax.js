
//چک کردن کد ملی 
$('#meli').on('change', function () {

    $("#result_codemeli").html('<i class="fa fa-refresh fa-spin"></i>');
    $("#result_codemeli").css('display', 'block');
    var meli_code = $('#meli').val();
    if (meli_code.length == 10) {
        if (meli_code == '1111111111' ||
            meli_code == '0000000000' ||
            meli_code == '2222222222' ||
            meli_code == '3333333333' ||
            meli_code == '4444444444' ||
            meli_code == '5555555555' ||
            meli_code == '6666666666' ||
            meli_code == '7777777777' ||
            meli_code == '8888888888' ||
            meli_code == '9999999999' ||
            meli_code == '0123456789'
        ) {
            $("#result_codemeli").text('كد ملي صحيح نمي باشد');
            $("#result_codemeli").css('display', 'block');
            $("#result_codemeli").css('color', 'red');
            console.log('error: meli ! ');

            $("#name").val('');
            $("#family").val('');

            return;
        }
        c = parseInt(meli_code.charAt(9));
        n = parseInt(meli_code.charAt(0)) * 10 +
            parseInt(meli_code.charAt(1)) * 9 +
            parseInt(meli_code.charAt(2)) * 8 +
            parseInt(meli_code.charAt(3)) * 7 +
            parseInt(meli_code.charAt(4)) * 6 +
            parseInt(meli_code.charAt(5)) * 5 +
            parseInt(meli_code.charAt(6)) * 4 +
            parseInt(meli_code.charAt(7)) * 3 +
            parseInt(meli_code.charAt(8)) * 2;
        r = n - parseInt(n / 11) * 11;

        if ((r == 0 && r == c) || (r == 1 && c == 1) || (r > 1 && c == 11 - r)) {
            console.log('POST data');
            var address = "/bimars/getBimarWithCodemeli";
            $.ajax({
                url: address,
                type: "POST",
                data: {
                    codemeli: meli_code
                },
                dataType: "JSON",
                success: function (jsonStr) {
                    var res = JSON.stringify(jsonStr);
                    if (res != '') {
                        $("#result_codemeli").text('بیمار وجود دارد');
                        $("#result_codemeli").css('display', 'block');
                        $("#result_codemeli").css('color', 'green');
                        $("#name").val(jsonStr.name);
                        $("#family").val(jsonStr.family);
                        console.log('success POST data');
                        getParvandeWithBimarId(jsonStr.Id);
                        console.log('affter parvande function');
                    }
                    else {
                        $("#result_codemeli").text('کد ملی صحیح است');
                        $("#result_codemeli").css('display', 'block');
                        $("#result_codemeli").css('color', 'green');

                        $("#name").val('');
                        $("#family").val('');

                        console.log('ok: codemeli');

                    }
                },
                error: function (jsonStr) {
                    console.log('error: POST data .... ');
                    console.log(JSON.stringify(jsonStr));
                    $("#result_codemeli").text('کد ملی صحیح است');
                    $("#result_codemeli").css('display', 'block');
                    $("#result_codemeli").css('color', 'green');
                    console.log('ok: meli ');

                    $("#name").val('');
                    $("#family").val('');
                }
            });
        }
        else {
            $("#result_codemeli").text('كد ملي صحيح نمي باشد');
            $("#result_codemeli").css('display', 'block');
            $("#result_codemeli").css('color', 'red');
            console.log('error: !meli ');

            $("#name").val('');
            $("#family").val('');
        }
    }
    else {
        $("#result_codemeli").text('طول کد ملی وارد شده باید 10 کاراکتر باشد');
        $("#result_codemeli").css('display', 'block');
        $("#result_codemeli").css('color', 'red');
        console.log('error: meli != 10');

        $("#name").val('');
        $("#family").val('');
        return;
    }

});

//
function getNumberOfProduct(productid) {
    console.log("productid:" + productid);
    console.log('begin POST data');
    var address = "/orders/getNumberOfProduct";
    var result = -1;
    $.ajax({
        url: address,
        type: "POST",
        data: {
            productid: productid
        },
        dataType: "JSON",
        async:false,
        success: function (jsonStr) {
            var res = JSON.stringify(jsonStr);
            if (res != '') {
                console.log('success : ' + res);
                result = res;
                //callback.call(result);
            }
            else {
                result = 0;
            }
        },
        error: function (jsonStr) {
            console.log('error: POST data .... ');
            console.log(JSON.stringify(jsonStr));
            result = 0;

        }
    });
    console.log(result);
    return result;
}

function saveMaxNumberOfProduct(productid,number) {
    console.log('begin POST data');
    var address = "/orders/saveMaxNumberOfProduct";
    var result = -1;
    $.ajax({
        url: address,
        type: "POST",
        data: {
            productid: productid,
            number: number
        },
        dataType: "JSON",
        async: false,
        success: function (jsonStr) {
            var res = JSON.stringify(jsonStr);
            if (res != '') {
                console.log('success : ' + res);
                result = res;
                //callback.call(result);
            }
            else {
                result = 0;
            }
        },
        error: function (jsonStr) {
            console.log('error: POST data .... ');
            console.log(JSON.stringify(jsonStr));
            result = 0;

        }
    });
    return result;
}

$('.pMaxNumber').change(function () {
    var number = this.value;
    var pid = $(this).data('pid');
    var result = Number(saveMaxNumberOfProduct(pid, number));

    if (result > 0)
    {
        console.log("ثبت");
        $('.myallert').html("ثبت شد");
    }
    else {
        console.log("خطا");
        $('.myallert').html("خطا:لطفا مجدد سعی کنید");
    }
    $('.myallert').addClass("show");
    setTimeout(function () {
        $('.myallert').removeClass('show');
    }, 2000);
});
